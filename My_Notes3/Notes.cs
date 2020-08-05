using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Notes3
{
    public class Notes
    {
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }
        public DateTime NoteDate { get; set; }
        public Guid NoteID { get; set; } = Guid.NewGuid();
        //public ObservableCollection<Notes> notes { get; set; }
        //public ObservableCollection<> 
        public ObservableCollection<string> Img { get; set; } = new ObservableCollection<string>();
        public string CurrentImage { get; set; }

        // public ObservableCollection<Uri> imageuri;
        public void Add_New_Image(Notes n, string imguri)
        {
            if (Img == null)
            {
                Img = new ObservableCollection<string>();
            }
            //var temp = new Uri();
            Img.Add(imguri);
        }
    }
}
