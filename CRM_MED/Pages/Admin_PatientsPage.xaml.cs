using CRM_MED.Models;
using CRM_MED.Views;
using Microsoft.EntityFrameworkCore;
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

namespace CRM_MED.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin_PatientsPage.xaml
    /// </summary>
    public partial class Admin_PatientsPage : Page
    {
        public Admin_PatientsPage()
        {
            InitializeComponent();
            var context = new CRMmedContext();
            var patients = context.Patients.Include(cs => cs.GenderCodeNavigation).ToList().OrderBy(p => p.Surname);
            LViewPatient.ItemsSource = patients;
        }
        private void TBFind_change(object sender, TextChangedEventArgs e)
        {
            var context = new CRMmedContext();
            var patients = context.Patients.Where(p => p.Surname.ToLower().Contains(TBFind.Text.ToLower())).ToList();
            LViewPatient.ItemsSource = patients;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            AddPatientWindow add = new AddPatientWindow();
            add.Show();
        }

        private void SC(object sender, SelectionChangedEventArgs e)
        {
            var context = new CRMmedContext();
            var item = (ListView)sender;
            var pat = (Patient)item.SelectedItem;
            var patient = context.Patients.Where(p => p.Name == pat.Name).ToList();

            AddPatientWindow infoPatient = new AddPatientWindow(pat);
            infoPatient.ShowDialog();

        }
    }
}
