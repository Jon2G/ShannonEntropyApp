using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShannonEntropy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FromImage : ContentPage
    {
        public FromImage(FileResult file) : this()
        {
            this.Model.Load(file);
        }
        public FromImage()
        {
            InitializeComponent();
        }
    }
}