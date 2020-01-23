$(document).ready(function () {
    var table = $('#myTable').DataTable({
        "columnDefs": [{
            "orderable": false,
            "targets": 1
        }],
        "ajax": loadToDoList(),
        "responsive": true
    });
});
function ClearScreen() {
    $('#Id').val('');
    $('#name').val('');
    $('#Update').hide();
    $('#Save').show();
}
function loadToDoList() {
    debugger;
    $.ajax({
        url: "/User/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            debugger; 
            var html = '';
            $.each(result, function (key, ToDoList) {
                html += '<tr>';
                html += '<td>' + ToDoList.name + '</td>';
                html += '<td><a href="#" class="fa fa-pencil" data-toggle="tooltip" title="Edit" id="Update" onclick="return GetbyId(' + ToDoList.id + ')"></a> |';
                html += ' <a href="#" class="fa fa-trash" data-toggle="tooltip" title="Delete" id="Delete" onclick="return Delete(' + ToDoList.id + ')" ></button ></td > ';
                html += '</tr>';
                html += '</tr>';
                html += '</tr>';
            });
            $('.todolistbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function Save() {
    debugger;
    var ToDoList = new Object();
    ToDoList.id = $('#Id').val();
    ToDoList.name = $('#name').val();
    ToDoList.status = $('#status').val();
    $.ajax({
        type: 'POST',
        url: '/User/InsertOrUpdate/',
        data: ToDoList
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Insert Successfully'
            });
            loadToDoList();
        }
        else {
            Swal.fire('Error', 'Insert Fail', 'error');
            ClearScreen();
        }
    });
    //}
}