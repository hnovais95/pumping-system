using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PumpingSystem.Presentation
{
    internal static class ExtensionMethods
    {
        public static Color FromHex(this string hex) =>
            ColorTranslator.FromHtml(hex);
    }
}
