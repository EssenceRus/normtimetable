using CRM_MED.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CRM_MED.Views
{
    /// <summary>
    /// Логика взаимодействия для Log_Window.xaml
    /// </summary>
    public partial class Log_Window : Window
    {
        public Log_Window()
        {
            InitializeComponent();

            Login.IsEnabled = false;
            Password.IsEnabled = false;
        }
       

        private void Enter(object sender, RoutedEventArgs e)
        {
            if (Auth.SelectedIndex == 1)
            {
                using (var context = new CRMmedContext())
                {
                    var password = context.Admins.First().Password;
                    if (Password.Password == password)
                    {
                        AdminWindow AdminWin = new AdminWindow();
                        
                        AdminWin.Show();

                    }
                    else
                    {
                        MessageBox.Show("Admin пароль неверный");
                    }
                }
            }
            else
            {
                using (var context = new CRMmedContext())
                {

                    var bol = context.Doctors.Where(d => d.Login == Login.Text).Any();
                    if (bol)
                    {
                        var doctor = context.Doctors.Where(d => d.Login == Login.Text).Include(d => d.SpecialityNavigation).ToList();
                        if (Password.Password == doctor[0].Password && Login.Text == doctor[0].Login)
                        {
                            DoctorWindow dcwin = new DoctorWindow();
                            dcwin.doctor = doctor[0];
                            dcwin.ShowDialog();
                            
                            
                            

                        }
                        else
                        {
                            MessageBox.Show("Неверный пароль");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин");
                    }
                }
            }
        }

        private void AuthChange(object sender, SelectionChangedEventArgs e)
        {
            if (Auth.SelectedIndex == 1)
            {
                Login.Visibility = Visibility.Collapsed;
                Login.IsEnabled = true;
                Password.IsEnabled = true;
            }
            else if(Auth.SelectedIndex == 0)
            {
                Login.Visibility = Visibility.Visible;
                Login.IsEnabled = true;
                Password.IsEnabled = true;
            }
           
        }
    }
}
