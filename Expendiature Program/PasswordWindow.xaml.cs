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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace Expendiature_Program
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        string path = @"..\..\logonattempts.ini";
        string filepath;
        public PasswordWindow()
        {
            filepath = System.IO.Path.GetFullPath(path);
            Time();
            InitializeComponent();
        }

        public void Time()
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0)
            };
            timer.Tick += (o, e) =>
            {
                date_time.Content = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            };
            timer.Start();
        }

        private void enter_btn_Click(object sender, RoutedEventArgs e)
        {
            if(password_box.Password == Properties.Settings.Default.Password)
            {
                writeattempts(true);
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }
            else
            {
                notification.Content = "Password incorrect";
                writeattempts(false);
            }
        }

        private void writeattempts(bool correct)
        {
            string buffer;
            if (correct)
                buffer = "true";
            else
                buffer = "false";
            string[] read = File.ReadAllLines(filepath);
            if (read == null)
            {
                StreamWriter file = File.AppendText(filepath);
                file.WriteLine("Correct\tTime/Date");
                file.WriteLine(buffer + "\t" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                file.Close();
            }
            else
            {
                StreamWriter file = File.AppendText(filepath);
                file.WriteLine(buffer + "\t" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                file.Close();
            }
        }
    }
}
