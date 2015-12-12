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
    public class TransactionList
    {
        private string date;
        private string description;
        private string debit;
        private double total;

        public List<Transaction> transactions = new List<Transaction>();

        public TransactionList()
        {

        }

        public void Withdraw(double amount)
        {
            total = total - amount;
        }

        public void Add(double amount)
        {
            total = total + amount;
        }

        public double Total()
        {
            total = 0;
            foreach(var item in transactions)
            {
                total = total + (double.Parse(item.Credit) - double.Parse(item.Debit));
            }
            return Math.Round(total,2);
        }
    }
}