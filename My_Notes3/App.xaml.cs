using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace My_Notes3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string _lang = "en";
        public static ObservableCollection<Courses> courses;
        public static int _last_opened_course;
        public static bool disable_welcomescreen;
        public static MyConverter converter = new MyConverter();
        public static int _last_opened_note;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
            courses = MYStorage.ReadXml<ObservableCollection<Courses>>("Courses_newtest.xml");

            //if (courses == null)
            //{
            //    courses = new ObservableCollection<Courses>();
            //    //code to import file path
            //    OpenFileDialog openFile = new OpenFileDialog();
            //    if (openFile.ShowDialog() == true)
            //    {
            //        Uri img_file = new Uri(openFile.FileName);
                //}
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            
            MYStorage.WriteXml<ObservableCollection<Courses>>(courses, "Courses_newtest.xml");
            //export data with new window
           
        }
    }
}
