using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace HaushaltsäquivalenteWPFApp
{
    class DefaultDesign : Design
    {
        public DefaultDesign() : base(background: (Color)ColorConverter.ConvertFromString("#bfbfbf"),
                                      foreground: (Color)ColorConverter.ConvertFromString("#000000"),
                                      tableHeader: (Color)ColorConverter.ConvertFromString("#7a7a7a"),
                                      tableColumn1: (Color)ColorConverter.ConvertFromString("#bdbdbd"),
                                      tableColumn2: (Color)ColorConverter.ConvertFromString("#ebebeb"),
                                      sideMenu: (Color)ColorConverter.ConvertFromString("#7a7a7a"),
                                      barChart: (Color)ColorConverter.ConvertFromString("#ebebeb"))
        {
        }

    }
}
