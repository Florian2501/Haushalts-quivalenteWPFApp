using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace HaushaltsäquivalenteWPFApp
{
    class BlueDesign : Design
    {
        public BlueDesign() : base(background: (Color)ColorConverter.ConvertFromString("#b4daee"),
                                   foreground: (Color)ColorConverter.ConvertFromString("#000000"),
                                   tableHeader: (Color)ColorConverter.ConvertFromString("#19709d"),
                                   tableColumn1: (Color)ColorConverter.ConvertFromString("#5ac7ff"),
                                   tableColumn2: (Color)ColorConverter.ConvertFromString("#c6dbe5"),
                                   sideMenu: (Color)ColorConverter.ConvertFromString("#19709d"),
                                   barChart: (Color)ColorConverter.ConvertFromString("#c6dbe5"))
        {
        }
    }
}
