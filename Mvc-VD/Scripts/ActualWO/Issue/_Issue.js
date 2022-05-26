$(function () {
    var row_id, row_id1, row_id2;
    $grid = $("#issueGrid").jqGrid({
        url: "/ActualWO/getIssue",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'mdii', name: 'mdii', width: 50, align: 'right', hidden: true },
            { label: 'doing_time', name: 'doing_time', width: 100, hidden: true },
            { label: 'WO NO', name: 'fo_no', width: 120, align: 'center' },
            { label: 'Product Code', name: 'style_no', width: 250, align: 'left' },
            { label: 'Product Name', name: 'style_nm', width: 250, align: 'left' },
            { label: 'Routing No', name: 'line_no', width: 120, align: 'center' },
            { label: 'Process', name: 'process_nm', width: 200, align: 'left' },
            { label: 'Start Date', name: 'start_dt', align: 'center', width: 120 },
            { label: 'End Date', name: 'end_dt', align: 'center', width: 120 },
            {
                label: 'Issue', name: 'title', width: 250, align: 'left', formatter: popupView, exportoptions: {
                    excel_parsers: true, excel_format: '',
                    replace_format: null
                }
            },
            { label: 'Modify', name: 'Modifly', width: 110, align: 'center', formatter: bntCellValue },
            { label: 'Issue', name: 'title2', width: 250, align: 'left', hidden: true },
            { label: 'content', name: 'content', width: 250, align: 'left', hidden: true },
            { label: 'pi', name: 'id1', width: 100, hidden: true },
            { label: 'photo_file', name: 'pt1', width: 100, hidden: true },
            { label: 'fi', name: 'idfi1', width: 100, hidden: true },
            { label: 'attach_file', name: 'fi1', width: 100, hidden: true },
            { label: 'pi', name: 'id2', width: 100, hidden: true },
            { label: 'photo_file', name: 'pt2', width: 100, hidden: true },
            { label: 'fi', name: 'idfi2', width: 100, hidden: true },
            { label: 'attach_file', name: 'fi2', width: 100, hidden: true },
            { label: 'pi', name: 'id3', width: 100, hidden: true },
            { label: 'photo_file', name: 'pt3', width: 100, hidden: true },
            { label: 'fi', name: 'idfi3', width: 100, hidden: true },
            { label: 'attach_file', name: 'fi3', width: 100, hidden: true },
            { label: 'old_no', name: 'old_no', width: 250, align: 'left', hidden: true },
            { label: 'Photo Qty', name: 'photo_qty', width: 200, align: 'right', formatter: function (cellValue, options, rowdata, action) { var html = (cellValue == null) ? '0' : cellValue; return html; } },
            { label: 'File Qty', name: 'file_qty', width: 120, align: 'right', formatter: function (cellValue, options, rowdata, action) { var html = (cellValue == null) ? '0' : cellValue; return html; } },
            { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
            { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change User', name: 'chg_id', width: 90, align: 'center' },
            { label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#issueGrid").jqGrid("getGridParam", 'selrow');
            row_id = $("#issueGrid").getRowData(selectedRowId);
            if (row_id != null) {
                $("#bdel_save_but").attr("disabled", false);
                $("#mdii").val(row_id.mdii);
                // Reset data
                $('#customFile_n').addClass("selected").html("");
                $('#customPhoto_n').addClass("selected").html("");
                $('#customFile_n2').addClass("selected").html("");
                $('#customPhoto_n2').addClass("selected").html("");
                $('#customFile_n3').addClass("selected").html("");
                $('#customPhoto_n3').addClass("selected").html("");

                $('#btndel_file_save_but').attr("disabled", true);
                $('#btndel_file_save_but2').attr("disabled", true);
                $('#btndel_file_save_but3').attr("disabled", true);
                $('#btndel_photo_save_but').attr("disabled", true);
                $('#btndel_photo_save_but2').attr("disabled", true);
                $('#btndel_photo_save_but3').attr("disabled", true);


                // Set data
                $('#mdii_m').val(row_id.mdii);
                $('#old_no_m').val(row_id.old_no);
                $('#fo_no_m').val(row_id.fo_no);
                $('#style_no_m').val(row_id.style_no);
                $('#style_nm_m').val(row_id.style_nm);
                $('#style_no_m').val(row_id.style_no);
                $('#process_nm_m').val(row_id.process_nm);
                $('#start_m').val(row_id.start_dt);
                $('#end_m').val(row_id.end_dt);
                $('#issue_m').val(row_id.title2);
                (row_id.doing_time == null || row_id.doing_time == '') ? $('#doing_time_m').val("0") : $('#doing_time_m').val(row_id.doing_time);
                loadIMG(row_id.mdii, row_id.old_no);
                getIssueView2(row_id.mdii, row_id.style_no, row_id.style_nm, row_id.fo_no, row_id.title2, row_id.process_nm, row_id.old_no, row_id.fi1, row_id.fi2, row_id.fi3);


                file = row_id.fi1;
                img = row_id.pt1;
                file2 = row_id.fi2;
                img2 = row_id.pt2;
                file3 = row_id.fi3;
                img3 = row_id.pt3;
                $('#pi_m').val(row_id.id1);
                $('#fi_m').val(row_id.idfi1);
                $('#pi_m2').val(row_id.id2);
                $('#fi_m2').val(row_id.idfi2);
                $('#pi_m3').val(row_id.id3);
                $('#fi_m3').val(row_id.idfi3);
                $('#customFile_n').addClass("selected").html(file);
                $('#customPhoto_n').addClass("selected").html(img);
                $('#customFile_n2').addClass("selected").html(file2);
                $('#customPhoto_n2').addClass("selected").html(img2);
                $('#customFile_n3').addClass("selected").html(file3);
                $('#customPhoto_n3').addClass("selected").html(img3);

                $('#customPhoto_m').val(row_id.id1);
                $('#customFile_m').val(row_id.idfi1);
                $('#customPhoto2_m').val(row_id.id2);
                $('#customFile2_m').val(row_id.idfi2);
                $('#customPhoto3_m').val(row_id.id3);
                $('#customFile3_m').val(row_id.idfi3);


                //if (row_id.idfi1 != "") {
                //    $("#btndel_file_save_but").attr("disabled", false);
                //}

                //if (row_id.idfi2 != "") {
                //    $("#btndel_file_save_but2").attr("disabled", false);
                //}

                //if (row_id.idfi3 != "") {
                //    $("#btndel_file_save_but3").attr("disabled", false);
                //}
                //if (row_id.id1 != "") {
                //    $("#btndel_photo_save_but").attr("disabled", false);
                //}

                //if (row_id.id2 != "") {
                //    $("#btndel_photo_save_but2").attr("disabled", false);
                //}

                //if (row_id.id3 != "") {
                //    $("#btndel_photo_save_but3").attr("disabled", false);
                //}
            }
        },
        gridComplete: function () {
            var rows = $("#issueGrid").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var fo_no = $("#issueGrid").getCell(rows[i], "fo_no");
                var process_nm = $("#issueGrid").getCell(rows[i], "process_nm");
                var start_dt = $("#issueGrid").getCell(rows[i], "start_dt");
                var condition_fo_no = $('#condition_fo_no').val();
                var condition_process_nm = $('#condition_process_nm').val();
                var condition_start_dt = $('#condition_start_dt').val();
                if ((fo_no == condition_fo_no) && (process_nm == condition_process_nm) && (start_dt == condition_start_dt)) {
                    $("#issueGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                }
            }
        },
        pager: "#issueGridPager",
        viewrecords: true,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: true,
        height: 500,
        width: $(".boxA").width(),
        autowidth: false,
        caption: 'Issue',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        loadonce: true,
        shrinkToFit: false,
        multiselect: false,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
    });

    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    $('#issueGrid').jqGrid('setGridWidth', $(".boxA").width());
    $(window).on("resize", function () {
        var newWidth = $("#issueGrid").closest(".ui-jqgrid").parent().width();
        $("#issueGrid").jqGrid("setGridWidth", newWidth, false);
    });

    /////////////////////////////////////////////////-------UPDATE------//////////////////////////////////////////////////////////

    function loadIMG(mdii, old_no) {
        $.ajax({
            url: "/ActualWO/getIMG?mdii=" + mdii + "&old_no=" + old_no,
            type: "get",
            dataType: "json",
            success: function (data) {
                var html = ''
                if (data != null && data != undefined) {

                    $.each(data, function (key, item) {
                        html += '<div> <img src="/Images/Issue/' + item.photo_file + '" style="height:290px" /> </div>'

                    });
                }
                $("#photo_item").html(html);
                $("#photo_item").slick({
                    lazyLoad: 'ondemand',
                    infinite: true,//Hiển thì 2 mũi tên
                    accessibility: true,
                    vertical: false,//Chay dọc
                    slidesToShow: 1,    //Số item hiển thị
                    slidesToScroll: 1, //Số item cuộn khi chạy
                    autoplay: true,  //Tự động chạy
                    autoplaySpeed: 3,  //Tốc độ chạy
                    speed: 3000,
                    arrows: true, //Hiển thị mũi tên
                    centerMode: false,  //item nằm giữa
                    dots: true,  //Hiển thị dấu chấm
                    draggable: true,  //Kích hoạt tính năng kéo chuột
                });
            },
            error: function (data) {
            }
        });
    }





    $("#searchBtn").click(function () {
        var wr = $("#s_fo_no").val().trim(),
            process = $("#s_process_nm").val().trim(),
            style = $("#s_style_no").val().trim(),
            start = $("#start").val(),
            end = $("#end").val()
        title = $("#s_title").val();

        $.ajax({
            url: "/ActualWO/searchIssue",
            type: "get",
            dataType: "json",
            data: {
                wr: wr,
                process: process,
                style: style,
                start: start,
                end: end,
                title: title
            },
            success: function (result) {
                $("#issueGrid").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });
});
//desplay
function bntCellValue(cellValue, options, rowdata, action) {
    var mdii = rowdata.mdii;
    var html = '<button style="color: dodgerblue;border: none;background: none; " onclick="modifly(' + mdii + ');"><i class="fa fa-edit fa-2x text-warning"></i></button>';
    return html;
};



function viewdetailnotice(mdii) {
    $.get("/ActualWO/viewdetailnotice?mdii=" + mdii, function (data) {
        $('#m_SumEditor').summernote('code', data[0].content);
    });
}

function getIssueView2(mdii, style_no, style_nm, fo_no, title2, process_nm, old_no, fi1, fi2, f3) {
    var html1 = title2 + "-" + fo_no + "-" + process_nm;
    $("#title1").html("<h4 class='red'>" + style_no + " " + '[' + style_nm + " " + ']' + "</h4>");

    $("#serial").html(html1);


    $('#mdii_v').val(mdii);

    $('#barcode').append(html1);
    $('#barcode').barcode(html1, "code128", { barWidth: 1, barHeight: 40 });


    $(document).ready(function () {

        $('#dataTable').DataTable({
            "bServerSide": true,
            "ajax": {
                "url": "/ActualWO/getIssue2?mdii=" + mdii,
                "type": "GET",
                "datatype": "json"
            },
            "searching": false,
            "paging": false,
            "bInfo": false,



            "columns": [



                { "data": "fo" },
                { "data": "line_no" },
                { "data": "process_nm" },
                { "data": "start_dt" },
                { "data": "end_dt" },
                { "data": "issue" },
                { "data": "content" }
            ],
            "bDestroy": true
        });

    });

    $("#file").html("<h4 class='red'>" + fi1 + " " + fi1 + " " + f3 + "</h4>");
};

function modifly(mdii) {
    $('.dialogM').dialog('open');
    viewdetailnotice(mdii);
}
$('#savestyleM').click(function () {
    var formData = new FormData;
    var fileUpload1 = $("#customFile_m").get(0);
    var files1 = fileUpload1.files;

    var fileUpload2 = $("#customPhoto_m").get(0);
    var files2 = fileUpload2.files;

    var fileUpload3 = $("#customFile2_m").get(0);
    var files3 = fileUpload3.files;

    var fileUpload4 = $("#customPhoto2_m").get(0);
    var files4 = fileUpload4.files;

    var fileUpload5 = $("#customFile3_m").get(0);
    var files5 = fileUpload5.files;

    var fileUpload6 = $("#customPhoto3_m").get(0);
    var files6 = fileUpload6.files;

    if (files1[0] != "undefined") { formData.append("file1", files1[0]); };
    if (files2[0] != "undefined") { formData.append("file2", files2[0]); };
    if (files3[0] != "undefined") { formData.append("file3", files3[0]); };
    if (files4[0] != "undefined") { formData.append("file4", files4[0]); };
    if (files5[0] != "undefined") { formData.append("file5", files5[0]); };
    if (files6[0] != "undefined") { formData.append("file6", files6[0]); };
    formData.append("old_no", $("#old_no_m").val()),
    formData.append("title", $("#issue_m").val()),
    formData.append("content", $('#m_SumEditor').summernote('code'));
    formData.append("doing_time", $("#doing_time_m").val());
    formData.append("mdii", $("#mdii_m").val());

    $.ajax({
        url: "/ActualWO/updateIssue",
        type: "POST",
        dataType: "json",
        data: formData,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result.result == true) {
                $('#photo_item').slick('refresh');
                jQuery("#issueGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                alert("SUCCESS: GOOD JOB");
            }
            else {
                alert('ERROR: Issue was not existing. Please check again.');
            }
        },
        error: function (result) {
            alert('ERROR: Update fail. Please check again.');
        }
    });
    //$('.dialog2').dialog('close');

});

function popupView(cellvalue, options, rowobject) {

    var id = rowobject.mdii;
    var html = "";
    if (cellvalue != null) {
        var html = '<button style="color: dodgerblue;border: none;background: none; " onclick="OpenPopup();">' + cellvalue + '</button>';
        //html = '<a href="#" data-toggle="modal"  style="color: dodgerblue; text-align: left;"  data-target="#modifyModal" data-backdrop="static" data-keyboard="false" class="poup_Productivity" onclick="Viewline(); ">' + cellvalue + '</a>';
    }
    return html;

};

function OpenPopup() {
    $('.dialoglinedetail').dialog('open');
}

$(function () {
    $(".dialogM").dialog({
        width: '70%',
        height: 700,
        maxWidth: 1000,
        maxHeight: 900,
        minWidth: '50%',
        minHeight: 900,
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

        close: function (event, ui) {
            $("#photo_item").empty();
            $('#photo_item').slick('unslick');
        },
        open: function (event, ui) {
            $('#m_SumEditor').summernote({
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


    $('#CloseDisplay').click(function () {
        $('.dialogM').dialog('close');
        jQuery("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    });

});

var getIssueView = "/ActualWO/getIssueView";
function _getIssueView(id) {
    $.get(getIssueView + "?id=" + id, function (data) {

        if (data != null && data != undefined) {
            $.each(data, function (key, item) {
                var html = '';
                html += '<div class="to"><h4 class="title_c"> Writer </h4><div class="title_name">' + item.reg_id + " " + item.reg_dt + '</div></div>'
                html += '<div class="to"><h4 class="title_c"> Title </h4><div class="title_name">' + item.title + '</div></div>'
                html += '<div class="to"><h4 class="title_c"> Content </h4><div class="title_name_content">' + item.content + '</div></div>'
                //html += '<tr><th class="col-md-1 border border-secondary"  scope="col" >Subject</th><td class="col-md-11 border border-secondary" scope="col" >' + item.title + '</td></tr>';
                //html += '<tr><th class="col-md-1 border border-secondary" scope="col" >Content</th><td class="col-md-11 border border-secondary" scope="col" >' + item.content + '</td></tr></table>';
                $("#info").html(html);
            });
        }
    });
}



$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
});

$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
});

$("#process_no_start").datepicker({
    dateFormat: 'yy-mm-dd',
});

$("#process_no_end").datepicker({
    dateFormat: 'yy-mm-dd',
});



$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#issueGrid").jqGrid('exportToPdf',
            options = {
                title: null,
                orientation: 'landscape',
                pageSize: 'A4',
                description: null,
                onBeforeExport: null,
                download: 'download',
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "issue.pdf",
                mimetype: "application/pdf"
            }
         );
    });
});

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#issueGrid").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "issue.xlsx",
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 40,
                 onBeforeExport: null,
                 replaceStr: null
             }
         );
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $("#issueGrid").jqGrid('exportToHtml',
            options = {
                title: '',
                onBeforeExport: null,
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                tableClass: 'jqgridprint',
                autoPrint: false,
                topText: '',
                bottomText: '',
                fileName: "issue",
                returnAsString: false
            }
         );
    });
});