using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaktionslogik für DetailPersonWindow.xaml
    /// </summary>
    public partial class DetailPersonWindow : Window
    {
        public DetailPersonWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the Table Window loads this function will be executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowPosition.CenterWindowOnScreen(this);

            //set the Color of the Background and the Menu to the Color of the Theme
            this.Background = new SolidColorBrush(ColorTheme.design.Background);
        }
    }
}
