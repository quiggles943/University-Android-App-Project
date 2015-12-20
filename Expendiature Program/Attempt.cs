using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expendiature_Program
{
    public class Attempt
    {
        bool correct;
        DateTime datestamp;

        public Attempt()
        {

        }

        public bool Correct
        {
            get { return correct; }
            set { correct = value; }
        }

        public DateTime Date
        {
            get { return datestamp; }
            set { datestamp = value; }
        }

    }
}
