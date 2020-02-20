var table = null;

$(document).ready(function () {
    //debugger;
    table = $('#myTable').DataTable({
        "serverSide": true, // for process server side  
        //"processing": true, // for show progress bar  
        //"filter": true, // this is for disable filter (search box)  
        //"orderMulti": false, // for disable multiple column at once 
        "ajax": {
            url: "/Supp/PageData/",
            //url: "/Supp/List/",
            type: "GET"
        },
        "responsive": true,
        "columnDefs":
            [{
                "targets": [1],
                "orderable": false
            }],
        "columns": [
            { "data": "name" },
            {
                "render": function (data, type, row) {
                    return '<button class="btn btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Update" onclick="return GetById(' + row.id + ')"> <i class="mdi mdi-pencil"></i></button > | <button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"> <i class="mdi mdi-eraser"></i></button >'
                }
            }]
    });
});

function ClearScreen() {
    $('#Id').val('');
    $('#name').val('');
    $('#Update').hide();
    $('#Save').show();
}

function Save() {
    debugger;
    if ($('#name').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Supplier Name',
            showConfirmButton: false,
            timer: 1500
        });
    } else {
        var Supp = new Object();
        Supp.id = $('#Id').val();
        Supp.name = $('#name').val();
        $.ajax({
            type: 'POST',
            url: '/Supp/InsertOrUpdate/',
            data: Supp
        }).then((result) => {
            //debugger;
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
    }
}

function GetById(Id) {
    //debugger;
    $.ajax({
        url: "/Supp/GetbyId/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        //data: { id: id },
        success: function (result) {
            //debugger;
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#name').val(obj.Name);
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
    //debugger;
    if ($('#name').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Supplier Name',
            showConfirmButton: false,
            timer: 1500
        });
    } else {
        //debugger;
        var data = new Object();
        data.Id = $('#Id').val();
        data.Name = $('#name').val();
        $.ajax({
            url: "/Supp/InsertOrUpdate/",
            data: data
        }).then((result) => {
            //debugger;
            $('#myModal').hide();
            if (result) {
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
}

function Delete(id) {
    //debugger;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            //debugger;
            $.ajax({
                url: "/Supp/Delete/",
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