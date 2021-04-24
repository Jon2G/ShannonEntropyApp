using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kit;
using Kit.Extensions;
using Kit.Forms.Extensions;
using Kit.Model;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ShannonEntropy.Resources;
using ShannonEntropy.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

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
        public FromPhotoPageViewModel()
        {
            this.PickFileCommand = new Command(PickFile);
        }

        private async void PickFile()
        {
            await Task.Yield();
            try
            {
                if (!await Permisos.TenemosPermiso(Permission.Storage))
                {
                    await Permisos.PedirPermiso(Permission.Storage, AppResources.AllowAccess);
                }
                var pfile = await FilePicker.PickAsync();
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
            FileInfo file = new FileInfo(pfile.FullPath);
            var mb = file.Length.ToSize(BytesConverter.SizeUnits.MB);
            if (mb > 1)
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
        public async void Load(FileResult pfile)
        {
            using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
            {
                await ReadFile(pfile);
            }
        }
    }
}
