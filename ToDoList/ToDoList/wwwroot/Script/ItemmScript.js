var table = null;

$(document).ready(function () {
    //debugger;
    table = $('#myTable').DataTable({
        //"serverSide": true, // for process server side  
        //"processing": true, // for show progress bar  
        //"filter": true, // this is for disable filter (search box)  
        //"orderMulti": false, // for disable multiple column at once 
        "ajax": {
            //url: "/Supp/PageData/",
            url: "/Itemm/List/",
            type: "GET"
        },
        "responsive": true,
        "columnDefs":
            [{
                "targets": [0, 4],
                "orderable": false
            }],
        "columns": [
            { "data": "name" },
            { "data": "stock" },
            { "data": "price" },
            { "data": "suppname" },
            {
                "render": function (data, type, row) {
                    return '<button class="btn btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Update" onclick="return GetById(' + row.id + ')"> <i class="mdi mdi-pencil"></i></button > | <button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"> <i class="mdi mdi-eraser"></i></button >'
                }
            }]
    });
    LoadSupplier();
});

function ClearScreen() {
    $('#Id').val('');
    $('#name').val('');
    $('#stock').val('');
    $('#price').val('');
    $('#Supplier').val('');
    $('#Update').hide();
    $('#Save').show();
}

var Suppliers = []
function LoadSupplier(element) {
    //debugger;
    if (Suppliers.length == 0) {
        $.ajax({
            type: "Get",
            url: "/Supp/LoadSupplier",
            success: function (data) {
                debugger;
                Suppliers = data;
                renderSupplier(element);
            }
        })
    }
    else {
        renderSupplier(element);
    }
}

function renderSupplier(element) {
    //debugger;
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select Supplier'));
    $.each(Suppliers, function (i, val) {
        debugger;
        $ele.append($('<option/>').val(val.id).text(val.name));
    })
}
LoadSupplier($('#Supplier'));

function Save() {
    debugger;
    if ($('#name').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The Item Name',
            showConfirmButton: false,
            timer: 1500
        });
    } else {
        var Itemm = new Object();
        Itemm.id = $('#Id').val();
        Itemm.name = $('#name').val();
        Itemm.stock = $('#stock').val();
        Itemm.price = $('#price').val();
        Itemm.suppId = $('#Supplier').val();
        $.ajax({
            type: 'POST',
            url: '/Itemm/InsertOrUpdate/',
            data: Itemm
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
    }
}

function GetById(Id) {
    //debugger;
    $.ajax({
        url: "/Itemm/GetbyId/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        //data: { id: id },
        success: function (result) {
            debugger;
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#name').val(obj.name);
            $('#stock').val(obj.stock);
            $('#price').val(obj.price);
            $('#Supplier').val(obj.suppname);
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
            title: 'Please Full Fill The Item Name',
            showConfirmButton: false,
            timer: 1500
        });
    } else {
        debugger;
        var data = new Object();
        data.Id = $('#Id').val();
        data.Name = $('#name').val();
        data.Stock = $('#stock').val();
        data.Price = $('#price').val();
        data.suppId = $('#Supplier').val();
        $.ajax({
            url: "/itemm/InsertOrUpdate/",
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
            debugger;
            $.ajax({
                url: "/itemm/Delete/",
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
