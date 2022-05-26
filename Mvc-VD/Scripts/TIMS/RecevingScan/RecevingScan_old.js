$(function () {
    $("#list").jqGrid
        ({
            url: "/TIMS/GetRDInfo",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'rid', name: 'rid', width: 50, align: 'center', hidden: true },
                { label: 'RD NO', name: 'rd_no', width: 100, align: 'center', formatter: rd_popup },
                { label: 'RD NO', name: 'rd_no', width: 100, align: 'left', hidden: true },
                { label: 'RD Name', name: 'rd_nm', width: 100, align: 'left' },
                { label: 'Status', name: 'sts_nm', width: 150, align: 'center', hidden: true },
                { label: 'Status', name: 'rd_sts_cd', width: 150, align: 'center', hidden: true },
                { label: 'Receiving Date', name: 'receiving_dt', width: 150, align: 'center', formatter: convertDate },
              
                { label: 'Remark', name: 'remark', width: 180, align: 'left' },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list").getRowData(selectedRowId);
            

                $("#m_rid").val(rowData.rid);
                $("#m_rd_nm").val(rowData.rd_nm);
                $("#m_rd_no").val(rowData.rd_no);
                $("#m_remark").val(rowData.remark);

                $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("show");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").addClass("active");

                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false);



            },

            pager: jQuery('#listPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption:  'RD Infomation',
            emptyrecords: "No data.",
            height: 200,
            width: null,
            autowidth: false,
            shrinkToFit: false,
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

                
            }
        });
});
function convertDate(cellValue, options, rowdata, action) {

    if (cellValue != null) {

        a = cellValue.substr(0, 4);
        b = cellValue.substr(4, 2);
        c = cellValue.substr(6, 2);

        var html = a + "-" + b + "-" + c;
        return html;
    }
    else {
        var html = "";
        return html;
    }



};
//-------INSERT------------//
$("#c_save_but").click(function () {
    var isValid = $('#form1').valid();
    if (isValid == true) {
        var rd_nm = $('#c_rd_nm').val();
        var remark = $('#c_remark').val();

        {
            $.ajax({
                url: "/TIMS/InsertRDInfo",
                type: "get",
                dataType: "json",
                data: {
                    rd_nm: rd_nm,
                    remark: remark,
                },

                success: function (response) {
                    if (response.result) {

                        var id = response.data[0].rid;

                        $("#list").jqGrid('addRowData', id, response.data[0], 'first');
                        $("#list").setRowData(id, false, { background: "#28a745", color: "#fff" });

                    } else {
                        alert('Data already exists, Please check again!!!');
                    }
                }
            });
        }
    }
});

//-------UPDATE------------//
$("#m_save_but").click(function () {
    var isValid = $('#form2').valid();
    if (isValid == false) {
        return false;
    }

    var rid = $('#m_rid').val();
    var rd_nm = $('#m_rd_nm').val();
    var remark = $('#m_remark').val();


    $.ajax({
        url: "/TIMS/UpdateRDInfo",
        type: "get",
        dataType: "json",
        data: {
            rid: rid,
            rd_nm: rd_nm,
            remark: remark

        },
        success: function (response) {
            if (response.result) {
                var id = response.data.rid;
                $("#list").setRowData(id, response.data, { background: "#28a745", color: "#fff" });


            } else {
                alert(response.message);
            }
        },

    });
});
$("#m_delete_but").click(function () {
    var isValid = $('#form2').valid();
    if (isValid == false) {
        return false;
    }
    var r = confirm("Are you make sure DELETE Events?");
    if (r === true) {
        var m_rid = $("#m_rid").val();
        $.ajax({
            url: "/TIMS/DeleteRDInfo?rid=" + m_rid,
            type: "POST",
            success: function (response) {
                if (response.result) {

                    $('#list').jqGrid('delRowData', m_rid);
                    document.getElementById("form2").reset();
                    document.getElementById("form1").reset();
                    $("#tab_2").removeClass("active");
                    $("#tab_1").addClass("active");
                    $("#tab_c2").removeClass("show");
                    $("#tab_c2").removeClass("active");
                    $("#tab_c1").addClass("active");

                    alert(response.message);

                } else {
                    alert(response.message);
                }
            }
        });
    }
});

//-------SEARCH------------//
$("#searchBtn").click(function () {
    var rd_no = $('#s_rd_no').val().trim();
    var rd_nm = $('#s_rd_nm').val().trim();

    $.ajax({
        url: "/TIMS/GetRDInfo",

        type: "get",
        dataType: "json",
        data: {
            rd_no: rd_no,
            rd_nm: rd_nm,

        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});



$(document).ready(function () {
    $("#lct_cd").on("keydown", function (e) {
        if (e.keyCode === 13) {
            var that = $(this);
            $("#mt_barcode").focus();
        }
    })
    $("#mt_barcode").on("keydown", function (e) {
        if (e.keyCode === 13) {
            var that = $(this);

            var mt_barcode = e.target.value.trim();
          


            $.ajax({
                url: "/TIMS/UpdateScan",
                type: "get",
                dataType: "json",
                data: {
                  
                    mt_barcode: mt_barcode
                },
                success: function (response) {
                    if (response.message == "") {
                        var id = response.data.wmtid;
                        $("#list").jqGrid('addRowData', id, response.data, 'first');
                        $("#list").setRowData(id, false, { background: "#d0e9c6" });

                       
                        $("#mt_barcode").val("");

                    } else {
                        alert(response.message);
                      
                        $("#mt_barcode").val("");


                    }
                },
                error: function (response) {
                    alert(response.message);
                    $("#mt_barcode").val("");
                }
            });
        }
    });
});
//----- RD POPUP------------------//
function rd_popup(cellValue) {
    var html = '<a class="css_pp_cilck" data-rd_no="' + cellValue + '" onclick="ViewSDPopup(this);">' + cellValue + '</a>';
    return html;
};

function ViewSDPopup(e) {
    $('.popup-dialog.RD_Info_Popup').dialog('open');
    var rd_no = $(e).data('rd_no');


    $.get("/TIMS/PartialView_RD_Info_Popup?" +
        "rd_no=" + rd_no + ""
        , function (html) {
            $("#PartialView_RD_Info_Popup").html(html);
        });
}

//----- list1------------------//
var prevCellVal = { cellId: undefined, value: undefined };
$(function () {
    $("#list1").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { label: 'ML NO', name: 'mt_cd', width: 350, align: 'left' },
                { label: 'Bobbin No', name: 'bb_no', width: 120, align: 'left' },

                { label: 'Material Type', name: 'mt_type_nm', width: 150, align: 'center' },
                { label: 'Material Type', name: 'mt_type', width: 180, align: 'center', hidden: true },

                { label: 'Quantity', name: 'gr_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
                { label: 'Status', name: 'sts_nm', width: 100, align: 'center' },

                { label: 'Departure', name: 'from_lct_nm', width: 180, align: 'center' },
                { label: 'Departure Status', name: 'lct_sts_cd', width: 180, align: 'center' },
                { label: 'Receiving Date', name: 'recevice_dt_tims', width: 180, align: 'center', formatter: convertDate },
                { name: "", width: 100, align: "center", label: "Action", resizable: false, title: false, 
                    cellattr: function (rowId, val, rawObject, cm, rdata) {
                        var result;

                        if (prevCellVal.value == val) {
                            result = ' style="" rowspanid="' + prevCellVal.cellId + '"';
                        }
                        else {
                            var cellId = this.id + '_row_' + rowId + '_' + cm.name;

                            result = ` rowspan="${rawObject.rowpan}" id="${cellId}">  <button class="btn btn-xs btn-danger"  data-bb_no="${rawObject.bb_no}"  onclick="Del_Row(this);" > Delete </button`;
                         
                            prevCellVal = { cellId: cellId, value: val };
                        }

                        return result;
                    }
                }, 

            ],
            
            pager: jQuery('#listPager1'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: '',
            emptyrecords: "No data.",
            height: 250,
            width: null,
            autowidth: false,
            shrinkToFit: false,
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
                prevCellVal = { cellId: undefined, value: undefined };
            }


        });
})

$("#bb_no").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var bb_no = e.target.value.trim();

        var rows = $('#list1').jqGrid('getRowData');

        for (i = 0; i < rows.length; i++) {
            var bb_no_view = rows[i].bb_no;

            if (bb_no == bb_no_view) {
                alert("Data Duplicated !!!");

                $("#bb_no").val("");
                document.getElementById("bb_no").focus();

                return false;

            }
        }

        $.ajax({
            url: "/TIMS/GetTimsReceiScanMLQR",
           
            type: "get",
            dataType: "json",
            data: {
                bb_no: bb_no
            },
            success: function (response) {
                if (response.result) {

                    for (var i = 0; i < response.data.length; i++) {
                        var id = response.data[i].wmtid;

                        $("#list1").jqGrid('addRowData', id, response.data[i], 'first');
                        $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });

                    }
                 
                    $("#bb_no").val("");
                    document.getElementById("bb_no").focus();

                } else {
                    alert(response.message);

                    $("#bb_no").val("");
                    document.getElementById("bb_no").focus();
                }

            }
        });
    }
});
function bntDelete(cellValue, options, rowdata, action) {
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-danger" onclick="Del_Row(' + wmtid + ');">Delete</button>';
    return html;

}
//arrtSetting = function (rowId, val, rawObject, cm) {
//    var attr = rawObject.attr[cm.name], result;
//        if (attr.rowspan) {
//            result = ' rowspan=' + '"' + attr.rowspan + '"';
//        } else if (attr.display) {
//            result = ' style="display:' + attr.display + '"';
//        }
//        return result;
//    };

function Del_Row(e) {

    var bb_no = e.dataset.bb_no;
    var rows = $('#list1').jqGrid('getRowData');

    for (var item of rows) {
        var wmtid = item.wmtid;
        var bb_no_view = item.bb_no;

        if (bb_no == bb_no_view) {
         
            $("#list1").jqGrid('delRowData', wmtid);
        }

    }


 


}
$("#Clear_all").on("click", function () {
    var r = confirm("Are you make sure DELETE Events?");
    if (r === true) {

        $("#list1").jqGrid('clearGridData');

    }

});


$("#Save_completed").on("click", function () {

    var rd_no = $("#m_rd_no").val();
    if (rd_no == "" || rd_no == null || rd_no == undefined) {
        alert("Please Choose RD No!!!")
        return false;
    }

    var rows = $("#list1").getDataIDs();
    if (rows == "" || rows == null || rows == undefined) {
        alert("Please Scan!!!")
        return false;
    }

    $.ajax({
        url: "/TIMS/UpdateMTQRRDList?data=" + rows + "&rd_no=" + rd_no,
        type: "get",
        dataType: "json",

        success: function (data) {

            if (data.result) {
                alert(data.message);
                $("#list1").jqGrid('clearGridData');
            }
            else {
                alert(data.message);
            }
        }
    });

});
$("#Select_completed").on("click", function () {

    var rd_no = $("#m_rd_no").val();
    if (rd_no == "" || rd_no == null || rd_no == undefined) {
        alert("Please Choose RD No!!!")
        return false;
    }

    $('.popup-dialog.List_ML_NO_Info_Popup').dialog('open');



    $.get("/TIMS/PartialView_List_ML_NO_Info_Popup?" +
        "rd_no=" + rd_no + ""
        , function (html) {
            $("#PartialView_List_ML_NO_Info_Popup").html(html);
        });


});

//----- help to focus on input------------------//
setTimeout(function () { $("#bb_no").focus(); }, 1);