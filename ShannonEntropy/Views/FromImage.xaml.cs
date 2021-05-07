using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShannonEntropy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FromImage 
    {
        public FromImage(FileResult file) : this()
        {
            this.Model.Load(file);
        }
        public FromImage()
        {
            InitializeComponent();
        }

        public void Load(CachedImage image)
        {
            this.Image.Source = image.Source;
        }
    }
}