using System;
using System.Collections.Generic;
using System.Text;

namespace HaushaltsäquivalenteWPFApp
{
    public class Task
    {
        //Constructor

        public Task(int id)
        {
            this.id = id;
            this.name = TaskList.Tasks[id - 1].name;
            this.value = TaskList.Tasks[id - 1].value;
            this.description = TaskList.Tasks[id - 1].description;
        }

        public Task(string name,string description, int value, int id)
        {
            this.name = name;
            this.description = description;
            this.value = value;
            this.id = id;
        }

        //Member
        private string name;
        private string description;
        private int value;
        private int id;

        //Properties
        public string Name {
            get 
            {
                return this.name;
            } 
        }
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        public int Value
        {
            get
            {
                return this.value;
            }
        }
        public int ID
        {
            get
            {
                return this.id;
            }
        }

    }
}
