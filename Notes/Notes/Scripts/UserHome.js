var IsShowListNones = Boolean(false);

//показать список заметок
$(document).on("click", "#Notes-List-Show", function () {
    if (IsShowListNones) {
        IsShowListNones = Boolean(false);
        $('.Notes-List-Container').removeClass("active");
    } else {
        IsShowListNones = Boolean(true);
        $('.Notes-List-Container').addClass("active");
    }
});

//выбор заметки
$(document).on("click", ".list-group-item:not(.Example)", function () {
    $('.list-group-item.active').removeClass('active');
    $(this).addClass('active');
    var NoteId = $(this).attr("note-id");
    $.ajax({
        type: "POST", url: "http://localhost/Notes/Note/GetNote",
        data: { Id: NoteId},
        success: function (data) {
            $('#Note-Date').val(data.Date);
            $('#Note-Title').val(data.Title);
            CKEDITOR.instances['Note-Discription'].setData(data.Description);
        },
        crossDomain: true,
        dataType: 'json'
    });
});

//выход
$(document).on("click", "#user-exit", function () {
    $.ajax({
        type: "POST", url: "http://localhost/Notes/User/Exit",
        success: function () { window.location.replace("http://localhost/Notes/User/Home");}
    });
});

//удаление заметки
$(document).on("click", ".Notes-List-Item-Controll-Delete", function () {
    var Note = $(this).closest(".list-group-item");
    var note_id = Note.attr("note-id");
    $.ajax({
        type: "POST", url: "http://localhost/Notes/Note/Delete?Id=" + note_id,
        crossDomain: true,
        dataType: 'json'
    });
    Note.remove();
});


//удаление тестовой заметки
$(document).on("click", ".Notes-List-Item-Controll-Delete-Example", function () {
    if ($(".list-group-item").length < 2) return;
    var Note = $(this).closest(".list-group-item").remove();
});

//Очистка
$(document).on("click", "#ClearNote", function () { ClearNoteEditPanel();});

function ClearNoteEditPanel() {
    $('#Note-Date').val(GetSurDate(new Date()));
    $('#Note-Title').val('');
    CKEDITOR.instances['Note-Discription'].setData('');
}

//добавление заметки
$(document).on("click", "#AddNote", function () {
    var Title = $('#Note-Title').val();
    CKEDITOR.instances['Note-Discription'].updateElement();//обновляем данные об элементе
    var Description = CKEDITOR.instances['Note-Discription'].getData();
    var Date = $('#Note-Date').val().replace('T',' ');
    //alert(typeof (Title)); alert(typeof (Description)); alert(typeof(Date));
    $.ajax({
        type: "POST", url: "http://localhost/Notes/Note/Add",
        data: { Title: Title, Description: Description, Date: Date },
        success: function (Id)
        {
            var NewNote = "<a class=\"list-group-item list-group-item-action\" note-id=\"" + Id + "\">" +
                "<div class=\"w-100\">" +
                "<div class=\"float-right\"><img src = \"../Content/Images/delete-ico.png\" alt=\"Удалить\" title=\"Удалить\" " +
                "class=\"Notes-List-Item-Controll-Delete\"/></div>" +
                "<div class=\"Note-List-Item-Date\">" + Date + "</div>" +
                "</div>" +
                "<div class=\"Notes-List-Item-Title\">" + Title + "</div>" +
                "</a>";
            $(NewNote).prependTo('#list-note-container');
        },
        crossDomain: true,
        dataType: 'json'
    });
});

//редактирование заметки
$(document).on("click", "#SaveNote", function () {
    var Id = $('.list-group-item.active').attr('note-id');
    if (Id == undefined) {
        $('#messageBoxText').html('<div class="text-center mb-3">Выберите заметку!</div>');
        $('#signupFormOk').modal('show');
        return;
    }
    var Title = $('#Note-Title').val();
    CKEDITOR.instances['Note-Discription'].updateElement();//обновляем данные об элементе
    var Description = CKEDITOR.instances['Note-Discription'].getData();
    var MyDate = $('#Note-Date').val().replace('T', ' ');

    $.ajax({
        type: "POST", url: "http://localhost/Notes/Note/Edit",
        data: { Id: Id, Title: Title, Description: Description, Date: MyDate },
        success: function () {
            var MyFormatDate = GetSimpleDate(new Date(MyDate.replace(' ', 'T')));
            $('.list-group-item.active .Note-List-Item-Date').text(MyFormatDate);
            $('.list-group-item.active .Notes-List-Item-Title').text(Title);
        },
        crossDomain: true
    });
});

function GetSimpleDate(D)
{
    var MyDate = "";
    var Year = D.getFullYear();
    var Month = D.getMonth() + 1;
    var Day = D.getDate();
    var Hours = D.getHours();
    var Min = D.getMinutes();

    if (Day < 10) MyDate += "0" + Day + ".";
    else MyDate += Day+".";

    if (Month < 10) MyDate += "0" + Month + ".";
    else MyDate += Month + ".";


    if (Year < 10) MyDate += "0" + Year;
    else MyDate += Year;

    MyDate += " ";

    if (Hours < 10) MyDate += "0" + Hours + ":";
    else MyDate += Hours + ":";

    if (Min < 10) MyDate += "0" + Min;
    else MyDate += Min;

    return MyDate;
}

//для даты
function zeroPadded(val) {
    if (val >= 10)
        return val;
    else
        return '0' + val;
}

function GetSurDate(d) {
    return (d.getFullYear() + "-" + zeroPadded(d.getMonth() + 1) + "-" + zeroPadded(d.getDate()) +
        "T" + zeroPadded(d.getHours()) + ":" + zeroPadded(d.getMinutes()));
}

//настройки при старте страницы
jQuery(document).ready(function ($) {
    $('#Note-Date').val(GetSurDate(new Date()));

    $(".Notes-List-Item-Description").dotdotdot();

    CKEDITOR.replace("Note-Discription");
});

//модальное окно для уведомления пользователя
$('#signupFormOk').modal({
    backdrop: "static",
    show: false
});