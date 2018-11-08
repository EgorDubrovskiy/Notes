
using System.Collections.Generic;

namespace Notes.Models
{
    public class User
    {
        public int Id { get; set; }
        public string MainEmail { get; set; }
        public string SecretKey { get; set; }
        public string Password { get; set; }
        public string RestoringEmail { get; set; }

        public ICollection<Note> Notes { get; set; }

        public User()
        {
            Notes = new List<Note>();
        }
    }
}