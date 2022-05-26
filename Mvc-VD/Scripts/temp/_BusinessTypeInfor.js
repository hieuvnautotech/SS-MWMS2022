//Create Grid
$(function () {

    $grid = $("#BusinessTypeInforGrid").jqGrid
   ({
       url: "/standard/getBusInforData?mt_cd=COM001",
       datatype: 'json',
       mtype: 'GET',
       colModel: [
           { key: true, label: 'cdid', name: 'cdid', width: 80, align: 'center', hidden: true },
           { key: false, label: 'Code', name: 'dt_cd', width: 100, align: 'left', sort: true },
           { key: false, label: 'Name', name: 'dt_nm', width: 150, align: 'left' },
           { key: false, label: 'Explain', name: 'dt_exp', width: 250, align: 'left' },
           { key: false, label: 'Use Y/N', name: 'use_yn', align: 'center', width: 80 },
       ],
       gridComplete: function () {
           var rows = $("#BusinessTypeInforGrid").getDataIDs();
           for (var i = 0; i < rows.length; i++) {
               var v_use_yn = $("#BusinessTypeInforGrid").getCell(rows[i], "use_yn");
               if (v_use_yn == "N") {
                   $("#BusinessTypeInforGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
               }
           }
       },


       onCellSelect: function (rowid, iCol, cellcontent, e) {
           if (iCol == 6) {
               $('.delete-btn').click(function () {
                   var cdid = $(this).data("id_delete");

                   $.ajax({
                       url: "/standard/deleteBus",
                       type: "post",
                       dataType: "json",
                       data: {
                           cdid: cdid,
                       },
                       success: function (result) {
                           $("#BusinessTypeInforGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                           document.getElementById("form1").reset();
                           $("#tab_2").removeClass("active");
                           $("#tab_1").addClass("active");
                           $("#tab_c2").removeClass("active");
                           $("#tab_c1").removeClass("hidden");
                           $("#tab_c2").addClass("hidden");
                           $("#tab_c1").addClass("active");

                       },
                       error: function (result) {
                           alert('User is not existing. Please check again');
                       }
                   });
               });
           }
       },
       //onSelectRow
       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#BusinessTypeInforGrid").jqGrid("getGridParam", 'selrow');
           var row_id = $("#BusinessTypeInforGrid").getRowData(selectedRowId);
           var cdid = row_id.cdid;
           var dt_cd = row_id.dt_cd;
           var dt_nm = row_id.dt_nm;
           var dt_exp = row_id.dt_exp;
           var use_yn = row_id.use_yn;

           $("#m_dt_cd").val(dt_cd);
           $("#m_dt_nm").val(dt_nm);
           $("#m_dt_exp").val(dt_exp);
           $("#m_cdid").val(cdid);
           $("#c_cdid").val(cdid);
           $("#m_use_yn").val(use_yn);

           $("#tab_1").removeClass("active");
           $("#tab_2").addClass("active");
           $("#tab_c1").removeClass("active");
           $("#tab_c2").removeClass("hidden");
           $("#tab_c1").addClass("hidden");
           $("#tab_c2").addClass("active");
           $("#m_save_but").attr("disabled", false);
           $("#del_save_but").attr("disabled", false);


       },
       pager: '#BusinessTypeInforPager',
       viewrecords: true,
       rowList: [50, 100, 200, 500, 1000],
       height: 400,
       width: $(".box-body").width() - 5,
       caption: 'Business Type Information',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
       autowidth: false,
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
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#BusinessTypeInforGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#BusinessTypeInforGrid").closest(".ui-jqgrid").parent().width();
        $("#BusinessTypeInforGrid").jqGrid("setGridWidth", newWidth, false);
    });


    function fun_check1() {
        if ($("#c_dt_cd").val().trim() == "") {
            alert("Please enter the Code.");
            $("#c_dt_cd").val("");
            $('#c_dt_cd').focus();
            return false;
        }
        if ($("#c_dt_nm").val().trim() == "") {
            alert("Please enter the Name.");
            $("#c_dt_nm").val("");
            $("#c_dt_nm").focus();
            return false;
        }
        return true;
    }
    $("#c_save_but").click(function () {
        if (fun_check1() == true) {
            //var cdid = $('#c_cdid').val();
            var dt_cd = $('#c_dt_cd').val();
            var dt_nm = $('#c_dt_nm').val();
            var use_yn = $('#c_use_yn').val();
            var dt_exp = $('#c_dt_exp').val();

            {
                $.ajax({
                    url: "/standard/insertBusInfor",
                    type: "POST",
                    dataType: "json",
                    data: {
                        dt_cd: dt_cd,
                        dt_nm: dt_nm,
                        use_yn: use_yn,
                        dt_exp: dt_exp,
                    },
                    success: function (data) {
                        if (data.result == true) {
                            var id = data.value.cdid;
                            $("#BusinessTypeInforGrid").jqGrid('addRowData', id, data.value, 'first');
                            $("#BusinessTypeInforGrid").setRowData(id, false, { background: "#d0e9c6" });
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
    function fun_check2() {
        if ($("#m_dt_nm").val().trim() == "") {
            alert("Please enter the Name.");
            $("#m_dt_nm").val("");
            $("#m_dt_nm").focus();
            return false;
        }
        return true;
    }
    $("#m_save_but").click(function () {
        if (fun_check2() == true) {
            var params = new Object();
            params.cdid = $('#m_cdid').val();
            params.dt_cd = $('#m_dt_cd').val();
            params.dt_nm = $('#m_dt_nm').val();
            params.use_yn = $('#m_use_yn').val();
            params.dt_exp = $('#m_dt_exp').val();
            
                $.ajax({
                    url: "/standard/updateBusInfo",
                    type: "POST",
                    dataType: "json",
                    data: params,
                    success: function (data) {
                        if (data.result == true) {
                            jQuery("#BusinessTypeInforGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            SuccessAlert(data.message);
                        }
                        else {
                            ErrorAlert(data.message);
                        }
                    }
                });
            }
    });

    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });

    //Search
    $("#searchBtn").click(function () {
        var s_dt_cd = $('#s_dt_cd').val().trim();
        var s_dt_nm = $('#s_dt_nm').val().trim();
        $.ajax({
            url: "/standard/searchBusInfo",
            type: "GET",
            dataType: "json",
            data: {
                dt_cd: s_dt_cd,
                dt_nm: s_dt_nm,
            },
            success: function (result) {
                $("#BusinessTypeInforGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");            
            },
            error: function (result) {
                alert('Not found');
            }
        });

    });

});

// Tab Create/Modifly
$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {
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
//Export
$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'BusinessType_Infor',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            type: 'xls',
            fileName: 'BusinessType_Infor',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'BusinessType_Infor',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});