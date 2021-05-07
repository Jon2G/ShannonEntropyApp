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
    public static class EntropyLibrary
    {
        private static Symbol FindSymbol(IEnumerable<Symbol> Symbols, int character)
        {
            int count = Symbols.Count();
            for (int i = 0; i < count; i++)
            {
                Symbol symbol = Symbols.ElementAt(i);
                if (symbol.Character == character)
                {
                    return symbol;
                }
            }
            return null;
        }

        public static Tuple<List<Symbol>,float> ReadFromFile(string file)
        {
            List<Symbol> Symbols = new List<Symbol>();
            using (FileStream fs = File.OpenRead(file))
            {
                while (true)
                {
                    int bit = fs.ReadByte();
                    if (bit <= 0) { break;}
                    var character = (char)bit;
                    if (character < 0) { break; }
                    Symbol symbol = FindSymbol(Symbols, character);
                    if (symbol is not null)
                    {
                        symbol.Count++;
                        continue;
                    }
                    Symbols.Add(new Symbol(character));
                }

            }
            Symbols.TrimExcess();
            return new Tuple<List<Symbol>, float>(Symbols,CalculateTotalEntropy(Symbols.ToArray()));
        }
        private static float CalculateTotalEntropy(Symbol[] Symbols)
        {
            float TotalEntropy = 0;
            uint TotalSymbols = 0;
            for (int i = 0; i < Symbols.Length; i++)
            {
                Symbol symbol = Symbols[i];
                TotalSymbols += symbol.Count;
            }

            for (int i = 0; i < Symbols.Length; i++)
            {
                Symbol symbol = Symbols[i];
                double count = symbol.Count;
                symbol.Frecuency = count / TotalSymbols;

                TotalEntropy += (float)(symbol.Frecuency *Math.Log(1 / symbol.Frecuency,2));
            }

            return (float) TotalEntropy;
        }

        public static float Calculate(params float[] Probabilities)
        {

            int size = Probabilities.Length;
            double totalEntropy = 0;
            for (int i = 0; i < size; i++)
            {
                float probability = Probabilities[i];
                if (probability > 0)
                    totalEntropy += (probability * Math.Log(1 / probability, 2));
            }

            float ent = (float)Math.Round((totalEntropy * 100));
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
            for (int i = 0; i < 256; i++)
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
                    totalEntropy += (float)(probability * Math.Log(1 / probability, 2));
            }
            return totalEntropy;
        }


    }
}
