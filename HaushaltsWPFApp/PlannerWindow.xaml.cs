using System;
using System.Windows;
using System.Windows.Media;
using System.Net.Mail;
using Ical.Net;
using Ical.Net.Serialization;
using Ical.Net.DataTypes;
using Ical.Net.CalendarComponents;
using System.IO;
using System.Windows.Controls;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// A special class to make it possible to give the information of the date to the function that calls the new EnterTaskToCalendarWindow
    /// </summary>
    class DateBorder : Border
    {
        //Constructor
        public DateBorder(DateTime date, string name) : base()
        {
            this.Date = date;
            this.NameOfPerson = name;
        }

        //Property
        public DateTime Date { get; set; }
        public string NameOfPerson { get; set; }
    }

    /// <summary>
    /// Interaktionslogik für PlannerWindow.xaml
    /// </summary>
    public partial class PlannerWindow : Window
    {
        public PlannerWindow()
        {
            InitializeComponent();

            lastMonday = getLastMonday();
            currentMonday = lastMonday;
        }

        private DateTime lastMonday;
        private DateTime currentMonday;
        private const string PathOfCalendarFile = @"Data/calendar.ics";

        /// <summary>
        /// Sends the calendar data as ics File to the given Mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //create a new calendar
            Ical.Net.Calendar calendar = new Ical.Net.Calendar();
            //Name the calendar
            //calendar.Name = "Kalendar - Haushaltsapp";
            //get the index of the combobox with names
            int index = ListOfNames.SelectedIndex;

            if(index == 0)//index 0 means "all"
            {
                foreach(string name in Persons.Names)
                {
                    //write the regular tasks in the calendar
                    EnterRegularTasksInCalendar(calendar, name);
                    //write the weekly tasks in the calendar
                    EnterWeeklyTasksInCalendar(calendar, name);
                }
            }
            else
            {
                //Get the selected name
                string name = Persons.Names[index - 1];
                //Write the regular tasks in the calendar
                EnterRegularTasksInCalendar(calendar, name);
                //write the weekly tasks in the calendar
                EnterWeeklyTasksInCalendar(calendar, name);
            }

            //Check if the calendar isnt empty
            if (calendar.Events.Count == 0)
            {
                MessageBox.Show("Der Kalendar, der gesendet werden soll ist leer. Das Senden wird deshalb abgebrochen.");
                return;
            }

            //Get the adress from the text block
            string mailAdress = MailAdressTextBox.Text;

            using (StreamWriter sw = new StreamWriter(PathOfCalendarFile))
            {
                //Write the calendar into a ics file
                sw.AutoFlush = true;
                var serializer = new CalendarSerializer(new SerializationContext());
                var serializedCalendar = serializer.SerializeToString(calendar);
                sw.WriteLine(serializedCalendar);
            }

            //Starts a new Thread where it sends the Mail, so you can click around while waiting for the mail
            ThreadPool.QueueUserWorkItem(sendCalendar, mailAdress);
        }

        /// <summary>
        /// Gets all tasks of a person that are in the future and adds them to the given calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="name"></param>
        public void EnterRegularTasksInCalendar(Ical.Net.Calendar calendar, string name)
        {
            //set a date that is today
            DateTime now = DateTime.Now;
            //Check wether a file is not in the correct format or older than one year and delete it if yes
            string path = @"Data\Calendar";
            //Get the files from the data directory
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            foreach (FileInfo file in fileInfos)
            {
                //Extract the date out of the file name and try to convert it
                DateTime date = new DateTime();
                string[] day = file.Name.Split(".");
                try
                {
                    date = Convert.ToDateTime(day[0] + "." + day[1] + "." + day[2]);
                }
                //Deltete if its not in the correct format
                catch (Exception)
                {
                    MessageBox.Show(file.Name + " konnte nicht gelesen werden und wird deshalb gelöscht.");
                    File.Delete(file.FullName);
                    continue;
                }

                //if the date is upcoming
                if (date >= now.AddDays(-1))
                {
                    //Get the tasks of the certain date and person
                    List<CalendarTask> tasksOfDay = DataReader.getTasksOfPersonOnDate(name, date, false);
                    //check if there are tasks
                    if (tasksOfDay != null)
                    {
                        //if yes go through the tasks 
                        foreach (CalendarTask task in tasksOfDay)
                        {
                            //get the times of the task
                            DateTime start = new DateTime(date.Year, date.Month, date.Day, task.Start.Hour, task.Start.Minute, 0);
                            DateTime end = new DateTime(date.Year, date.Month, date.Day, task.End.Hour, task.End.Minute, 0);

                            //add the task to the calendar
                            calendar.Events.Add(new CalendarEvent
                            {
                                Class = "PUBLIC",
                                Summary = name + ": " + task.Name,
                                Created = new CalDateTime(now),
                                Description = task.Description,
                                Start = new CalDateTime(start),
                                End = new CalDateTime(end),
                                Sequence = 0,
                                Uid = Guid.NewGuid().ToString(),
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the tasks that are weekly for a given person and enter them in the given calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="name"></param>
        public void EnterWeeklyTasksInCalendar(Ical.Net.Calendar calendar, string name)
        {
            //Get a now value
            DateTime now = DateTime.Now;
            //go through every day of the week
            for (int i = 0; i < 7; i++)
            {
                DateTime date = DateTime.Now.AddDays(i);
                //init the week
                List<CalendarTask> calendarTasks = new List<CalendarTask>();

                List<CalendarTask> weeklyTasks = DataReader.getTasksOfPersonOnDate(name, date, true);
                //add the tasks to the list if they exist
                if (weeklyTasks != null)
                {
                    calendarTasks.AddRange(weeklyTasks);
                }
                //go through the tasks and add them to the calendar
                foreach (CalendarTask task in calendarTasks)
                {
                    //get starting and end time of the tasks
                    DateTime start = new DateTime(date.Year, date.Month, date.Day, task.Start.Hour, task.Start.Minute, 0);
                    DateTime end = new DateTime(date.Year, date.Month, date.Day, task.End.Hour, task.End.Minute, 0);
                    //Create the pattern to repeat the task every week for the next year
                    RecurrencePattern recurrence = new RecurrencePattern
                    {
                        Frequency = FrequencyType.Weekly,
                        Interval = 1,
                        ByDay = new List<WeekDay>
                                        {
                                            new WeekDay
                                            {
                                                DayOfWeek = date.DayOfWeek
                                            }
                                        },
                        Until = DateTime.Now.AddYears(1)
                    };

                    calendar.Events.Add(new CalendarEvent
                    {
                        Class = "PUBLIC",
                        Summary = name + ": " + task.Name,
                        Created = new CalDateTime(now),
                        Description = task.Description,
                        Start = new CalDateTime(start),
                        End = new CalDateTime(end),
                        Sequence = 0,
                        Uid = Guid.NewGuid().ToString(),
                        RecurrenceRules = new List<RecurrencePattern>() { recurrence }
                    });
                }
            }
        }

        /// <summary>
        /// This function sends the file of the given path to the given receiver mail adress
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="path"></param>
        private void sendCalendar(object adressObject)
        {

            string adress = (string) adressObject;
            MailMessage email = new MailMessage();
            email.From = new MailAddress("haushaltsappmail@gmail.com", "Haushaltsapp Mailsender");

            try
            {
                email.To.Add(adress);//email.To.Add(ReceiverTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Die Email-Adresse war in einem falschen Format!", "Achtung!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            email.Subject = "Haushaltsaufgaben Kalender";
            email.Body = "Im Anhang befindet sich die Kalender Datei, welche Sie einfach in Ihr Kalenderprogramm einbinden können.";

            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(PathOfCalendarFile);
            email.Attachments.Add(attachment);

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            try
            {

                client.EnableSsl = true;

                client.UseDefaultCredentials = false;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Credentials = new System.Net.NetworkCredential("haushaltsappmail@gmail.com", "uuwczzfriicsovcf");


                client.Send(email);
                MessageBox.Show("Gesendet!", "Infofenster", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Das Senden ist fehlgeschlagen: " + ex.Message + ex.ToString());
            }
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

            //Create a new Row
            RowDefinition DateRow = new RowDefinition();
            //add the new row to the grid
            CalendarGrid.RowDefinitions.Add(DateRow);
            
            //call the print function for the current week through giving it the last monday
            printCurrentWeek();

            //Fill the combo box with the names of all persons that could receive a message
            fillListOfNamesForMailing();
            
            //Check wether a file is not in the correct format or older than one year and delete it if yes
            string path = @"Data\Calendar";
            //Get the file sfrom the data directory
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            foreach (FileInfo file in fileInfos)
            {
                //Extract the date out of the file name and try to convert it
                DateTime date = new DateTime();
                string[] day = file.Name.Split(".");
                try
                {
                    date = Convert.ToDateTime(day[0] + "." + day[1] + "." + day[2]);
                }
                //Deltete if its not in the correct format
                catch (Exception)
                {
                    MessageBox.Show(file.Name + " konnte nicht gelesen werden und wird deshalb gelöscht.");
                    File.Delete(file.FullName);
                    continue;
                }
                //delete if its too old
                if (date < DateTime.Today.AddYears(-1))
                {
                    MessageBox.Show(file.Name + " ist älter als ein Jahr und wird deshalb gelöscht.");
                    File.Delete(file.FullName);
                    continue;
                }
                //MessageBox.Show(file.Name + " wurde korrekt gelesen und ist nicht älter als ein jahr.");
            }
        }

        /// <summary>
        /// This fills the ComboBox with all the names that can receive a Mail with the calendar
        /// </summary>
        public void fillListOfNamesForMailing()
        {
            foreach(string name in Persons.Names)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = name;
                ListOfNames.Items.Add(comboBoxItem);
            }
        }

        /// <summary>
        /// This function prints the names to the calendar grid
        /// </summary>
        private void printNames()
        {
            //Create a new Textblock for the name column
            TextBlock Names = new TextBlock();
            Names.Text = "Namen";
            CalendarGrid.Children.Add(Names);

            int line = 1;
            foreach (string name in Persons.Names)
            {
                //Create a new Row
                RowDefinition row = new RowDefinition();
                //add the new row to the grid
                CalendarGrid.RowDefinitions.Add(row);

                //create a border
                Border border = new Border();
                //Fill it in the same color as the font
                border.BorderBrush = new SolidColorBrush(ColorTheme.design.Foreground);
                border.BorderThickness = new Thickness(1);
                //Add it to the correct row
                Grid.SetRow(border, line);

                //Create a new Textblock for the name
                TextBlock NameBlock = new TextBlock();
                NameBlock.Text = name;
                NameBlock.Margin = new Thickness(5, 5, 5, 95);
                //Make the text bold
                NameBlock.FontWeight = FontWeights.Bold;
                //Add the Textblock to the border
                border.Child = NameBlock;
                //Add the border to the Grid
                CalendarGrid.Children.Add(border);

                //increase the line counter
                line++;
            }
        }

        /// <summary>
        /// Returns the date of the monday of the current week
        /// </summary>
        /// <returns></returns>
        private DateTime getLastMonday()
        {
            //Get the last monday to print
            DateTime date = DateTime.Now;
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            return date;
        }

        /// <summary>
        /// Prints the Dates of the current week
        /// </summary>
        /// <param name="monday"></param>
        public void printCurrentWeek()
        {
            //Clear the grid to fill it correctly
            CalendarGrid.Children.Clear();

            //print the names in the grid
            printNames();

            DateTime currentDay = currentMonday;
            for (int i = 0; i < 7; i++)
            {
                //increase the day
                currentDay = currentMonday.AddDays(i);

                //Create a new Textblock for the date
                TextBlock DateBlock = new TextBlock();
                //Convert the date to string
                DateBlock.Text = currentDay.ToString("ddd. dd.MM.yy");
                //Style the Text
                DateBlock.FontWeight = FontWeights.Bold;
                DateBlock.HorizontalAlignment = HorizontalAlignment.Center;
                

                if(currentDay.Date == DateTime.Today.Date)
                {
                    
                    DateBlock.FontWeight = FontWeights.Bold;
                    DateBlock.Foreground = Brushes.Red;
                }

                //Set it to the correct column
                Grid.SetColumn(DateBlock, i + 1);
                //add it to the grid
                CalendarGrid.Children.Add(DateBlock);

                //print the current day
                printDay(currentDay, i + 1);
            }
        }

        /// <summary>
        /// This function prints the given day in the calendar
        /// </summary>
        /// <param name="day"></param>
        private void printDay(DateTime day, int column)
        {
            int rowOfName = 1;
            foreach(string name in Persons.Names)
            {
                //create a border
                DateBorder border = new DateBorder(day, name);

                //Add the event that opens the window to enter a task
                border.MouseLeftButtonDown += OpenEnterTaskToCalendarWindow;

                //Fill it in the same color as the font
                border.BorderBrush = new SolidColorBrush(ColorTheme.design.Foreground);
                border.BorderThickness = new Thickness(1);
                //Add it to the correct row
                Grid.SetRow(border, rowOfName);
                Grid.SetColumn(border, column);

                //increase the row number for next loop
                rowOfName++;

                //Create a Textblock where the Tasks will appeare
                TextBlock TaskBlock = new TextBlock();
                TaskBlock.Margin = new Thickness(5, 5, 5, 5);
                border.Child = TaskBlock;

                //Activate the wrapping for long tasks
                TaskBlock.TextWrapping = TextWrapping.Wrap;

                //Add the border with the Textblock to the grid
                CalendarGrid.Children.Add(border);

                //Get the sorted List of Tasks
                List<CalendarTask> calendarTasks = DataReader.getTasksOfPersonOnDate(name, day, false);

                List<CalendarTask> repeatingTasks = DataReader.getTasksOfPersonOnDate(name, day, true);

                //cancel if the list is empty
                if (calendarTasks == null)
                {
                    calendarTasks = repeatingTasks;
                }
                else
                {
                    if (repeatingTasks != null) calendarTasks.AddRange(repeatingTasks);
                }

                if (calendarTasks == null) continue;


                //Sort the list beacuase it could be unsorted if there are weekly and daily tasks
                calendarTasks.Sort(CompareTasksByTime);

                foreach (CalendarTask calendarTask in calendarTasks)
                {
                    //if it is here, the task id and the starting and ending time are correct read in
                    try
                    {
                        //try to add it to the Textblock
                        TaskBlock.Text += calendarTask.Start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + calendarTask.End.ToString("HH:mm", new CultureInfo("de-DE")) + " " + calendarTask.Name + "\n";
                    }
                    catch
                    {
                        MessageBox.Show($"Bei {name} gab es am {day.ToString("dd.MM.yy")} Probleme beim ausgeben der Aufgaben durch Grenzunterschreitung.", "Achtung!");
                        continue;
                    }
                }
                    
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
        /// Thid function sets the calendar to the last week
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastWeekButton_Click(object sender, RoutedEventArgs e)
        {
            //set the current monday to the monday from last week
            this.currentMonday = this.currentMonday.AddDays(-7);
            //call the function that prints the calendar to update it
            printCurrentWeek();
        }

        /// <summary>
        /// This function sets the calendar to the next week
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextWeekButton_Click(object sender, RoutedEventArgs e)
        {
            //set the current monday to the monday from next week
            this.currentMonday = this.currentMonday.AddDays(7);
            //call the function that prints the calendar to update it
            printCurrentWeek();
        }

        /// <summary>
        /// This function sets the calendar to the current week
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentWeekButton_Click(object sender, RoutedEventArgs e)
        {
            //set the current monday to the monday from last week
            this.currentMonday = this.lastMonday;
            //call the function that prints the calendar to update it
            printCurrentWeek();
        }

        /// <summary>
        /// Opens a new EnterTaskToCalendarWindow to enter a task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void OpenEnterTaskToCalendarWindow(object sender, EventArgs arg)
        {
            //sender as DateBorder -> Date
            EnterTaskToCalendarWindow enterTaskToCalendarWindow = new EnterTaskToCalendarWindow((sender as DateBorder).NameOfPerson, (sender as DateBorder).Date, this);
            enterTaskToCalendarWindow.Show();
        }
    }
}
