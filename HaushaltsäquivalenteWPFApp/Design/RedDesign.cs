using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace HaushaltsäquivalenteWPFApp
{
    class RedDesign : Design
    {
        public RedDesign() : base(background: (Color)ColorConverter.ConvertFromString("#ffeded"),
                                  foreground: (Color)ColorConverter.ConvertFromString("#000000"),
                                  tableHeader: (Color)ColorConverter.ConvertFromString("#ac2d2d"),
                                  tableColumn1: (Color)ColorConverter.ConvertFromString("#f53f26"),
                                  tableColumn2: (Color)ColorConverter.ConvertFromString("#ffa598"),
                                  sideMenu: (Color)ColorConverter.ConvertFromString("#ac2d2d"))
        {
        }
    }
}
