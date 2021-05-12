using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading.Forms;
using Kit;
using Kit.Extensions;
using Kit.Forms.Extensions;
using Kit.Forms.Services;
using Kit.Model;
using ShannonEntropy.EntropyLibrary;
using ShannonEntropy.Models;
using ShannonEntropy.Resources;
using ShannonEntropy.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;
using PermissionStatus = Xamarin.Essentials.PermissionStatus;

namespace ShannonEntropy.ViewModels
{
    public class FromPhotoPageViewModel : ModelBase
    {
        private FileImageSource _Image;
        public FileImageSource Image
        {
            get => _Image;
            set
            {
                _Image = value;
                Raise(() => Image);
            }
        }

        public ICommand PickFileCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand SamplesCommand { get; set; }
        public ICommand CleanCommand { get; set; }
        public FromPhotoPageViewModel()
        {
            this.PickFileCommand = new Command(PickFile);
            this.CalculateCommand = new Xamarin.Forms.Command<CachedImage>(Calculate);
            this.TakePhotoCommand = new Command(TakePhoto);
            this.SamplesCommand = new Command(Samples);
            this.CleanCommand = new Command(Clean);
        }
        private async void Clean()
        {
            if (await Acr.UserDialogs.UserDialogs.Instance.
                ConfirmAsync(AppResources.CleanTextAsk,
                    AppResources.Alert, AppResources.Yes, AppResources.Cancel))
            {
                this.Image =null;
            }
        }
        private void Samples()
        {
            Shell.Current.Navigation.PushAsync(new ImagesSamples());
        }

        private async void TakePhoto()
        {
            var permiso = new Permissions.Camera();
            if (!await Permisos.TenemosPermiso(permiso))
            {
                RequestCameraPage request = new RequestCameraPage();
                await request.ShowDialog();
                if (!permiso.ShouldShowRationale())
                {
                    await Task.Delay(500);
                    Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedCamera,
                        AppResources.Alert);
                    return;
                }
                return;
            }
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo is not null)
            {
                await Task.Delay(500);
                await ReadFile(photo);
            }
        }

        private async void PickFile()
        {
            await Task.Yield();
            try
            {
                var permiso = new Permissions.Photos();
                if (!await Permisos.TenemosPermiso(new Permissions.Photos()))
                {
                    RequestCameraPage request = new RequestCameraPage();
                    await request.ShowDialog();
                    if (!permiso.ShouldShowRationale())
                    {
                        await Task.Delay(500);
                        Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedCamera,
                            AppResources.Alert);
                        return;
                    }
                    return;
                }
                var pfile = await MediaPicker.PickPhotoAsync();
                if (pfile is not null)
                {
                    await Task.Delay(500);
                    using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
                    {
                        await ReadFile(pfile);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "While picking file");
                Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.ErrorPickingFile,
                    "Lo sentimos...", "Ok");
            }

        }

        private async Task ReadFile(FileResult pfile)
        {
            await Task.Yield();
            this.Image = null;
            if (pfile.ContentType.Contains("text"))
            {
                if (await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(
                    AppResources.ItsAFileNotAnImage, AppResources.Alert,
                    AppResources.Yes, AppResources.Cancel))
                {
                    await Shell.Current.Navigation.PopToRootAsync(true);
                    await Shell.Current.Navigation.PushAsync(new FromTextPage(pfile));
                }
                return;
            }
            if (!pfile.ContentType.Contains("image"))
            {
                if (!await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(
                    AppResources.NotPlainTextFile, AppResources.Alert,
                    AppResources.Yes, AppResources.Cancel))
                {
                    return;
                }

            }

            FileInfo file = await pfile.LoadPhotoAsync();
            var mb = file.Length.ToSize(BytesConverter.SizeUnits.MB);
            if (mb > 20)
            {
                if (!await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(
                    AppResources.BigFile, AppResources.Alert,
                    AppResources.Yes, AppResources.Cancel))
                {
                    return;
                }
            }
            this.Image = (FileImageSource)FileImageSource.FromFile(file.FullName);
        }
        private async void Calculate(CachedImage Image)
        {
            if (Image?.Source is null)
            {
                return;
            }
            PictureHystogram PictureHystogram = null;
            using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
            {
                PictureHystogram =await Task.Run(()=>EntropyLibrary.EntropyLibrary.CalculateEntropy(Image));
            }
            await App.Current.MainPage.Navigation.PushAsync(new PhotoResults(PictureHystogram));
        }
        public async void Load(FileResult pfile)
        {
            using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
            {
                await ReadFile(pfile);
            }
        }
    }
}
