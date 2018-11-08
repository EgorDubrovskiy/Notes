using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notes.Models
{
    public class Note : ICloneable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public object Clone()
        {
            var NewNote = new
            {
                Title,
                Description,
                Date = MyLibrary.GetSimpleDate(Date)
            };
            return NewNote;
        }
    }

}