using ShannonEntropy.Services;
using ShannonEntropy.Views;
using System;
using System.Globalization;
using System.Threading;
using ShannonEntropy.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ShannonEntropy
{
    public partial class App : Application
    {
        public static CultureInfo UsaCultureInfo => CultureInfo.GetCultureInfo("en-us");
        public static CultureInfo MexCultureInfo => CultureInfo.GetCultureInfo("es-mx");
        public App()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
            AppResources.Culture = CultureInfo.InstalledUICulture;
            if (AppResources.Culture.TwoLetterISOLanguageName != "en" && AppResources.Culture.TwoLetterISOLanguageName != "es")
            {
                Thread.CurrentThread.CurrentUICulture = MexCultureInfo;
                AppResources.Culture = MexCultureInfo;
            }

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
