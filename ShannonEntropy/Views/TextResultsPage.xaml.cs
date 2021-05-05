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
        internal void Calculate(StringBuilder text)
        {
            this.Appearing += async (o, e) =>
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
                {
                    await this.Model.Calculate(text);
                }
            };
        }
        internal void Calculate(FileInfo file)
        {
            this.Appearing +=async (o, e) =>
            {
                using (Acr.UserDialogs.UserDialogs.Instance.Loading(AppResources.PleaseWait))
                {
                    await this.Model.Calculate(file);
                }
            };
        }


    }
}