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
using ShannonEntropy.Models;
using Xamarin.Forms.Internals;

namespace ShannonEntropy.ViewModels
{
    public class TextResultsPageViewModel : ModelBase
    {
        public ObservableConcurrentDictionary<char, Symbol> Symbols { get; set; }
        public double TotalEntropy { get; set; }

        public TextResultsPageViewModel()
        {
            this.Symbols = new ObservableConcurrentDictionary<char, Symbol>();
        }

        public async Task Calculate(FileInfo file)
        {
            await Task.Yield();
            using (var fileStream = File.OpenRead(file.FullName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF7))
            {
                string line = await streamReader.ReadLineAsync();
                while (line is not null)
                {
                    line.ForEach(AddCharacter);
                    line = await streamReader.ReadLineAsync();
                }
            }

            CalculateTotalEntropy();
        }
        //public async Task Calculate(FileInfo file)
        //{
        //    await Task.Yield();
        //    using (var fileStream = File.OpenRead(file.FullName))
        //    using (var streamReader = new StreamReader(fileStream, Encoding.UTF7))
        //    {
        //        int character = streamReader.Read();
        //        while (character != -1)
        //        {
        //            AddCharacter((char)character);
        //            character = streamReader.Read();
        //        }
        //    }

        //    CalculateTotalEntropy();
        //}
        public async Task Calculate(StringBuilder text)
        {
            await Task.Yield();
            for (int i = 0; i < text.Length; i++)
            {
                AddCharacter(text[i]);
            }
            CalculateTotalEntropy();
        }
        public void AddCharacter(char character)
        {
            if (this.Symbols.TryGetValue(character, out Symbol symbol))
            {
                symbol.Count++;
                return;
            }
            this.Symbols.Add(character, new Symbol(character));
        }
        private void CalculateTotalEntropy()
        {
            foreach (var symbol in Symbols.Values)
            {
                symbol.CalculateFrecuency(Symbols.Values.Sum(x => x.Count));
            }
            this.TotalEntropy = EntropyCalculator.Calculate(Symbols.Values.Select(x => x.Frecuency));
            Raise(() => TotalEntropy);
        }
    }
}
