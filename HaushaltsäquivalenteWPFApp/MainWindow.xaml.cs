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
using System.IO;

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
            //center the Window
            WindowPosition.CenterWindowOnScreen(this);

            //check for a name and ask for one if there is no
            if (!PersonExists())
            {
                OpenFirstPersonWindow();
            }
            //check for a task and if there is none ask for one
            if (!TaskExists())
            {
                OpenFirstTaskWindow();
            }
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

        /// <summary>
        /// Opens a new Window where you can enter new Tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow();
            newTaskWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Opens a new Window of the NewPerson Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewPersonButton_Click(object sender, RoutedEventArgs e)
        {
            NewPersonWindow tableWindow = new NewPersonWindow();
            tableWindow.Show();
            this.Close();
        }
    
        /// <summary>
        /// This function checks wether there is at least a person in the file. It returns true if there are files with names and else false.
        /// </summary>
        /// <returns></returns>
        public bool PersonExists()
        {
            string names = "";
            try
            {
                names = File.ReadAllText(@"Data\Persons.txt");
            }
            catch(Exception)
            {
                MessageBox.Show("Die Datei Persons.txt existiert noch nicht.");
            }

            if (String.IsNullOrWhiteSpace(names))
            {
                return false;
            }
            else return true;
        }

        /// <summary>
        /// This Function opens a new FirstPersonWindow that asks for a name
        /// </summary>
        public void OpenFirstPersonWindow()
        {
            FirstPersonWindow firstPersonWindow = new FirstPersonWindow();
            firstPersonWindow.Show();
            this.Close();
        }

        /// <summary>
        /// This function checks wether there is at least a task in the file. It returns true if there are files with tasks and else false.
        /// </summary>
        /// <returns></returns>
        public bool TaskExists()
        {
            string task = "";
            try
            {
                task = File.ReadAllText(@"Data\Tasks.txt");
            }
            catch (Exception)
            {
                MessageBox.Show("Die Datei Tasks.txt existiert noch nicht.");
            }

            if (String.IsNullOrWhiteSpace(task))
            {
                return false;
            }
            else return true;
        }

        /// <summary>
        /// This Function opens a new FirstTaskWindow that asks for a name.
        /// </summary>
        public void OpenFirstTaskWindow()
        {
            FirstTaskWindow firstTaskWindow = new FirstTaskWindow();
            firstTaskWindow.Show();
            this.Close();
        }

        /// <summary>
        /// This function opens a new HelpWindow that explains the usage of the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
            this.Close();
        }

        /// <summary>
        /// This function opens a new PlannerWindow that can structure the calendar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlannerButton_Click(object sender, RoutedEventArgs e)
        {
            PlannerWindow plannerWindow = new PlannerWindow();
            plannerWindow.Show();
            this.Close();
        }
    }
}