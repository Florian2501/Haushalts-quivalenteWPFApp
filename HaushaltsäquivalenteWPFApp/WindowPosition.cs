using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// A class with functions to position a given window.
    /// </summary>
    public static class WindowPosition
    {
        /// <summary>
        /// This function centers the given window on the screen.
        /// </summary>
        /// <param name="window">This Window will be centered.</param>
        public static void CenterWindowOnScreen(Window window)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = window.Width;
            double windowHeight = window.Height;
            window.Left = (screenWidth / 2) - (windowWidth / 2);
            window.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
