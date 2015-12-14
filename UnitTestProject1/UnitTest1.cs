using System;
using Expendiature_Program;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expendiature_Program
{
    [TestClass]
    public class UnitTest1
    {
        Random rnd = new Random();
        TransactionList list = new TransactionList();
        [TestMethod]
        public void ListTotal()
        {
            list.Clear();
            double expected = 5;
            
            Transaction no1 = new Transaction() { Date = "02/12/15", Description = "Test amount", Credit = "£10" };
            Transaction no2 = new Transaction() { Date = "04/12/15", Description = "New test amount", Debit = "-£5" };
            list.Add(no1);
            list.Add(no2);

            double actual = list.Total();
            Assert.AreEqual(expected, actual,"Values are not equal");
        }

        [TestMethod]
        public void Checklist()
        {
            list.Clear();
            
            Transaction no1 = new Transaction() { Date = "02/12/15", Description = "Test amount", Credit = "£10" };
            Transaction no2 = new Transaction() { Date = "04/12/15", Description = "New test amount", Credit = "£20" };
            Transaction expected = no2;
            list.Add(no1);
            list.Add(no2);

            Transaction actual = list.Find("New test amount");

            Assert.AreEqual(expected, actual, "Not correct object found");
        }

        [TestMethod]
        public void newChecklist()
        {
            list.Clear();
            double actualtotal = 0;
            
            for(int i = 0; i <= 100; i++)
            {
               actualtotal = actualtotal + createTransaction();
            }

            double expected = list.Total();
            Assert.AreEqual(expected, actualtotal, 0.001, "Values not identical");
        }
        public double createTransaction()
        {
            double total;
            Transaction test = new Transaction() { Date = (DateTime.Now.AddDays(rnd.Next(-5,50)).ToShortDateString()), Description = "Test amount", Credit = ("£"+rnd.Next(-50,100)) };
            total = double.Parse(test.Credit);
            list.Add(test);
            return total;
        }

        [TestMethod]
        public void findTest()
        {
            string expected = "50";
            list.Clear();
            Transaction no1 = new Transaction() { Date = "02/12/15", Description = "Test amount", Credit = "£10" };
            Transaction no2 = new Transaction() { Date = "04/12/15", Description = "New test amount", Debit = "-£50" };
            Transaction no3 = new Transaction() { Date = "01/12/15", Description = "test amount 2", Debit = "-£5" };
            list.Add(no1);
            list.Add(no2);
            list.Add(no3);

            string actual = list.Find("New test amount").Debit;
            Assert.AreEqual(expected, actual, "Values not correct");
        }
    }
}
