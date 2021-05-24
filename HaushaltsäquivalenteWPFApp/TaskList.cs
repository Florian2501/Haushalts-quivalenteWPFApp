using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace HaushaltsäquivalenteWPFApp
{
    public static class TaskList
    {
        private static List<Task> tasks;

        public static List<Task> Tasks
        {
            get
            {
                readTasks();
                return tasks;
            }
        }

        private static void readTasks()
        {
            tasks = new List<Task>();

            try//try to open the Tasks.txt file to read all the possible tasks
            {
                using (StreamReader sr = new StreamReader(@"Data\Tasks.txt", Encoding.UTF7))
                {
                    if (sr == null) return;
                    string line = "";
                    //read line by line till it is empty or end of file
                    while ((line = sr.ReadLine()) != null)
                    {
                        //check for empty line and skip it
                        if (String.IsNullOrEmpty(line)) continue;
                        //split hte line into the wanted parts of id, name, description and value
                        string[] currentTask = line.Split(';');
                        try
                        {
                            //and try to create a new Task with this values
                            tasks.Add(new Task(currentTask[1], currentTask[2], Convert.ToInt32(currentTask[3]), Convert.ToInt32(currentTask[0])));
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show($"Problems in the Tasks.txt file. Value could not be converted correctly.\nThe Task {currentTask[1]} will not be counted.");

                        }
                        catch (OverflowException)
                        {
                            MessageBox.Show($"Problems in the Tasks.txt file. Value was to big.\nThe Task {currentTask[1]} will not be counted.");
                        }
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("The Tasks.txt file could not be read.");
            }
            catch (Exception)
            {
                MessageBox.Show("Something with the Tasks.txt file went wrong.");
            }
        }
    }
}
