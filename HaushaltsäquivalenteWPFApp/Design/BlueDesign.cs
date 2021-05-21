using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace HaushaltsäquivalenteWPFApp
{
    class BlueDesign : Design
    {
        public BlueDesign() : base((Color)ColorConverter.ConvertFromString("#b4daee"), (Color)ColorConverter.ConvertFromString("#19709d"), (Color)ColorConverter.ConvertFromString("#5ac7ff"), (Color)ColorConverter.ConvertFromString("#c6dbe5"), (Color)ColorConverter.ConvertFromString("#00405f"))
        {
        }
    }
}
