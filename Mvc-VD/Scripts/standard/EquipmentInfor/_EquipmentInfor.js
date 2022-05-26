$("#Equipment").jqGrid
({
    url: "/standard/equimentData",
    datatype: 'json',
    mtype: 'GET',
    colModel: [
        { label: 'emno', name: 'emno', key: true, width: 40, align: 'center', hidden: true },
        { label: 'Asset No', name: 'as_no', width: 150, align: 'center' },
        { label: 'Equip Code', name: 'as_cd', width: 90, align: 'center' },
        { label: 'Equip Name', name: 'as_nm', width: 90, align: 'center' },
        { label: 'Cost Center ', name: 'cost_dept_cd', width: 120, align: 'right' },
        { label: 'Buying Date', name: 'puchs_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { label: 'Changer', name: 'charge_id', width: 60, sorttype: false, sortable: false },
        { label: 'Location State', name: 'lct_sts_cd', width: 80, align: 'center' },
        { label: 'Create Name', name: 'reg_id', width: 100, align: 'center' },
        { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { label: 'Change Name', name: 'chg_id', width: 100, align: 'center' },
        { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
    ],
    gridComplete: function () {
        var rows = $("#Equipment").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var as_cd = $("#Equipment").getCell(rows[i], "as_cd");
            var giatri = $('#remark_color').val();
            if (as_cd == giatri) {
                $("#Equipment").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },
    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#Equipment").jqGrid("getGridParam", 'selrow');
        var row_id = $("#Equipment").getRowData(selectedRowId);
        var as_no = row_id.as_no;
        var emno = row_id.emno;
        var as_cd = row_id.as_cd;
        var as_nm = row_id.as_nm;
        var lct_sts_cd = row_id.lct_sts_cd;
        var charge_id = row_id.charge_id;
        var puchs_dt = row_id.puchs_dt;
        var cost_dept_cd = row_id.cost_dept_cd;

        $("#m_as_no").val(as_no);
        $("#m_as_cd").val(as_cd);
        $("#m_as_nm").val(as_nm);
        $("#m_lct_sts_cd").val(lct_sts_cd);
        $("#m_changr_id").val(charge_id);
        $("#m_puchs_dt").val(puchs_dt);
        $("#m_cost_dept_cd").val(cost_dept_cd);
        $("#m_emno").val(emno);

        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#tab_1").removeClass("active");

        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);


    },
    pager: '#EquipmentPager',
    viewrecords: true,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    height: 400,
    width: $(".box-body").width() - 5,
    loadonce: true,
    caption: 'Equipment Information',
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
$('#Equipment').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#Equipment").closest(".ui-jqgrid").parent().width();
    $("#Equipment").jqGrid("setGridWidth", newWidth, false);
});
$("#c_save_but").click(function () {
    if ($("#form1").valid() == true) {
        var emno = $('#c_emno').val();
        var as_no = $('#c_as_no').val();
        var as_cd = $('#c_as_cd').val();
        var puchs_dt = $('#c_puchs_dt').val();
        var as_nm = $('#c_as_nm').val();
        var cost_dept_cd = $('#c_cost_dept_cd').val();
        var changr_id = $('#c_changr_id').val();
        var lct_sts_cd = $('#c_lct_sts_cd').val();
        $.ajax({
            url: "/standard/inserteqm",
            type: "get",
            dataType: "json",
            data: {
                emno: emno,
                as_no: as_no,
                as_cd: as_cd.toUpperCase(),
                as_nm: as_nm,
                puchs_dt: puchs_dt,
                changr_id: changr_id,
                cost_dept_cd: cost_dept_cd,
                lct_sts_cd: lct_sts_cd,
            },
            success: function (data) {
                if (data.result == 0) {
                    var c_as_cd = $('#c_as_cd').val();
                    $('#remark_color').val(c_as_cd.toUpperCase());
                    jQuery("#Equipment").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    alert("Equipment Infor was existing. Please checking again.");
                }
            }
        });
    }
});


$("#m_save_but").click(function () {
    if ($("#form2").valid() == true) {
        var emno = $('#m_emno').val();
        var as_no = $('#m_as_no').val();
        var as_cd = $('#m_as_cd').val();
        var puchs_dt = $('#m_puchs_dt').val();
        var as_nm = $('#m_as_nm').val();
        var cost_dept_cd = $('#m_cost_dept_cd').val();
        var changr_id = $('#m_changr_id').val();
        var lct_sts_cd = $('#m_lct_sts_cd').val();
        $.ajax({
            url: "/standard/UpdateEquipmentinfo",
            type: "get",
            dataType: "json",
            data: {
                emno: emno,
                as_no: as_no,
                as_cd: as_cd.toUpperCase(),
                as_nm: as_nm,
                puchs_dt: puchs_dt,
                changr_id: changr_id,
                cost_dept_cd: cost_dept_cd,
                lct_sts_cd: lct_sts_cd,

            },
            success: function (data) {
                if (data.result != 0) {
                    var m_as_cd = $('#m_as_cd').val();
                    $('#remark_color').val(m_as_cd.toUpperCase());
                    jQuery("#Equipment").setGridParam({ datatype: "json" }).trigger('reloadGrid');

                }
                else {
                    alert("Equipment Information was existing. Please checking again !");
                }
            }
        });
    }
    
});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});

$("#searchBtn").click(function () {
    var s_depart_cd = $('#s_depart_cd').val().trim();
    var s_depart_nm = $('#s_depart_nm').val().trim();
    $.ajax({
        url: "/standard/searchEquipmentinfo",
        type: "get",
        dataType: "json",
        data: {
            s_depart_cd: s_depart_cd,
            s_depart_nm: s_depart_nm,
        },
        success: function (result) {

            $("#Equipment").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});

$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").addClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
});
$("#tab_2").on("click", "a", function (event) {
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");

    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
});

$('#pdfBtn').click(function (e) {
    $('#Equipment').tableExport({
        type: 'pdf',
        pdfFontSize: '12',
        fileName: '',
        escape: false,
    });
});

$('#excelBtn').click(function (e) {
    $('#Equipment').tableExport({
        type: 'xls',
        fileName: 'Dept_Info',
        escape: false,
    });
});

$('#htmlBtn').click(function (e) {
    $('#Equipment').tableExport({
        type: 'doc',
        fileName: 'Dept_Info',
        escape: false,
    });
});

$("#m_puchs_dt").datepicker({
    dateFormat: 'yy-mm-dd',
});


$("#c_puchs_dt").datepicker({
    dateFormat: 'yy-mm-dd',
});

$("#form1").validate({
    rules: {
        "as_cd": {
            required: true,
        },
        "as_nm": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "as_cd": {
            required: true,
        },
        "as_nm": {
            required: true,
        },
    },
});