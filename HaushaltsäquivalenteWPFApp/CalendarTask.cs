using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HaushaltsäquivalenteWPFApp
{
    public class CalendarTask : Task
    {
        //Constructor
        public CalendarTask(int id, DateTime start, DateTime end) : base(id)
        {
            this.Start = start;
            this.End = end;
        }

        //Properties
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        //Methods

        public bool Overlaps(CalendarTask compare)
        {
            if ((this.Start <= compare.Start && this.End > compare.Start) || (compare.Start <= this.Start && compare.End > this.Start)) return true;
            else return false;
        }

        public override string ToString()
        {
            return this.ID.ToString()+";"+this.Start.ToString("HH:mm",new CultureInfo("de-DE"))+";"+this.End.ToString("HH:mm", new CultureInfo("de-DE"));
        }
        public override bool Equals(object obj)
        {
            if (obj is CalendarTask)
            {
                CalendarTask compare = (CalendarTask)obj;
                if(compare.ID == this.ID && compare.Start.Hour == this.Start.Hour && compare.Start.Minute == this.Start.Minute && compare.End.Hour == this.End.Hour && compare.End.Minute == this.End.Minute)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return this.End.Minute + this.End.Hour*100+this.Start.Minute*10000+this.Start.Hour*1000000+this.ID*100000000;
        }
    }
}
