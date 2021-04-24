using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Kit.Model;
using ShannonEntropy.Resources;
using ShannonEntropy.Views;
using Xamarin.Forms;

namespace ShannonEntropy.ViewModels
{
    public class MainPageViewModel : ModelBase
    {
        public ICommand FromTextCommand { get; set; }
        public ICommand FromPhotoCommand { get; set; }
        public ICommand FrecuenyEntropyCommand { get; set; }
        public ICommand TheoryCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand ChangeLanguajeCommand { get; set; }
        public MainPageViewModel()
        {
            this.FromTextCommand = new Command(FromText);
            this.FromPhotoCommand = new Command(FromPhoto);
            this.FrecuenyEntropyCommand = new Command(FrecuenyEntropy);
            this.TheoryCommand = new Command(Theory);
            this.AboutCommand = new Command(About);
            this.ChangeLanguajeCommand = new Command(ChangeLanguaje);
        }

        private async void FromText() => await Shell.Current.Navigation.PushAsync(new FromTextPage(), true);

        private async void FromPhoto() => await Shell.Current.Navigation.PushAsync(new FromImage(), true);

        private async void FrecuenyEntropy() => await Shell.Current.Navigation.PushAsync(new FrecuencyVsEntropyPage(), true);

        private async void Theory() => await Shell.Current.Navigation.PushAsync(new TheoryPage(), true);

        private async void About() => await Shell.Current.Navigation.PushAsync(new AboutPage(), true);

        private void ChangeLanguaje()
        {
            if (AppResources.Culture.TwoLetterISOLanguageName == "en")
            {
                Thread.CurrentThread.CurrentUICulture = App.MexCultureInfo;
                AppResources.Culture = App.MexCultureInfo;
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = App.UsaCultureInfo;
                AppResources.Culture = App.UsaCultureInfo;
            }
            App.Current.MainPage = new AppShell();
        }
    }
}
