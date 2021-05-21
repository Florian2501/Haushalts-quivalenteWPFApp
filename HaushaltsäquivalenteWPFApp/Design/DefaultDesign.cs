using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace HaushaltsäquivalenteWPFApp
{
    class DefaultDesign : Design
    {
        public DefaultDesign() : base((Color)ColorConverter.ConvertFromString("#8B8378"), (Color)ColorConverter.ConvertFromString("#8B8378"), (Color)ColorConverter.ConvertFromString("#8B8378"), (Color)ColorConverter.ConvertFromString("#7B8378"), (Color)ColorConverter.ConvertFromString("#7B8378"))
        {
        }

    }
}
