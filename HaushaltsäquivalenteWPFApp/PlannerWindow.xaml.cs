using System;
using System.Windows;
using System.Windows.Media;
using System.Net.Mail;
using Ical.Net;
using Ical.Net.Serialization;
using Ical.Net.DataTypes;
using Ical.Net.CalendarComponents;
using System.IO;


namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaktionslogik für PlannerWindow.xaml
    /// </summary>
    public partial class PlannerWindow : Window
    {
        public PlannerWindow()
        {
            InitializeComponent();
        }


        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

            DateTime now = DateTime.Now;

            Calendar calendar = new Calendar();
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


            MailMessage email = new MailMessage();
            email.From = new MailAddress("haushaltsappmail@gmail.com", "Haushaltsapp Mailsender");
            email.To.Add("florischierz1@gmail.com");
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

        }
    }
}
