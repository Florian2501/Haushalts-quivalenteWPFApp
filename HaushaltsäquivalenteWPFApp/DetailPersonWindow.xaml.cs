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
        //Constructor
        public DetailPersonWindow(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        //Members
        private string name;

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

            NameHeadline.Text = this.name;

            int WeekSum = 0;
            for(int i=0; i<7; i++)
            {
                WeekSum += DataReader.GetValueOf(this.name, DateTime.Today.AddDays(-1 * i));
            }


            int MonthSum = 0;
            for (int i = 0; i < 30; i++)
            {
                MonthSum += DataReader.GetValueOf(this.name, DateTime.Today.AddDays(-1 * i));
            }

            int YearSum = 0;
            for (int i = 0; i < 365; i++)
            {
                YearSum += DataReader.GetValueOf(this.name, DateTime.Today.AddDays(-1 * i));
            }

            LastWeek.Text = WeekSum.ToString();
            AverageLastWeek.Text = (WeekSum / 7.0).ToString("F2");
            LastMonth.Text = MonthSum.ToString();
            AverageLastMonth.Text = (MonthSum / 30.0).ToString("F2");
            LastYear.Text = YearSum.ToString();
            AverageLastYear.Text = (YearSum / 365.0).ToString("F2");

        }
    }
}
