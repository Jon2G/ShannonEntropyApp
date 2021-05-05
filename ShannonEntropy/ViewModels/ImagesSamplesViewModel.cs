using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FFImageLoading.Forms;
using Kit.Extensions;
using Kit.Model;
using ShannonEntropy.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShannonEntropy.ViewModels
{
    public class ImagesSamplesViewModel : ModelBase
    {
        public List<FileImageSource> Samples { get; }
        public ICommand CalculateCommand { get; }
        public ImagesSamplesViewModel()
        {
            this.CalculateCommand = new Xamarin.Forms.Command<CachedImage>(Calculate);
            this.Samples=new List<FileImageSource>()
            {
                "i65.png","i70.png","i69.jpg","i81.png","full_24bits_rgb"
            };
        }

        private void Calculate(CachedImage obj)
        {
            if (obj is  null)
            {
                return;
            }
            Shell.Current.Navigation.PopAsync(true);
            if (Shell.Current.Navigation.NavigationStack.LastOrDefault() is FromImage fromImage)
            {
                fromImage.Load(obj);
            }
        }
    }
}
