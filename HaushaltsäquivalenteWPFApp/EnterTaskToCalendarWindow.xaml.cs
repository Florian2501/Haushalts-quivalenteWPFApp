using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaktionslogik für EnterTaskToCalendarWindow.xaml
    /// </summary>
    public partial class EnterTaskToCalendarWindow : Window
    {
        public EnterTaskToCalendarWindow(string name, DateTime date)
        {
            InitializeComponent();
            this.NameOfPerson = name;
            this.Date = date;
        }

        //Porperties
        public string NameOfPerson { get; set; }
        public DateTime Date { get; set; }

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

            //Set the description text in the left menu
            DescriptionTextBlock.Text = "Hier kannst du die Aufgaben für " + this.NameOfPerson + " am " + this.Date.ToString("dd.MM.yy") + " verwalten.";

            //Set the Headline text
            HeadlineTextBlock.Text = this.NameOfPerson + " - " + this.Date.ToString("dd.MM.yy");
            //fill the list of the tasks of today
            updateCalendarTaskList();
            //fill the scrolldown menu
            fillScrollListOfTasks();
        }

        /// <summary>
        /// This function updates the list of Tasks on the top by reading the data from the files again
        /// </summary>
        public void updateCalendarTaskList()
        {
            //Clear the list
            ListOfTasks.Items.Clear();
            //Get the sorted List of Tasks
            List<CalendarTask> calendarTasks = DataReader.getTasksOfPersonOnDate(this.NameOfPerson, this.Date);
            //cancel if the list is empty
            if (calendarTasks == null) return;
            //Go through the list and add the tasks to the Scrollviewer
            foreach(CalendarTask task in calendarTasks)
            {
                ListOfTasks.Items.Add(task.Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + task.End.ToString("HH:mm", new CultureInfo("de-DE")) + " " + task.Name);
            }
        }

        /// <summary>
        /// This function fills the scrolldown menu with the existing tasks
        /// </summary>
        private void fillScrollListOfTasks()
        {
            //Loads the Names of all Tasks to the TasksCB
            foreach (Task task in TaskList.Tasks)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = task.Name;
                NameOfCurrentTask.Items.Add(comboBoxItem);
            }
        }

        /// <summary>
        /// This function gets called when a item in the task list gets selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOfTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListOfTasks.SelectedIndex;
            //Get the sorted List of Tasks
            List<CalendarTask> calendarTasks = DataReader.getTasksOfPersonOnDate(this.NameOfPerson, this.Date);
            if (index < 0 || index >= calendarTasks.Count)
            {
                //MessageBox.Show("Die Auswahl der Aufgabe ist ungültig!");
                return;
            }
            CalendarTask selectedTask = calendarTasks[index];

            //Set the correct task in the scrolldown menu that was selected in the time table
            NameOfCurrentTask.SelectedItem = NameOfCurrentTask.Items[selectedTask.ID - 1];
            //Set the starting time of the selected event
            StartHourOfCurrentTask.Text = selectedTask.Start.Hour.ToString("D2");
            StartMinuteOfCurrentTask.Text = selectedTask.Start.Minute.ToString("D2");
            //Set the ending time of the selected event
            EndHourOfCurrentTask.Text = selectedTask.End.Hour.ToString("D2");
            EndMinuteOfCurrentTask.Text = selectedTask.End.Minute.ToString("D2");
            //Set the description and the value of the selected task
            DescriptionOfCurrentTask.Text = selectedTask.Description;
            PointsOfCurrentTask.Text = selectedTask.Value.ToString();
        }

        /// <summary>
        /// This function gets called when the selection in the scrolldown menu gets chenged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameOfCurrentTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = NameOfCurrentTask.SelectedIndex;

            //Set the description and the value of the selected task
            DescriptionOfCurrentTask.Text = TaskList.Tasks[index].Description;
            PointsOfCurrentTask.Text = TaskList.Tasks[index].Value.ToString();
        }

        /// <summary>
        /// It deletes the selected CalendarTask of the ListBox from the file and the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ListOfTasks.SelectedIndex;

            
            //Get the sorted List of Tasks
            List<CalendarTask> calendarTasks = DataReader.getTasksOfPersonOnDate(this.NameOfPerson, this.Date);
            if (index < 0 || index >= calendarTasks.Count)
            {
                MessageBox.Show("Die Auswahl der Aufgabe ist ungültig!");
                return;
            }
            //Ask in a MessageBox if the task really should be deleted
            var dialogResult = MessageBox.Show("Die Aufgabe " + calendarTasks[index].Name + " ("+calendarTasks[index].Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + calendarTasks[index].End.ToString("HH:mm", new CultureInfo("de-DE")) + ") soll entfernt werden?", "Achtung!", MessageBoxButton.YesNo);
            //Cancel the event if the user clicks no
            if(dialogResult == System.Windows.MessageBoxResult.No)
            {
                return;
            }
            //Remove the selected task
            calendarTasks.Remove(calendarTasks[index]);
            //Get the file of the date without the line of the person
            string writeback = GetLinesWithoutCurrentPerson();
            //Add the name to the writeback data
            writeback += this.NameOfPerson;
            //Add the tasks with times to the writeback data
            foreach(CalendarTask task in calendarTasks)
            {
                writeback += ";" + task.ToString();
            }
            //create the task
            string path = @"Data/Calendar/" + this.Date.ToString("dd.MM.yy") + ".txt";

            //Write it back to the file
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine(writeback);
            }

            //update the list now without the task
            updateCalendarTaskList();

            //Set the starting time of the selected event
            StartHourOfCurrentTask.Text = "";
            StartMinuteOfCurrentTask.Text = "";
            //Set the ending time of the selected event
            EndHourOfCurrentTask.Text = "";
            EndMinuteOfCurrentTask.Text = "";
            //Set the description and the value of the selected task
            DescriptionOfCurrentTask.Text = "";
            PointsOfCurrentTask.Text = "";
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// This functions reads the date file and collects all lines, that are not from the current Person of the Window
        /// </summary>
        /// <returns></returns>
        private string GetLinesWithoutCurrentPerson()
        {
            //create the writeback line and the path
            string writeback = "";
            string path = @"Data/Calendar/" + this.Date.ToString("dd.MM.yy") + ".txt";

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = "";
                    //read the lines till the end
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Add every line that is not the current person
                        if (line.Split(';')[0] != this.NameOfPerson)
                        {
                            writeback += line + "\n";
                        }
                    }
                }
            }
            //catch exceptions
            catch (IOException)
            {
                MessageBox.Show("The Date file could not be read. It will be created now."); //if the date file could not be found or the person is not in the date file the default value is 0

                Directory.CreateDirectory(@"Data/Calendar");
                StreamWriter sw = new StreamWriter(path);
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Something with the Date file went wrong.");
            }

            return writeback;
        }

        /// <summary>
        /// This function gets called when the Checkbox gets filled and sets the times to 0-24
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
                //Set the time
                StartHourOfCurrentTask.Text = "00";
                StartMinuteOfCurrentTask.Text = "00";
                EndHourOfCurrentTask.Text = "24";
                EndMinuteOfCurrentTask.Text = "00";
                //Make it not changable
                StartHourOfCurrentTask.IsEnabled = false;
                StartMinuteOfCurrentTask.IsEnabled = false;
                EndHourOfCurrentTask.IsEnabled = false;
                EndMinuteOfCurrentTask.IsEnabled = false;
        }

        /// <summary>
        /// This function gets called when the Checkbox gets empty and enables the time TextBoxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //Make it changeable again
            StartHourOfCurrentTask.IsEnabled = true;
            StartMinuteOfCurrentTask.IsEnabled = true;
            EndHourOfCurrentTask.IsEnabled = true;
            EndMinuteOfCurrentTask.IsEnabled = true;
        }
    }
}
