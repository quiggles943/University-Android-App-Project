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

namespace Expendiature_Program
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void keypress(object sender, KeyEventArgs e)
        {
            if (new2.Password == new1.Password)
                passwordcorrect_label.Content = "Passwords match";
            else
                passwordcorrect_label.Content = "Passwords do not match";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (new2.Password == new1.Password)
            {
                Properties.Settings.Default.Password = new2.Password;
                Properties.Settings.Default.Save();
                this.Close();
            }
            else
                passwordcorrect_label.Content = "Passwords do not match";
        }
    }
}
