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
    public partial class FromTextPage
    {
        public FromTextPage(FileResult pfile) : this()
        {
            this.Model.Load(pfile);
        }
        public FromTextPage()
        {
            InitializeComponent();
        }

        private void InputView_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Model.TextChanged(e);
        }
    }
}