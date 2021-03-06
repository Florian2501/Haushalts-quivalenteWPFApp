using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;
using System.Windows;

namespace HaushaltsäquivalenteWPFApp
{
    public class Design
    {
        //Constructor
        public Design(Color background, Color foreground, Color tableHeader, Color tableColumn1, Color tableColumn2, Color sideMenu, Color barChart)
        {
            this.Background = background;
            this.Foreground = foreground;
            this.TableHeader = tableHeader;
            this.TableColumn1 = tableColumn1;
            this.TableColumn2 = tableColumn2;
            this.SideMenu = sideMenu;
            this.BarChart = barChart;
        }

        //Fields and Properties
        private Color background;
        public Color Background
        {
            get
            {
                return background;
            }
            set
            {
                try
                {
                    background = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Farbe konnte nicht übernommen werden.");
                }
            }
        }

        private Color foreground;
        public Color Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                try
                {
                    foreground = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Farbe konnte nicht übernommen werden.");
                }
            }
        }

        private Color tableHeader;
        public Color TableHeader
        {
            get
            {
                return tableHeader;
            }
            set
            {
                try
                {
                    tableHeader = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Farbe konnte nicht übernommen werden.");
                }
            }
        }

        private Color tableColumn1;
        /// <summary>
        /// This is the color of the sum fields. Should be more intense then the TableColumn2.
        /// </summary>
        public Color TableColumn1
        {
            get
            {
                return tableColumn1;
            }
            set
            {
                try
                {
                    tableColumn1 = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Farbe konnte nicht übernommen werden.");
                }
            }
        }

        private Color tableColumn2;
        /// <summary>
        /// This is the color of the date and the name fields. Should be less intense then TableColumn1.
        /// </summary>
        public Color TableColumn2
        {
            get
            {
                return tableColumn2;
            }
            set
            {
                try
                {
                    tableColumn2 = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Farbe konnte nicht übernommen werden.");
                }
            }
        }

        private Color sideMenu;
        public Color SideMenu
        {
            get
            {
                return sideMenu;
            }
            set
            {
                try
                {
                    sideMenu = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Farbe konnte nicht übernommen werden.");
                }
            }
        }

        private Color barChart;
        public Color BarChart
        {
            get
            {
                return barChart;
            }
            set
            {
                try
                {
                    barChart = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Farbe konnte nicht übernommen werden.");
                }
            }
        }
    }
}
