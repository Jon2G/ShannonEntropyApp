using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FFImageLoading.Forms;
using Kit.Extensions;
using Kit.Model;
using Kit.Sql.Reflection;
using ShannonEntropy.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShannonEntropy.ViewModels
{
    public class TextSamplesViewModel : ModelBase
    {
        public List<StringBuilder> Samples { get; }
        public ICommand CalculateCommand { get; }
        public TextSamplesViewModel()
        {
            this.CalculateCommand = new Xamarin.Forms.Command<StringBuilder>(Calculate);
            this.Samples = new List<StringBuilder>();
            using (ReflectionCaller caller = ReflectionCaller.FromAssembly<ImagesSamplesViewModel>())
            {
                foreach (string resource in caller.FindResources(x => x.EndsWith(".txt")))
                {
                    this.Samples.Add(new StringBuilder(ReflectionCaller.ToText(caller.GetResource(resource), Encoding.UTF8)));
                }
            }
        }

        private void Calculate(StringBuilder obj)
        {
            if (string.IsNullOrEmpty(obj?.ToString()))
            {
                return;
            }
            Shell.Current.Navigation.PopAsync(true);
            if (Shell.Current.Navigation.NavigationStack.LastOrDefault() is FromTextPage fromText)
            {
                fromText.Load(obj);
            }
        }
    }
}
