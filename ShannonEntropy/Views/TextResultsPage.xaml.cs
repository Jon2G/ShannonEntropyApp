using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ShannonEntropy.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShannonEntropy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextResultsPage : ContentPage
    {
        public TextResultsPage()
        {
            InitializeComponent();
        }
        internal async void Calculate(StringBuilder text)
        {
            using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
            {
                await this.Model.Calculate(text);
            }
        }
        internal async void Calculate(FileInfo file)
        {
            using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
            {
                await this.Model.Calculate(file);
            }
        }
    }
}