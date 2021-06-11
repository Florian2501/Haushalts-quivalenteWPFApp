using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaktionslogik für NewPerson.xaml
    /// </summary>
    public partial class NewPersonWindow : Window
    {
        public NewPersonWindow()
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
            WindowPosition.CenterWindowOnScreen(this);
            //Sets the correct colors of the ColorTheme
            this.Background = new SolidColorBrush(ColorTheme.design.Background);
            this.Foreground = new SolidColorBrush(ColorTheme.design.Foreground);
            SideMenu.Background = new SolidColorBrush(ColorTheme.design.SideMenu);
            updateNameList();
        }

        /// <summary>
        /// This function updates the names in the list of forgiven names.
        /// </summary>
        private void updateNameList()
        {
            UsedNames.Items.Clear();
            foreach (string name in Persons.Names)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = name;
                UsedNames.Items.Add(textBlock);
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
        /// Creates a new entry in the persons file. The name from the Textbox will be taken for it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //check wether the entered name is already forgiven
            string newName = NewNameTextBox.Text;
            if (Persons.Names.Contains(newName) || String.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Der Name ist schon vergeben oder leer. Alle vergebenen Namen siehst du unten. Bitte gib einen anderen ein.");
                return;
            }
            //if it is here the name is not forgiven
            //create the path
            string path = @"Data\Persons.txt";
            string allNames = "";
            //create the new string to be written in the Person file
            foreach(string name in Persons.Names)
            {
                allNames += name + "\n";
            }
            //add the new name to the others
            allNames += newName;

            //MessageBox.Show(allNames);

            //automatically utf 8 encoded writing, only adds it to the end
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine(allNames);
            }
            //Confirm that it worked out
            MessageBox.Show(newName + " wurde als neuer Teilnehmer eingetragen.");
            //clear the Textbox with the new Name
            NewNameTextBox.Text = "";
            //update the list of the given names
            updateNameList();
        }

        /// <summary>
        /// This function deletes the selected Person from the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePersonButton_Click(object sender, RoutedEventArgs e)
        {
            //Check wether a name is selected
            if(UsedNames.SelectedIndex < 0)
            {
                MessageBox.Show("Es ist kein Name ausgewählt.");
                return;
            }
            //check wether its not the last person, but if it is return
            if(UsedNames.Items.Count <= 1)
            {
                MessageBox.Show("Die letzte Person darf nicht gelöscht werden.");
                return;
            }
            //get the name that was selected
            string selectedName = Persons.Names[UsedNames.SelectedIndex];

            //create the path
            string path = @"Data\Persons.txt";
            string allNames = "";
            //create the new string to be written in the Person file
            foreach (string name in Persons.Names)
            {
                if (name.Equals(selectedName)) continue;
                allNames += name + "\n";
            }

            //MessageBox.Show(allNames);

            //automatically utf 8 encoded writing, only adds it to the end
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.AutoFlush = true;
                sw.WriteLine(allNames);
            }
            //Confirm that it worked out
            MessageBox.Show(selectedName + " wurde als Teilnehmer gelöscht eingetragen.");
            //update the list
            updateNameList();
        }
    }
}
