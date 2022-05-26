
$(function () {
    var row_id, row_id2;
    $("#DevelopCommonGrid").jqGrid
   ({
       url: "/DevManagement/getDevelopCommon",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'id', name: 'mt_id', index: 'mt_id', width: 10, hidden: true },
           { key: false, label: 'Main Code', name: 'mt_cd', index: 'mt_cd', width: 100, align: 'center' },
           { key: false, label: 'Main Name', name: 'mt_nm', width: 150 },
           { key: false, label: 'Main Explain', name: 'mt_exp', width: 300 },
           { key: false, label: 'Use Y/N', name: 'use_yn', width: 100, align: 'center' }
       ],
       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#DevelopCommonGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#DevelopCommonGrid").getRowData(selectedRowId);
           var mt_cd = row_id.mt_cd;
           var mt_nm = row_id.mt_nm;
           var mt_exp = row_id.mt_exp;
           var use_yn = row_id.use_yn;
           var mt_id = row_id.mt_id;

           document.getElementById("form2").reset();
           $("#m_save_but").attr("disabled", false);
           if (Session_authName == "root") { $("#del_save_but").attr("disabled", false); }        
           $("#tab_1").removeClass("active");
           $("#tab_2").addClass("active");
           $("#tab_c1").removeClass("active");
           $("#tab_c2").removeClass("hidden");
           $("#tab_c1").addClass("hidden");
           $("#tab_c2").addClass("active");
           $("#DevCommDtGridPager").clearGridData();
           $("#DevCommDtGrid").setGridParam({ url: "/DevManagement/getDevCommDtData?" + "mt_cd=" + mt_cd, datatype: "json" }).trigger("reloadGrid");

           $("#m_mt_cd").val(mt_cd);
           $("#m_mt_nm").val(mt_nm);
           $("#m_mt_exp").val(mt_exp);
           $("#m_use_yn").val(use_yn);
           $("#m_mt_id").val(mt_id);
           $("#dc_mt_cd").val(mt_cd);
   
       },

       pager: "#DevelopCommonGridPager",
       viewrecords: true,
       rowList: [50, 100, 200, 500],
       height: 250,
       width: $(".box-body").width() - 5,
       caption: 'Develop Common',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
       autowidth: false,
       jsonReader:
       {
           root: "rows",
           page: "page",
           total: "total",
           records: "records",
           repeatitems: false,
           Id: "0"
       },

       multiselect: false,
   });
    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    $('#DevelopCommonGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#DevelopCommonGrid").closest(".ui-jqgrid").parent().width();
        $("#DevelopCommonGrid").jqGrid("setGridWidth", newWidth, false);
    });
    $("#c_save_but").click(function () {
        debugger;
        var mt_cd = $('#c_mt_cd').val();
        var mt_nm = $('#c_mt_nm').val();
        var mt_exp = $('#c_mt_exp').val();
        var use_yn = $('#c_use_yn').val();


        if ($("#c_mt_nm").val().trim() == "") {
            alert("Please enter Main Name");
            $("#c_mt_nm").val("");
            $("#c_mt_nm").focus();
            return false;
        }
        if ($("#c_mt_exp").val().trim() == "") {
            alert("Please enter Main Explain");
            $("#c_mt_exp").val("");
            $("#c_mt_exp").focus();
            return false;
        }
        else {
            $.ajax({
                url: "/DevManagement/insertDevelopCommonMT",
                type: "POST",
                dataType: "json",
                data: {
                    mt_cd: mt_cd,
                    mt_nm: mt_nm,
                    mt_exp: mt_exp,
                    use_yn: use_yn,
                },
                success: function (data) {
                    if (data.result == true) {
                        var id = data.value.mt_id;
                        $("#DevelopCommonGrid").jqGrid('addRowData', id, data.value, 'first');
                        $("#DevelopCommonGrid").setRowData(id, false, { background: "#d0e9c6" });
                        //jQuery("#DevelopCommonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                }
            });
        }
    });

    //Modifly Common main
    $("#m_save_but").click(function () {
        debugger; 

        //ahihi
        var mt_cd = $('#m_mt_cd').val();
        var mt_nm = $('#m_mt_nm').val();
        var mt_exp = $('#m_mt_exp').val();
        var use_yn = $('#m_use_yn').val();
        var mt_id = $('#m_mt_id').val();
        $.ajax({
            url: "/DevManagement/updateDevelopCommonMT",
            type: "PUT",
            dataType: "json",
            data: {
                mt_cd: mt_cd,
                mt_nm: mt_nm,
                mt_exp: mt_exp,
                use_yn: use_yn,
                mt_id: mt_id,
            },
            success: function (data) {
                if (data.result == true) {
                    //jQuery("#DevelopCommonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    var id = data.value.mt_id;
                    $("#DevelopCommonGrid").setRowData(id, data.value, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });

    });

});

// Tab
$("#tab_1").on("click", "a", function (event) {
    $("#DevelopCommonGrid").trigger('reloadGrid');
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {
    $("#DevelopCommonGrid").trigger('reloadGrid');
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

$(function () {
    $("#DevCommDtGrid").jqGrid({
       url: "/DevManagement/getDevCommDtData",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'cdid', name: 'cdid', width: 80, align: 'center', hidden: true },
           { key: false, label: 'mt_cd', name: 'mt_cd', width: 80, align: 'center', hidden: true, },
           { key: false, label: 'Main Name', name: 'mt_nm', width: 120, align: 'center' },
           { key: false, label: 'Detail Code', name: 'dt_cd', width: 100, align: 'center' },
           { key: false, label: 'Detail Name', name: 'dt_nm', index: 'dt_nm', width: 150, },
           { key: false, label: 'Detail Explain', name: 'dt_exp', width: 250, },
           { key: false, label: 'Use Y/N', name: 'use_yn', align: 'center', width: 80 },
           { key: false, label: 'Order', name: 'dt_order', align: 'center', width: 80 }
       ],

       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#DevCommDtGrid").jqGrid("getGridParam", 'selrow');
           row_id2 = $("#DevCommDtGrid").getRowData(selectedRowId);

           var dm_cdid = row_id2.cdid;
           var dm_mt_cd = row_id2.mt_cd;
           var dm_dt_cd = row_id2.dt_cd;
           var dm_dt_nm = row_id2.dt_nm;
           var dm_dt_exp = row_id2.dt_exp;
           var dm_dt_order = row_id2.dt_order;
           var dm_use_yn = row_id2.use_yn;

           document.getElementById("form4").reset();
           $("#tab_d1").removeClass("active");
           $("#tab_d2").addClass("active");
           $("#tab_dc1").removeClass("active");
           $("#tab_dc2").removeClass("hidden");
           $("#tab_dc1").addClass("hidden");
           $("#tab_dc2").addClass("active");
           $("#dm_save_but").attr("disabled", false);
           $("#ddel_save_but").attr("disabled", false);

           $("#dm_cdid_cd").val(dm_cdid);
           $("#dm_mt_cd").val(dm_mt_cd);
           $("#dm_dt_cd").val(dm_dt_cd);
           $("#dm_dt_nm").val(dm_dt_nm);
           $("#dc_mt_cd").val(dm_mt_cd);
           $("#dm_dt_exp").val(dm_dt_exp);
           $("#dm_dt_order").val(dm_dt_order);
           $("#dm_use_yn").val(dm_use_yn);
          
       },

       pager: '#DevCommDtGridPager',
       viewrecords: true,
       rowList: [10, 20, 30, 40, 50, 100],
       height: 150,
       width: $(".box-body").width() - 5,
       rownumbers: true,
       gridview: true,
       caption: 'Dev Common Detail',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       pager: "#DevCommDtGridPager",
       shrinkToFit: false,
       autowidth: false,
       jsonReader:
       {
           root: "rows",
           page: "page",
           total: "total",
           records: "records",
           repeatitems: false,
           Id: "0"
       },

       multiselect: false,
   });
    $('#DevCommDtGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#DevCommDtGrid").closest(".ui-jqgrid").parent().width();
        $("#DevCommDtGrid").jqGrid("setGridWidth", newWidth, false);
    });


    function fun_check1() {

        if ($("#dc_mt_cd").val() == "") {
            alert("Please select Common Main Grid.");
            $("#c_mt_nm").val("");
            $("#c_mt_nm").focus();
            return false;
        }
        if ($("#dc_dt_cd").val() == "") {
            alert("Please enter the code");
            $("#dc_dt_cd").val("");
            $("#dc_dt_cd").focus();
            return false;
        }
        if ($("#dc_dt_nm").val() == "") {
            alert("Please enter the name");
            $("#dc_dt_nm").val("");
            $("#dc_dt_nm").focus();
            return false;
        }
        if ($("#dc_dt_exp").val() == "") {
            alert("Please enter the explain");
            $("#dc_dt_exp").val("");
            $("#dc_dt_exp").focus();
            return false;
        }
        if ($("#dc_dt_order").val() == "") {
            $("#dc_dt_order").val(1);
        }
        return true;
    }

    $("#dc_save_but").click(function () {
        debugger;
        if (fun_check1() == true) {
            var dt_cd = $('#dc_dt_cd').val();
            var dt_nm = $('#dc_dt_nm').val();
            var dt_exp = $('#dc_dt_exp').val();
            var use_yn = $('#dc_use_yn').val();
            var mt_cd = $('#dc_mt_cd').val();
            var dt_order = $('#dc_dt_order').val();

            $.ajax({
                url: "/DevManagement/insertDevCommDt",
                type: "Post",
                dataType: "json",
                data: {
                    dt_cd: dt_cd,
                    dt_nm: dt_nm,
                    dt_exp: dt_exp,
                    use_yn: use_yn,
                    mt_cd: mt_cd,
                    dt_order: dt_order,
                },
                success: function (data) {
                    debugger;
                    if (data.result == true) {
                        var id = data.value.cdid;
                        $("#DevCommDtGrid").jqGrid('addRowData', id, data.value, 'first');
                        $("#DevCommDtGrid").setRowData(id, false, { background: "#d0e9c6" });
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                }
            });
        }
    });

    function fun_check2() {
        if ($("#dm_dt_order").val() == "") {
            var selectedRowId = $("#DevCommDtGrid").getGridParam('selrow');
            var row = $("#DevCommDtGrid").getRowData(selectedRowId);
            var dt_order = row.dt_order;
            $("#dm_dt_order").val(dt_order);
        }
        return true;
    }

    $("#dm_save_but").click(function () {

        if (fun_check2() == true) {
            var dt_cd = $('#dm_dt_cd').val();
            var dt_nm = $('#dm_dt_nm').val();
            var dt_exp = $('#dm_dt_exp').val();
            var use_yn = $('#dm_use_yn').val();
            var mt_cd = $('#dm_mt_cd').val();
            var cdid = $('#dm_cdid_cd').val();
            var dt_order = $('#dm_dt_order').val();

            {
                $.ajax({
                    url: "/DevManagement/updateDevCommDt",
                    type: "get",
                    dataType: "json",
                    data: {
                        dt_cd: dt_cd,
                        dt_nm: dt_nm,
                        dt_exp: dt_exp,
                        use_yn: use_yn,
                        mt_cd: mt_cd,
                        cdid: cdid,
                        dt_order: dt_order,
                    },
                    success: function (data) {
                        if (data.result == true) {
                            jQuery("#DevCommDtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            SuccessAlert(data.message);
                        }
                        else {
                            ErrorAlert(data.message);
                        }
                    }

                });
            }
        }
    });
});
$("#tab_d1").on("click", "a", function (event) {
    $("#DevCommDtGrid").trigger('reloadGrid');
    document.getElementById("form3").reset();
    $("#tab_d2").removeClass("active");
    $("#tab_d1").addClass("active");
    $("#tab_dc2").removeClass("active");
    $("#tab_dc1").removeClass("hidden");
    $("#tab_dc2").addClass("hidden");
    $("#tab_dc1").addClass("active");
    var selectedRowId = $("#DevelopCommonGrid").getGridParam('selrow');
    var mt_cd = $("#DevelopCommonGrid").getCell(selectedRowId, "mt_cd");
    var dm_mt_cd = row_id2.mt_cd;
    $("#dm_mt_cd").val(dm_mt_cd);
    $("#dc_mt_cd").val(mt_cd);
    $("#dc_dt_order").val(1);


});
$("#tab_d2").on("click", "a", function (event) {
    $("#DevCommDtGrid").trigger('reloadGrid');
    $("#dm_save_but").attr("disabled", true);
    $("#ddel_save_but").attr("disabled", true);
    document.getElementById("form4").reset();
    $("#tab_d1").removeClass("active");
    $("#tab_d2").addClass("active");
    $("#tab_dc1").removeClass("active");
    $("#tab_dc2").removeClass("hidden");
    $("#tab_dc1").addClass("hidden");
    $("#tab_dc2").addClass("active");
});