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
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                {
                    //if the date file exists open it
                    string line = "";
                    //go through the date file line by line
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

        public static List<int> GetListOfDoneTasks(int numberOfDays, string name)
        {
            //create the list to store the numbers of done tasks
            List<int> numberOfDoneTasks = new List<int>();
            //create a 0 for each task that could exist -> init values, so that every position in the list is connected to a task by (id-1)
            foreach(var _ in TaskList.Tasks)
            {
                numberOfDoneTasks.Add(0);
            }
            //collect the numbers of done tasks in the last ... days
            for(int j= 0; j < numberOfDays; j++)
            {
                //count the days back
                DateTime date = DateTime.Today.AddDays(-1 * j);

                string path = @"Data\Days\" + date.ToString("yyyy.MM.dd") + ".txt";

                try
                {
                    using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                    {
                        //if the date file exists open it
                        string line = "";
                        //go through the date file line by line
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
                                //add the quantity to the fitting position of the list
                                numberOfDoneTasks[taskID - 1] += quantity;
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

                
            }
            return numberOfDoneTasks;
        }
    }
}
