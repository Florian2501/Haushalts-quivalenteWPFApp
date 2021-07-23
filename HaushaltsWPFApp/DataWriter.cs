using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;

namespace HaushaltsäquivalenteWPFApp
{
    static class DataWriter
    {
        /// <summary>
        /// This writes back the done Task for the given Person in the file of today
        /// </summary>
        /// <param name="doneTask"></param>
        /// <param name="name"></param>
        public static void EnterDoneTask(Task doneTask, string name, DateTime date)
        {
            Directory.CreateDirectory(@"Data/Days");
            //create the path of the day
            string path = @"Data\Days\" + date.ToString("yyyy.MM.dd") + ".txt";
            //MessageBox.Show("Name: " + name);
            //MessageBox.Show("Name: " + doneTask.Name);
            string writeBack = "";
            bool foundPerson = false;
            bool foundTask = false;
            //check wether the file of the day exists
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
                        //check wether this line fits to the wanted person of the PersonCombobox if not check next line
                        if (!name.Equals(personsTasks[0]))
                        {
                            writeBack += line + "\n";
                            continue;
                        }

                        //if the line of the wanted person on the wanted date is found go through the line and get the task by id and sum up the points the person earns for this job

                        foundPerson = true;
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

                            //check wether the task in the line at the current position is the new done task to be added
                            if (taskID == doneTask.ID)
                            {
                                foundTask = true;
                                //if it is found increase the quantity of this task in the splitted line array
                                personsTasks[i] = (quantity + 1).ToString();
                                //exit the loop because it cant find this task a second time
                                break;
                            }
                        }

                        int index = 0;
                        //add the line of the wanted person to the write back string
                        foreach (string part in personsTasks)
                        {
                            if (String.IsNullOrEmpty(part)) continue;
                            writeBack += part + ((index == personsTasks.Length - 1) ? "" : ";");
                            index++;
                        }
                        //check wether the task was already in the line then its quantity is increased already
                        if (!foundTask)
                        {
                            //if it wasnt in the line then add the id and the quantity at the and
                            writeBack += ";" + doneTask.ID.ToString() + ";1";
                        }
                        writeBack += "\n";
                    }

                    if (!foundPerson)
                    {
                        writeBack += name + ";" + doneTask.ID.ToString() + ";1";
                    }
                }
            }

            catch (IOException)
            {
                MessageBox.Show("The Date file could not be read. It will be created now."); //if the date file could not be found or the person is not in the date file the default value is 0

            }
            catch (Exception)
            {
                MessageBox.Show("Something with the Date file went wrong.");
            }

            //Write the data into the file
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create), Encoding.UTF8))
            {
                sw.AutoFlush = true;
                //if the file didnt already exist the writeback string is empty so it should create a new file and write only the info of the donetask into the file
                if (String.IsNullOrEmpty(writeBack))
                {
                    sw.WriteLine(name + ";" + doneTask.ID.ToString() + ";1");
                }
                //if it could read the file, writeback is the string consisting of the former infos and the new donetask
                else
                {
                    sw.WriteLine(writeBack);
                }
            }
            MessageBox.Show(doneTask.Name + " wurde für " + name + " eingetragen.");
        }
    }
}
