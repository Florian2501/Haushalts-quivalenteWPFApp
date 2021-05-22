using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The function behind the Table Button to open the TableWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            TableWindow tableWindow = new TableWindow();
            tableWindow.Show();
            this.Close();
        }
        
        /// <summary>
        /// The function that will be executed when the window gets loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Set the Colors of the Back and Foreground
            this.Background = new SolidColorBrush(ColorTheme.design.Background);
            this.Foreground = new SolidColorBrush(ColorTheme.design.Foreground);

            //Check which design is the current design and choose it as the selected one in the ComboBox
            //Red
            if (ColorTheme.design is RedDesign)
            {
                ColorSelection.SelectedItem = ColorSelection.Items[1];
            }
            //Blue
            else if (ColorTheme.design is BlueDesign)
            {
                ColorSelection.SelectedItem = ColorSelection.Items[2];
            }
            //default
            else
            {
                ColorSelection.SelectedItem = ColorSelection.Items[0];
            }
        }

        /// <summary>
        /// Ends the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The function that will be executed when a item of the Color combobox is selscted. It changes the whole color theme to this.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorSelection_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //When the combobox selction changes it will change the Colortheme of the App to:
            switch (ColorSelection.SelectedIndex)
            {
                case 1://The red design
                    ColorTheme.design = new RedDesign();
                    break;
                case 2://The blue design
                    ColorTheme.design = new BlueDesign();
                    break;
                default://The default design
                    ColorTheme.design = new DefaultDesign();
                    break;
            }
            this.Background = new SolidColorBrush(ColorTheme.design.Background);
            this.Foreground = new SolidColorBrush(ColorTheme.design.Foreground);
        }

        /// <summary>
        /// Opens the TaskWindow and closes this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoneTasksButton_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow taskWindow = new TaskWindow();
            taskWindow.Show();
            this.Close();
        }
    }
}