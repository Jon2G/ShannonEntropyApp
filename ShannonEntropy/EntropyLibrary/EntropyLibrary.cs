using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ShannonEntropy.Models;

namespace ShannonEntropy.EntropyLibrary
{
    public class EntropyLibrary : IDisposable
    {
        private readonly EntropyLibrarySafeHandle _handle;

        public EntropyLibrary()
        {
            //string a = EntropyLibraryWrapper.getTemplateInfo();

            _handle = EntropyLibraryWrapper.GetEntropyLibrary();
        }

        public void ReadFromFile(string file) =>
            EntropyLibraryWrapper.ReadFromFile(_handle, file);

        public double GetTotalEntropy() =>
            EntropyLibraryWrapper.GetTotalEntropy(_handle);

        public IEnumerable<Symbol> GetSymbols()
        {
            ulong lenght = EntropyLibraryWrapper.GetSymbolsLenght(_handle);
            Symbol[] symbols = new Symbol[lenght];
            for (ulong i = 0; i < lenght; i++)
            {
                symbols[i] = new Symbol(EntropyLibraryWrapper.GetSymbolChar(_handle, i))
                {
                    Count = EntropyLibraryWrapper.GetSymbolCount(_handle, i),
                    Frecuency = EntropyLibraryWrapper.GetSymbolFrecuency(_handle, i)
                };
            }
            return symbols;
        }
        public static float Calculate(params float[] Probabilities)
        {
            IntPtr array = Marshal.AllocHGlobal(sizeof(float) * Probabilities.Length);
            float[] events = new float[Probabilities.Length];
            Marshal.Copy(events, 0, array, Probabilities.Length);
            float ent = (float)Math.Round((EntropyLibraryWrapper.Calculate(array, Probabilities.Length) * 100));
            if (ent > 100)
            {
                ent = 100;
            }
            return ent;
        }



        public void Dispose()
        {
            if (_handle != null && !_handle.IsInvalid)
            {
                _handle.Dispose();
            }
        }

        public void Release()
        {
            EntropyLibraryWrapper.ReleaseEntropyLibrary(_handle);
        }
    }
}
