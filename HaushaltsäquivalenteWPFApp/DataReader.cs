using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;

namespace HaushaltsäquivalenteWPFApp
{
    public static class DataReader
    {
        /// <summary>
        /// Retruns the sum of points a certain person did on a certain day after reading the files
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <returns></returns>
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
                Directory.CreateDirectory(@"Data/Days");
                StreamWriter sw = new StreamWriter(path);
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Something with the Date file went wrong.");
            }

            return sum;
        }

        /// <summary>
        /// Returns a List of tasks that a certain person done after reading the files
        /// </summary>
        /// <param name="numberOfDays"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
                    Directory.CreateDirectory(@"Data/Days");
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

        /// <summary>
        /// This function sorts the tasks by time in a given array
        /// </summary>
        /// <param name="tasksArray"></param>
        public static void sortTasks(ref string[] tasksArray)
        {
            for (int i = 1; i < tasksArray.Length / 3; i++)
            {
                for (int j = 0; j < tasksArray.Length / 3 - i; j++)
                {
                    DateTime date1 = DateTime.Today, date2 = DateTime.Today;
                    try
                    {
                        date1 = Convert.ToDateTime(tasksArray[3 * j + 2]);
                        date2 = Convert.ToDateTime(tasksArray[3 * j + 2 + 3]);
                    }
                    catch
                    {
                        MessageBox.Show("Es gab einen Fehler beim Sortieren der Daten. Sie bleiben unsortiert.");
                        return;
                    }

                    if (date1 > date2)
                    {
                        string help = tasksArray[3 * j + 1];
                        tasksArray[3 * j + 1] = tasksArray[3 * j + 1 + 3];
                        tasksArray[3 * j + 1 + 3] = help;

                        help = tasksArray[3 * j + 2];
                        tasksArray[3 * j + 2] = tasksArray[3 * j + 2 + 3];
                        tasksArray[3 * j + 2 + 3] = help;

                        help = tasksArray[3 * j + 3];
                        tasksArray[3 * j + 3] = tasksArray[3 * j + 3 + 3];
                        tasksArray[3 * j + 3 + 3] = help;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an array of the tasks of the person and the timeslots for the tasks
        /// </summary>
        /// <param name="name"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static List<CalendarTask> getTasksOfPersonOnDate(string name, DateTime day, bool weekly)
        {
            List<CalendarTask> TaskList = new List<CalendarTask>();
            string path = @"Data/" + ((weekly) ? (@"WeeklyTasks/" + day.DayOfWeek.ToString()) : (@"Calendar/" + day.ToString("dd.MM.yy") + ".txt"));
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Split(';')[0] == name)
                        {
                            string[] TasksOfDay = line.Split(';');

                            //Check correct length name + x*3
                            if ((TasksOfDay.Length % 3) != 1)
                            {
                                //MessageBox.Show($"Bei {name} gab es am {day.ToString("dd.MM.yy")} Probleme beim einlesen der Aufgaben durch einen Längenfehler.", "Achtung!");
                                continue;
                            }

                            //Sort the array of tasks by time
                            sortTasks(ref TasksOfDay);


                            //Go through the array if it exists and is in correct length and read the data
                            for (int i = 1; i < TasksOfDay.Length; i++)
                            {
                                int TaskID = 0;
                                DateTime start = DateTime.Today;
                                DateTime end = DateTime.Today;
                                //try to convert the task and the time stemps of it
                                try
                                {
                                    TaskID = Convert.ToInt32(TasksOfDay[i]);
                                    i++;
                                    start = Convert.ToDateTime(TasksOfDay[i], new CultureInfo("de-DE"));
                                    i++;
                                    end = Convert.ToDateTime(TasksOfDay[i], new CultureInfo("de-DE"));
                                }
                                catch
                                {
                                    MessageBox.Show($"Bei {name} gab es am {day.ToString("dd.MM.yy")} Probleme beim einlesen der Aufgaben durch inkorrektes Format.", "Achtung!");
                                    continue;
                                }

                                //Add the read in task to the list
                                TaskList.Add(new CalendarTask(TaskID, start, end, weekly));

                            }

                            return TaskList;
                        }
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("The Date file could not be read. It will be created now."); //if the date file could not be found or the person is not in the date file the default value is 0

                Directory.CreateDirectory(@"Data/" + ((weekly) ? "WeeklyTasks" : "Calendar"));
                StreamWriter sw = new StreamWriter(path);
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Something with the Date file went wrong.");
            }
            return null;
        }

    }
}
