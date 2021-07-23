using System;
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
    /// Interaktionslogik für EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        private int taskID;
        private NewTaskWindow parentWindow;

        public EditTaskWindow(int taskID, NewTaskWindow newTaskWindow)
        {
            InitializeComponent();
            this.taskID = taskID;
            this.parentWindow = newTaskWindow;
        }

        /// <summary>
        /// Load function that fills the TextBoxes with the infos of the selected taks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowPosition.CenterWindowOnScreen(this);
            //Sets the correct colors of the ColorTheme
            this.Background = new SolidColorBrush(ColorTheme.design.Background);
            this.Foreground = new SolidColorBrush(ColorTheme.design.Foreground);

            Task selectedTask = TaskList.Tasks[taskID - 1];
            TaskNameTextBox.Text = selectedTask.Name;
            TaskDescriptionTextBox.Text = selectedTask.Description;
            TaskPointsTextBox.Text = selectedTask.Value.ToString();
        }

        /// <summary>
        /// Writes back the changed values of the task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //Test the TextBoxes wether they are empty
            //if yes return and show a Message Box
            if (String.IsNullOrEmpty(TaskNameTextBox.Text))
            {
                MessageBox.Show("Es muss ein Aufgabenname vergeben sein!");
                return;
            }

            if (String.IsNullOrEmpty(TaskDescriptionTextBox.Text))
            {
                MessageBox.Show("Es muss eine Aufgabenbeschreibung vergeben sein!");
                return;
            }
            //Replace the ; with , because it could destroy the whole file format
            string name = TaskNameTextBox.Text.Replace(';', ',');
            string description = TaskDescriptionTextBox.Text.Replace(';', ',');
            int points = 0;
            try
            {
                points = Convert.ToInt32(TaskPointsTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Die Punkteanzahl war in einem ungültigen Format.\nDie Punktzahl muss größer als 0 sein.");
                return;
            }
            //if it comes here all fields are correctly filled, based on the design
            
            //creates the new line that will represent the new task in the File
            string editedTask = this.taskID.ToString() + ";" + name + ";" + description + ";" + points.ToString();

            string path = @"Data\Tasks.txt";

            //this string will be a list of all tasks
            string allTasks = "";

            //read in the tasks and store them in a string to write back
            using (StreamReader sr = new StreamReader(path))
            {
                string line = "";
                int id = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    //check for empty line and skip it
                    if (String.IsNullOrEmpty(line)) continue;

                    //check wether the line is the new edited line
                    if(id == this.taskID)
                    {
                        //add the edited line
                        allTasks += editedTask + "\n";
                    }
                    //if not simply add the old line
                    else
                    {
                        allTasks += line + "\n";
                    }

                    //Increase the id
                    id++;
                }
            }

            //automatically utf 8 encoded writing, only adds it to the end
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine(allTasks);
            }
            //Confirm that it worked out
            MessageBox.Show(name + " wurde eingetragen.");

            //Update the list of the parent window
            parentWindow.updateTaskList();

            //Close this window
            this.Close();
        }
    }
}
