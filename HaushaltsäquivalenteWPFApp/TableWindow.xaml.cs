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

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaktionslogik für Table.xaml
    /// </summary>
    public partial class TableWindow : Window
    {
        public TableWindow()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// When the Table Window loads this function will be executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Es gibt "+ Persons.NumberOfPersons.ToString()+" Personen, die teilnehmen.");
            createTable(7);
            //TODO check wether a file is more than a year old and delete it if yes
            //add a scrollbar to enter the timespan
            //improve the main window an create a new one to enter your tasks, to enter a new task and to enter a new person
        }

        /// <summary>
        /// This procedure creates a Table with the participating People in it and the informations about them of the last days, that are saved
        /// </summary>
        /// <param name="numberOfDays"></param>
        public void createTable(int numberOfDays)
        {
            //create a new Table in the FlowDocument FlowDoc
            Table table = new Table();
            FlowDoc.Blocks.Clear();
            FlowDoc.Blocks.Add(table);
            table.FontFamily = new FontFamily("Arial");
            table.CellSpacing = 10;
            table.Background = Brushes.Gray;
            table.BorderThickness = new Thickness(2);
            table.BorderBrush = Brushes.Black;

            //Add columns to the Table until every person has its own column
            int numberOfPersons = Persons.NumberOfPersons;
            for (int i =0; i < numberOfPersons+1; i++)
            {
                table.Columns.Add(new TableColumn());

                if (i % 2 == 0)
                    table.Columns[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B8378"));
                else
                {
                    table.Columns[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CDC0B0"));
                }
            }

            //Adds the first row to the Table
            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[0].Rows.Add(new TableRow());
            TableRow currentRow = table.RowGroups[0].Rows[0];
            //Customizes the first row
            Color color = (Color)ColorConverter.ConvertFromString("#00acd3");
            currentRow.Background = new SolidColorBrush(color);
            currentRow.FontSize = 40;
            currentRow.FontWeight = System.Windows.FontWeights.Bold;
            //Adds new cell to the first row that spans all over the Table. This is the headline
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"Haushaltsäquivalente der letzten {numberOfDays} Tage"))));
            currentRow.Cells[0].ColumnSpan = numberOfPersons;

            // Add the second (header) row for the names of the Persons
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Bold;

            //Add Table cell "Namen"
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Namen"))));
            // Add cells with the names to the second row.
            foreach(string name in Persons.Names)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(name))));
            }

            //Add a new RowGroup to the table where the points will be displayed
            table.RowGroups.Add(new TableRowGroup());

            //Array to count and sum up the points per Person a day
            int[] sumOfPersons = new int[numberOfPersons];
            
            for(int i=0; i < numberOfDays; i++)
            {
                int personIndex = 0;
                table.RowGroups[1].Rows.Add(new TableRow());
                currentRow = table.RowGroups[1].Rows[i];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(DateTime.Today.AddDays(-1 * i).ToString("ddd: dd.MM.yy")))));
                foreach (string name in Persons.Names)
                {
                    sumOfPersons[personIndex] += DataReader.GetValueOf(name, DateTime.Today.AddDays(-1 * i));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(DataReader.GetValueOf(name, DateTime.Today.AddDays(-1 * i)).ToString()))));
                    personIndex++;
                }

                //TODO add sum function and improve the design
            }

            //Add a sum line to the Table under the names in tablerowgroup 0
            table.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table.RowGroups[0].Rows[2];
            //Add the first cell descriptiom "Summe"
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Summe"))));

            //Add for each person a cell with the sum of the points
            foreach (int sum in sumOfPersons)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(sum.ToString()))));
            }
        }
    }
}

/*
  <Table CellSpacing="5" x:Name="Table1" >

                <Table.Columns>
                    <TableColumn Width="*"/>
                    <TableColumn Width="*"/>
                    <TableColumn Width="*"/>
                    <TableColumn Width="*"/>
                </Table.Columns>

                <TableRowGroup>

                    <!-- Title row for the table. -->
                    <TableRow Background="SkyBlue">
                        <TableCell ColumnSpan="4" TextAlignment="Center">
                            <Paragraph FontSize="24pt" FontWeight="Bold">Planetary Information</Paragraph>
                        </TableCell>
                    </TableRow>

                    <!-- Header row for the table. -->
                    <TableRow Background="LightGoldenrodYellow">
                        <TableCell>
                            <Paragraph FontSize="14pt" FontWeight="Bold">Planet</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph FontSize="14pt" FontWeight="Bold">Mean Distance from Sun</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph FontSize="14pt" FontWeight="Bold">Mean Diameter</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph FontSize="14pt" FontWeight="Bold">Approximate Mass</Paragraph>
                        </TableCell>
                    </TableRow>

                    <!-- Sub-title row for the inner planets. -->
                    <TableRow>
                        <TableCell ColumnSpan="4">
                            <Paragraph FontSize="14pt" FontWeight="Bold">The Inner Planets</Paragraph>
                        </TableCell>
                    </TableRow>

                    <!-- Four data rows for the inner planets. -->
                    <TableRow>
                        <TableCell>
                            <Paragraph>Mercury</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>57,910,000 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>4,880 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>3.30e23 kg</Paragraph>
                        </TableCell>
                    </TableRow>
                    <TableRow Background="lightgray">
                        <TableCell>
                            <Paragraph>Venus</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>108,200,000 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>12,103.6 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>4.869e24 kg</Paragraph>
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>
                            <Paragraph>Earth</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>149,600,000 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>12,756.3 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>5.972e24 kg</Paragraph>
                        </TableCell>
                    </TableRow>
                    <TableRow Background="lightgray">
                        <TableCell>
                            <Paragraph>Mars</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>227,940,000 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>6,794 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>6.4219e23 kg</Paragraph>
                        </TableCell>
                    </TableRow>

                    <!-- Sub-title row for the outter planets. -->
                    <TableRow>
                        <TableCell ColumnSpan="4">
                            <Paragraph FontSize="14pt" FontWeight="Bold">The Major Outer Planets</Paragraph>
                        </TableCell>
                    </TableRow>

                    <!-- Four data rows for the major outter planets. -->
                    <TableRow>
                        <TableCell>
                            <Paragraph>Jupiter</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>778,330,000 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>142,984 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>1.900e27 kg</Paragraph>
                        </TableCell>
                    </TableRow>
                    <TableRow Background="lightgray">
                        <TableCell>
                            <Paragraph>Saturn</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>1,429,400,000 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>120,536 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>5.68e26 kg</Paragraph>
                        </TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>
                            <Paragraph>Uranus</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>2,870,990,000 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>51,118 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>8.683e25 kg</Paragraph>
                        </TableCell>
                    </TableRow>
                    <TableRow Background="lightgray">
                        <TableCell>
                            <Paragraph>Neptune</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>4,504,000,000 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>49,532 km</Paragraph>
                        </TableCell>
                        <TableCell>
                            <Paragraph>1.0247e26 kg</Paragraph>
                        </TableCell>
                    </TableRow>

                    <!-- Footer row for the table. -->
                    <TableRow>
                        <TableCell ColumnSpan="4">
                            <Paragraph FontSize="10pt" FontStyle="Italic">
                                Information from the
                                <Hyperlink NavigateUri="http://encarta.msn.com/encnet/refpages/artcenter.aspx">Encarta</Hyperlink>
                                web site.
                            </Paragraph>
                        </TableCell>
                    </TableRow>

                </TableRowGroup>
            </Table>*/
