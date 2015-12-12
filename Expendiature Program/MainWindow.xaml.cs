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
        private static Excel.Worksheet Personal = null;
        private static Excel.Worksheet Student = null;
        Excel.Range d = null;
        Excel.Range desc = null;
        Excel.Range deb = null;
        Excel.Range cred = null;
        int[] lastrows = new int[20];
        Transaction t = new Transaction();
        TransactionList list = new TransactionList();
        int selected;
        int i;
        public MainWindow()
        {           
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
            
            
        }

        private void load()
        {     
            MyApp = new Excel.Application();
            Mybook = MyApp.Workbooks.Open("expendiature");
            Personal = Mybook.Sheets[1];
            Student = Mybook.Sheets[5];
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
            listView1.Items.Clear();
            listView1.ItemsSource = null;
            list.transactions.Clear();
        }

        private void Load_Personal_Click(object sender, RoutedEventArgs e)
        {
            set();
            if (sheet_selection.SelectedIndex == -1)
            {
                Info_box.Text = "No sheet selected";
                return;
            }
            else
            {
                for (int i = lastrows[sheet_selection.SelectedIndex]; i >= 2; i--)
                {
                    readin(i, sheet_selection.SelectedIndex);
                }
                listView1.ItemsSource = list.transactions;
                total_label.Content = ("£" + list.Total());
                Info_box.Text =(collection[sheet_selection.SelectedIndex].Name + " selected");
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
            list.transactions.Add(new Transaction
            {
                Date = date.ToShortDateString(),
                Description = description,
                Debit = debit,
                Credit = credit
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

        private void closed(object sender, EventArgs e)
        {
            Mybook.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sheet_selection.SelectedIndex == -1)
            {
                return;
            }
            Load_Personal.Content = ("Load " + collection[sheet_selection.SelectedIndex].Name);
        }

        private void ListSheets()
        {
            sheet_selection.Items.Clear();
            foreach (Excel.Worksheet sheet in MyApp.Worksheets)
            {
                sheet_selection.Items.Add(sheet.Name);
            }
            i = 0;
            foreach (var item in sheet_selection.Items)
            {
                collection[i] = Mybook.Sheets[i+1];
                lastrows[i] = collection[i].Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                i++;
            }

        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            set();
            if (sheet_selection.SelectedIndex == -1)
            {
                Info_box.Text = "No sheet selected";
                return;
            }
            else
            {
                for (int i = lastrows[sheet_selection.SelectedIndex]; i >= 2; i--)
                {
                    readin(i, sheet_selection.SelectedIndex);
                }

                foreach(var item in list.transactions)
                {
                    if(item.Description.Contains(search_box.Text))
                    {
                        listView1.Items.Add(item);
                    }
                }
                //listView1.ItemsSource = list.transactions;
                total_label.Content = ("£" + list.Total());
                Info_box.Text =(collection[sheet_selection.SelectedIndex].Name + " selected");
            }
        }
    }
}
