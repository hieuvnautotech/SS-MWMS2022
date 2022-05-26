var row_id, row_id2;
$("#layoutGrid").jqGrid
({
    url: "/System/_getLayoutMT",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { name: "ldno", width: 100, align: "left", label: "ldno", resizable: true },
        { name: "layer_lct_cd", width: 100, align: "center", label: "layer_lct_cd", resizable: true },
   

    ],


    //onSelectRow: function (rowid, status, e) {

    //    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
    //    var selectedRowId = $("#layoutGrid").jqGrid("getGridParam", 'selrow');
    //    row_id = $("#layoutGrid").getRowData(selectedRowId);
    //    var lmno = row_id.lmno;
    //    var ml_cd = row_id.ml_cd;
    //    var ml_nm = row_id.ml_nm;


    //    document.getElementById("form2").reset();
    //    $("#m_save_but").attr("disabled", false);
    //    if (Session_authName == "root") {
    //        $("#del_save_but").attr("disabled", false);
    //    }
    //    $("#tab_1").removeClass("active");
    //    $("#tab_2").addClass("active");
    //    $("#tab_c1").removeClass("active");
    //    $("#tab_c2").removeClass("hidden");
    //    $("#tab_c1").addClass("hidden");
    //    $("#tab_c2").addClass("active");
    //    //$("#commondtGridPager").clearGridData();
    //    //$("#layoutdtGrid").setGridParam({ url: "/system/getLayoutDT?" + "ml_cd=" + ml_cd, datatype: "json" }).trigger("reloadGrid");

    //    $("#m_lmno").val(lmno);
    //    $("#m_ml_cd").val(ml_cd);
    //    $("#m_ml_nm").val(ml_nm);
    //    //$("#m_use_yn").val(use_yn);
    //    //$("#m_mt_id").val(mt_id);
    //    //$("#dc_mt_cd").val(mt_cd);
    //    //$("#dc_mt_nm").val(mt_nm);

    //    //document.getElementById("form3").reset();
    //    //$("#tab_d2").removeClass("active");
    //    //$("#tab_d1").addClass("active");
    //    //$("#tab_dc2").removeClass("active");
    //    //$("#tab_dc1").removeClass("hidden");
    //    //$("#tab_dc2").addClass("hidden");
    //    //$("#tab_dc1").addClass("active");
    //    //$("#dm_mt_cd").val(dm_mt_cd);
    //    //$("#dc_mt_nm").val(mt_nm);
    //    //$("#dc_mt_cd").val(mt_cd);
    //    //$("#dc_cdid_cd").val(1);
    //    //$("#dc_dt_order").val(1);
    //    //$("#dc_mt_cd").val(mt_cd);
    //    //$("#dc_mt_nm").val(mt_nm);

    //},
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    autowidth: false,
    multiselect: false,
    sortable: true,
    loadonce: true,
    pager: '#layoutGridPager',
    viewrecords: true,
    height: 1000,
    width: $(".box-body").width() - 5,
    caption: 'Common Main',
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
$('#layoutGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#commonGrid").closest(".ui-jqgrid").parent().width();
    $("#layoutGrid").jqGrid("setGridWidth", newWidth, false);
});

$("#c_save_but").click(function () {
    //if ($("#form1").valid() == true) {
    //var mt_cd = $('#c_mt_cd').val().toUpperCase();
    var ml_cd = $('#c_ml_cd').val();
    //var ml_nm = $('#c_ml_nm').val();
    //var use_yn = $('#c_use_yn').val();
    $.ajax({
        url: "/system/_insertLayoutMT",
        type: "get",
        dataType: "json",
        data: {
            layer_lct_cd: ml_cd,
     
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
                alert("Common Information was existing. Please checking again !");
            }
        }
    });
    //}
});

