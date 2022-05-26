
$(function () {
    $("#list").jqGrid
    ({
        //url: "/wipwms/getshippingmanual_wip",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
            { label: 'mt_qrcode', name: 'mt_qrcode', width: 350, align: 'left', hidden: true },
            { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 350, align: 'left', hidden: true },
            { label: 'to_lct_cd', name: 'to_lct_cd', width: 350, align: 'left', hidden: true },
            { label: 'BOM NO', name: 'bom_no', width: 100, align: 'left', hidden: true },
             { label: 'BOM NO', name: 'bom_no1', width: 150, align: 'left', formatter: bom_no_subtr },
           // { label: 'ML NO', name: 'mt_cd', width: 350, align: 'left' },
            { label: 'MT NO', name: 'mt_no', width: 300, align: 'left' },
            { label: 'MT Name', name: 'mt_nm', width: 350, align: 'left' },
            { label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
            { label: 'Need Group', name: 'need_qty', width: 100, align: 'left' , hidden: true},
            { label: 'Selected Qty', name: 'select_qty', width: 150, align: 'right' },
            
            //{ label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
            //{ label: 'Location Status', name: 'lct_sts_nm', width: 150, align: 'left' },
            //{ label: 'Worker', name: 'worker', width: 150, align: 'left' },
            //{ label: 'Departure', name: 'from_lct_cd', width: 100, align: 'left', hidden: true },
            //{ label: 'Out Date', name: 'output_dt', width: 150, align: 'left' },
            //{ label: 'Destination', name: 'destination', width: 150, align: 'left', hidden: true },
            //{ label: 'Input Date', name: 'input_dt', width: 100, align: 'left' },
            //{ label: 'Bobbin No', name: 'bb_no', width: 100, align: 'left' },
            //{ label: 'Description', name: 're_mark', width: 150, align: 'left' },
            //{ label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            //{ key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            //{ label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            //{ key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            $('#wmtid').val(row_id.wmtid);

        },
        pager: "#gridpager",
        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 650,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 50,
        caption: 'Shipping Manual(WIP)',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: false,
        datatype: function (postData) { getData(postData); },
        viewrecords: true,
        multiselect: false,
        subGrid: true, // set the subGrid property to true to show expand buttons for each row
        subGridOptions: { expandOnLoad: false },
        subGridRowExpanded: showChildGrid,
        subGridRowcollapse: showChildGrid1,// javascript function that will take care of showing the child grid
        onSelectRow: editRow,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },

        gridComplete: function () {
            $('.loading').hide();

        },
    });
});//grid1


function bom_no_subtr(cellvalue, options, rowObject) {
    //var a, b, c, d;
    //cellvalue = rowObject.bom_no;
    if (cellvalue == null) {
        return ""
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        d = cellvalue.substr(cellvalue.length - 1, 1);
        c = parseInt(b);

        return a + c + d;
    }
};



//function bom_no_subtr2(cellvalue, options, rowObject) {
//    var a, b, c, d;
//    cellvalue = rowObject.bom_no;
//    if (cellvalue == null) {
//        return ""
//    }
//    else {
//        a = cellvalue.substr(0, 1);
//        b = cellvalue.substr(1, 11);
//        d = cellvalue.substr(cellvalue.length - 1, 1);
//        c = parseInt(b);

//        return a + c + d;
//    }
//};
var selRowIds_selected = [];
var childGridID_selected = [];


function showChildGrid1(parentRowID, parentRowKey)
{
    alert("close" + parentRowKey + "");
}

function showChildGrid(parentRowID, parentRowKey) {
    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    // send the parent row primary key to the server so that we know which grid to show
    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    var rowData = $("#list").jqGrid("getRowData", parentRowKey);
    var rel_bom = rowData.bom_no;
    var mt_no = rowData.mt_no;
    $("#" + childGridID).jqGrid({
        url: "/wipwms/searchshippingmanual_wip?bom_no=" + rel_bom + "&mt_no=" + mt_no,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [

            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
            { label: 'mt_qrcode', name: 'mt_qrcode', width: 350, align: 'left', hidden: true },
            { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 350, align: 'left', hidden: true },
            { label: 'to_lct_cd', name: 'to_lct_cd', width: 350, align: 'left', hidden: true },
            { label: 'BOM NO', name: 'bom_no', width: 100, align: 'left', hidden: true },
            { label: 'ML NO', name: 'mt_cd', width: 450, align: 'left' },
            { label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
            { label: 'MT Name', name: 'mt_nm', width: 250, align: 'left' },
            { label: 'Group QTY', name: 'gr_qty', width: 100, align: 'left' },
            { label: 'Status', name: 'mt_sts_nm', width: 100, align: 'left' },
            { label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
            { label: 'Location Status', name: 'lct_sts_nm', width: 150, align: 'left' },
            { label: 'Worker', name: 'worker_id', width: 80, align: 'left' },
            { label: 'Departure', name: 'from_lct_cd', width: 100, align: 'left', hidden: true },
            { label: 'Output Date', name: 'output_dt', width: 150, align: 'center', formatter: _Date },
            { label: 'Destination', name: 'destination', width: 150, align: 'left', hidden: true },
            { label: 'Input Date', name: 'input_dt', width: 100, align: 'center', formatter: _Date },
            { label: 'Bobbin No', name: 'bb_no', width: 100, align: 'left' },
            { label: 'Description', name: 're_mark', width: 150, align: 'left' },
            { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

        ],
        shrinkToFit: false,
        loadonce: true,
        width: '100%',
        height: '100%',
       // subGrid: false, // set the subGrid property to true to show expand buttons for each row
       // subGridRowExpanded: showThirdLevelChildGrid, // javascript function that will take care of showing the child grid
        pager: "#" + childGridPagerID,
    //    rowNum: 2000,
        rownumbers: true,
        loadonce: true,
        multiPageSelection: false,
        multiselect: true,
        rowList: [50, 100, 200, 300],
        viewrecords: true,
        rowNum: 50,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"

        },
        onSelectRow: function (id, rowid, status, e) {

            if (id) {
                $("#" + childGridID).jqGrid('editRow', id, true);
            }
            var selected_gr_qty1 = 0;
            var i, selRowIds = $("#" + childGridID).jqGrid("getGridParam", "selarrrow"), n, rowData;
            if (childGridID_selected.includes(childGridID) == false)
                childGridID_selected.push(childGridID);
            if (selRowIds.length == 0) {
                selected_gr_qty1 = 0;
                $("#list").jqGrid("setCell", parentRowKey, "select_qty", selected_gr_qty1);
            }
            else {
                for (i = 0 ; i < selRowIds.length; i++) {
                    var rowData = $("#" + childGridID).jqGrid("getRowData", selRowIds[i]);
                    selected_gr_qty1 += parseInt(rowData.gr_qty);
                    $("#list").jqGrid("setCell", parentRowKey, "select_qty", selected_gr_qty1);
                }
            }
           
        },
        onSelectAll: function (id, status) {
            var i, selRowIds = $("#" + childGridID).jqGrid("getGridParam", "selarrrow"), n, rowData;
            if (childGridID_selected.includes(childGridID) == false)
                childGridID_selected.push(childGridID);
            if (selRowIds.length == 0) {
                selected_gr_qty1 = 0;
                $("#list").jqGrid("setCell", parentRowKey, "select_qty", selected_gr_qty1);
            }
            else {
                var selected_gr_qty1 = 0;
               
                for (i = 0 ; i < selRowIds.length; i++) {
                    var rowData = $("#" + childGridID).jqGrid("getRowData", selRowIds[i]);
                    selected_gr_qty1 += parseInt(rowData.gr_qty);
                    $("#list").jqGrid("setCell", parentRowKey, "select_qty", selected_gr_qty1);
                }
            }
        }

    });

}



var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#list");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSelection = id;
    }
}



$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#list").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "ReceivePutin.xlsx",
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 40,
                 onBeforeExport: null,
                 replaceStr: null
             }
         );
    });

    $('#htmlBtn').click(function (e) {
        $("#list").jqGrid('exportToHtml',
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
                fileName: "Receive Putin",
                returnAsString: false
            }
         );
    });

    $('#pdfBtn').click(function (e) {
        $("#list").jqGrid('exportToPdf',
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
                fileName: "ReceivePutin.pdf",
                mimetype: "application/pdf"
            }
         );
    });

    var getadmin = "/ShippingMgt/getadmin";
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

    _getLocation();
    _getFactory();
    _GetStatus();

    $("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();
    $("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

});

var getLocation = "/wipwms/getLocation";
var getFactory = "/wipwms/Warehouse";
var GetStatus = "/wipwms/GetStatus";
function _getLocation() {
    $.get(getLocation, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Destination*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#shelf_cd").html(html);
        }
    });
}
function _getFactory() {
    $.get(getFactory, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">Destination</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#c_lct_cd").html(html);
        }
    });
}
function _GetStatus() {
    $.get(GetStatus, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">Status</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#mt_sts_cd").html(html);
        }
    });
}

function IsRowsSelected() {
    var selRowIds = $("#list").jqGrid("getGridParam", 'selarrrow');
    if (selRowIds.length == 0) {
        alert("Please select Grid.");
        return false;
    }
    return true;
};

function getData(pdata) {
    $('.loading').show();
    var params = new Object();
    if (jQuery('#list').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.bom_no = $('#bom_no').val();
    params.mt_no = $('#c_mt_no').val();
    params.mt_barcode = $('#mt_barcode').val();
    params.mt_sts_cd = $('#mt_sts_cd').val();
    params.style_no = $('#style_no').val();
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        //url: "/wipwms/searchshippingmanual_wip",   
        url: "/wipwms/searchshippingmanual_wip_bom",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list")[0];
                grid.addJSONData(data);
                $('.loading').hide();
            }
        }
    })
};

$("#searchBtn").click(function () {

    $("#list").clearGridData();

  

    var grid = $("#list");
    grid.jqGrid('setGridParam', { search: true });

    var pdata = grid.jqGrid('getGridParam', 'postData');

    getData(pdata);
});

//$("#searchBtn").click(function () {
//    var shelf_cd = $('#shelf_cd').val();
//    var mt_no = $('#c_mt_no').val();
//    var mt_barcode = $('#mt_barcode').val();
//    var mt_sts_cd = $('#mt_sts_cd').val();
//    var lct_cd = $('#c_lct_cd').val();
//    $.ajax({
//        url: "/wipwms/searchshippingmanual_wip",
//        type: "get",
//        dataType: "json",
//        data: {
//            shelf_cd: shelf_cd,
//            mt_no: mt_no,
//            mt_barcode: mt_barcode,
//            mt_sts_cd: mt_sts_cd,
//            lct_cd: lct_cd,
//        },
//        success: function (result) {
//            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
//        }
//    });
//});

$("#pickingBtn").on("click", function () {
   // var selRowIds = $('#list').jqGrid("getGridParam", "selarrrow");

    var userid = $("#userid").val();
    var uname = $("#uname").val();
    for (j = 0; j < childGridID_selected.length; j++)
    {
        var i, selRowIds = $("#" + childGridID_selected[j]).jqGrid("getGridParam", "selarrrow"), n, rowData;
        if(selRowIds!=undefined)
        for (k = 0; k < selRowIds.length; k++)
            selRowIds_selected.push(selRowIds[k]);
    }
    $('#loading').show();
    if (selRowIds_selected != undefined && selRowIds_selected != "") {
        $.ajax({
            type: "get",
            dataType: 'json',
            url: "/wipwms/Updateshippingmanual_wip?id=" + selRowIds_selected,
            data: {
                worker_id: userid,
            },
            success: function (response) {
                $('#loading').hide();
                if (response.result) {
                    for (j = 0; j < childGridID_selected.length; j++) {
                        var data_show = response.data;
                        for (i = 0; i < data_show.length; i++) {
                            var id = data_show[i].wmtid;
                            $("#" + childGridID_selected[j]).setRowData(id, data_show[i], { background: "#d0e9c6" });
                        }

                        //$.each(data.data.Data, function (key, item) {
                        //    var id = item.sdmid;
                        //    $("#" + childGridID_selected[j]).setRowData(id, item, { background: "#d0e9c6" });
                        //});

                    }
                    //for (j = 0; j < childGridID_selected.length; j++) {
                    //    $("#" + childGridID_selected[j]).setRowData(id, data = false, { background: "#d0e9c6" });
                    //    //var i, selRowIds = $("#" + childGridID_selected[j]).jqGrid("getGridParam", "selarrrow"), n, rowData;
                    //    //if (selRowIds != undefined)
                    //    //    for (k = 0; k < selRowIds.length; k++) {
                    //    //        //var id = selRowIds[k];

                    //    //        $("#" + childGridID_selected[j]).setRowData(id, data = false, { background: "#d0e9c6" });
                    //    //        $('#loading').hide();
                    //    //    }
                    //}
                }
                else alert(response.mess);
          
            },
            error: function (error) {
                $('#loading').hide();
                alert("Error: " + eval(error));
            }

        });
    }
});

/*
$("#getPutin").on("click", function () {
    var shelf_cd = $('#shelf_cd').val();
    var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData;

    if ($("#shelf_cd").val().trim() == "") {
        alert("Please enter your line name");
        $("#c_line_nm").val("");
        $("#c_line_nm").focus();
        return false;
    }
    if (selRowIds == "") {
        alert("Please select row you want change");
        $("#c_line_nm").val("");
        $("#c_line_nm").focus();
        return false;
    }
    $.ajax({
        type: "get",
        dataType: 'json',
        url: "/wipwms/Updateshippingmanual_wip?id=" + selRowIds,
        data: {
            lct_cd: shelf_cd,
        },
        success: function (data) {
            console.log(data);
            for (var i = 0; i < data.length; i++) {
                
                var id = data[i].wmtid;
                $("#list").setRowData(id, data[i], { background: "#d0e9c6" });
            }
        }
    });

});
*///button getPutin



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
                html += '<label style="width:100%">&nbsp;&nbsp;&nbsp;</label><input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname">';
                $("#new").html(html);
            });
        }
    });
}

function _Date(cellvalue, options, rowObject) {
    if (cellvalue == null)
        return "";
    var reg = /(\d{4})(\d{2})(\d{2})/;
    if (reg.test(cellvalue))
        return cellvalue.replace(reg, "$1-$2-$3");
    else {
        reg = /(\d{4})(\d{2})\-(\d{2})/;
        if (reg.test(cellvalue))
            return cellvalue.replace(reg, "$1-$2-$3");
        else {
            reg = /(\d{4})\-(\d{2})\-(\d{2})/;
            if (reg.test(cellvalue))
                return cellvalue.replace(reg, "$1-$2-$3");
            else {
                reg = /(\d{4})(\d{2}).(\d{2})/;
                if (reg.test(cellvalue))
                    return cellvalue.replace(reg, "$1-$2-$3");
                else {
                    reg = /(\d{4}).(\d{2}).(\d{2})/;
                    if (reg.test(cellvalue))
                        return cellvalue.replace(reg, "$1-$2-$3");
                    else {
                        reg = /(\d{4})(\d{2})\\(\d{2})/;
                        if (reg.test(cellvalue))
                            return cellvalue.replace(reg, "$1-$2-$3");
                        else {
                            reg = /(\d{4})\\(\d{2})\\(\d{2})/;
                            if (reg.test(cellvalue))
                                return cellvalue.replace(reg, "$1-$2-$3");
                            else {
                                reg = /(\d{4})\\(\d{2})\.(\d{2})/;
                                if (reg.test(cellvalue))
                                    return cellvalue.replace(reg, "$1-$2-$3");
                                else {
                                    reg = /(\d{4})\.(\d{2})\\(\d{2})/;
                                    if (reg.test(cellvalue))
                                        return cellvalue.replace(reg, "$1-$2-$3");
                                    else
                                        return cellvalue;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
};

