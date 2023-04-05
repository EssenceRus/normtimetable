using CRM_MED.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для DoctorTimeTableWindow.xaml
    /// </summary>
    public partial class DoctorTimeTableWindow : Window
    {
        

        public Doctor _currentDoctor { get; set; }

        public ObservableCollection<WorkTime> workTimes;

        public List<DateTime> timeList = new List<DateTime>();
        public DoctorTimeTableWindow(Doctor doctor)
        {
            InitializeComponent();
            var context = new CRMmedContext();
            _currentDoctor = doctor;
            workTimes = new ObservableCollection<WorkTime>(context.WorkTimes.Include(w=>w.DayNavigation).Where(w => w.DoctorId == _currentDoctor.DoctorId).ToList());
            
            for (int j = 480; j < 1230; j += 30)
            {
                string hour = (j / 60).ToString();
                string minute = (j % 60).ToString();
                if (minute == "0")
                {
                    minute = "00";
                }
                timeList.Add(new DateTime(2000,1,1,int.Parse(hour),int.Parse(minute),0));
            }
            DG.ItemsSource = workTimes;
            CBworkstart.ItemsSource = timeList;
            CBworkend.ItemsSource = timeList;
            CBlaunchstart.ItemsSource = timeList;
            //CBlaunchend.ItemsSource = timeList;
            DataContext = this;
        }
        private void Load(object sender, RoutedEventArgs e)
        {
            var context = new CRMmedContext();
            //dayOfWeeks = context.DayOfWeeks.ToList();
            //var work = context.WorkTimes.ToList();
            
        }

        private void SaveTimeTable(object sender, RoutedEventArgs e)
        {
               
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 7; i++)
            {
                if (workTimes[i].WorkStart> workTimes[i].WorkEnd)
                {
                    sb.Append($"В {workTimes[i].DayNavigation.DayOfWeekName} время работы выставленно не корректно\n");
                }
                if (workTimes[i].WorkEnd.AddHours(-6) < workTimes[i].WorkStart)
                {
                    sb.Append($"В {workTimes[i].DayNavigation.DayOfWeekName} меньше 5 рабочих часов\n");
                }
                if (workTimes[i].WorkStart > workTimes[i].LunchStart || workTimes[i].WorkEnd < workTimes[i].LunchStart.AddHours(1));
                {
                    sb.Append($"В {workTimes[i].DayNavigation.DayOfWeekName} не корректно выставлено время обеда\n");
                }
            }
            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString());
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    workTimes[i].LunchEnd = workTimes[i].LunchStart.AddHours(1);
                }
                using (var context = new CRMmedContext())
                {
                    context.WorkTimes.UpdateRange(workTimes);
                    context.SaveChanges();
                }
            }

        }
    }
}
