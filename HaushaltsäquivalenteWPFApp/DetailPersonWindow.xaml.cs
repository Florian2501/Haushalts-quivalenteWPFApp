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
            List<int> listOfLastWeek = DataReader.GetListOfDoneTasks(7, name);


            int MonthSum = 0;
            for (int i = 0; i < 30; i++)
            {
                MonthSum += DataReader.GetValueOf(this.name, DateTime.Today.AddDays(-1 * i));
            }
            List<int> listOfLastMonth = DataReader.GetListOfDoneTasks(7, name);


            int YearSum = 0;
            for (int i = 0; i < 365; i++)
            {
                YearSum += DataReader.GetValueOf(this.name, DateTime.Today.AddDays(-1 * i));
            }
            List<int> listOfLastYear = DataReader.GetListOfDoneTasks(7, name);


            LastWeek.Text = WeekSum.ToString();
            AverageLastWeek.Text = (WeekSum / 7.0).ToString("F2");
            
            for(int i= 0; i<listOfLastWeek.Count;i++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = GridLength.Auto;
                LastWeekGrid.RowDefinitions.Add(newRow);

                TextBlock nameBlock = new TextBlock();
                nameBlock.Text = TaskList.Tasks[i].Name;
                Grid.SetColumn(nameBlock, 0);
                Grid.SetRow(nameBlock, i);

                LastWeekGrid.Children.Add(nameBlock);

                TextBlock percentBlock = new TextBlock();
                percentBlock.Text = listOfLastWeek[i].ToString();//add to divide it by the sum of done tasks
                Grid.SetColumn(percentBlock, 1);
                Grid.SetRow(percentBlock, i);

                LastWeekGrid.Children.Add(percentBlock);

                TextBlock numberBlock = new TextBlock();
                numberBlock.Text = listOfLastWeek[i].ToString();
                Grid.SetColumn(numberBlock, 2);
                Grid.SetRow(numberBlock, i);

                LastWeekGrid.Children.Add(numberBlock);
            }
            /////////////////////////////////////////////////////////////TODO///////////////////////////////////////
            ///Add this to all the others
            ///Add buttons to make it visible and the other two not visible
            ///Add the sum function for the done tasks and make the percents
            ///Perhaps sort the table
            ///Add a graphical system for it

            LastMonth.Text = MonthSum.ToString();
            AverageLastMonth.Text = (MonthSum / 30.0).ToString("F2");

            LastYear.Text = YearSum.ToString();
            AverageLastYear.Text = (YearSum / 365.0).ToString("F2");
        }
    }
}
