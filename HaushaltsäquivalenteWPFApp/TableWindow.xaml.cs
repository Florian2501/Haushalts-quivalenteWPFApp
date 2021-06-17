using System;
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
    /// Interaktionslogik für Table.xaml
    /// </summary>
    public partial class TableWindow : Window
    {
        //Constructor
        public TableWindow()
        {
            InitializeComponent();
            this.PersonTable = new Table();
        }

        //Members
        private Table PersonTable;


        /// <summary>
        /// When the Table Window loads this function will be executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowPosition.CenterWindowOnScreen(this);

            //set the Color of the Background and the Menu to the Color of the Theme
            this.Background = new SolidColorBrush(ColorTheme.design.Background);
            SideMenu.Background = new SolidColorBrush(ColorTheme.design.SideMenu);
            
            //MessageBox.Show("Es gibt "+ Persons.NumberOfPersons.ToString()+" Personen, die teilnehmen.");

            //Check wether a file is not in the correct format or older than one year and delete it if yes
            string path = @"Data\Days";
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
                    date = Convert.ToDateTime(day[0]+"."+day[1]+"."+day[2]);
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

            //create a table with the default value of days 7
            createTable(7);


            //add a button to list only the current day but with more details of the tasks
        }

        /// <summary>
        /// This procedure creates a Table with the participating People in it and the informations about them of the last days, that are saved
        /// </summary>
        /// <param name="numberOfDays"></param>
        public void createTable(int numberOfDays)
        {
            //create a new Table in the FlowDocument FlowDoc
            Table table = new Table();
            //make the new table accessible
            this.PersonTable = table;
            //Define the Doc reader
            FlowDoc.Blocks.Clear();
            FlowDoc.Blocks.Add(table);
            table.FontFamily = new FontFamily("Arial");
            table.CellSpacing = 2;
            table.BorderThickness = new Thickness(2);
            table.BorderBrush = Brushes.Black;
            table.Background = Brushes.White;
            
            //Add columns to the Table until every person has its own column
            int numberOfPersons = Persons.NumberOfPersons;
            for (int i =0; i < numberOfPersons+1; i++)
            {               
                table.Columns.Add(new TableColumn());

                /*if (i % 2 == 0)
                {
                    //table.Columns[i].Background = new SolidColorBrush(ColorTheme.design.TableColumn1);//Color of the first column
                }
                else
                {
                    //table.Columns[i].Background = new SolidColorBrush(ColorTheme.design.TableColumn2);//Color of the second column
                }*/
            }

            ///////////////////////////////////////////////////HEADLINE
            //Adds the first row to the Table
            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[0].Rows.Add(new TableRow());
            TableRow currentRow = table.RowGroups[0].Rows[0];
            //Customizes the first row
            currentRow.Background = new SolidColorBrush(ColorTheme.design.TableHeader);//Color of the Headline
            currentRow.FontSize = 40;
            currentRow.FontWeight = System.Windows.FontWeights.Bold;
            //Adds new cell to the first row that spans all over the Table. This is the headline
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"Haushaltsäquivalente der letzten {numberOfDays} Tage"))));
            currentRow.Cells[0].ColumnSpan = numberOfPersons+1;

            //////////////////////////////////////////////////////NAMES
            // Add the second (header) row for the names of the Persons
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[1];
            currentRow.FontWeight = FontWeights.Bold;

            //Add a new RowGroup to the table where the points will be displayed
            table.RowGroups.Add(new TableRowGroup());
            //Add the last row for the detail "buttons" of the Persons
            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[2].Rows.Add(new TableRow());
            TableRow lastRow = table.RowGroups[2].Rows[0];
            lastRow.FontWeight = FontWeights.Bold;

            //Add Table cell "Namen"
            Run run = new Run("Namen");
            Paragraph paragraph = new Paragraph(run);
            paragraph.Padding = new Thickness(2);
            paragraph.BorderThickness = new Thickness(1);
            paragraph.BorderBrush = Brushes.Black;
            paragraph.Margin = new Thickness(2);
            paragraph.Background = new SolidColorBrush(ColorTheme.design.TableColumn2);
            //Create another paragraph with the same properties for the bottom
            Run run2 = new Run("Namen");
            Paragraph copy = new Paragraph(run2);
            copy.Padding = new Thickness(2);
            copy.BorderThickness = new Thickness(1);
            copy.BorderBrush = Brushes.Black;
            copy.Margin = new Thickness(2);
            copy.Background = new SolidColorBrush(ColorTheme.design.TableColumn2);
            //Add both to the table
            currentRow.Cells.Add(new TableCell(paragraph));
            lastRow.Cells.Add(new TableCell(copy));

            
            // Add cells with the names to the second row.
            foreach (string name in Persons.Names)
            {
                //create the name blocks below the header
                run = new Run(name);
                paragraph = new Paragraph(run);
                paragraph.Padding = new Thickness(2);
                paragraph.BorderThickness = new Thickness(1);
                paragraph.BorderBrush = Brushes.Black;
                paragraph.Margin = new Thickness(2);
                paragraph.Background = new SolidColorBrush(ColorTheme.design.TableColumn2);
                //Make the name clickable that it opens a new DetailPersonWindow
                paragraph.MouseLeftButtonDown += OpenDetailWindow_MouseLeftButtonDown;
                //create the name blocks  at hte bottom of the tale
                run2 = new Run(name);
                copy = new Paragraph(run2);
                copy.Padding = new Thickness(2);
                copy.BorderThickness = new Thickness(1);
                copy.BorderBrush = Brushes.Black;
                copy.Margin = new Thickness(2);
                copy.Background = new SolidColorBrush(ColorTheme.design.TableColumn2);
                //Make the name clickable that it opens a new DetailPersonWindow
                copy.MouseLeftButtonDown += OpenDetailWindow_MouseLeftButtonDown;

                currentRow.Cells.Add(new TableCell(paragraph));
                lastRow.Cells.Add(new TableCell(copy));
            }
     
            ///////////////////////////////////////////////////////DAYS AND POINTS

            //Array to count and sum up the points per Person a day
            int[] sumOfPersons = new int[numberOfPersons];
            
            for(int i=0; i < numberOfDays; i++)
            {
                int personIndex = 0;
                table.RowGroups[1].Rows.Add(new TableRow());
                currentRow = table.RowGroups[1].Rows[i];

                run = new Run(DateTime.Today.AddDays(-1 * i).ToString("ddd: dd.MM.yy"));
                paragraph = new Paragraph(run);
                paragraph.Padding = new Thickness(2);
                paragraph.BorderThickness = new Thickness(1);
                paragraph.BorderBrush = Brushes.Black;
                paragraph.Margin = new Thickness(2);
                paragraph.Background = new SolidColorBrush(ColorTheme.design.TableColumn2);
                currentRow.Cells.Add(new TableCell(paragraph));

                //currentRow.Cells.Add(new TableCell(new Paragraph(new Run(DateTime.Today.AddDays(-1 * i).ToString("ddd: dd.MM.yy")))));
                foreach (string name in Persons.Names)
                {
                    sumOfPersons[personIndex] += DataReader.GetValueOf(name, DateTime.Today.AddDays(-1 * i));
                    
                    run = new Run(DataReader.GetValueOf(name, DateTime.Today.AddDays(-1 * i)).ToString());
                    paragraph = new Paragraph(run);
                    paragraph.Padding = new Thickness(2);
                    paragraph.BorderThickness = new Thickness(1);
                    paragraph.BorderBrush = Brushes.Black;
                    paragraph.Margin = new Thickness(2);
                    currentRow.Cells.Add(new TableCell(paragraph));
                    //currentRow.Cells.Add(new TableCell(new Paragraph(new Run(DataReader.GetValueOf(name, DateTime.Today.AddDays(-1 * i)).ToString()))));
                    personIndex++;
                }

            }

            /////////////////////////////////////////////////////////SUM
            //Add a sum line to the Table under the names in tablerowgroup 0
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[2];
            //Add the first cell descriptiom "Summe"
            run = new Run("Summe");
            paragraph = new Paragraph(run);
            paragraph.Padding = new Thickness(2);
            paragraph.FontWeight = FontWeights.Bold;
            paragraph.BorderThickness = new Thickness(1);
            paragraph.BorderBrush = Brushes.Black;
            paragraph.Margin = new Thickness(2);
            paragraph.Background = new SolidColorBrush(ColorTheme.design.TableColumn2);
            currentRow.Cells.Add(new TableCell(paragraph));
            //currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Summe"))));

            //Add for each person a cell with the sum of the points
            foreach (int sum in sumOfPersons)
            {
                run = new Run(sum.ToString());
                paragraph = new Paragraph(run);
                paragraph.Padding = new Thickness(2);
                paragraph.FontWeight = FontWeights.Bold;
                paragraph.BorderThickness = new Thickness(1);
                paragraph.BorderBrush = Brushes.Black;
                paragraph.Margin = new Thickness(2);
                paragraph.Background = new SolidColorBrush(ColorTheme.design.TableColumn1);
                currentRow.Cells.Add(new TableCell(paragraph));
                //currentRow.Cells.Add(new TableCell(new Paragraph(new Run(sum.ToString()))));
            }

            //////////////////////////////////////////////////////BAR CHARTS
            //create array of names and points of the persons that it can be sorted
            (int, string)[] places = new (int, string)[numberOfPersons];
            for(int i=0; i<numberOfPersons; i++)
            {
                places[i] = (sumOfPersons[i], Persons.Names[i]);
            }

            //sort the array by points descending ;
            for(int i=1; i< places.Length-1; i++)
            {
                for(int j=0; j<places.Length-i; j++)
                {
                    if (places[j].Item1 < places[j + 1].Item1)
                    {
                        (int, string) help = places[j];
                        places[j] = places[j + 1];
                        places[j + 1] = help;
                    }
                }
            }

            //define a variable to count up for the places
            int place = 1;

            //make a new grid in which the places will be listed with 2 columns for names and points
            Grid placeGrid = new Grid();
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            placeGrid.ColumnDefinitions.Add(column1);
            placeGrid.ColumnDefinitions.Add(column2);

            //creates 2 rows for every Person for the name + points and the bar
            for(int i=0; i<numberOfPersons*2; i++)
            {
                placeGrid.RowDefinitions.Add(new RowDefinition());
            }

            //delete the last table of places
            TopMenu.Children.Clear();
            //Add the grid to the menu bar
            TopMenu.Children.Add(placeGrid);

            foreach ((int points, string name) in places)
            {
                //Build the Textblock of place and Name
                TextBlock nameBlock = new TextBlock();
                nameBlock.Margin = new Thickness(15, 5, 0,0);
                nameBlock.Text = place.ToString() + ". " + name;
                Grid.SetRow(nameBlock, (place-1) * 2);//every second row

                //Build the text block with the points
                TextBlock pointsBlock = new TextBlock();
                pointsBlock.Text = points.ToString();
                pointsBlock.Margin = new Thickness(15, 5, 0, 0);
                Grid.SetRow(pointsBlock, (place - 1) * 2);
                Grid.SetColumn(pointsBlock, 1);//every second row and in the second column

                //add the textblocks to the fitting Gridcell in the Menu
                placeGrid.Children.Add(nameBlock);
                placeGrid.Children.Add(pointsBlock);

                //create a rectangle in the correct length and in relation to the first place -> will be always the whole line (190)
                Rectangle bar = new Rectangle();
                //check wether it is not a number when noone even has a point
                double percent = (double)points / ((double)places[0].Item1);
                if (Double.IsNaN(percent))
                {
                    bar.Width = 1;
                }
                else
                {
                    bar.Width = 169.0 * percent + 1.0;//+1 that there is even something if the points are 0
                }
                bar.Height = 20;
                bar.Fill = new SolidColorBrush(ColorTheme.design.BarChart);//The given color for the bar chart
                bar.HorizontalAlignment = HorizontalAlignment.Left;
                bar.Margin = new Thickness(5, 0, 0, 0);

                //set the rectangle to the row that is empty under each name
                Grid.SetRow(bar, (place * 2) - 1);
                //make it over the whole width
                Grid.SetColumnSpan(bar, 2);
                //add the abr to the Grid
                placeGrid.Children.Add(bar);

                place++;
            }
        }

        /// <summary>
        /// When the OK Button for the days gets clicked this procedure will be executed and tries to create a table with the entered amount of days
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaysButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfDays = 0;
            try
            {
                numberOfDays = Convert.ToInt32(DaysTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Die eingegebene Anzahl Tage ist ungültig.");
                return;
            }
            if (numberOfDays <= 0 || numberOfDays>365)
            {
                MessageBox.Show("Die Anzahl Tage muss zwischen 1 und 365 liegen.");
                return;
            }

            createTable(numberOfDays);
            
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
        /// Opens a new Window where you can enter new Tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow();
            newTaskWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Occurs when the name in the table gets clicked. It opens a new DetailPersonWindow about the person.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDetailWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Paragraph paragraph = sender as Paragraph;
            TextRange textRange = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
            string name = textRange.Text;
            DetailPersonWindow detailPersonWindow = new DetailPersonWindow(name);
            detailPersonWindow.Show();
        }
    }
}
