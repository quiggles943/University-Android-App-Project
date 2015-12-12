using System;
using Expendiature_Program;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expendiature_Program
{
    [TestClass]
    public class UnitTest1
    {
        TransactionList list = new TransactionList();
        [TestMethod]
        public void ListTotal()
        {
            list.transactions.Clear();
            double expected = 5;
            
            Transaction no1 = new Transaction() { Date = "02/12/15", Description = "Test amount", Credit = "£10" };
            Transaction no2 = new Transaction() { Date = "04/12/15", Description = "New test amount", Debit = "-£5" };
            list.transactions.Add(no1);
            list.transactions.Add(no2);

            double actual = list.Total();
            Assert.AreEqual(expected, actual,"Values are not equal");
        }

        [TestMethod]
        public void Checklist()
        {
            list.transactions.Clear();
            
            Transaction no1 = new Transaction() { Date = "02/12/15", Description = "Test amount", Credit = "£10" };
            Transaction no2 = new Transaction() { Date = "04/12/15", Description = "New test amount", Credit = "£20" };
            Transaction expected = no2;
            list.transactions.Add(no1);
            list.transactions.Add(no2);

            Transaction actual = list.transactions.Find(x => x.Description == "New test amount");

            Assert.AreEqual(expected, actual, "Not correct object found");
        }
    }
}
