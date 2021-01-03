// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

function DeletePopUp() {
    return Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    });
}

function SweetAlertSubmitedSuccessfully() {
    Swal.mixin({
        toast: true,
        background: 'MediumSeaGreen',
        position: 'bottom-right',
        showConfirmButton: false,
        timer: 3000
    }).fire({
        type: 'success',
        title: '<span style="color:floralwhite">Submited Successfuly </span>'
    })
};

function SweetAlertSubmitFailed(ErrorMessage) {  // confirmButtonText: 'Cool'
    Swal.mixin({
        toast: true,
        position: 'bottom-right',
        showConfirmButton: false,
        timer: 5000
    }).fire({
        type: 'error',
        title: '<span style="color:Tomato" >' + ErrorMessage + '</span>'
    })
};



function SubmitedSuccessfully(title) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    Command: toastr["success"](title + " Submited successfully")
};

showProductInPopup = (url, title) => {
    $.ajax({
        type: "Get",
        url: url,
        success: function (res) {
            $("#product-form-modal .modal-title").html(title);
            $("#product-form-modal .modal-body").html(res);
            $("#product-form-modal").modal('show');
        }
    })
};

jQueryAjaxPostToAddOrEditProduct = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.success) {
                    $("#view-all-products").html(res.html);
                    $("#product-form-modal .modal-title").html('');
                    $("#product-form-modal .modal-body").html('');
                    $("#product-form-modal").modal('hide');
                    SubmitedSuccessfully("Product");
                }
                else {
                    toastr.error(res.error)
                    $("#product-form-modal .modal-body").html(res.html);
                }
            },
            error: function (err) {
                toastr.error("Error in submitting product")
                console.log(err);
            }
        })
    } catch (e) {
        console.log(e);
    }
    //to prevent default form submit event
    return false;
};

jQueryAjaxDeleteProduct = form => {
    swal({
        title: "Are you sure?",
        text: "Once deleted Product, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete)
            try {
                $.ajax({
                    type: 'POST',
                    url: form.action,
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        console.log("ressssssss " + res);

                        if (res.success) {
                            toastr.success("Product Deleted successfully")
                            $("#view-all-products").html(res.html);
                        } else {
                            toastr.error(res.error)
                            $("#view-all-products").html(res.html);
                        }
                    },
                    error: function (err) {
                        toastr.error("Error in Deleting Product")
                        console.log(err);
                    }
                })
            } catch (e) {
                console.log(e);
            }
    });
    //to prevent default form submit event
    return false;
};

showProductDetailsInPopup = (url, title) => {
    $.ajax({
        type: "Get",
        url: url,
        success: function (res) {
            $("#product-form-modal .modal-title").html(title);
            $("#product-form-modal .modal-body").html(res);
            $("#product-form-modal").modal('show');
        }
    })
};

showSupplierInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#supplier-form-modal .modal-title").html(title);
            $("#supplier-form-modal .modal-body").html(res);
            $("#supplier-form-modal").modal('show');
        }
    })
};

jQueryAjaxPostToAddOrEditSupplier = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.success) {
                    $("#view-all-suppliers").html(res.html);
                    $("#supplier-form-modal .modal-title").html('');
                    $("#supplier-form-modal .modal-body").html('');
                    $("#supplier-form-modal").modal('hide');
                    SubmitedSuccessfully("Supplier");
                }
                else {
                    toastr.error(res.error)
                    $("#supplier-form-modal .modal-body").html(res.html);
                }
            },
            error: function (err) {
                toastr.error("Error in submitting supplier")
                console.log(err);
            }
        })
    } catch (e) {
        console.log("hereee");
        console.log(e);
    }
    //to prevent default form submit event
    return false;
};

jQueryAjaxDeleteSupplier = form => {
    swal({
        title: "Are you sure?",
        text: "Once deleted supplier, \"All\" products of this supplier will permanently go away!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete)
            try {
                $.ajax({
                    type: 'POST',
                    url: form.action,
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        if (res.success) {
                            toastr.success("Suplier deleted successfully")
                            $("#view-all-suppliers").html(res.html);
                        } else {
                            toastr.error(res.error)
                            $("#view-all-suppliers").html(res.html);
                        }
                    },
                    error: function (err) {
                        toastr.error("Error in deleting supplier")
                        console.log(err);
                    }
                })
            } catch (e) {
                console.log(e);
            }
    });
    //to prevent default form submit event
    return false;
};

https://www.thecodehubs.com/server-side-pagination-using-datatable-in-net-core/
$(document).ready(function () {
    var table =$("#table-storage").DataTable({
        autoWidth: true,
        processing: true,
        serverSide: true,
        paging: true,
        searching: { regex: true },
        responsive: true,
        lengthMenu: [[5, 10, 15, 20], [5, 10, 15, 20]],
        columnDefs: [{
            "targets": [5, 6],
            "orderable": false,
            "searchable": false
        }],
       // sDom: "ltipr", to delete search button
        ajax: {
            url: "/Storage/Storages",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (data) {
                return JSON.stringify(data);
            }
        },
        columns: [
            { data: "name" },
            { data: "phone" },
            {
                data: "enabled",
                render: function (data, type, row) {
                    if (data == true) {
                        return `<div style="text-align:center">
                                    <button class="btn btn-primary btn-sm btn-circle"  disabled>
                                        <i class="fas fa-check"></i>
                                    </button>
                                </div>`;
                    }
                    else
                        return ``;
                }
            },
            { data: "city" },
            {data: "address"},
            {
                data: "id",
                render: function (data, type, row) {
                    return `<div style="text-align:center">
                                <a class="btn btn-warning btn-sm btn-circle" onclick="showStorageInPopup(${data},'UpdateStorage')">
                                     <i class="fas fa-exclamation-triangle text-white"></i>
                                </a>
                            </div>`;
                }
            },
            {
                data: "id",
                render: function (data, type, row) {
                    return `<div style="text-align:center">
                                <a class="btn btn-danger btn-sm btn-circle" onclick="jQueryAjaxDeleteStorage(${data})">
                                    <i class="fas fa-trash text-white"></i>
                                </a>
                            </div>`;
                }
            }
        ]
    });
    //$('#search-btn-id').on('keyup click', function () {
    //    table.search($('#search-input-id').val()).draw();
    //});


    //table.on('order.dt search.dt', function () {
    //    table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //        cell.innerHTML = i + 1;
    //    });
    //}).draw();

    //Perform these operations on datatables
    //'l' - Length changing
    //'f' - Filtering input
    //'t' - The table!
    //'i' - Information
    //'p' - Pagination
    //'r' - pRocessing
    //For removing default search box just remove the f character from sDom.
});

showStorageInPopup = (id, title) => {
    // from this function we have to make jquery ajax get request 
    //to AddOrEditStorage method with int param in Storage controller
    $.ajax({
        type: "Get",
        url: "/Storage/AddOrEditStorage?id=" + id,
        success: function (res) {
            $("#form-modal .modal-title").html(title);
            $("#form-modal .modal-body").html(res);
            $("#form-modal").modal('show');
        }
    })
};

jQueryAjaxPostToAddOrEditStorage = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.success) {
                    $("#form-modal .modal-title").html('');
                    $("#form-modal .modal-body").html('');
                    $("#form-modal").modal('hide');
                    SweetAlertSubmitedSuccessfully();
                    window.location.href = 'Storages';
                }
                else {
                    SweetAlertSubmitFailed(res.error)
                    $("#form-modal .modal-body").html(res.html);
                }
            },
            error: function (err) {
                SweetAlertSubmitFailed("Error in Submitting storage")
                console.log(err);
            }
        })
    } catch (e) {
        console.log(e);
    }
    // to prevent default form submit event
    return false;
};

jQueryAjaxDeleteStorage = (id) => {
    DeletePopUp().then((result) => {
        console.log(id+" hhhhh  "+ $('#RequestVerificationToken').val())
        if (result.value)
            try {
                $.ajax({
                    type: 'Get',
                    url: "/Storage/Delete?id="+id,
                    //data: JSON.stringify({
                    //    // __RequestVerificationToken: $('#RequestVerificationToken').val(),
                    //    id: "78"
                    //}),
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        if (res.success) {
                           
                           window.location.href = 'Storages';
                          //  table.ajax.reload();
                            SweetAlertSubmitedSuccessfully();
                        } else {
                            SweetAlertSubmitFailed(res.error)
                            window.location.href = 'Storages';
                        }
                    },
                    error: function (err) {
                        SweetAlertSubmitFailed("Error in Deleting storage");
                    }
                })
            } catch (e) {
                console.log(e);
            }
    });
    //to prevent default form submit event
    return false;
};
