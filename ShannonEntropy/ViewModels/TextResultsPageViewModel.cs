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
using P42.Utils;
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
                EntropyLibrary.EntropyLibrary calc = new();
                lock (calc)
                {
                    using (calc)
                    {
                        string temp = Path.Combine(Tools.Instance.TemporalPath, $"{Guid.NewGuid():N}.txt");
                        FileInfo file = new FileInfo(temp);
                        if (!file.Directory.Exists)
                        {
                            file.Directory.Create();
                        }
                        File.WriteAllText(file.FullName, text.ToString());
                        calc.ReadFromFile(temp);
                        this.TotalEntropy = calc.GetTotalEntropy();
                        this.Symbols = calc.GetSymbols();
                        calc.Release();
                    }
                }
            });
        }

        public Task Calculate(FileInfo file)
        {
            return Task.Run(() =>
            {
                EntropyLibrary.EntropyLibrary calc = new();
                lock (calc)
                {
                    using (calc)
                    {
                        calc.ReadFromFile(file.FullName);
                        this.TotalEntropy = calc.GetTotalEntropy();
                        this.Symbols = calc.GetSymbols();
                        calc.Release();
                    }
                }
            });
        }
    }
}
