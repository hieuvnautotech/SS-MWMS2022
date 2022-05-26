$(function () {
    $(".dialog2").dialog({
        width: '70%',
        height: 700,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        zIndex: 1000,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
            $('#c_SumEditor').summernote({
                placeholder: 'Please give me content ....',
                tabsize: 2,
                height: 250,
                minHeight: 100,
                maxHeight: 800,
                focus: true,
                toolbar: [
                  ['style', ['bold', 'italic', 'underline', 'clear']],
                  ['font', ['strikethrough', 'superscript', 'subscript']],
                  ['fontsize', ['fontsize']],
                  ['color', ['color']],
                  ['para', ['ul', 'ol', 'paragraph']],
                  ['height', ['height']],
                  ['table', ['table']],
                  ['insert', ['link', 'picture', 'video']],
                  ['view', ['fullscreen', 'codeview', 'help']],
                ],
                popover: {
                    image: [
                      ['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
                      ['float', ['floatLeft', 'floatRight', 'floatNone']],
                      ['remove', ['removeMedia']]
                    ],
                    link: [
                      ['link', ['linkDialogShow', 'unlink']]
                    ],
                    table: [
                      ['add', ['addRowDown', 'addRowUp', 'addColLeft', 'addColRight']],
                      ['delete', ['deleteRow', 'deleteCol', 'deleteTable']],
                    ],
                    air: [
                      ['color', ['color']],
                      ['font', ['bold', 'underline', 'clear']],
                      ['para', ['ul', 'paragraph']],
                      ['table', ['table']],
                      ['insert', ['link', 'picture']]
                    ]
                },
                codemirror: {
                    theme: 'paper'
                }
            });
        },
    });


    $("#ac_save_but").click(function () {
        $('.dialog2').dialog('open');
        document.getElementById("form1").reset();
        document.getElementById("form2").reset();
        document.getElementById("form3").reset();
        $('#c_SumEditor').summernote('code', "");
        $("#doing_time_c").val("3");
        $("#errorTxt").empty();
        $(".info_customPhoto").empty();
    });
    $('#savestyle2').click(function () {
        if (!($("#form1").valid())) { return false; };
            var formData = new FormData;
            var fileUpload1 = $("#customFile").get(0);
            var files1 = fileUpload1.files;

            var fileUpload2 = $("#customPhoto").get(0);
            var files2 = fileUpload2.files;

            var fileUpload3 = $("#customFile2").get(0);
            var files3 = fileUpload3.files;

            var fileUpload4 = $("#customPhoto2").get(0);
            var files4 = fileUpload4.files;

            var fileUpload5 = $("#customFile3").get(0);
            var files5 = fileUpload5.files;

            var fileUpload6 = $("#customPhoto3").get(0);
            var files6 = fileUpload6.files;

            if (files1[0] != "undefined") { formData.append("file1", files1[0]); };
            if (files2[0] != "undefined") { formData.append("file2", files2[0]); };
            if (files3[0] != "undefined") { formData.append("file3", files3[0]); };
            if (files4[0] != "undefined") { formData.append("file4", files4[0]); };
            if (files5[0] != "undefined") { formData.append("file5", files5[0]); };
            if (files6[0] != "undefined") { formData.append("file6", files6[0]); };
            formData.append("old_no", $("#old_no_c").val()),
            formData.append("title", $("#issue_c").val()),
            formData.append("content", $('#c_SumEditor').summernote('code'));
            formData.append("doing_time", $("#doing_time_c").val());

        $.ajax({
            url: "/ActualWO/insertIssue",
            type: "POST",
            dataType: "json",
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.result == true) {
                    jQuery("#issueGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    alert("SUCCESS: GOOD JOB");
                }
                else {
                    alert('ERROR: Issue was existing. Please check again.');
                }
            },
            error: function (result) {
                alert('ERROR: Insert fail. Please check again.');
            }
        });
        //$('.dialog2').dialog('close');

    });

});
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$("#customPhoto").on('change', function () {
    var image_holder = $("#info_customPhoto");
    image_holder.empty();
    if (typeof (FileReader) != "undefined") {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("<img />", {
                "src": e.target.result,
                "width": "100px"
            }).appendTo(image_holder);
        }
        image_holder.show();
        reader.readAsDataURL($(this)[0].files[0]);
    }
});

$("#customPhoto2").on('change', function () {
    var image_holder = $("#info_customPhoto2");
    image_holder.empty();
    if (typeof (FileReader) != "undefined") {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("<img />", {
                "src": e.target.result,
                "width": "100px"
            }).appendTo(image_holder);
        }
        image_holder.show();
        reader.readAsDataURL($(this)[0].files[0]);
    }
});

$("#customPhoto3").on('change', function () {
    var image_holder = $("#info_customPhoto3");
    image_holder.empty();
    if (typeof (FileReader) != "undefined") {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("<img />", {
                "src": e.target.result,
                "width": "100px"
            }).appendTo(image_holder);
        }
        image_holder.show();
        reader.readAsDataURL($(this)[0].files[0]);
    }
});

$("#form1").validate({
    rules: {
        "fo_no": {
            required: true,
        },
    },
    errorLabelContainer: '.errorTxt',
});