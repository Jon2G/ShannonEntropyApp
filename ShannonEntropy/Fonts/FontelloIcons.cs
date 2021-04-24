using System;
using System.Collections.Generic;
using System.Text;
using ShannonEntropy.Fonts;
using Xamarin.Forms;

[assembly: ExportFont("fontello_1.ttf", Alias = FontelloIcons.Font)]
namespace ShannonEntropy.Fonts
{
    public static class FontelloIcons
    {
        public const string Photo = "\uE80D";
        public const string Hearth = "\uE800";
        public const string Globe = "\uE801";
        public const string Letter = "\uE802";
        public const string Feather = "\uE803";
        public const string SpeedMeter = "\uE804";
        public const string Font = "FontIcon";
    }
}
