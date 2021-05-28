﻿using System;
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
    /// Interaktionslogik für NewTaskWindow.xaml
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        public NewTaskWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the correct Design of the Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Sets the correct colors of the ColorTheme
            this.Background = new SolidColorBrush(ColorTheme.design.Background);
            this.Foreground = new SolidColorBrush(ColorTheme.design.Foreground);
            SideMenu.Background = new SolidColorBrush(ColorTheme.design.SideMenu);
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
        ///Tests the input in the TextBoxes and if its correct writes the new Task in the File 
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
            //Gets the highest forgiven ID and increase it to the new ID of the new Task
            int newID = TaskList.Tasks[TaskList.Tasks.Count - 1].ID + 1;
            //creates the new line that will represent the new task in the File
            string newTask = newID.ToString() + ";" + description + ";" + points.ToString();

            string path = @"Data\Tasks.txt";

            //automatically utf 8 encoded writing, only adds it to the end
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine("\n" + newTask);
            }
        }
    }
}
