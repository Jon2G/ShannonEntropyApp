using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using ImageProcessing.JPEGCodec;
using ImageProcessing.PNGCodec;
using ImageProcessing.TGACodec;
using Kit;
using MoyskleyTech.ImageProcessing.Image;
using ShannonEntropy.Models;
using Xamarin.Forms;

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
            uint lenght = EntropyLibraryWrapper.GetSymbolsLenght(_handle);
            Symbol[] symbols = new Symbol[lenght];
            for (uint i = 0; i < lenght; i++)
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
            Marshal.Copy(Probabilities, 0, array, Probabilities.Length);
            float ent = (float)Math.Round((EntropyLibraryWrapper.Calculate(array, Probabilities.Length) * 100));
            if (ent > 100)
            {
                ent = 100;
            }
            return ent;
        }
        public static async Task<PictureHystogram> CalculateEntropy(CachedImage FileInfo)
        {
            await Task.Yield();
            PictureHystogram hystogram = new PictureHystogram();
            try
            {
                var png = new Hjg.Pngcs.Chunks.PngChunkIHDR(new Hjg.Pngcs.ImageInfo(10, 10, 8, true));
                using (var memory = new MemoryStream(await FileInfo.GetImageAsJpgAsync()))
                {
                    memory.Position = 0;
                    BitmapFactory factory = new BitmapFactory();
                    factory.AddCodec(new BitmapCodec());
                    factory.AddCodec(new PngCodec());
                    factory.AddCodec(new JPEGCodec());
                    factory.AddCodec(new TGACodec());
                    using (Image<Pixel> result = factory.Decode(memory))
                    {
                        using (Image<Pixel> bitmap = result.GetBitmap(0, 0, result.Width, result.Height))
                        {
                            await hystogram.Red.Calculate(result, (x, y) => bitmap.Get(x, y).R, Pixels.Red);
                            await hystogram.Green.Calculate(result, (x, y) => bitmap.Get(x, y).G, Pixels.Green);
                            await hystogram.Blue.Calculate(result, (x, y) => bitmap.Get(x, y).B, Pixels.Blue);
                            hystogram.TotalEntropy = await EntropyOfImage(bitmap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "BarcodeDecoding.GetImage");
                await Tools.Instance.CustomMessageBox.Show(ex.Message);
            }
            return hystogram;
        }

        public static async Task<float> EntropyOfImage(Image<Pixel> bitmap)
        {
            await Task.Yield();
            byte[] rgbs = new byte[256];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Pixel pixel = bitmap.Get(i, j);
                    byte rgb8 = (byte)(((pixel.R / 32) << 5) + ((pixel.G / 32) << 2) + (pixel.B / 64));
                    rgbs[rgb8]++;
                }
            }

            ulong total = 0;
            for (int i = 0; i <256; i++)
            {
                total += rgbs[i];
            }
 
            float[] frecuency = new float[256];
            for (int i = 0; i < 256; i++)
            {
                frecuency[i] = (float)rgbs[i] / total;
            }

            float totalEntropy = 0;
            for (int i = 0; i < 256; i++)
            {
                float probability = frecuency[i];
                if (probability > 0)
                    totalEntropy += (float)(probability *Math.Log(1 / probability,2));
            }
            return totalEntropy;
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
