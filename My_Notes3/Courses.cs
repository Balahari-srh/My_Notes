using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Notes3
{
    public class Courses
    {
        public string CourseName { get; set; }
        public string ProfessorName { get; set; }
        public string ProfessorContact { get; set; }
        public Guid CourseID { get; set; } = Guid.NewGuid();
        public ObservableCollection<Notes> notes { get; set; }
        public ObservableCollection<Notes> Add_New_Note(String NoteTitle, String NoteContent, DateTime NoteDate)
        {
            if(notes ==  null)
            {
                notes = new ObservableCollection<Notes>();

            }
            var temp = new Notes { NoteTitle = NoteTitle, NoteContent = NoteContent, NoteDate = NoteDate };
            notes.Add(temp);
            return notes;

        }
        public void Del_Note(dynamic Notes_to_delete)
        {
            // var temp = from m in notes where m.NoteTitle.Equals(NoteTitle) select m;
            // var delete_note = temp as Notes;
            List<Notes> temp_copy = new List<Notes>();
            foreach (var item in Notes_to_delete)
            {
                temp_copy.Add(new Notes() { NoteID = item.NoteID });
            }
            foreach (var item in temp_copy)
            {
                var del_note = notes.FirstOrDefault(x=>x.NoteID==item.NoteID);
                notes.Remove(del_note);
            }
            //notes.Remove(NoteTitle);
            
        }
    }
}
