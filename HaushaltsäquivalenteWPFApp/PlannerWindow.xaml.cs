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
using System.Windows.Shapes;
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
        public DateBorder(DateTime date) : base()
        {
            this.Date = date;
        }

        //Property
        public DateTime Date { get; set; }
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

        /// <summary>
        /// Sends the calendar data as ics File to the given Mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: readCalendar() returns Calendar

            DateTime now = DateTime.Now;

            Ical.Net.Calendar calendar = new Ical.Net.Calendar();
            calendar.Events.Add(new CalendarEvent
            {
                Class = "PUBLIC",
                Summary = "Perfect Sum",
                Created = new CalDateTime(DateTime.Now),
                Description = "PerfectDetails",
                Start = new CalDateTime(now.AddDays(2)),
                End = new CalDateTime(now.AddDays(3)),
                Sequence = 0,
                Uid = Guid.NewGuid().ToString(),
                Location = "PerfectLocation"
            });

            calendar.Events.Add(new CalendarEvent
            {
                Class = "PUBLIC",
                Summary = "Perfect Sum2",
                Created = new CalDateTime(DateTime.Now),
                Description = "PerfectDetails2",
                Start = new CalDateTime(now.AddDays(3)),
                End = new CalDateTime(now.AddDays(4)),
                Sequence = 0,
                Uid = Guid.NewGuid().ToString(),
                Location = "PerfectLocation2"
            });

            string path = @"./test.ics";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                var serializer = new CalendarSerializer(new SerializationContext());
                var serializedCalendar = serializer.SerializeToString(calendar);
                sw.WriteLine(serializedCalendar);
            }

            //Starts a new Thread where it sends the Mail, so you can click around while waiting for the mail
            ThreadPool.QueueUserWorkItem(sendCalendar, path);
        }

        /// <summary>
        /// This function sends the file of the given path to the given receiver mail adress
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="path"></param>
        private void sendCalendar(object pathObject)
        {

            string path = (string)pathObject;
            MailMessage email = new MailMessage();
            email.From = new MailAddress("haushaltsappmail@gmail.com", "Haushaltsapp Mailsender");
            email.To.Add("florischierz1@gmail.com");//email.To.Add(ReceiverTextBox.Text);
            email.Subject = "Testmail";
            email.Body = "Das hier ist der Text der Mail. Im Anhang ist die Kaledner datei.";

            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(path);
            email.Attachments.Add(attachment);

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            try
            {

                client.EnableSsl = true;

                client.UseDefaultCredentials = false;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Credentials = new System.Net.NetworkCredential("haushaltsappmail@gmail.com", "uuwczzfriicsovcf");


                client.Send(email);
                MessageBox.Show("Gesendet!", "Infofenster", MessageBoxButton.OKCancel, MessageBoxImage.Hand);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Nicht geklappt " + ex.Message + ex.ToString());
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
        private void printCurrentWeek()
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
                DateBorder border = new DateBorder(day);

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

                //read the datefile and search for person
                string[] TasksOfDay = getTasksOfPersonOnDate(name, day);
                //check if it is empty and then there are no tasks to print, so continue in loop
                if (TasksOfDay == null) 
                {
                    //MessageBox.Show($"Für {name} gab es am {day.ToString("dd.MM.yy")} keine Aufgaben.", "Achtung!");
                    continue; 
                }
                //Check correct length name + x*3
                if((TasksOfDay.Length % 3) != 1)
                {
                    //MessageBox.Show($"Bei {name} gab es am {day.ToString("dd.MM.yy")} Probleme beim einlesen der Aufgaben durch einen Längenfehler.", "Achtung!");
                    continue;
                }

                //Sort the array of tasks by time
                sortTasks(ref TasksOfDay);

                //Go through the array if it exists and is in correct length and read the data
                for (int i = 1; i< TasksOfDay.Length; i++)
                {
                    int TaskID = 0;
                    DateTime start = DateTime.Today;
                    DateTime end = DateTime.Today;
                    //try to convert the task and the time stemps of it
                    try
                    {
                        TaskID = Convert.ToInt32(TasksOfDay[i]);
                        i++;
                        start = Convert.ToDateTime(TasksOfDay[i], new CultureInfo("de-DE"));
                        i++;
                        end = Convert.ToDateTime(TasksOfDay[i], new CultureInfo("de-DE"));
                    }
                    catch
                    {
                        MessageBox.Show($"Bei {name} gab es am {day.ToString("dd.MM.yy")} Probleme beim einlesen der Aufgaben durch inkorrektes Format.", "Achtung!");
                        continue;
                    }
                    //if it is here, the task id and the starting and ending time are correct read in
                    try
                    {
                        //try to add it to the Textblock
                        TaskBlock.Text += start.ToString("HH:mm", new CultureInfo("de-DE")) + "-" + end.ToString("HH:mm", new CultureInfo("de-DE")) + " " + TaskList.Tasks[TaskID - 1].Name + "\n";
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
        /// This function sorts the tasks by time in a given array
        /// </summary>
        /// <param name="tasksArray"></param>
        private void sortTasks(ref string[] tasksArray)
        {
            for(int i=1; i<tasksArray.Length/3; i++)
            {
                for(int j=0; j < tasksArray.Length/3 - i; j++)
                {
                    DateTime date1=DateTime.Today, date2=DateTime.Today;
                    try
                    {
                        date1 = Convert.ToDateTime(tasksArray[3*j + 2]);
                        date2 = Convert.ToDateTime(tasksArray[3*j + 2 + 3]);
                    }
                    catch
                    {
                        MessageBox.Show("Es gab einen Fehler beim Sortieren der Daten. Sie bleiben unsortiert.");
                        return;
                    }

                    if (date1 > date2)
                    {
                        string help = tasksArray[3*j + 1];
                        tasksArray[3*j + 1] = tasksArray[3*j + 1 + 3];
                        tasksArray[3*j + 1 + 3] = help;
                        
                        help = tasksArray[3*j + 2];
                        tasksArray[3*j + 2] = tasksArray[3*j + 2 + 3];
                        tasksArray[3*j + 2 + 3] = help;

                        help = tasksArray[3*j + 3];
                        tasksArray[3*j + 3] = tasksArray[3*j + 3 + 3];
                        tasksArray[3*j + 3 + 3] = help;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an array of the tasks of the person and the timeslots for the tasks
        /// </summary>
        /// <param name="name"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private string[] getTasksOfPersonOnDate(string name, DateTime day)
        {
            string path = @"Data/Calendar/" + day.ToString("dd.MM.yy") + ".txt";
            try
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Split(';')[0] == name)
                        {
                            return line.Split(';');
                        }
                    }
                }
            }
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
            return null;
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
            //TODO add name of person to border
            //sender as DateBorder -> Date
        }
    }
}
