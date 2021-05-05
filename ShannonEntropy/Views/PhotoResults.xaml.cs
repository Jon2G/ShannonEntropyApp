using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShannonEntropy.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShannonEntropy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoResults : ContentPage
    {
        public PhotoResults(PictureHystogram PictureHystogram)
        {
            this.BindingContext= PictureHystogram;
            InitializeComponent();
        }
    }
}