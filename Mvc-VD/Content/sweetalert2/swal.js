/// <reference path="../../sweetalert2/sweetalert2.all.min.js" />

function swalSuccess() {
    return window.Swal.fire({
        title: "Good job !",
        text: "Success !",
        type: "success"
    });
};

function swalSuccessText(e) {
    return window.Swal.fire({
        title: "Good job !",
        text: e,
        type: "success"
    });
};

function swalYesNo() {
    return window.Swal.fire({
        title: "Are you sure ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: "Yes, I am sure !",
        cancelButtonText: "No !",
        animation: false,
        customClass: {
            popup: 'animated bounce'
        }
    });
};

function swalYesNoTitle(title) {
    return window.Swal.fire({
        title: title,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: "Yes, I am sure !",
        cancelButtonText: "No !",
        animation: false,
        customClass: {
            popup: 'animated bounce'
        }
    });
};