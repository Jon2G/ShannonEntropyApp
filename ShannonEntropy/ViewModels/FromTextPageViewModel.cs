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
using ShannonEntropy.Resources;
using ShannonEntropy.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace ShannonEntropy.ViewModels
{
    public class FromTextPageViewModel : ModelBase
    {
        private StringBuilder _Text;
        public StringBuilder Text
        {
            get => _Text;
            set
            {
                _Text = value;
                Raise(() => Text);
            }
        }

        private int _MaxLenght;

        public int MaxLenght
        {
            get => _MaxLenght;
            set
            {
                _MaxLenght = value;
                Raise(() => MaxLenght);
            }
        }

        private int _Lenght;

        public int Lenght
        {
            get => _Lenght;
            set
            {
                _Lenght = value;
                Raise(() => Lenght);
            }
        }

        public ICommand PickFileCommand { get; set; }
        public ICommand CleanCommand { get; set; }
        public Command CalculateCommand { get; set; }
        public ICommand SamplesCommand { get; set; }
        public FromTextPageViewModel()
        {
            this.Text = new StringBuilder(500);
            this.MaxLenght = 500;
            this.PickFileCommand = new Command(PickFile);
            this.CleanCommand = new Command(Clean);
            this.CalculateCommand = new Command(Calculate, canExecute: () => Lenght > 0);
            this.SamplesCommand = new Command(Samples);
        }
        private void Samples()
        {
            Shell.Current.Navigation.PushAsync(new TextSamples());
        }
        private async void Calculate()
        {
            var page = new TextResultsPage();
            page.Calculate(this.Text);
            await Shell.Current.Navigation.PushAsync(page);
        }
        private async void Calculate(FileInfo file)
        {
            var page = new TextResultsPage();
            page.Calculate(file);
            await Shell.Current.Navigation.PushAsync(page);
        }

        private async void Clean()
        {
            if (await Acr.UserDialogs.UserDialogs.Instance.
                ConfirmAsync(AppResources.CleanTextAsk,
                    AppResources.Alert, AppResources.Yes, AppResources.Cancel))
            {
                this.Text = new StringBuilder(500);
                this.MaxLenght = 500;
            }
        }

        private async void PickFile()
        {
            await Task.Yield();
            try
            {
                if (await Permisos.EnsurePermission<Permissions.StorageRead>(AppResources.AllowAccess) != PermissionStatus.Granted)
                {
                    Acr.UserDialogs.UserDialogs.Instance.Alert(AppResources.HasDeniedStorage, AppResources.Alert, "Ok");
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
            this.Text.Clear();
            if (pfile.ContentType.Contains("image"))
            {
                if (await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(
                    AppResources.ItsAnImageNotAFile, AppResources.Alert,
                    AppResources.Yes, AppResources.Cancel))
                {
                    await Shell.Current.Navigation.PopToRootAsync(true);
                    await Shell.Current.Navigation.PushAsync(new FromImage(pfile));
                }
                return;
            }
            if (pfile.ContentType != "text/plain")
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
            if (mb > 20)
            {
                if (!await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(
                    AppResources.BigFile, AppResources.Alert,
                    AppResources.Yes, AppResources.Cancel))
                {
                    return;
                }
            }
            Calculate(file);
        }



        public void Load(StringBuilder sb)
        {
            this.MaxLenght = sb.Length;
            this.Text = sb;
            Raise(()=>Lenght);
        }
        public async void Load(FileResult pfile)
        {
            using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
            {
                await ReadFile(pfile);
            }
        }

        public void TextChanged(TextChangedEventArgs e)
        {
            this.Text.Clear().Append(e.NewTextValue);
            this.Lenght = this.Text.Length;
            this.CalculateCommand?.ChangeCanExecute();
        }
    }
}
