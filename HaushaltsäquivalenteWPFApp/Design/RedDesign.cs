using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace HaushaltsäquivalenteWPFApp
{
    class RedDesign : Design
    {
        public RedDesign() : base((Color)ColorConverter.ConvertFromString("#ffeded"), (Color)ColorConverter.ConvertFromString("#ac2d2d"), (Color)ColorConverter.ConvertFromString("#f53f26"), (Color)ColorConverter.ConvertFromString("#ffa598"), (Color)ColorConverter.ConvertFromString("#ac2d2d"))
        {
        }
    }
}
