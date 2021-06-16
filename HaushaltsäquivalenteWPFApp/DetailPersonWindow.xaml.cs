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

        /// <summary>
        /// The function for the detail button of the last 7 days. It calls the createDetails() function with the value 7.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Last7DaysButton_Click(object sender, RoutedEventArgs e)
        {
            createDetails(7);
        }

        /// <summary>
        /// The function for the detail button of the last 30 days. It calls the createDetails() function with the value 30.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Last30DaysButton_Click(object sender, RoutedEventArgs e)
        {
            createDetails(30);
        }

        /// <summary>
        /// The function for the detail button of the last 365 days. It calls the createDetails() function with the value 365.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Last365DaysButton_Click(object sender, RoutedEventArgs e)
        {
            createDetails(365);
        }

        /// <summary>
        /// Fills the grid int the DetailPersonWindow with the infos of the done tasks.
        /// </summary>
        /// <param name="days">Number of days, the details will be created in the grid.</param>
        private void createDetails(int days)
        {
            //get the list with the values of the last several times
            List<int> listOfLastTime = DataReader.GetListOfDoneTasks(days, this.name);
            //Clear the Grid before refilling it
            LastTimeGrid.Children.Clear();
            //make the headline up to date
            LastDaysHeadline.Text = "Details zur Verteilung der Aufgaben der letzten " + days.ToString() + " Tage:";

            //Add new row to the grid
            RowDefinition newRow = new RowDefinition();
            newRow.Height = GridLength.Auto;
            LastTimeGrid.RowDefinitions.Add(newRow);
            //Add the HEader of the grid in bold
            //add the name of the task header
            TextBlock nameHeader = new TextBlock();
            nameHeader.Text = "Aufgabenname";
            nameHeader.FontWeight = FontWeights.Bold;
            Grid.SetColumn(nameHeader, 0);
            Grid.SetRow(nameHeader, 0);
            //add it to the grid
            LastTimeGrid.Children.Add(nameHeader);
            //add the percent header
            TextBlock percentHeader = new TextBlock();
            percentHeader.Text = "Anteil";
            percentHeader.FontWeight = FontWeights.Bold;
            Grid.SetColumn(percentHeader, 1);
            Grid.SetRow(percentHeader, 0);
            //add it to the grid
            LastTimeGrid.Children.Add(percentHeader);
            //add the number header
            TextBlock numberHeader = new TextBlock();
            numberHeader.Text = "Anzahl";
            numberHeader.FontWeight = FontWeights.Bold;
            Grid.SetColumn(numberHeader, 2);
            Grid.SetRow(numberHeader, 0);
            //add it to the grid
            LastTimeGrid.Children.Add(numberHeader);
            //Fill the table with the values of the tasks
            for (int i = 0; i < listOfLastTime.Count; i++)
            {
                //add new row to the grid
                newRow = new RowDefinition();
                newRow.Height = GridLength.Auto;
                LastTimeGrid.RowDefinitions.Add(newRow);
                //add the current name of the task
                TextBlock nameBlock = new TextBlock();
                nameBlock.Text = TaskList.Tasks[i].Name;
                nameBlock.TextWrapping = TextWrapping.Wrap;
                Grid.SetColumn(nameBlock, 0);
                Grid.SetRow(nameBlock, i + 1);

                LastTimeGrid.Children.Add(nameBlock);
                //add the percentage of the current task
                TextBlock percentBlock = new TextBlock();
                percentBlock.Text = ((double)listOfLastTime[i]/(double)sumUpDays(listOfLastTime)*100.0).ToString("F2") + "%";//add to divide it by the sum of done tasks
                Grid.SetColumn(percentBlock, 1);
                Grid.SetRow(percentBlock, i + 1);

                LastTimeGrid.Children.Add(percentBlock);
                //add the total number of times the task was done
                TextBlock numberBlock = new TextBlock();
                numberBlock.Text = listOfLastTime[i].ToString();
                Grid.SetColumn(numberBlock, 2);
                Grid.SetRow(numberBlock, i + 1);

                LastTimeGrid.Children.Add(numberBlock);
            }
            /////////////////////////////////////////////////////////////TODO///////////////////////////////////////
            ///Perhaps sort the table
            ///Add a graphical system for it
        }

        /// <summary>
        /// This function sums up the int values of a int list. In this class it is for summing up the number of done tasks for calculating the percents in the createDetails() function.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Sum of the int values of a int list.</returns>
        private int sumUpDays(List<int> list)
        {
            int sum = 0;
            foreach(int i in list)
            {
                sum += i;
            }
            return sum;
        }
    }
}
