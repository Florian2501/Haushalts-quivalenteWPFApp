using System;
using System.Windows;
using System.IO;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaktionslogik für FirstPersonWindow.xaml
    /// </summary>
    public partial class FirstPersonWindow : Window
    {
        public FirstPersonWindow()
        {
            WindowPosition.CenterWindowOnScreen(this);
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //check wether the entered name is already forgiven
            string newName = NewNameTextBox.Text;
            if (String.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Der Name ist ungültig.");
                return;
            }

            Directory.CreateDirectory(@"Data");

            //create the path
            string path = @"Data\Persons.txt";

            //automatically utf 8 encoded writing, only adds it to the end
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine(newName);
            }
            //Confirm that it worked out
            MessageBox.Show(newName + " wurde als neuer Teilnehmer eingetragen.");
            //close this window and open a new Main Window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

        /// <summary>
        /// Function that will be executed when the window loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowPosition.CenterWindowOnScreen(this);
        }
    }
}
