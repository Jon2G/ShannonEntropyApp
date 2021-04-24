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
    public class FromTextPageViewModel : ModelBase
    {
        private string _Text;
        public string Text
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

        public ICommand PickFileCommand { get; set; }
        public ICommand CleanCommand { get; set; }
        public FromTextPageViewModel()
        {
            this.Text = string.Empty;
            this.MaxLenght = 500;
            this.PickFileCommand = new Command(PickFile);
            this.CleanCommand = new Command(Clean);
        }

        private async void Clean()
        {
            if (await Acr.UserDialogs.UserDialogs.Instance.
                ConfirmAsync(AppResources.CleanTextAsk,
                    AppResources.Alert, AppResources.Yes, AppResources.Cancel))
            {
                this.Text = String.Empty;
                this.MaxLenght = 500;
            }
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
            this.Text = string.Empty;
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
            if (mb > 1)
            {
                if (!await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(
                    AppResources.BigFile, AppResources.Alert,
                    AppResources.Yes, AppResources.Cancel))
                {
                    return;
                }
            }
            string text = await ReadAllTextAsync(pfile.FullPath);
            if (text.Length > MaxLenght)
            {
                MaxLenght = text.Length;
            }
            this.Text = text;
        }

        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            var stringBuilder = new StringBuilder();
            using (var fileStream = File.OpenRead(filePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF7))
            {
                string line = await streamReader.ReadLineAsync();
                while (line != null)
                {
                    stringBuilder.AppendLine(line);
                    line = await streamReader.ReadLineAsync();
                }
                return stringBuilder.ToString();
            }
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
