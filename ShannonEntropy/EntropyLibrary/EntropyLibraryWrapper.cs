using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ShannonEntropy.EntropyLibrary
{
    public static class EntropyLibraryWrapper
    {
        const string DllName = "libEntropyLibrary";
        [DllImport(DllName)]
        public static extern string getTemplateInfo();

        [DllImport(DllName, EntryPoint = nameof(GetEntropyLibrary))]
        public static extern EntropyLibrarySafeHandle GetEntropyLibrary();

        [DllImport(DllName, EntryPoint = nameof(ReleaseEntropyLibrary))]
        public static extern void ReleaseEntropyLibrary(EntropyLibrarySafeHandle entropyLibrarySafeHandle);

        [DllImport(DllName, EntryPoint = nameof(GetTotalEntropy))]
        public static extern double GetTotalEntropy(EntropyLibrarySafeHandle handle);

        [DllImport(DllName, EntryPoint = nameof(ReadFromFile))]
        public static extern int ReadFromFile(EntropyLibrarySafeHandle handle, string FilePath);

        [DllImport(DllName, EntryPoint = nameof(GetSymbolsLenght))]
        public static extern uint GetSymbolsLenght(EntropyLibrarySafeHandle handle);

        [DllImport(DllName, EntryPoint = nameof(GetSymbolChar))]
        public static extern char GetSymbolChar(EntropyLibrarySafeHandle handle,ulong index);

        [DllImport(DllName, EntryPoint = nameof(GetSymbolCount))]
        public static extern int GetSymbolCount(EntropyLibrarySafeHandle handle, ulong index);

        [DllImport(DllName, EntryPoint = nameof(GetSymbolFrecuency))]
        public static extern float GetSymbolFrecuency(EntropyLibrarySafeHandle handle, ulong index);

        [DllImport(DllName, EntryPoint = nameof(Calculate))]
        public static extern float Calculate(IntPtr EventsProbability, int size);

    }
}
