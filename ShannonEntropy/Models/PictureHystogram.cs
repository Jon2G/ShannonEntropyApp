using System;
using System.Collections.Generic;
using System.Text;
using Kit.Model;

namespace ShannonEntropy.Models
{
    public class PictureHystogram:ModelBase
    {
        private float _TotalEntropy;

        public float TotalEntropy
        {
            get => _TotalEntropy;
            set
            {
                _TotalEntropy = value;
                Raise(()=>TotalEntropy);
            }
        }

        public ColorHystogram Red { get; }
        public ColorHystogram Green { get; }
        public ColorHystogram Blue { get; }

        public PictureHystogram()
        {
            Red = new ColorHystogram();
            Green = new ColorHystogram();
            Blue = new ColorHystogram();
        }
    }
}
