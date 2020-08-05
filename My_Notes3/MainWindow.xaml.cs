using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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

namespace My_Notes3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> cultures = new List<string> { "en", "de", "ar" };
        public MainWindow()
        {
            App._lang = Properties.Settings.Default.language;
            CultureInfo.CurrentCulture = new CultureInfo(App._lang);
            CultureInfo.CurrentUICulture = new CultureInfo(App._lang);
            App._last_opened_course = Properties.Settings.Default.last_opened_course;
            InitializeComponent();
            //Grid_Parent.DataContext = App.courses; // this is the reason why course name is always being displayesd
           
       
            
               
            //Refresh_Combo_Box();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Combx_Language_select.ItemsSource = cultures;
            Combx_SelectCourse.ItemsSource = App.courses;
            Combx_SelectCourse.DataContext = App.courses;
            Combx_SelectCourse.SelectedIndex = App._last_opened_course;

            var selected_course = from m in App.courses select m;
            Lbx_Notes.DataContext = selected_course;
            var temp = selected_course as Courses;
            if (temp != null)
            {

                var notes_list = temp.notes;
                Lbx_Notes.ItemsSource = notes_list;
                Lbx_Notes.SelectedIndex = App._last_opened_note;
                

            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            //var lang = Combx_Language_select.SelectedItem as string;
            App._last_opened_course = Combx_SelectCourse.SelectedIndex;
            App._last_opened_note = Lbx_Notes.SelectedIndex;
            //App._lang = lang;
            Properties.Settings.Default.last_opened_course = App._last_opened_course;
            Properties.Settings.Default.last_opened_note = App._last_opened_note;
           // Properties.Settings.Default.language = App._lang;
            Properties.Settings.Default.Save();
        }

        private void Combx_SelectCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_course = Combx_SelectCourse.SelectedItem as Courses;
            //Tbx_ViewSelectedCourse.Text = selected_course.CourseName;
            SP_CourseName.DataContext = Combx_SelectCourse.SelectedItem;
            //SelectedCourse = selected_course.CourseName.ToLower();
            Refresh_List_Box(selected_course);
            
        }
        private void Refresh_List_Box(Courses course_name)
        {
            if (course_name != null)
            {
                var notes = course_name.notes;
                Lbx_Notes.ItemsSource = notes;
                Lbx_Notes.SelectedIndex = 0;
            }

        }
        private void Lbx_Notes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combx_SelectCourse.SelectedItem !=null)
            {
                var selected_note = Lbx_Notes.SelectedItem as Notes;
                Refresh_Img_ListBox(selected_note);
            }
            

        }

        private void Refresh_Img_ListBox(Notes note_name)
        {
            if(note_name != null && note_name.Img.Count>0)
            {
                Lbx_Multi_img.Visibility = Visibility.Visible;
                SL_ImgScale.Visibility = Visibility.Visible;
                SV_selected_image.Visibility = Visibility.Visible;
                Fr_NoImage.Visibility = Visibility.Hidden;
                List<BitmapImage> img_list = new List<BitmapImage>();
              
                {
                    var load_img = note_name.Img;
                    foreach (var item in load_img)
                    {
                        BitmapImage temp = new BitmapImage(new Uri(item == null ? "" : item.ToString()));
                        img_list.Add(temp);
                    }
                    Lbx_Multi_img.ItemsSource = img_list;
                }
            }
            else
            {
                Lbx_Multi_img.Visibility = Visibility.Hidden;
                SL_ImgScale.Visibility = Visibility.Hidden;
                SV_selected_image.Visibility = Visibility.Hidden;
                Fr_NoImage.Visibility = Visibility.Visible;

            }
            // Lbx_Multi_img.SelectedIndex = 1;
            
        }
        private void Btn_Add_New_Course_Click(object sender, RoutedEventArgs e)
        {
            var new_course = new Courses { CourseName = "New Course", ProfessorName = "Professor Name", ProfessorContact ="Email/Contact" };
            App.courses.Add(new_course);
            
            Combx_SelectCourse.SelectedItem = new_course;
            Tbx_Course_Name.Text = new_course.CourseName;
            Tbx_Course_Name.Focus();
            Tbx_Course_Name.SelectAll();

        }

    
   
        //Method to refresh and reload the list box after a note is created
        private void Refresh_List_Box_Chosen_Course(Courses course_name, Notes new_note)
        {
            if (course_name != null)
            {
                var notes = course_name.notes;
                Lbx_Notes.ItemsSource = notes;
                Lbx_Notes.ScrollIntoView(new_note);
                
                


            }
        }
        private void Btn_Add_New_Note_Click(object sender, RoutedEventArgs e)
        {
            var Course_Name = Combx_SelectCourse.SelectedItem as Courses;
            var New_Note = new Notes { NoteTitle = "Add Note Title", NoteContent = "Add your Contents here", NoteDate = DateTime.Today.Date };
            var temp_notes = Course_Name.Add_New_Note(New_Note.NoteTitle, New_Note.NoteContent, New_Note.NoteDate);
            Tbx_Note_Title.Focus();
            Tbx_Note_Title.SelectAll();
            Refresh_List_Box_Chosen_Course(Course_Name,New_Note);
        }

        private void Btn_Delete_Note_Click(object sender, RoutedEventArgs e)
        {
            var Course_Name = Combx_SelectCourse.SelectedItem as Courses;
            var selected_note = Lbx_Notes.SelectedItems;
            var selectednotecount = Lbx_Notes.SelectedItems.Count;
            if (Course_Name == null || selected_note == null)
            {
                MessageBox.Show("Please Select a course before proceeding to delete", "Error!");
            }
            string messageBoxText = "Are you sure you want to remove the "+selectednotecount+" selected notes?";
            string caption = "Delete Confirmation";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            //MessageBox.Show(messageBoxText, caption, button, icon);
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // User pressed Yes button
                    
                        Course_Name.Del_Note(selected_note);
                        Refresh_List_Box(Course_Name);
                    
                    break;
                case MessageBoxResult.No:
                    // User pressed No button
                    // ...
                    break;
                case MessageBoxResult.Cancel:
                    // User pressed Cancel button
                    // ...
                    break;
            }
           
            
            
          
        }

        private void Btn_Delete_Course_Click(object sender, RoutedEventArgs e)
        {
            var del_course = Combx_SelectCourse.SelectedItem;
            var del_course_name = Combx_SelectCourse.SelectedItem as Courses;
            var dname = del_course_name.CourseName;
            var cnt = Combx_SelectCourse.SelectedIndex;
            var notesincourse = Lbx_Notes.Items.Count;
            if (del_course == null)
            {
                MessageBox.Show("Please Select a Course to be Deleted", "Error");
            }
            string messageBoxText = "The "+dname+" course contains "+notesincourse+" notes. \nAre you sure you want to delete this course?";
            string caption = "Delete Confirmation";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Exclamation;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // User pressed Yes button
                    // ...
                    App.courses.Remove(del_course as Courses);
                    if (Combx_SelectCourse.SelectedIndex == -1)
                    {
                        Combx_SelectCourse.SelectedIndex = cnt - 1;
                    }
                    else
                    {
                        Combx_SelectCourse.SelectedIndex++;
                    }
                    break;
                case MessageBoxResult.No:
                    // User pressed No button
                    // ...
                    break;
                case MessageBoxResult.Cancel:
                    // User pressed Cancel button
                    // ...
                    break;
            }
            {
                
                
                //Refresh_Combo_Box();
                //Lbx_Notes.ItemsSource = null;
                // Refresh_List_Box(null);
            }
            

        }

        private void Btn_Add_Image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            //openFile.Filter = "Image Files(*.PNG;*.JPG)| *.PNG |*.JPG";
            if (openFile.ShowDialog() == true)
            {
                //string uriString = "test";
                Uri img_file = new Uri(openFile.FileName);
                //Uri uri = new Uri(uriString, UriKind.Absolute);
                //Imgbx_Main.Source = new BitmapImage(img_file);
                //Imgbx_Main.Stretch = Stretch.UniformToFill;
                var selected_note = Lbx_Notes.SelectedItem as Notes;
                string converteduri = img_file.ToString();
                selected_note.Add_New_Image(selected_note, converteduri);
                selected_note.CurrentImage = converteduri;
                Refresh_Img_ListBox(selected_note);
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show("Image added Successfully", "Success",button, icon);
                //Lbx_Notes_SelectionChanged(null, null);
                //Add_New_Image(Notes n, String ImageName)
                //IMG_Notes.Source = new BitmapImage(img_file);

            }
        }
       
        private void Tbx_Search_Notes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Tbx_Search_Notes.Focus();
            Tbx_Search_Notes.SelectAll();
        }
        private void Tbx_Search_Notes_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selected_course = Combx_SelectCourse.SelectedItem as Courses;
            if (selected_course != null)
            {
                string input = Tbx_Search_Notes.Text.ToLower();
                
                var lst = from m in selected_course.notes where m.NoteTitle.ToLower().Contains(input) | m.NoteContent.ToLower().Contains(input) select m;

                Lbx_Notes.ItemsSource = lst;
            }
            //else
            //{
            //    MessageBox.Show("Please select a Course before proceeding to Search/filter Notes contents", "Error!");
            //}
            
        }

        private void Btn_Delete_Image_Click(object sender, RoutedEventArgs e)
        {
            var index = Lbx_Multi_img.SelectedIndex;
            var notes = Lbx_Notes.SelectedItem as Notes;
            notes.Img.RemoveAt(index);
            Refresh_Img_ListBox(notes);
        }

        private void Combx_Language_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lang = Combx_Language_select.SelectedItem as string;
            App._last_opened_course = Combx_SelectCourse.SelectedIndex;
            App._last_opened_note = Lbx_Notes.SelectedIndex;
            App._lang = lang;
            Properties.Settings.Default.last_opened_course = App._last_opened_course;
            Properties.Settings.Default.last_opened_note = App._last_opened_note;
            Properties.Settings.Default.language = App._lang;
            Properties.Settings.Default.Save();
            Process.Start(Application.ResourceAssembly.Location);
            App.Current.Shutdown();
        }
    }
}
