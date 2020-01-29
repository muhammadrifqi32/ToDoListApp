$(document).ready(function () {
    var table = $('#myTable').DataTable({
        "columnDefs": [{
            "orderable": false,
            "targets": 2
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
    var todostatus = null;
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
                if (ToDoList.status == 0) {
                    todostatus = "On Progress";
                }
                else{
                    todostatus = "Done";
                }
                html += '<tr>';
                html += '<td>' + ToDoList.name + '</td>';
                html += '<td>' + todostatus + '</td>';
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
function GetbyId(Id) {
    debugger;
    $.ajax({
        url: "/User/GetbyId/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        //data: { id: id },
        success: function (result) {
            debugger;
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#name').val(obj.Name);
            $('#status').val(obj.Status);
            $('#myModal').modal('show');
            $('#Update').show();
            $('#Save').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
function Update() {
    debugger;
    var data = new Object();
    data.Id = $('#Id').val();
    data.Name = $('#name').val();
    data.Status = $('#status').val();
    $.ajax({
        url: "/User/InsertOrUpdate/",
        data: data
    }).then((result) => {
        debugger;
        $('#myModal').hide();
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 1500
            });
            loadToDoList();
        }
        else {
            Swal.fire('Error', 'Update Fail', 'error');
            ClearScreen();
        }
    });
}
function Delete(id) {
    debugger;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "/User/Delete/",
                type: "DELETE",
                data: { id: id },
                success: function (result) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully'
                    });
                    loadToDoList();
                },
                error: function (result) {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            });
        };
    });
}