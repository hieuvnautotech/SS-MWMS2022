$(function () {
    $("#picMGrid").jqGrid
    ({
        url: "/ShippingMgt/getpicM",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
           { label: 'NO', name: 'wmtid', width: 50, align: 'center', hidden: true },
           { key: true, label: 'NO', name: 'sdmid', width: 50, align: 'center', hidden: true },
           { label: 'ML No', name: 'mt_cd', width: 400, align: 'left' },
           { label: 'Group Qty', name: 'gr_qty', width: 110, align: 'right' },
           { label: 'Unit', name: 'unit_cd', width: 110, align: 'left' },
           { label: 'Status', name: 'mt_sts_cd', width: 110, align: 'center' },
           { label: 'Location', name: 'lct_cd', width: 180, align: 'left' },
           { label: 'Location Status', name: 'lct_sts_cd', width: 120, align: 'center' },
           { label: 'Worker', name: 'worker', width: 150, align: 'left' },
           { label: 'Work date', name: 'work_dt', width: 150, align: 'left' },
           { label: 'MT No', name: 'mt_no', width: 300, align: 'left' },
           { label: 'MT Name', name: 'mt_nm', width: 300, align: 'left' },
           { label: 'SD NO', name: 'sd_no', width: 180, align: 'left' },
           { label: 'SDM NO', name: 'sdm_no', width: 120, align: 'center' }
        ],
        pager: "#picMGridPager",
        pager: jQuery('#picMGridPager'),
        viewrecords: true,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        height: 400,
        width: $(".box-body").width(),
        autowidth: false,
        rowNum: 50,
        caption: 'Picking (M)',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
        multiselect: true,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
    }),



    $("#m_save_but").on("click", function () {
        $('#loading').show();
        var i, selRowIds = $('#picMGrid').jqGrid("getGridParam", "selarrrow"), n, rowData,
        //sd_no = $("#c_sd_no").val(),
        userid = $("#userid").val(),

        uname = $("#uname").val();
        if (funcheck1()) {
            $.ajax({
                type: "get",
                dataType: 'json',
                url: "/ShippingMgt/updatepicM?id=" + selRowIds,

                data: {
                    //sd_no: sd_no,
                    userid: userid,
                    //uname: uname,
                },
                success: function (data) {
                    $('#loading').hide();
                    $.each(data, function (key, item) {
                        var id = item.sdmid;
                        $("#picMGrid").setRowData(id, item, { background: "#d0e9c6" });
                    });
                },
                error: function (data) {

                    $('#loading').hide();
                }

            });
        }

    })

    function funcheck1() {

        var selRowIds = $("#picMGrid").jqGrid("getGridParam", 'selarrrow');


        //if ($("#c_sd_no").val().trim() == "") {
        //    alert("Please enter SD NO");
        //    $("#c_sd_no").val("");
        //    $("#c_sd_no").focus();
        //    return false;
        //}

        if (selRowIds.length == 0) {
            alert("Please select Grid.");
            return false;
        }
        return true;
    };

});
$(document).ready(function () {

    $.ajax({
        url: '/Home/GetSessionMenuData',
        type: 'GET',
        datatype: 'json',
        success: function (SessionData) {
            $("#userid").val(SessionData.userid);
            if (SessionData.userid == "root") {
                $("#uname").val("root");
            }
            else {
                $.get(getadmin + "?id=" + SessionData.userid, function (data) {
                    console.log(data.length)
                    if (data.length == 0) {
                        $("#uname").val("");
                    }
                    else if (data != null && data != undefined) {
                        $("#uname").val(data[0].uname);
                    }
                });
            }
        },
    });


    var wage = document.getElementById("userid");
    wage.addEventListener("keydown", function (e) {
        if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
            var id = e.target.value;
            getchar(id);
        }
    });
});
var getadmin = "/ShippingMgt/getadmin";
$("#searchBtn").click(function () {
    //var c_sd_no = $('#c_sd_no').val().trim();
    var mt_no = $('#mt_no').val().trim();
    var sd_no = $('#c_sd_no').val().trim();
    var mt_barcode = $('#mt_barcode').val().trim();
    var start = $('#start').val();
    var end = $('#end').val();
    $.ajax({
        url: "/ShippingMgt/searchpicM",
        type: "get",
        dataType: "json",
        data: {
            sd_no: sd_no,
            mt_no: mt_no,
            mt_barcode: mt_barcode,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#picMGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});




$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#picMGrid").jqGrid('exportToPdf',
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
                fileName: "picM.pdf",
                mimetype: "application/pdf"
            }
         );
    });
});

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#picMGrid").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "picM.xlsx",
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
        $("#picMGrid").jqGrid('exportToHtml',
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
                fileName: "picM",
                returnAsString: false
            }
         );
    });
});


$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
});

$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
});

var getType = "/ShippingMgt/getType";

$(document).ready(function () {
    _getTypeS();
    $("#m_lct_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getTypeS() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#m_lct_cd").html(html);
        }
    });
}


var getadmin = "/ShippingMgt/getadmin";


$(document).ready(function () {
    var wage = document.getElementById("userid");
    wage.addEventListener("keydown", function (e) {
        if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
            var id = e.target.value;
            getchar(id);
        }
    });
});

function getchar(id) {
    $.get(getadmin + "?id=" + id, function (data) {
        if (data != null && data != undefined) {
            $.each(data, function (key, item) {
                var html = '';
                html += '<label style="width:100%">&nbsp;&nbsp;</label><input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname">';
                $("#new").html(html);
            });
        }
    });
}