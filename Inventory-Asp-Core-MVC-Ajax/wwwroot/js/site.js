// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//========================================= Global ==========================================//
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
        background: 'Green',
        position: 'bottom-left',
        showConfirmButton: false,
        timer: 5000, 
        customClass: 'my-swal2-styling'
    }).fire({
        type: 'success',
        title: '<span style="color:floralwhite">Submited Successfuly</span>'
    })
};

function SweetAlertSubmitFailed(ErrorMessage) {  // confirmButtonText: 'Cool'
    Swal.mixin({
        toast: true,
        background: 'red',
        position: 'bottom-left',
        showConfirmButton: false,
        timer: 5000,
        customClass: 'my-swal2-styling'
    }).fire({
        type: 'error',
        title: '<span style="color:White" >' + ErrorMessage + '</span>'
    })
};
//========================================= Global ==========================================//


//========================================================================== Product ========//
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
//========================================================================== Product ========//


//============================================================== Supplier ===================//
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
//============================================================== Supplier ===================//


//=========================================================================== Storage =======//
https://www.thecodehubs.com/server-side-pagination-using-datatable-in-net-core/

var storageDataTable;
$(document).ready(function () {
    storageDataTable = $("#table-storage").DataTable({
       autoWidth: true,
       // processing: true,
        serverSide: true,
        paging: true,
      //  searching: { regex: true },
        responsive: true,
        scrollX: true,
        sDom: "ltip", 
        lengthMenu: [[8, 15, 20, 50], [8, 15, 20, 50]],
        columnDefs: [
            //{
            //    "searchable": false,
            //    "orderable": false,
            //    "targets": 0
            //},
            {
                "targets": [5, 6],
                "orderable": false,
                "searchable": false
            }],
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
            { data: "name", "autoWidth": true },
            { data: "phone", "autoWidth": true  },
            {
                data: "enabled",
                render: function (data, type, row) {
                    if (data == true) {
                        return `<div style="text-align:center">
                                        <i class="fas fa-check" style="color:blue"></i>
                                </div>`;
                    }
                    else
                        return ``;
                }, "autoWidth": true 
            },
            { data: "city", "autoWidth": true },
            { data: "address", "autoWidth": true  },
            {
                data: "id",
                render: function (data, type, row) {
                    return `<div style="text-align:center">
                                <a class="my-mousechange"  onclick="showStorageInPopup(${data},'UpdateStorage')">
                                     <i class="fas fa-edit fa-2x" style="color:green"></i>
                                </a>
                            </div>`;
                }, "autoWidth": true 
            },
            {
                data: "id",
                render: function (data, type, row) {
                    return `<div style="text-align:center">
                                <a class="my-mousechange" onclick="jQueryAjaxDeleteStorage(${data})">
                                    <i class="fas fa-trash fa-2x" style="color:red"></i>
                                </a>
                            </div>`;
                }, "autoWidth": true 
            }
        ]
    });
    //sDom : Perform these operations on datatables
    //'l' - Length changing
    //'f' - Filtering input
    //'t' - The table!
    //'i' - Information
    //'p' - Pagination
    //'r' - pRocessing
    //For removing default search box just remove the f character from sDom.
});

SearchStorage = () => {
    storageDataTable.search($("#search-storage-input-id").val()).draw();
}

showStorageInPopup = (id, title) => {
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
                    storageDataTable.draw();
                    SweetAlertSubmitedSuccessfully();
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
        if (result.value)
            try {
                $.ajax({
                    type: "Post",
                    url: "/Storage/Delete",
                    contentType: "application/x-www-form-urlencoded",
                    data: {
                        __RequestVerificationToken: $('#table-storage input[name="__RequestVerificationToken"]').val(),
                        id: id
                    },
                    success: function (res) {
                        if (res.success) {
                            storageDataTable.draw();
                            SweetAlertSubmitedSuccessfully();
                        } else {
                            storageDataTable.draw();
                            SweetAlertSubmitFailed(res.error)
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
//=========================================================================== Storage =======//