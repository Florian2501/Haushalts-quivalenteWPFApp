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
        public EnterTaskToCalendarWindow(string name, DateTime date, PlannerWindow plannerWindow)
        {
            InitializeComponent();
            this.NameOfPerson = name;
            this.Date = date;
            this.plannerWindow = plannerWindow;
        }

        //Porperties
        public string NameOfPerson { get; set; }
        public DateTime Date { get; set; }
        public PlannerWindow plannerWindow { get; set; }

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
        /// This combines all ClaendarTasks of the day and sorts them
        /// </summary>
        /// <returns></returns>
        public List<CalendarTask> GetWeeklyAndDailyTasksSorted()
        {
            //Get the sorted List of Tasks
            List<CalendarTask> calendarTasks = DataReader.getTasksOfPersonOnDate(this.NameOfPerson, this.Date, false);

            List<CalendarTask> repeatingTasks = DataReader.getTasksOfPersonOnDate(this.NameOfPerson, this.Date, true);

            //cancel if the list is empty
            if (calendarTasks == null)
            {
                calendarTasks = repeatingTasks;
            }
            else
            {
                if (repeatingTasks != null) calendarTasks.AddRange(repeatingTasks);
            }

            if (calendarTasks == null) return null;


            //Sort the list beacuase it could be unsorted if there are weekly and daily tasks
            calendarTasks.Sort(CompareTasksByTime);
            return calendarTasks;
        }

        /// <summary>
        /// This function updates the list of Tasks on the top by reading the data from the files again
        /// </summary>
        public void updateCalendarTaskList()
        {
            //Clear the list
            ListOfTasks.Items.Clear();

            //get the list of all tasks
            List<CalendarTask> calendarTasks = GetWeeklyAndDailyTasksSorted();

            if (calendarTasks == null) return;

            //Go through the list and add the tasks to the Scrollviewer
            foreach (CalendarTask task in calendarTasks)
            {
                ListOfTasks.Items.Add(task.Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + task.End.ToString("HH:mm", new CultureInfo("de-DE")) + " " + task.Name);
            }
        }

        /// <summary>
        /// The sorting function for the tasklist
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CompareTasksByTime(CalendarTask x, CalendarTask y)
        {
            if (x.Start < y.Start)
            {
                return -1;
            }
            else if (x.Start == y.Start)
            {
                return 0;
            }
            else return 1;
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
            List<CalendarTask> calendarTasks = GetWeeklyAndDailyTasksSorted();

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

            //set the weekly checkbox
            if (selectedTask.IsWeekly)
            {
                WeeklyCheckBox.IsChecked = true;
            }
            else
            {
                WeeklyCheckBox.IsChecked = false;
            }
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
            List<CalendarTask> calendarTasks = GetWeeklyAndDailyTasksSorted();

            if (index < 0 || index >= calendarTasks.Count)
            {
                MessageBox.Show("Die Auswahl der Aufgabe ist ungültig!");
                return;
            }
            //Ask in a MessageBox if the task really should be deleted
            var dialogResult = MessageBox.Show("Die Aufgabe " + calendarTasks[index].Name + " ("+calendarTasks[index].Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + calendarTasks[index].End.ToString("HH:mm", new CultureInfo("de-DE")) + ") soll entfernt werden?" + ((calendarTasks[index].IsWeekly) ? " Die Aufgabe ist wöchentlich und wird aus jeder Woche entfernt!" : ""), "Achtung!", MessageBoxButton.YesNo);
            //Cancel the event if the user clicks no
            if(dialogResult == System.Windows.MessageBoxResult.No)
            {
                return;
            }
            //Remove the selected task
            calendarTasks.Remove(calendarTasks[index]);

            //Write the data to the file
            WriteBackCalendarData(calendarTasks);

            //update the list now without the task
            updateCalendarTaskList();

            //Set the starting time of the selected event
            StartHourOfCurrentTask.Text = "";
            StartMinuteOfCurrentTask.Text = "";
            //Set the ending time of the selected event
            EndHourOfCurrentTask.Text = "";
            EndMinuteOfCurrentTask.Text = "";

            //update the calendar
            this.plannerWindow.printCurrentWeek();
        }

        /// <summary>
        /// This changes the data of the selected existing task to the entered values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ListOfTasks.SelectedIndex;


            //Get the sorted List of Tasks
            List<CalendarTask> calendarTasks = GetWeeklyAndDailyTasksSorted();

            if (index < 0 || index >= calendarTasks.Count)
            {
                MessageBox.Show("Die Auswahl der Aufgabe ist ungültig!");
                return;
            }
            //Ask in a MessageBox if the task really should be changed
            var dialogResult = MessageBox.Show("Die Aufgabe " + calendarTasks[index].Name + " (" + calendarTasks[index].Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + calendarTasks[index].End.ToString("HH:mm", new CultureInfo("de-DE")) + ") soll zu den eingegebenen Daten geändert werden?", "Achtung!", MessageBoxButton.YesNo);
            //Cancel the event if the user clicks no
            if (dialogResult == System.Windows.MessageBoxResult.No)
            {
                return;
            }

            //initialize minute and hour values
            int startHour = 0;
            int startMinute = 0;
            int endHour = 0;
            int endMinute = 0;

            try
            {
                //Convert the TextBlocks
                startHour = Convert.ToInt32(StartHourOfCurrentTask.Text);
                startMinute = Convert.ToInt32(StartMinuteOfCurrentTask.Text);
                endHour = Convert.ToInt32(EndHourOfCurrentTask.Text);
                endMinute = Convert.ToInt32(EndMinuteOfCurrentTask.Text);
            }
            catch
            {
                MessageBox.Show("Die Uhrzeiten können nicht in Zahlen umgewandelt werden");
                return;
            }
            //Check if the values are in correct format
            if (startHour < 0 || startHour > 24 || endHour < 0 || endHour > 24)
            {
                MessageBox.Show("Die Stundenzahl muss zwischen 0 und 24 sein.");
                return;
            }

            if (startMinute < 0 || startMinute > 60 || endMinute < 0 || endMinute > 60)
            {
                MessageBox.Show("Die Minutenzahl muss zwischen 0 und 60 sein.");
                return;
            }
            //initialize the Date values
            DateTime Start, End;
            try
            {
                Start = Convert.ToDateTime(startHour.ToString("D2") + ":" + startMinute.ToString("D2"), new CultureInfo("de-DE"));
                End = Convert.ToDateTime(endHour.ToString("D2") + ":" + endMinute.ToString("D2"), new CultureInfo("de-DE"));
            }
            catch
            {
                MessageBox.Show("Es gab einen Fehler beim umwandeln der Zeiten.");
                return;
            }
            //Check if the starting time if before ending time
            if (End < Start)
            {
                MessageBox.Show("Die Startzeit kann nicht nach der Endzeit liegen.");
                return;
            }
            //Get the index of the selected task
            int selectedTaskIndex = NameOfCurrentTask.SelectedIndex;
            //check if a task is selected
            if (selectedTaskIndex < 0 || selectedTaskIndex >= TaskList.Tasks.Count)
            {
                MessageBox.Show("Es muss eine Aufgabe ausgewählt werden.");
                return;
            }


            //if a task was selected, get this task
            Task task = TaskList.Tasks[selectedTaskIndex];

            //get whether it is weekly
            bool weekly = (bool) WeeklyCheckBox.IsChecked;

            //make the task to a calendarTask with the date times
            CalendarTask calendarTask = new CalendarTask(task.ID, Start, End, weekly);

            if(calendarTask.IsWeekly && !calendarTasks[index].IsWeekly)
            {
                //Ask in a MessageBox if it should be changed from not weekly to weekly
                var result = MessageBox.Show("Die Aufgabe " + calendarTasks[index].Name + " (" + calendarTasks[index].Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + calendarTasks[index].End.ToString("HH:mm", new CultureInfo("de-DE")) + ") soll zu wöchentlich geändert werden? Sie wird dann jede Woche " + this.Date.DayOfWeek.ToString() + " auftauchen.", "Achtung!", MessageBoxButton.YesNo);
                //Cancel the event if the user clicks no
                if (result == System.Windows.MessageBoxResult.No)
                {
                    return;
                }
            }
            else if (!calendarTask.IsWeekly && calendarTasks[index].IsWeekly)
            {
                //Ask in a MessageBox if it should be changed from not weekly to weekly
                var result = MessageBox.Show("Die Aufgabe " + calendarTasks[index].Name + " (" + calendarTasks[index].Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + calendarTasks[index].End.ToString("HH:mm", new CultureInfo("de-DE")) + ") soll nicht mehr wöchentlich sein? Sie wird dann nicht mehr jede Woche " + this.Date.DayOfWeek.ToString() + " auftauchen.", "Achtung!", MessageBoxButton.YesNo);
                //Cancel the event if the user clicks no
                if (result == System.Windows.MessageBoxResult.No)
                {
                    return;
                }
            }

            //replace the old task with the new infos
            calendarTasks[index] = calendarTask;

            //Write the data to the file
            WriteBackCalendarData(calendarTasks);

            //update the list now without the task
            updateCalendarTaskList();

            //Set the starting time of the selected event
            StartHourOfCurrentTask.Text = "";
            StartMinuteOfCurrentTask.Text = "";
            //Set the ending time of the selected event
            EndHourOfCurrentTask.Text = "";
            EndMinuteOfCurrentTask.Text = "";

            //update the calendar
            this.plannerWindow.printCurrentWeek();
        }

        /// <summary>
        /// Saves the input values as new task in the files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            //initialize minute and hour values
            int startHour = 0;
            int startMinute = 0;
            int endHour = 0;
            int endMinute = 0;

            try 
            {
                //Convert the TextBlocks
                startHour = Convert.ToInt32(StartHourOfCurrentTask.Text);
                startMinute = Convert.ToInt32(StartMinuteOfCurrentTask.Text);
                endHour = Convert.ToInt32(EndHourOfCurrentTask.Text);
                endMinute = Convert.ToInt32(EndMinuteOfCurrentTask.Text);
            }
            catch
            {
                MessageBox.Show("Die Uhrzeiten können nicht in Zahlen umgewandelt werden");
                return;
            }
            //Check if the values are in correct format
            if(startHour<0 || startHour>24 || endHour<0 || endHour > 24)
            {
                MessageBox.Show("Die Stundenzahl muss zwischen 0 und 24 sein.");
                return;
            }

            if (startMinute < 0 || startMinute > 60 || endMinute < 0 || endMinute > 60)
            {
                MessageBox.Show("Die Minutenzahl muss zwischen 0 und 60 sein.");
                return;
            }
            //initialize the Date values
            DateTime Start, End;
            try
            {
                Start = Convert.ToDateTime(startHour.ToString("D2") + ":" + startMinute.ToString("D2"), new CultureInfo("de-DE"));
                End = Convert.ToDateTime(endHour.ToString("D2") + ":" + endMinute.ToString("D2"), new CultureInfo("de-DE"));
            }
            catch
            {
                MessageBox.Show("Es gab einen Fehler beim umwandeln der Zeiten.");
                return;
            }
            //Check if the starting time if before ending time
            if (End < Start)
            {
                MessageBox.Show("Die Startzeit kann nicht nach der Endzeit liegen.");
                return;
            }
            //Get the index of the selected task
            int index = NameOfCurrentTask.SelectedIndex;
            //check if a task is selected
            if(index<0 || index >= TaskList.Tasks.Count)
            {
                MessageBox.Show("Es muss eine Aufgabe ausgewählt werden.");
                return;
            }
            //if a task was selected, get this task
            Task task = TaskList.Tasks[index];

            //check if it is weekly
            bool weekly = (bool) WeeklyCheckBox.IsChecked;

            //make the task to a calendarTask with the date times
            CalendarTask calendarTask = new CalendarTask(task.ID, Start, End, weekly);
            //get all the other tasks that already exist
            List<CalendarTask> calendarTasks = GetWeeklyAndDailyTasksSorted();
            //if there are no tasks initialize the list
            if(calendarTasks == null)
            {
                calendarTasks = new List<CalendarTask>();
            }
            //add the new CalendarTask to the list
            calendarTasks.Add(calendarTask);
            //write the list to the files
            WriteBackCalendarData(calendarTasks);
            //update the list with the new task
            updateCalendarTaskList();

            //Set the starting time of the selected event
            StartHourOfCurrentTask.Text = "";
            StartMinuteOfCurrentTask.Text = "";
            //Set the ending time of the selected event
            EndHourOfCurrentTask.Text = "";
            EndMinuteOfCurrentTask.Text = "";

            //update the calendar
            this.plannerWindow.printCurrentWeek();
        }

        /// <summary>
        /// Writes the data back to the file. It only changes the line of the current person of this Window
        /// </summary>
        /// <param name="calendarTasks"></param>
        private void WriteBackCalendarData(List<CalendarTask> calendarTasks)
        {
            /////////////////////////////////////////////////////////One day tasks
            //Get the file of the date without the line of the person from the tasks that are only on one day
            string writeback = GetLinesWithoutCurrentPerson(false);
            //Add the name to the writeback data
            writeback += this.NameOfPerson;
            //Add the tasks with times to the writeback data
            foreach (CalendarTask task in calendarTasks)
            {
                if(!task.IsWeekly) writeback += ";" + task.ToString();
            }
            //create the task
            string path = @"Data/Calendar/" + this.Date.ToString("dd.MM.yy") + ".txt";

            //Write it back to the file
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine(writeback);
            }

            //////////////////////////////////////////////////////////Weekly tasks
            //Get the file of the date without the line of the person from the tasks that are weekly
            writeback = GetLinesWithoutCurrentPerson(true);
            //Add the name to the writeback data
            writeback += this.NameOfPerson;
            //Add the tasks with times to the writeback data
            foreach (CalendarTask task in calendarTasks)
            {
                if (task.IsWeekly) writeback += ";" + task.ToString();
            }
            //create the task
            path = @"Data/WeeklyTasks/" + this.Date.DayOfWeek.ToString();

            //Write it back to the file
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine(writeback);
            }

        }

        /// <summary>
        /// This functions reads the date file and collects all lines, that are not from the current Person of the Window
        /// </summary>
        /// <returns></returns>
        private string GetLinesWithoutCurrentPerson(bool weekly)
        {
            //create the writeback line and the path
            string writeback = "";
            string path = @"Data/" + ((weekly) ? (@"WeeklyTasks/" + this.Date.DayOfWeek.ToString()) : (@"Calendar/" + this.Date.ToString("dd.MM.yy") + ".txt"));

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

                Directory.CreateDirectory(@"Data/" + ((weekly) ? "WeeklyTasks" : "Calendar"));
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
                EndHourOfCurrentTask.Text = "23";
                EndMinuteOfCurrentTask.Text = "59";
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

        /// <summary>
        /// This saves the selected task for this person
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterTaskButton_Click(object sender, RoutedEventArgs e)
        {
            int index = ListOfTasks.SelectedIndex;

            //Get the sorted List of Tasks
            List<CalendarTask> calendarTasks = GetWeeklyAndDailyTasksSorted();

            if (index < 0 || index >= calendarTasks.Count)
            {
                MessageBox.Show("Die Auswahl der Aufgabe ist ungültig!");
                return;
            }
            //Ask in a MessageBox if the task really should be deleted
            var dialogResult = MessageBox.Show("Die Aufgabe " + calendarTasks[index].Name + " (" + calendarTasks[index].Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + calendarTasks[index].End.ToString("HH:mm", new CultureInfo("de-DE")) + ") soll für " + this.NameOfPerson + " eingetragen werden?", "Achtung!", MessageBoxButton.YesNo);
            //Cancel the event if the user clicks no
            if (dialogResult == System.Windows.MessageBoxResult.No)
            {
                return;
            }
            //Get the selected task
            CalendarTask selectedTask = calendarTasks[index];
            //write back the selected task
            DataWriter.EnterDoneTask(new Task(selectedTask.ID), this.NameOfPerson, this.Date);
        }
    }
}
