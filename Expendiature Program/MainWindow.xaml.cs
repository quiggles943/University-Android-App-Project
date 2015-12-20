using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;

namespace Expendiature_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Office.Interop.Excel.Worksheet[] collection = new Microsoft.Office.Interop.Excel.Worksheet[20];
        private static Excel.Workbook Mybook = null;
        private static Excel.Application MyApp = null;
        Excel.Range d = null;
        Excel.Range desc = null;
        Excel.Range deb = null;
        Excel.Range cred = null;
        int[] lastrows = new int[20];
        Transaction t = new Transaction();
        TransactionList list = new TransactionList();
        int selected;
        int i;
        string path = @"..\..\logonattempts.ini";
        string filepath;

        public List<Attempt> attempts = new List<Attempt>();
        public MainWindow()
        {
            filepath = System.IO.Path.GetFullPath(path);
            DateTime Today = DateTime.Today;            
            InitializeComponent();
            load();           
            set();
            datePicker.SelectedDate = Today.Date;
            ListView listView = listView1;
            GridView gridView = new GridView();
            listView.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Date",
                DisplayMemberBinding = new Binding("Date"),
                Width = 100
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Description",
                DisplayMemberBinding = new Binding("Description"),
                Width = 200
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Debit",
                DisplayMemberBinding = new Binding("Debit")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Credit",
                DisplayMemberBinding = new Binding("Credit")
            });
            search_combobox.SelectedIndex = 0;
            sheet_selection.SelectedIndex = 0;
            attemtsreadin();
            last_unsuccessful.Content = attempts.Last(x => x.Correct == false).Date.ToString();
        }

        private void load()
        {     
            MyApp = new Excel.Application();
            Mybook = MyApp.Workbooks.Open("expendiature");
            ListSheets();
        }

        private void reload(object sender, RoutedEventArgs e)
        {
            
            for (int j = 0; j <= i; j++ )
            {
                collection[j] = null;
                
            }
            Mybook.Close();
            load();
            sheet_selection.SelectedIndex = selected;
            Load_Personal_Click(sender, e);
        }
        private void set()
        {
            listView1.ItemsSource = null;
            listView1.Items.Clear();
            
            list.Clear();
        }

        private void Load_Personal_Click(object sender, RoutedEventArgs e)
        {
            set();
            if (sheet_selection.SelectedIndex == -1 || sheet_selection.SelectedIndex == 0)
            {
                Info_box.Text = "No sheet selected";
                return;
            }
            else
            {
                for (int i = lastrows[sheet_selection.SelectedIndex-1]; i >= 2; i--)
                {
                    readin(i, sheet_selection.SelectedIndex-1);
                }
                listView1.ItemsSource = list.Source;
                total_label.Content = ("£" + list.Total());
                Info_box.Text =(collection[sheet_selection.SelectedIndex-1].Name + " selected");
                numOfTransactions.Content = list.Transactions();
            }
        }

        private void readin(int i, int worksheet)
        {
            DateTime date;
            string description = String.Empty;
            string debit = "";
            string credit = "";
            d = collection[worksheet].get_Range("C" + i);
            date = DateTime.Parse(d.Text);
            desc = collection[worksheet].get_Range("D" + i);
            deb = collection[worksheet].get_Range("E" + i);
            cred = collection[worksheet].get_Range("F" + i);
            if (desc != null)
                description = desc.Text.ToString();
            if (deb.Text != "")
                debit = deb.Text;
            else
                debit = "00.00";
            if (cred.Text != "")
                credit = (cred.Text);
            else
                credit = "00.00";
            list.Add(new Transaction
            {
                Date = date.ToShortDateString(),
                Description = description,
                Debit = debit,
                Credit = credit,
                Row = i
            });
        }

        private void closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Mybook.Close();
        }

        private void add_transaction_Click(object sender, RoutedEventArgs e)
        {
            selected = sheet_selection.SelectedIndex;
            double debit;
            double credit;
            string date = datePicker.SelectedDate.ToString();
            string description = desc_box.Text;
            string amount = amount_box.Text;
            if (add_rbtn.IsChecked == true)
            {
                try
                {
                    if (amount.StartsWith("-"))
                    {
                        if (!double.TryParse(amount.Substring(1), out debit))
                        {
                            throw new ArgumentException("Not valid number");
                        }
                        else
                            collection[sheet_selection.SelectedIndex].Cells[lastrows[sheet_selection.SelectedIndex] + 1, 5] = debit;
                    }
                    else
                    {
                        if (!double.TryParse(amount, out credit))
                        {
                            throw new ArgumentException("Not valid number");
                        }
                        else
                            collection[sheet_selection.SelectedIndex].Cells[lastrows[sheet_selection.SelectedIndex] + 1, 6] = credit;
                    }
                    collection[sheet_selection.SelectedIndex].Cells[lastrows[sheet_selection.SelectedIndex] + 1, 3] = date;
                    collection[sheet_selection.SelectedIndex].Cells[lastrows[sheet_selection.SelectedIndex] + 1, 4] = description;
                    Mybook.Save();
                    Info_box.Text = ("Transaction added to " + collection[sheet_selection.SelectedIndex].Name);
                    reload(sender, e);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (edit_rbtn.IsChecked == true)
            {
                try
                {
                    if (amount.StartsWith("-"))
                    {
                        if (!double.TryParse(amount.Substring(1), out debit))
                        {
                            throw new ArgumentException("Not valid number");
                        }
                        else
                            collection[sheet_selection.SelectedIndex-1].Cells[list.Find(listView1.SelectedItem).Row, 5] = debit;
                    }
                    else
                    {
                        if (!double.TryParse(amount, out credit))
                        {
                            throw new ArgumentException("Not valid number");
                        }
                        else
                            collection[sheet_selection.SelectedIndex-1].Cells[list.Find(listView1.SelectedItem).Row, 6] = credit;
                    }
                    collection[sheet_selection.SelectedIndex-1].Cells[list.Find(listView1.SelectedItem).Row, 3] = date;
                    collection[sheet_selection.SelectedIndex-1].Cells[list.Find(listView1.SelectedItem).Row, 4] = description;
                    Mybook.Save();
                    Info_box.Text = ("Transaction edited" + collection[sheet_selection.SelectedIndex-1].Name);
                    reload(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void closed(object sender, EventArgs e)
        {
            Mybook.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sheet_selection.SelectedIndex == -1 || sheet_selection.SelectedIndex == 0)
            {
                return;
            }
            Load_Personal.Content = ("Load " + collection[sheet_selection.SelectedIndex-1].Name);
        }

        private void ListSheets()
        {
            search_combobox.Items.Clear();
            //search_combobox.Items.Add("--Select Sheet--");
            //search_combobox.Items.Add("All Sheets");
            sheet_selection.Items.Clear();
            //sheet_selection.Items.Add("--Select Sheet--");
            foreach (Excel.Worksheet sheet in MyApp.Worksheets)
            {
                sheet_selection.Items.Add(sheet.Name);
                search_combobox.Items.Add(sheet.Name);
            }
            i = 0;
            foreach (var item in sheet_selection.Items)
            {
                collection[i] = Mybook.Sheets[i+1];
                lastrows[i] = collection[i].Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                i++;
            }
            sheet_selection.Items.Insert(0, "--Select Sheet--");
            search_combobox.Items.Insert(0, "--Select Sheet--");
            search_combobox.Items.Insert(1, "All Sheets");
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            set();
            if (search_combobox.SelectedIndex == -1 || search_combobox.SelectedIndex == 0 || search_combobox.SelectedIndex == 1)
            {
                Info_box.Text = "No sheet selected";
                return;
            }
            else
            {
                for (int i = lastrows[search_combobox.SelectedIndex - 2]; i >= 2; i--)
                {
                    readin(i, search_combobox.SelectedIndex - 2);
                }

                foreach(var item in list.Source)
                {
                    if(item.Description.Contains(search_box.Text))
                    {
                        listView1.Items.Add(item);
                    }
                }
                //listView1.ItemsSource = list.transactions;
                total_label.Content = ("£" + list.Total());
                Info_box.Text =(collection[search_combobox.SelectedIndex-2].Name + " selected");
            }
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //desc_box.Text = list.Find(listView1.SelectedItem).Description;
        }

        private void double_Click(object sender, MouseButtonEventArgs e)
        {
            if (edit_rbtn.IsChecked == true)
            {
                datePicker.SelectedDate = DateTime.Parse(list.Find(listView1.SelectedItem).Date);
                desc_box.Text = list.Find(listView1.SelectedItem).Description;
                if (list.Find(listView1.SelectedItem).Credit == "0")
                    amount_box.Text = "-" + list.Find(listView1.SelectedItem).Debit;
                else
                    amount_box.Text = list.Find(listView1.SelectedItem).Credit;
            }
            else
                return;
        }

        private void edit_rbtn_Checked(object sender, RoutedEventArgs e)
        {
            add_transaction.Content = "Edit Transaction";
            datePicker.SelectedDate = DateTime.Today.Date;
            desc_box.Clear();
            amount_box.Clear();
            listView1.SelectedIndex = -1;
        }

        private void add_rbtn_Checked(object sender, RoutedEventArgs e)
        {
            add_transaction.Content = "Add Transaction";
            datePicker.SelectedDate = DateTime.Today.Date;
            desc_box.Clear();
            amount_box.Clear();
            listView1.SelectedIndex = -1;
        }

        private void changePassword_btn_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changepassword = new ChangePassword();
            changepassword.ShowDialog();
        }

        private void attemtsreadin()
        {
            int filelength = 0;
            StreamReader r = new StreamReader(filepath);
            using (r)
            {
                while (r.ReadLine() != null) { filelength++; }
            }
            int i = 1;
            string[] file = System.IO.File.ReadAllLines(filepath);
            int len = file.Length;
            while (i < (len))
            {
                string[] column = file[i].Split('\t');
                int j = 0;
                while (j < (column.Length))
                {
                    string buffer = column[j];

                    j++;
                }
                attempts.Add(new Attempt() { Correct = Convert.ToBoolean(column[0]), Date = Convert.ToDateTime(column[1]) });
                
                i++;
            }
            var ordereddates = attempts.OrderBy(x => x.Date);
            attempts = ordereddates.ToList();
        }
    }
}
