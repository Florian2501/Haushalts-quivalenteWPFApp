using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace HaushaltsäquivalenteWPFApp
{
    class DefaultDesign : Design
    {
        public DefaultDesign() : base((Color)ColorConverter.ConvertFromString("#bfbfbf"), (Color)ColorConverter.ConvertFromString("#7a7a7a"), (Color)ColorConverter.ConvertFromString("#bdbdbd"), (Color)ColorConverter.ConvertFromString("#ebebeb"), (Color)ColorConverter.ConvertFromString("#7a7a7a"))
        {
        }

    }
}
