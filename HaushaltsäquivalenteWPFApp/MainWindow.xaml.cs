using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HaushaltsäquivalenteWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            TableWindow tableWindow = new TableWindow();
            tableWindow.Show();
            this.Close();
        }
    }
}

/*<ListBox Name="lb" Width="100" Height="55" SelectionMode="Single">
            <ListBoxItem>Item 1</ListBoxItem>
            <ListBoxItem>Item 2</ListBoxItem>
            <ListBoxItem>Item 3</ListBoxItem>
            <ListBoxItem>Item 4</ListBoxItem>
            <ListBoxItem>Item 5</ListBoxItem>
            <ListBoxItem>Item 6</ListBoxItem>
            <ListBoxItem>Item 7</ListBoxItem>
            <ListBoxItem>Item 8</ListBoxItem>
            <ListBoxItem>Item 9</ListBoxItem>
            <ListBoxItem>Item 10</ListBoxItem>
        </ListBox>
*/
