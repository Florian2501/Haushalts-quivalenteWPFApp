using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace HaushaltsäquivalenteWPFApp
{
    public static class Persons
    {
        //Member und Properties
        //returns the number of Persons in this App
        public static int NumberOfPersons { get { return Names.Count; } }

        //returns the lsit of the people
        private static List<string> names;
        public static List<string> Names { 
            get 
            {
                readPersons();
                return names; 
            } 
        }
  

        //Methods

        /// <summary>
        /// Reads in the file with the names of the People participating in the App
        /// </summary>
        /// <returns></returns>
        private static void readPersons()
        {
            names = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(@"Data\Persons.txt", Encoding.UTF8))
                {
                    if (sr == null) return;
                    string line = "";
                    while ((line = sr.ReadLine()) != null )
                    {
                        if (String.IsNullOrEmpty(line)) continue;
                        names.Add(line);
                    }
                }
            }
            catch(IOException)
            {
                MessageBox.Show("The Persons.txt file could not be read.");
            }
        }
    }
}
