using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Globalization;
using System.Collections;
using System.Windows.Threading;


namespace Expendiature_Program
{
    public class Transaction
    {
        private string date;
        private string description;
        private double debit;
        private double credit;
        private int row;
        
        public Transaction()
        {

        }

        public Transaction(string date, string description, string debit, string credit, int row)
        {
            Date = date;
            Description = description;
            Debit = debit;
            Credit = credit;
            Row = row;
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Debit
        {
            get
            {
                return  System.Convert.ToString(debit);
            }
            set
            {
                string buffer = value.TrimStart('-', '£');
                double result;
                if (double.TryParse(buffer, out result))
                {
                    debit = result;
                }
                else
                    throw new ArgumentException("Value is not a number");
            }
        }
        public double input(string value)
        {
            if (value.StartsWith("-£"))
            {
                string buffer = value.TrimStart('-', '£');
                double a = Double.Parse(buffer);
                return a;
            }
            else if (value.StartsWith("£"))
            {
                string buffer = value.TrimStart('£');
                double a = Double.Parse(buffer);
                return a;
            }
            else
            {
                return 00.00;
            }

        }

        public string Credit
        {
            get
            {
                return System.Convert.ToString(credit);
            }
            set
            {
                string buffer = value.TrimStart('£');
                double result;
                if (double.TryParse(buffer, out result))
                {
                    credit = result;
                }
                else
                    throw new ArgumentException("Value is not a number");
            }
        }

        public int Row
        {
            get { return row; }
            set { row = value;}
        }
    }
}