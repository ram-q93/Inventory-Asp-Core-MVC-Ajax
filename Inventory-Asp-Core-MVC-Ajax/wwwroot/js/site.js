// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*
$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});
*/

showStorageInPopup = (url, title) => {
    // from this function we have to make jquery ajax get request to AddOrEditStorage method with int param in Storage controller
    $.ajax({
        type: "Get",
        url: url,
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
                    $("#view-all-storages").html(res.html);
                    $("#form-modal .modal-title").html('');
                    $("#form-modal .modal-body").html('');
                    $("#form-modal").modal('hide'); 
                    SubmitedSuccessfully("Storage");
                }
                else {
                    toastr.error(res.error)
                    $("#form-modal .modal-body").html(res.html);
                }
            },
            error: function (err) {
                toastr.error("Error in Submitting storage")
                console.log(err);
            }
        })
    }catch(e) {
        console.log(e);
    }
    //to prevent default form submit event
    return false;
};

jQueryAjaxDeleteStorage = form => {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons:true,
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
                            toastr.success("Storage Deleted successfully")
                            $("#view-all-storages").html(res.html);
                        } else {
                            toastr.error(res.error)
                            $("#view-all-storages").html(res.html);
                        }
                    },
                    error: function (err) {
                        toastr.error("Error in Deleting storage")
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
    }).then((willDelete) =>
    {
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
                        toastr.error("Error in deleting supplier" )
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

function searchStorage(searchQuery) {
    console.log(searchQuery);
    $.ajax({
        type: "GET",
        url: "Storage/Storages?query=" + searchQuery,
        //data: new { query = searchQuery },
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        success: function (res) {
            if (res.success) {
                $("#view-all-storages").html(res.html);
            } else {
                toastr.error(res.error)
            }
        },
        error: function (err) {
            toastr.error("Error")
            console.log("Error");
            console.log(err);
        }

    });
};