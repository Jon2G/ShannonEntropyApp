using MoyskleyTech.ImageProcessing.Image;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Kit;
using Kit.Model;
using Xamarin.Forms;
using Point = MoyskleyTech.ImageProcessing.Image.Point;

namespace ShannonEntropy.Models
{
    public class ColorHystogram : ModelBase
    {
        public FileImageSource HystogramPhoto { get; set; }
        public ColorHystogram()
        {

        }

        internal async Task Calculate(Image<Pixel> result, Func<int, int, byte> GetByte, Pixel Color)
        {
            await Task.Yield();
            var Hystogram = new int[256];
            int MaxValue = 0;
            for (int i = 0; i < result.Width; i++)
            {
                for (int j = 0; j < result.Height; j++)
                {
                    int value = GetByte.Invoke(i, j);
                    Hystogram[value]++;
                    if (MaxValue < Hystogram[value])
                        MaxValue = Hystogram[value];
                }
            }

            int histHeight = 128;
            Bitmap img = new Bitmap(histHeight, 128);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(Pixels.Transparent);
                float fmax = (float)MaxValue;
                for (int i = 0; i < Hystogram.Length; i++)
                {
                    var pct = Hystogram[i] / fmax;   // What percentage of the max is this value?
                    if (pct >= 0.8999)
                    {
                        continue;
                    }

                    g.DrawLine(Color,
                        new Point(i, img.Height),
                        new Point(i, img.Height - (int)(pct * histHeight))  // Use that percentage of the height
                    );
                }
            }

            string path = $"{Tools.Instance.TemporalPath}/{Guid.NewGuid():N}.png";
            var file = new FileInfo(path);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            img.Save(file.FullName);
            this.HystogramPhoto = (FileImageSource)ImageSource.FromFile(file.FullName);
            Raise(() => HystogramPhoto);
        }
    }
}
