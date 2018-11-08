using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notes.Helpers
{
    public static class ListNotesItemHelper
    {
        public static MvcHtmlString CreateList(this HtmlHelper html, List<Note> notes)
        {
            string a = "";
            if(notes.Count==0)
            {
                a += "<a class=\"list-group-item list-group-item-action Example\" \">" +
                         "<div class=\"w-100\">" +
                             "<div class=\"float-right\"><img src = \"/Notes/Content/Images/delete-ico.png\" alt=\"Удалить\" title=\"Удалить\" " +
                             "class=\"Notes-List-Item-Controll-Delete-Example\"/></div>" +
                             "<div class=\"Note-List-Item-Date\">" + MyLibrary.GetSimpleDateFormat2(DateTime.Now) + "</div>" +
                         "</div>" +
                         "<div class=\"Notes-List-Item-Title\">Пример заголовка заметки</div>" +
                     "</a>";
            }
            else
            for (int i=0; i< notes.Count;i++)
            {
                a += "<a class=\"list-group-item list-group-item-action\" note-id=\""+notes[i].Id+"\">"+
                        "<div class=\"w-100\">" +
                            "<div class=\"float-right\"><img src = \"/Notes/Content/Images/delete-ico.png\" alt=\"Удалить\" title=\"Удалить\" " +
                            "class=\"Notes-List-Item-Controll-Delete\"/></div>" +
                            "<div class=\"Note-List-Item-Date\">"+ MyLibrary.GetSimpleDateFormat2(notes[i].Date)+"</div>" +
                        "</div>" +
                        "<div class=\"Notes-List-Item-Title\">"+ notes[i].Title+"</div>" +
                    "</a>";
            }

            string res = 
                "<div class=\"row m-0\">" +
                     "<div class=\"col-12 p-0\">" +
                        "<div class=\"list-group\" id=\"list-note-container\">"+
                            a+
                        "</div>" +
                     "</div>" +
                "</div>";
            return new MvcHtmlString(res);
        }
    }
}