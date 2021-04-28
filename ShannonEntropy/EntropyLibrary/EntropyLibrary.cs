using System;
using System.Collections.Generic;
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
