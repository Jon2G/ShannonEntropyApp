using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShannonEntropy.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShannonEntropy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrecuencyVsEntropyPage : ContentPage
    {
        public FrecuencyVsEntropyPage()
        {
            InitializeComponent();
            this.Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FrecuencyVsEntropyPageViewModel.TotalEntropy))
            {
                SliderT.Value = this.Model.TotalEntropy;
            }
        }

        private void RadialSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {

        }
    }
}