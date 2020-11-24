// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
                }
                else {
                    $("#form-modal .modal-body").html(res.html);
                }
            },
            error: function (err) {
                console.log(err);
            }
        })
        //to prevent default form submit event
        return false;
    }catch(e) {
        console.log(e);
    }
};