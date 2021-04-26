using System;
using System.Collections.Generic;
using System.Text;
using Kit.Model;

namespace ShannonEntropy.Models
{
    public class Symbol : ModelBase
    {
        public char Character { get; }
        private int _Count;

        public int Count
        {
            get => _Count;
            set
            {
                _Count = value;
                Raise(() => Count);
            }
        }

        public double Frecuency { get; set; }

        public Symbol(char Character)
        {
            this.Character = Character;
            this.Count = 1;
            this.Frecuency = 0;
        }
        public void CalculateFrecuency(int TotalSymbols)
        {
            this.Frecuency = ((double)this.Count / TotalSymbols);
            Raise(() => Frecuency);
        }
    }
}
