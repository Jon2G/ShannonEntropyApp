using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kit.Model;
using ShannonEntropy.Models;

namespace ShannonEntropy.ViewModels
{
    public class FrecuencyVsEntropyPageViewModel : ModelBase
    {
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

        private double _Event1;
        public double Event1
        {
            get => _Event1;
            set
            {
                _Event1 = value;
                CalculateEntropy();
            }
        }

        private double _Event2;
        public double Event2
        {
            get => _Event2; set
            {
                _Event2 = value;
                CalculateEntropy();
            }
        }

        private bool _EquiProbable;

        public bool EquiProbable
        {
            get => _EquiProbable;
            set
            {
                _EquiProbable = value;
                Raise(() => EquiProbable);
            }
        }

        public FrecuencyVsEntropyPageViewModel()
        {
            this.Event1 = 0;
            this.Event2 = 0;
            this.TotalEntropy = 0;
        }

        private void CalculateEntropy()
        {
            if (Event1 == Event2)
            {
                if (Event1 == 0)
                {
                    this.TotalEntropy = 0;
                }
                else
                {
                    this.TotalEntropy = 100;
                }

                this.EquiProbable = true;
            }
            else
            {
                this.EquiProbable = false;
                double ent =
                    Math.Round((EntropyCalculator.Calculate(Event1 / 100d, Event2 / 100d) * 100), 2);
                if (ent > 100)
                {
                    ent = 100;
                }
                this.TotalEntropy = ent;
            }
        }
    }
}
