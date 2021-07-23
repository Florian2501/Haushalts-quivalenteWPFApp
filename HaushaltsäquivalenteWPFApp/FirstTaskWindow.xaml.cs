using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaktionslogik für FirstTaskWindow.xaml
    /// </summary>
    public partial class FirstTaskWindow : Window
    {
        public FirstTaskWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This function tries to get the input from the TextBoxes and write it in a file.
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
            string newTask = "1;" + name + ";" + description + ";" + points.ToString();


            Directory.CreateDirectory(@"Data");

            string path = @"Data\Tasks.txt";

            //automatically utf 8 encoded writing, only adds it to the end
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine(newTask);
            }
            //Confirm that it worked out
            MessageBox.Show(name + " wurde als neue Aufgabe eingetragen.");
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
