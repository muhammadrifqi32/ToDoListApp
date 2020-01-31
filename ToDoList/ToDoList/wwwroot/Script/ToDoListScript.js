var table = null;

$(document).ready(function () {
    debugger;
    table = $('#myTable').DataTable({
        //"processing": true, // for show progress bar  
        //"serverSide": true, // for process server side  
        //"filter": true, // this is for disable filter (search box)  
        //"orderMulti": false, // for disable multiple column at once 
        "ajax": {
            url: "/User/List/" + $('#filter').val(),
            type: "GET"
        },
        "columnDefs":
            [{
                "targets": [1,2],
                "orderable": false
            }],
        "columns": [
            { "data": "name" },
            {
                "render": function (data, type, row) {
                    if (row.status == 0) {
                        return "On Progress";
                    }
                    else {
                        return "Done";
                    }
                }
            },
            {
                "render": function (data, type, row) {
                    return '<button class="btn btn-warning " data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')"> <i class="mdi mdi-pencil"></i></button >' + '&nbsp;' +
                        '<button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"> <i class="mdi mdi-eraser"></i></button >'
                }
            }]      
    });
});

$('#filter').change(function () {
    debugger;
    table.ajax.url('/User/list/' + $('#filter').val()).load();
})
function ClearScreen() {
    $('#Id').val('');
    $('#name').val('');
    $('#Update').hide();
    $('#Save').show();
}
//function loadToDoList() {
//    var todostatus = null;
//    debugger;
//    $.ajax({
//        url: "/User/List/" + $('#filter').val(),
//        type: "GET",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        async: false,
//        success: function (result) {
//            debugger;
//            var html = '';
//            $.each(result, function (key, ToDoList) {
//                if (ToDoList.status == 0) {
//                    todostatus = "On Progress";
//                }
//                else {
//                    todostatus = "Done";
//                }
//                html += '<tr>';
//                html += '<td>' + ToDoList.name + '</td>';
//                html += '<td>' + todostatus + '</td>';
//                html += '<td><a href="#" class="fa fa-pencil" data-toggle="tooltip" title="Edit" id="Update" onclick="return GetbyId(' + ToDoList.id + ')"></a> |';
//                html += ' <a href="#" class="fa fa-trash" data-toggle="tooltip" title="Delete" id="Delete" onclick="return Delete(' + ToDoList.id + ')" ></button ></td > ';
//                html += '</tr>';
//                html += '</tr>';
//                html += '</tr>';
//            });
//            $('.todolistbody').html(html);
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
////}
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
            table.ajax.reload();
        }
        else {
            Swal.fire('Error', 'Insert Fail', 'error');
            ClearScreen();
        }
    });
    //}
}
function GetById(Id) {
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
            table.ajax.reload();
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
                    table.ajax.reload();
                },
                error: function (result) {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            });
        };
    });
}