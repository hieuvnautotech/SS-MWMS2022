var row_id, row_id2;
$("#layoutGrid").jqGrid
({
    url: "/System/getLayoutMT",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { name: "lmno", width: 100, align: "left", label: "No", resizable: true, hidden: true },
        { name: "ml_cd", width: 100, align: "center", label: "Code", resizable: true },
        { name: "ml_nm", width: 250, label: "Name", resizable: true },

    ],

    onSelectRow: function (rowid, status, e) {

        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#layoutGrid").jqGrid("getGridParam", 'selrow');
        row_id = $("#layoutGrid").getRowData(selectedRowId);
        var lmno = row_id.lmno;
        var ml_cd = row_id.ml_cd;
        var ml_nm = row_id.ml_nm;


        document.getElementById("form2").reset();
        $("#m_save_but").attr("disabled", false);
        if (Session_authName == "root") {
            $("#del_save_but").attr("disabled", false);
        }
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        //$("#commondtGridPager").clearGridData();
        //$("#layoutdtGrid").setGridParam({ url: "/system/getLayoutDT?" + "ml_cd=" + ml_cd, datatype: "json" }).trigger("reloadGrid");

        $("#m_lmno").val(lmno);
        $("#m_ml_cd").val(ml_cd);
        $("#m_ml_nm").val(ml_nm);
        //$("#m_use_yn").val(use_yn);
        //$("#m_mt_id").val(mt_id);
        //$("#dc_mt_cd").val(mt_cd);
        //$("#dc_mt_nm").val(mt_nm);

        //document.getElementById("form3").reset();
        //$("#tab_d2").removeClass("active");
        //$("#tab_d1").addClass("active");
        //$("#tab_dc2").removeClass("active");
        //$("#tab_dc1").removeClass("hidden");
        //$("#tab_dc2").addClass("hidden");
        //$("#tab_dc1").addClass("active");
        //$("#dm_mt_cd").val(dm_mt_cd);
        //$("#dc_mt_nm").val(mt_nm);
        //$("#dc_mt_cd").val(mt_cd);
        //$("#dc_cdid_cd").val(1);
        //$("#dc_dt_order").val(1);
        //$("#dc_mt_cd").val(mt_cd);
        //$("#dc_mt_nm").val(mt_nm);

    },
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    autowidth: false,
    multiselect: false,
    sortable: true,
    loadonce: true,
    pager: '#layoutGridPager',
    viewrecords: true,
    height: 120,
    width: $(".box-body").width() - 10,
    caption: 'Factory Layout Main',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
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

});
//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
//$('#layoutGrid').jqGrid('setGridWidth', $(".box-body").width());
//$(window).on("resize", function () {
//    var newWidth = $("#commonGrid").closest(".ui-jqgrid").parent().width();
//    $("#layoutGrid").jqGrid("setGridWidth", newWidth, false);
//});

$("#c_save_but").click(function () {
    //if ($("#form1").valid() == true) {
    //var mt_cd = $('#c_mt_cd').val().toUpperCase();
    var ml_cd = $('#c_ml_cd').val();
    var ml_nm = $('#c_ml_nm').val();
    //var use_yn = $('#c_use_yn').val();
    $.ajax({
        url: "/system/insertLayoutMT",
        type: "get",
        dataType: "json",
        data: {
            ml_cd: ml_cd,
            ml_nm: ml_nm,
            //mt_exp: mt_exp,
            //use_yn: use_yn,
        },
        success: function (data) {
            if (data.result == true) {
                var id = data.data2.lmno;
                //$('#mt_remark_color').val(mt_cd);
                //jQuery("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');

                $("#layoutGrid").jqGrid('addRowData', id, data.data2, 'first');
                $("#layoutGrid").setRowData(id, false, { background: "#d0e9c6" });

                jQuery("#layoutdtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');

                //$("#commondtGrid").setGridParam({ url: "/system/GetJqGridDataCommDt?" + "mt_cd=" + mt_cd, datatype: "json" }).trigger("reloadGrid");
            }
            else {
                alert("Layout Information was existing. Please checking again !");
            }
        }
    });
    //}
});


//Modifly Common main
$("#m_save_but").click(function () {
    //if ($("#form2").valid() == true) {
    //var ml_cd = $('#c_ml_cd').val();
    //var ml_nm = $('#c_ml_nm').val();

    $.ajax({
        url: "/system/updateLayoutMT",
        type: "get",
        dataType: "json",
        data: {
            m_lmno: $('#m_lmno').val(),
            m_ml_cd: $('#m_ml_cd').val(),
            m_ml_nm: $('#m_ml_nm').val(),
            //use_yn: use_yn,
            //mt_id: mt_id,
        },
        success: function (data) {
            if (data.result == true) {
                //var mt_cd = data.mt_cd;

                var id = data.data2.lmno;
                console.log(data.data2);

                //var id = data.fmno;
                //$("#WmMgtGrid").setRowData(id, data, { background: "#d0e9c6" });
                //var id = data.fmno;
                $("#layoutGrid").setRowData(id, data.data2, { background: "#d0e9c6" });

            }
            else {
                alert("Common Information was not existing. Please checking again !");
            }
        }
    });
    //}

});

$("#del_save_but").click(function () {
    $('#dialogDangerous_cm').dialog('open');
});
$("#ddel_save_but").click(function () {
    $('#dialogDangerous_dt').dialog('open');
});


// Tab
$("#tab_1").on("click", "a", function (event) {
    $("#commonGrid").trigger('reloadGrid');
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {
    $("#commonGrid").trigger('reloadGrid');
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       


$("#layoutdtGrid").jqGrid
({
    url: "/System/getLayoutDT",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
           { name: "ldno", width: 50, align: "left", label: ' ', resizable: true, hidden: true },
           //{ name: "bien_id", width: 50, align: "left", label: ' '},
           { name: "html1", width: 300, align: "left", label: '01', formatter: btn_option },

                      { name: "html2", width: 300, align: "left", label: '02', formatter: btn_option },
                                 { name: "html3", width: 300, align: "left", label: '03', formatter: btn_option },
                                            { name: "html4", width: 300, align: "left", label: '04', formatter: btn_option },
                                                       { name: "html5", width: 300, align: "left", label: '05', formatter: btn_option },
     //{

     //    label: ' ', name: 'ml_cd', width: 350, align: 'center', editable: true, edittype: "select",
     //    editoptions: {
     //        dataUrl: "/System/getmlcd",
     //        cacheUrlData: true,
     //        buildSelect: function (data) {
     //            var m, k, l, n, data = data.split(',"'), s = "<select>";
     //            for (i = 0, j = data.length / 2; i < data.length / 2, j < data.length; i++, j++) {
     //                m = data[i].replace('["', "");
     //                l = m.replace('"', "");

     //                k = data[j].replace('"', "");
     //                n = k.replace(']', "");

     //                s += '<option value="' + l + '">' + n + '</option>';
     //                console.log(s);
     //            }
     //            return s += '</select>';

     //        },
     //        dataEvents: [{
     //            type: 'change',
     //            fn: function (e) {
     //                //console.log(e);
     //                var oldVal = $(this).val(), rowid = $("#layoutdtGrid").jqGrid('getGridParam', 'selrow');
     //                $.ajax({
     //                    url: "/system/updatemlcd?id=" + rowid + "&ml_cd=" + oldVal,
     //                    type: "get",
     //                    success: function (data) {

     //                        if (data.result == true) {
     //                            var id = data.data2.ldno;
     //                            //$('#mt_remark_color').val(mt_cd);
     //                            //jQuery("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');

     //                            //$("#layoutdtGrid").jqGrid('addRowData', id, data.data2, 'first');
     //                            $("#layoutdtGrid").setRowData(id, false, { background: "#d0e9c6" });

     //                            //jQuery("#layoutdtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
     //                        }
     //                        //console.log(data);
     //                        ////$("#layoutGrid").jqGrid('addRowData', id, data.data2, 'first');
     //                        ////$("#layoutGrid").setRowData(id, false, { background: "#d0e9c6" });


     //                        //jQuery("#layoutdtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
     //                    }
     //                });
     //            }
     //        }]

     //    },
     //    //cellattr: function (rowId, value, rowObject, colModel, arrData) {
     //    //    if (value == 'Approval') {
     //    //        return 'style=background-color:#36A2EB;';
     //    //    }
     //    //    if (value == 'Stop') {
     //    //        return 'style=background-color:#FFCE56;';
     //    //    }

     //    //    if (value == 'Cancel') {
     //    //        return 'style=background-color:#FF6384;';
     //    //    }
     //    //    else {
     //    //        return value;
     //    //    }
     //    //}
     //},

    ],
    //gridComplete: function () {
    //    var rows = $("#commondtGrid").getDataIDs();
    //    for (var i = 0; i < rows.length; i++) {
    //        var mt_cd = $("#commondtGrid").getCell(rows[i], "mt_cd");
    //        var dt_cd = $("#commondtGrid").getCell(rows[i], "dt_cd");
    //        var giatri_mt_cd = $('#mt_remark_color').val();
    //        var giatri_dt_cd = $('#dt_remark_color').val();
    //        if ((mt_cd == giatri_mt_cd) && (dt_cd == giatri_dt_cd)) {
    //            $("#commondtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
    //        }
    //    }
    //},
    onSelectRow: function (rowid, selected, status, e) {
        //$("#abc").val();
        var lastSelection = "";
        //var selectedRowId = $("#WrMgtGrid").jqGrid("getGridParam", 'selrow');
        //row_id = $("#WrMgtGrid").getRowData(selectedRowId);
        if (rowid && rowid !== lastSelection) {
            var grid = $("#layoutdtGrid");
            grid.jqGrid('editRow', rowid, { keys: true, focusField: 4 });
            lastSelection = rowid;

            //var selectedRowId = $("#WrMgtGrid").jqGrid("getGridParam", "selrow");
            //row_id = $("#WrMgtGrid").getRowData(selectedRowId);
            //getIssueView2(row_id.fo_no, row_id.fno);

        }



    },
    

    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    autowidth: false,
    multiselect: false,
    sortable: true,
    loadonce: true,
    pager: '#layoutdtGridPager',
    viewrecords: true,
    height: 1060,
    width: $(".box-body2").width() - 10,
    //caption: 'Common ',
    caption: 'Factory Layout Detail',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
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
});
//bnt option






function btn_option(cellValue, options, rowdata, action) {


    var lastFive = cellValue.substr(cellValue.length - 5); // => "Tabs1"


    var lastFive1 = lastFive.substr(0,4);


    //$("select#id_of_select_element option").filter(":selected").val();

    //var option = $('option:selected', this).attr('id');
    //var id = $(this).children(":selected").attr("id");


    console.log(rowdata);

    var html4 = '<br><button class="btn btn-xs btn-info" data-ldno="' + lastFive1 + '" onclick="btn_save_gr(this);">Save</button>';
    var html5 = '<button class="btn btn-xs btn-danger" data-ldno="' + lastFive1 + '" onclick="btn_del_gr(this);">Delete</button>';
    return cellValue + "&nbsp" + html4 + "&nbsp" + html5;
    //return $.get("/System/getmlcd", function (data) {

    //    var tong_quat = "";
    //    var html = '<select id="myselection' + ldno + '">';
    //    var html2 = '<select id="myselection2' + ldno + '">';
    //    $.each(data.kq, function (key, item) {
    //        html += '<option value=' + item.ml_cd + '>' + item.ml_cd + '</option>';
    //    });

    //    for (var i = 0; i < data.kq2.length; i++) {
    //        html2 += '<option value=' + data.kq2[i].lct_cd + '>' + data.kq2[i].prounit_cd + '</option>';
    //    }
    //    html += '</select>'
    //    html2 += '</select>'
    //   
    //   return  tong_quat = html + html2 + "&nbsp&nbsp&nbsp" + html4 + "&nbsp&nbsp&nbsp" + html5;
    //    console.log(tong_quat);
    //    ////$('#test').html(tong_quat);
    //    //$('#test').html(tong_quat);
    //});

    ////var kq = $('#test').html();


}


function btn_save_gr(e) {
    console.log(e);
    var stt = $(e).data("ldno")



    //var id = $(this).children(":selected").attr("id");




    var bien1 = $("#myselection_" + stt + "").val(), select1, select2;
    if ((bien1 == "") || (bien1 == undefined)) {
        select1 = bien1;
    }
    var bien2 = $("#myselection2_" + stt + "").val();
    if ((bien2 == "") || (bien2 == undefined)) {
        select2 = bien2;
    }
    $.ajax({
        url: "/system/saveLayoutDT",
        type: "post",
        dataType: "json",
        data: {
            ldno: $(e).data("ldno"),
            select1: bien1,
            select2: bien2,
        },
        success: function (data) {
            if (data.result) {
                //var mt_cd = data.mt_cd;

                var id = data.data2.ldno;



                //var id = data.fmno;
                $("#layoutdtGrid").setRowData(id, data.data2, { background: "#d0e9c6" });

            }
            else {
                alert("Common Information was not existing. Please checking again !");
            }


            //jQuery("#layoutdtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
        },
        //error: function (result) {
        //    alert('User is not existing. Please check again');
        //}
    });
}

function btn_del_gr(id) {


   
    var stt = $(id).data("ldno")
    console.log(stt);
    $.ajax({
        url: "/system/deleteLayoutDT?id=" + stt,
        type: "post",
        dataType: "json",
        success: function (data) {
            //jQuery("#WmMgtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            //$("#WrMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');

            //$("#WrMgtGrid").jqGrid('delRowData', id);


            //$("#WmMgtGrid").setRowData(data.data2.fmno, data.data2, { background: "#d0e9c6" });


            jQuery("#layoutdtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
        },
        error: function (result) {
            alert('User is not existing. Please check again');
        }
    });
}





//$('#commondtGrid').jqGrid('setGridWidth', $(".box-body").width());
//$(window).on("resize", function () {
//    var newWidth = $("#commondtGrid").closest(".ui-jqgrid").parent().width();
//    $("#commondtGrid").jqGrid("setGridWidth", newWidth, false);
//});

$("#dc_save_but").click(function () {
    if ($("#form3").valid() == true) {
        var dc_mt_cd = $('#dc_mt_cd').val();
        var dc_dt_cd = $('#dc_dt_cd').val();
        var dc_dt_nm = $('#dc_dt_nm').val();
        var dc_dt_exp = $('#dc_dt_exp').val();
        var dc_dt_order = $('#dc_dt_order').val();
        var dc_use_yn = $('#dc_use_yn').val();
        $.ajax({
            url: "/system/insertCommonDetail",
            type: "get",
            dataType: "json",
            data: {
                dc_mt_cd: dc_mt_cd,
                dc_dt_cd: dc_dt_cd,
                dc_dt_nm: dc_dt_nm,
                dc_dt_exp: dc_dt_exp,
                dc_dt_order: (dc_dt_order == "") ? 0 : dc_dt_order,
                dc_use_yn: dc_use_yn,
            },
            success: function (data) {
                if (data.result == 0) {
                    var mt_cd = data.mt_cd;
                    var dt_cd = data.dt_cd;
                    $('#mt_remark_color').val(mt_cd);
                    $('#dt_remark_color').val(dt_cd);
                    jQuery("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    alert("Common Detail Information was existing. Please checking again !");
                }
            }
        });
    }
});
function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}

//Modifly Common main
$("#dm_save_but").click(function () {
    if ($("#form4").valid() == true) {
        var dm_cdid = $("#dm_cdid_cd").val();
        var dm_mt_cd = $('#dm_mt_cd').val();
        var dm_dt_cd = $('#dm_dt_cd').val();
        var dm_dt_nm = $('#dm_dt_nm').val();
        var dm_dt_exp = $('#dm_dt_exp').val();
        var dm_dt_order = $('#dm_dt_order').val();
        var dm_use_yn = $('#dm_use_yn').val();
        $.ajax({
            url: "/system/updateCommonDetail",
            type: "get",
            dataType: "json",
            data: {
                dm_cdid: dm_cdid,
                dm_mt_cd: dm_mt_cd,
                dm_dt_cd: dm_dt_cd,
                dm_dt_nm: dm_dt_nm,
                dm_dt_exp: dm_dt_exp,
                dm_dt_order: dm_dt_order,
                dm_use_yn: dm_use_yn,
            },
            success: function (data) {
                if (data.result != 0) {
                    var mt_cd = data.mt_cd;
                    var dt_cd = data.dt_cd;
                    $('#mt_remark_color').val(mt_cd);
                    $('#dt_remark_color').val(dt_cd);
                    jQuery("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    alert("This Common Detail was not existing. Please checking again");
                }
            }
        });
    }
});

$("#tab_d1").on("click", "a", function (event) {
    document.getElementById("form3").reset();
    $("#tab_d2").removeClass("active");
    $("#tab_d1").addClass("active");
    $("#tab_dc2").removeClass("active");
    $("#tab_dc1").removeClass("hidden");
    $("#tab_dc2").addClass("hidden");
    $("#tab_dc1").addClass("active");
    var selectedRowId = $("#commonGrid").getGridParam('selrow');
    //var mt_cd = $("#commonGrid").getCell(selectedRowId, "mt_cd");

    //var dm_mt_cd = row_id2.mt_cd;
    //$("#dm_mt_cd").val(dm_mt_cd);
    //$("#dc_mt_nm").val(mt_nm);
    //$("#dc_mt_cd").val(mt_cd);
    //$("#dc_cdid_cd").val(1);
    //$("#dc_dt_order").val(1);

    $("#dc_mt_cd").val($("#m_mt_cd").val());
    $("#dc_mt_nm").val($("#m_mt_nm").val());

});
$("#tab_d2").on("click", "a", function (event) {

    document.getElementById("form4").reset();
    $("#tab_d1").removeClass("active");
    $("#tab_d2").addClass("active");
    $("#tab_dc1").removeClass("active");
    $("#tab_dc2").removeClass("hidden");
    $("#tab_dc1").addClass("hidden");
    $("#tab_dc2").addClass("active");
    $("#dm_save_but").attr("disabled", true);
    $("#ddel_save_but").attr("disabled", true);
});

function bntCellValue(cellValue, options, rowdata, action) {
    var html = '<button class="btn btn-xs btn-danger deletebtn" title="Delete" data-commonmaincode="' + rowdata.mt_cd + '" >X</button>';
    return html;
};

function bntCellValueDetail(cellValue, options, rowdata, action) {
    var html = '<button class="btn btn-xs btn-danger detaildeletebtn" title="Delete" data-commondt_manincode="' + rowdata.mt_cd + '" data-commondt_dtcd="' + rowdata.dt_cd + '" >X</button>';
    return html;
};

//$("#form1").validate({
//    rules: {
//        "mt_nm": {
//            required: true,
//        },
//    },
//});

//$("#form2").validate({
//    rules: {
//        "mt_nm": {
//            required: true,
//        },
//    },
//});

//$("#form3").validate({
//    rules: {
//        "dt_cd": {
//            required: true,
//        },
//        "dt_nm": {
//            required: true,
//        },
//    },
//});

//$("#form4").validate({
//    rules: {
//        "dt_cd": {
//            required: true,
//        },
//        "dt_nm": {
//            required: true,
//        },
//    },
//});
