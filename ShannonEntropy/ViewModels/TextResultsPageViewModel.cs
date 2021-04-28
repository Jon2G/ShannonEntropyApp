using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task Calculate(StringBuilder text)
        {
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

        //public async Task Calculate(StringBuilder text)
        //{
        //    await Task.Yield();
        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        AddCharacter(text[i]);
        //    }
        //    CalculateTotalEntropy();
        //}
        //public void AddCharacter(char character)
        //{
        //    if (this.Symbols.TryGetValue(character, out Symbol symbol))
        //    {
        //        symbol.Count++;
        //        return;
        //    }
        //    this.Symbols.Add(character, new Symbol(character));
        //}
        //private void CalculateTotalEntropy()
        //{
        //    foreach (var symbol in Symbols.Values)
        //    {
        //        symbol.CalculateFrecuency(Symbols.Values.Sum(x => x.Count));
        //    }
        //    this.TotalEntropy = EntropyCalculator.Calculate(Symbols.Values.Select(x => x.Frecuency));
        //    Raise(() => TotalEntropy);
        //}
    }
}
