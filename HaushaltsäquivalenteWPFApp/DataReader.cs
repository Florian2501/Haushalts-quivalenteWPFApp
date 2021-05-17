using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace HaushaltsäquivalenteWPFApp
{
    public static class DataReader
    {
        public static int GetValueOf(string name, DateTime date)
        {
          
            //try to open the File with the correct Date
            int sum = 0;
            string path = @"Data\Days\" + date.ToString("yyyy.MM.dd") + ".txt"; 
            try 
            { 
                using (StreamReader sr = new StreamReader(path))
                {
                    //if the date file exists open it
                    string line = "";
                    //go through the file line by line
                    while ((line = sr.ReadLine()) != null)
                    {
                        //check for empty line and skip it
                        if (String.IsNullOrEmpty(line)) continue;

                        //split the line into the wanted pieces
                        string[] personsTasks = line.Split(';');
                        //check wether this line fits to the wanted person if not check next line
                        if (!name.Equals(personsTasks[0])) continue;

                        //if the line of the wanted person on the wanted date is found go through the line and get the task by id and sum up the points the person earns for this job

                        for (int i = 1; i < personsTasks.Length; i++)
                        {
                            int taskID = 0;
                            int quantity = 0;
                            try
                            {
                                taskID = Convert.ToInt32(personsTasks[i]);
                                i++;
                                quantity = Convert.ToInt32(personsTasks[i]);
                            }
                            catch (FormatException)
                            {
                                MessageBox.Show($"Problems in the Date file. Value or ID could not be converted correctly.\nIt will not count.");

                            }
                            catch (OverflowException)
                            {
                                MessageBox.Show($"Problems in the Date file. Value was to big.\nIt will not count.");
                            }
                            catch (IndexOutOfRangeException)
                            {
                                MessageBox.Show($"Problems in the Date file. Index out of range.\nIt will not count.");
                            }
                            //try to find the task by id and sum up the value
                            foreach (Task task in TaskList.Tasks)
                            {
                                if(task.ID == taskID)
                                {
                                    sum += quantity * task.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            catch (IOException)
            {
                //MessageBox.Show("The Date file could not be read. It will be created now."); //if the date file could not be found or the person is not in the date file the default value is 0

                StreamWriter sw = new StreamWriter(path);
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Something with the Date file went wrong.");
            }

            return sum;
        }
    }
}
