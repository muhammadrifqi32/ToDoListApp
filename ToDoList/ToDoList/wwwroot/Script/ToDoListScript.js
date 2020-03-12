var table = null;

$(document).ready(function () {
    //debugger;
    Bar();
    Donut();
    //Area();
    table = $('#myTable').DataTable({
        "serverSide": true, // for process server side  
        //"processing": true, // for show progress bar  
        //"filter": true, // this is for disable filter (search box)  
        //"orderMulti": false, // for disable multiple column at once 
        "ajax": {
            url: "/User/PageData/" + $('#filter').val(),
            type: "GET"
        },
        "responsive": true,
        "columnDefs":
            [{
                "targets": [0, 5],
                "orderable": false
            }],
        "columns": [
            {
                "render": function (data, type, row) {
                    if (row.status == 0) {
                        return '<button type="button" class="btn btn-secondary" id="Checked" onclick="return Checkedlist(' + row.id + ')"><i class="fa fa-square-o" title="Checked"></i></button>';
                    }
                    else {
                        return '<button disabled type="button" class="btn btn-secondary" id="Unchecked" onclick="return Uncheckedlist(' + row.id + ')"><i class="fa fa-check-square-o" title="Unchecked"></i></button>';
                    }
                }
            },
            { "data": "name" },
            {
                "data": "createDate", "render": function (data) {
                    //debugger;
                    return moment(data).tz("Asia/Jakarta").format('MMMM Do YYYY, h:mm:ss a');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Done Yet";
                    var nulldate = "0001-01-01T00:00:00+00:00";
                    //debugger;
                    if (data == nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).tz("Asia/Jakarta").format('MMMM Do YYYY, h:mm:ss a');
                    }
                }
            },
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
                    if (row.status == 1) {
                        return '<button disabled class="btn btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Update" onclick="return GetById(' + row.id + ')"> <i class="mdi mdi-pencil"></i></button > | <button disabled class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"> <i class="mdi mdi-eraser"></i></button >'
                    }
                    else {
                        return '<button class="btn btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Update" onclick="return GetById(' + row.id + ')"> <i class="mdi mdi-pencil"></i></button > | <button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"> <i class="mdi mdi-eraser"></i></button >'
                    }
                }
            }]
    });
});

$('#filter').change(function () {
    //debugger;
    table.ajax.url('/User/PageData/' + $('#filter').val()).load();
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

//Morris Bar

//First Version
function Bar() {
    //debugger;
    $.ajax({
        type: 'GET',
        url: '/User/GetStatus/',
        success: function (data) {
            debugger;
            Morris.Bar({
                element: 'morris-bar-chart',
                data: $.each(JSON.parse(data), function (index, val) {
                    if (val.Status == 0) {
                        val.Status = "On Progress";
                    }
                    else {
                        val.Status = "Done";
                    }
                    [{
                        Status: val.Status,
                        Total: val.Total
                    }]
                }),
                xkey: 'Status',
                ykeys: ['Total'],
                labels: ['ToDoList Progress'],
                barColors: ['#55ce63', '#2f3d4a', '#009efb'],
                hideHover: 'auto',
                gridLineColor: '#eef0f2',
                resize: true
            });
        }
    })
};

function Donut() {
    //debugger;
    $.ajax({
        type: 'GET',
        url: '/User/GetStatusDonut/',
        success: function (data) {
            //debugger;
            Morris.Donut({
                element: 'morris-donut-chart',
                data: $.each(JSON.parse(data), function (index, val) {
                    if (val.label == 0) {
                        val.label = "On Progress";
                    }
                    else {
                        val.label = "Done";
                    }
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                resize: true,
                colors: ['#009efb', '#55ce63', '#2f3d4a']
            });
        }
    })
};

//function Area() {
//    //debugger;
//    $.ajax({
//        type: 'GET',
//        url: '/User/GetStatus/',
//        success: function (data) {
//            //debugger;
//            Morris.Area({
//                element: 'morris-area-chart',
//                data: $.each(JSON.parse(data), function (index, val) {
//                    if (val.Status == 0) {
//                        val.Status = "On Progress";
//                    }
//                    else {
//                        val.Status = "Done";
//                    }
//                    [{
//                        Status: val.Status,
//                        Total: val.Total
//                    }]
//                }),
//                lineColors: ['#55ce63', '#2f3d4a', '#009efb'],
//                xkey: 'Status',
//                ykeys: ['Total'],
//                labels: ['ToDoList Progress'],
//                pointSize: 0,
//                lineWidth: 0,
//                resize: true,
//                fillOpacity: 0.8,
//                behaveLikeLine: true,
//                gridLineColor: '#e0e0e0',
//                hideHover: 'auto'
//            });
//        }
//    })
//};
//First Version


//Second Version
//function Bar() {
//    //debugger;
//    $.ajax({
//        type: 'GET',
//        url: '/User/GetStatus/',
//    }).then((result) => {
//        if (result != "internal server error") {
//            $("#morris-bar-chart").show();
//            $("#morris-no-data-found").hide();
//            chart.setData(JSON.parse(result));
//        }
//        else {
//            $("#morris-bar-chart").hide();
//            $("#morris-no-data-found").show();
//            document.getElementById("morris-no-data-found").innerHTML = "internal server error";
//        }
//    });
//}
//var chart = Morris.Bar({
//    element: 'morris-bar-chart',
//    data: [0, 0],
//    xkey: 'Status',
//    ykeys: ['Total'],
//    labels: ['ToDoList Progress'],
//    barColors: ['#55ce63', '#2f3d4a', '#009efb'],
//    resize: true
//});
//Second Version

//Morris Bar

function Save() {
    //debugger;
    if ($('#name').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The To Do List Name',
            showConfirmButton: false,
            timer: 1500
        });
    } else {
        var ToDoList = new Object();
        ToDoList.id = $('#Id').val();
        ToDoList.name = $('#name').val();
        $.ajax({
            type: 'POST',
            url: '/User/InsertOrUpdate/',
            data: ToDoList
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
        //}
    }
}
function GetById(Id) {
    //debugger;
    $.ajax({
        url: "/User/GetbyId/" + Id,
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
    //debugger;
    if ($('#name').val() == 0) {
        Swal.fire({
            position: 'center',
            type: 'error',
            title: 'Please Full Fill The To Do List Name',
            showConfirmButton: false,
            timer: 1500
        });
    } else {
        var data = new Object();
        data.Id = $('#Id').val();
        data.Name = $('#name').val();
        $.ajax({
            url: "/User/InsertOrUpdate/",
            data: data
        }).then((result) => {
            //debugger;
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
function Checkedlist(Id) {
    //debugger;
    $.ajax({
        url: "/User/CheckedTodoList/",
        data: { id: Id },
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            table.ajax.reload();
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
        }
    });
}
function Uncheckedlist(Id) {
    //debugger;
    $.ajax({
        url: "/User/UncheckedTodoList/",
        data: { id: Id },
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            table.ajax.reload();
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
        }
    });
}