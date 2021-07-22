using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaktionslogik für TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public TaskWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// CLoses the current TableWindow and opens the Main Window again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Sets the correct Design of the Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowPosition.CenterWindowOnScreen(this);

            //Sets the correct colors of the ColorTheme
            this.Background = new SolidColorBrush(ColorTheme.design.Background);
            this.Foreground = new SolidColorBrush(ColorTheme.design.Foreground);
            SideMenu.Background = new SolidColorBrush(ColorTheme.design.SideMenu);

            //Loads the Names of all persons to the PersonCB
            foreach(string name in Persons.Names)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = name;
                PersonComboBox.Items.Add(comboBoxItem);
            }

            //Loads the Names of all Tasks to the TasksCB
            foreach(Task task in TaskList.Tasks)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = task.Name;
                TaskComboBox.Items.Add(comboBoxItem);
            }
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
        /// This function is executed when the TaskCB value changes and then updates the point value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = TaskComboBox.SelectedIndex;
            if (index >= 0)
            {
                PointsTextBox.Text = TaskList.Tasks[index].Value.ToString();
                DescriptionTextBlock.Text = TaskList.Tasks[index].Name + ":\n" + TaskList.Tasks[index].Description;
            }
        }

        /// <summary>
        /// This Function wll be executed after clicking on the cofirm button and will write the done task in the fitting file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //Check wether both Comboboxes have a selection
            if(PersonComboBox.SelectedIndex >=0 && TaskComboBox.SelectedIndex >= 0)
            {
                string name = Persons.Names[PersonComboBox.SelectedIndex];
                //MessageBox.Show("Name: " + name);
                Task doneTask = TaskList.Tasks[TaskComboBox.SelectedIndex];
                //write back the data
                DataWriter.EnterDoneTask(doneTask, name, DateTime.Today);
            }
            //If not a value in every CB: error message
            else
            {
                MessageBox.Show("Es müssen sowohl eine Person, als auch eine Aufgabe ausgewählt sein!");
            }
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
    }
}
