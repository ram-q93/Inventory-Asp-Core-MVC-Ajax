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
    try{
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $("#view-all-storages").html(res.html);
                    $("#form-modal .modal-title").html('');
                    $("#form-modal .modal-body").html('');
                    $("#form-modal").modal('hide'); 
                    $.notify('Submited successfully', { globalPosition: 'top center', className: 'success' })
                    console.log("data : " + data);
                    console.log("res is Valid");
                }
                else {
                    $("#form-modal .modal-body").html(res.html);
                    console.log("res NOT Valid");
                }
            },
            error: function (err) {
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
    if (confirm('Are you sure to delete this form?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $("#view-all-storages").html(res.html);
                    $.notify('Deleted successfully', { globalPosition: 'top center', className: 'success' })
                },
                error: function (err) {
                    console.log(err);
                }
            })
        } catch (e) {
            console.log(e);
        }
    }
    //to prevent default form submit event
    return false;
};