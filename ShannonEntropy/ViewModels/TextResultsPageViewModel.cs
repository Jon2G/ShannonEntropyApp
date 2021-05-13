using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit;
using Kit.Model;
using ShannonEntropy.EntropyLibrary;
using ShannonEntropy.Models;
using Xamarin.Forms.Internals;

namespace ShannonEntropy.ViewModels
{
    public class TextResultsPageViewModel : ModelBase
    {
        private IEnumerable<Symbol> _Symbols;

        public IEnumerable<Symbol> Symbols
        {
            get => _Symbols;
            set
            {
                _Symbols = value;
                Raise(() => Symbols);
            }
        }

        private double _TotalEntropy;

        public double TotalEntropy
        {
            get => _TotalEntropy;
            set
            {
                _TotalEntropy = value;
                Raise(() => TotalEntropy);
            }
        }

        public TextResultsPageViewModel()
        {
            this.Symbols = null;
        }

        public Task Calculate(StringBuilder text)
        {
            return Task.Run(() =>
            {
                string temp = Path.Combine(Tools.Instance.TemporalPath, $"{Guid.NewGuid():N}.txt");
                FileInfo file = new FileInfo(temp);
                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }
                File.WriteAllText(file.FullName, text.ToString(),Encoding.UTF7);
                var result = EntropyLibrary.EntropyLibrary.ReadFromFile(temp);
                this.TotalEntropy = result.Item2;
                this.Symbols = result.Item1;
            });
        }

        public Task Calculate(FileInfo file)
        {
            return Task.Run(() =>
            {
                var calc = EntropyLibrary.EntropyLibrary.ReadFromFile(file.FullName);
                this.TotalEntropy = calc.Item2;
                this.Symbols = calc.Item1;
            });
        }
    }
}
