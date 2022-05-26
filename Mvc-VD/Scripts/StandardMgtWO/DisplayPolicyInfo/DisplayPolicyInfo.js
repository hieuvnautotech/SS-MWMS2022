
$(function () {
    var row_id, row_id2;
    $grid = $("#dpinfoGrid").jqGrid
   ({
       url: "/StandardMgtWO/getDisplayPolicyInfo",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'mpid', name: 'mpid', width: 10, hidden: true },
           { label: 'Code', name: 'policy_code', width: 100, align: 'center' },
           { label: 'Name', name: 'policy_name', width: 200, align: 'left' },
           { label: 'Start Date', name: 'policy_start_dt', width: 150, align: 'center', formatter: DateFormatter },
           { label: 'End Date', name: 'policy_end_dt', width: 150, align: 'center', formatter: DateFormatter2 },
           { label: 'Policy Type', name: 'policy_typecd', width: 100, align: 'center', hidden: true,/*formatter: fomartStrToHour2*/ },
           { label: 'Policy Type', name: 'policy_type', width: 100, align: 'center',/*formatter: fomartStrToHour2*/ },
           { label: 'Time', name: 'doing_time', width: 100, align: 'center', /*formatter: fomartStrToHour2*/ },
           { label: 'Use Y/N', name: 'use_yn', width: 100, align: 'center' },
           { label: 'Description', name: 're_mark', width: 200 },
           { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
       ],

       gridComplete: function () {
           var rows = $("#wpmtGrid").getDataIDs();
           for (var i = 0; i < rows.length; i++) {
               var v_use_yn = $("#wpmtGrid").getCell(rows[i], "use_yn");
               if (v_use_yn == "N") {
                   $("#wpmtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
               }

               var wid = $("#wpmtGrid").getCell(rows[i], "wid");
               if (wid == $('#color_bg_line').val()) {
                   $("#wpmtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
               }
           }
       },

       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#dpinfoGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#dpinfoGrid").getRowData(selectedRowId);

           var a = row_id.work_starttime,
               b = row_id.work_endtime,
               c = row_id.work_hour,
               e = row_id.lunch_start_time,
               f = row_id.lunch_end_time,
               g = row_id.dinner_start_time,
               h = row_id.dinner_end_time;


           $("#m_mpid").val(row_id.mpid);
           $("#m_policy_code").val(row_id.policy_code);
           $("#m_policy_name").val(row_id.policy_name);
           $("#m_doing_time").val(row_id.doing_time);
           $("#m_policy_start_dt").val(row_id.policy_start_dt);

           $("#m_policy_end_dt").val(row_id.policy_end_dt);


           $("#m_policy_type").val(row_id.policy_typecd);
           $("#m_use_yn").val(row_id.use_yn);
           $("#m_re_mark").val(row_id.re_mark);

           $("#tab_1").removeClass("active");
           $("#tab_2").addClass("active");
           $("#tab_c1").removeClass("active");
           $("#tab_c2").removeClass("hidden");
           $("#tab_c1").addClass("hidden");
           $("#tab_c2").addClass("active");
           $("#m_save_but").attr("disabled", false);


       },

       pager: "#dpinfoGridPager",
       pager: jQuery('#dpinfoGridPager'),
       viewrecords: true,
       rowNum: 50,
       rowList: [50, 100, 200, 500, 1000],
       height: 300,
       width: $(".box-body").width() - 5,
       caption: 'Display Policy Information',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
       multiselect: false,
       autowidth: false,
       loadonce: true,
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
    $('#dpinfoGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#dpinfoGrid").closest(".ui-jqgrid").parent().width();
        $("#dpinfoGrid").jqGrid("setGridWidth", newWidth, false);
    });

    function DateFormatter(cellvalue, options, rowObject) {
        return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6")
    };


    function DateFormatter2(cellvalue, options, rowObject) {
        return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6");

    };


    function fomartStrToHour2(cellvalue, options, rowObject) {

        return cellvalue.replace(/(\d{1,2})(\d{2})$/, "$1:$2");

    }


    function formartDecToHour2(cellvalue, options, rowObject) {
        var sign = cellvalue < 0 ? "-" : "",
            min = Math.floor(Math.abs(cellvalue)),
            sec = Math.floor((Math.abs(cellvalue) * 60) % 60);
        return sign + (min < 10 ? "" : "") + min + ":" + (sec < 10 ? "0" : "") + sec
    }


    function formartDecToHour(minutes) {
        var sign = minutes < 0 ? "-" : "",
            min = Math.floor(Math.abs(minutes)),
            sec = Math.floor((Math.abs(minutes) * 60) % 60);
        return sign + (min < 10 ? "" : "") + min + ":" + (sec < 10 ? "0" : "") + sec
    }


    function fomartStrToHour(s) {
        return hours = s.substring(0, s.length - 2),
            mins = s.substring(s.length - 2),
            res = hours + ":" + mins,
            console.log(res), res
    }


    function fomartHourToStr(n) {
        var res = n.replace(/\D/g, ""),
            hour = res.substring(0, res.length - 2),
            minute = res.substring(res.length - 2);
        return (hour.length < 2 ? "0" : "") + hour + ":" + minute
    }


    function formarHourToDec(t) {
        var arr = t.split(':'),
            dec = parseInt((arr[1] / 6) * 10, 10);
        return parseFloat(parseInt(arr[0], 10) + '.' + (dec < 10 ? '0' : '') + dec)
    }

    $("#c_save_but").click(function () {
        var policy_name = $("#c_policy_name").val(),
            e = $("#c_policy_code").val(),
            id = $("#c_mpid").val(),
            b = $("#c_policy_start_dt").val(),
            c = $("#c_policy_end_dt").val(),
            f = $("#c_doing_time").val(),
            g = $("#c_policy_type").val(),
            use_yn = $("#c_use_yn").val(),
            re_mark = $("#c_re_mark").val();

        if ($("#form1").valid() == true) {

            $.ajax({
                url: "/StandardMgtWO/insertDisplayPolicyInfo",
                type: "get",
                dataType: "json",
                data: {
                    policy_name: policy_name,
                    policy_code: e,
                    policy_start_dt: b,
                    policy_end_dt: c,
                    doing_time: f,
                    policy_type: g,
                    use_yn: use_yn,
                    re_mark: re_mark,
                },
                success: function (data) {
                    var id = data.mpid;
                    $("#dpinfoGrid").jqGrid('addRowData', id, data, 'first');
                    $("#dpinfoGrid").setRowData(id, false, { background: "#d0e9c6" });
                    document.getElementById("form1").reset();

                }
            });
        }
    });

    //Modifly Common main
    $("#m_save_but").click(function () {

        var policy_name = $("#m_policy_name").val(),
            e = $("#m_policy_code").val(),
            id = $("#m_mpid").val(),
            b = $("#m_policy_start_dt").val(),
            c = $("#m_policy_end_dt").val(),
            f = $("#m_doing_time").val(),
            g = $("#m_policy_type").val(),
            use_yn = $("#m_use_yn").val(),
            re_mark = $("#m_re_mark").val();
        if ($("#form2").valid() == true) {
            $.ajax({
                url: "/StandardMgtWO/updateDisplayPolicyInfo",
                type: "get",
                dataType: "json",
                data: {
                    id: id,
                    policy_name: policy_name,
                    policy_code: e,
                    policy_start_dt: b,
                    policy_end_dt: c,
                    doing_time: f,
                    policy_type: g,
                    use_yn: use_yn,
                    re_mark: re_mark,
                },
                success: function (data) {
                    var id = data.mpid;
                    $("#dpinfoGrid").setRowData(id, data, { background: "#d0e9c6" });
                }
            });
        }
    });

    var getPolicyType = "/StandardMgtWO/getPolicyType";
    $(document).ready(function () {
        getPolicyTypeS();
        $("#s_policy_type").on('change', function () {
            var id = $(this).val();
            if (id != undefined && id != '') {
            }
        });
    });
    function getPolicyTypeS() {

        $.get(getPolicyType, function (data) {
            if (data != null && data != undefined && data.length) {
                var html = '';
                html += '<option value="" selected="selected">*Policy Type ALL*</option>';
                $.each(data, function (key, item) {
                    console.log(item);
                    html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
                });
                $(".policy_type").html(html);
            }
        });
    }


     $("#searchBtn").click(function () {
        var policy_type = $('#s_policy_type').val().trim();
        $.ajax({
            url: "/StandardMgtWO/searchDisplayPolicyInfo",
            type: "get",
            dataType: "json",
            data: {
                policy_type: policy_type
            },
            success: function (result) {

                $("#dpinfoGrid").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");


            }
        });

    });

});


$('#c_policy_start_dt').datetimepicker({

    format: 'HH:mm',
});

$('#c_policy_end_dt').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',

});

$('#m_policy_start_dt').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',
});

$('#m_policy_end_dt').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',
});


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
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////

function loadValue() {
    document.getElementById("c_policy_end_dt").value = "99991231235959";
}



$.validator.addMethod("endDateC", function (value, element) {
    var startDate = $("#c_policy_start_dt").val();
    var date1 = (new Date(startDate.replace(/^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/, '$1-$2-$3 $4:$5:$6'))).getTime();
    var date2 = (new Date(value.replace(/^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/, '$1-$2-$3 $4:$5:$6'))).getTime();
    return date1 <= date2 || value == "";
}, "End date must be after start date");



$.validator.addMethod("startDateC", function (value, element) {
    var startDate = $("#c_policy_end_dt").val();
    var date1 = (new Date(startDate.replace(/^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/, '$1-$2-$3 $4:$5:$6'))).getTime();
    var date2 = (new Date(value.replace(/^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/, '$1-$2-$3 $4:$5:$6'))).getTime();
    return date1 >= date2 || value == "";
}, "Start date must be before end date");



$.validator.addMethod("endDateM", function (value, element) {
    var startDate = $("#m_policy_start_dt").val();
    var date1 = (new Date(startDate.replace(/^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/, '$1-$2-$3 $4:$5:$6'))).getTime();
    var date2 = (new Date(value.replace(/^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/, '$1-$2-$3 $4:$5:$6'))).getTime();
    return date1 <= date2 || value == "";
}, "End date must be after start date");

$.validator.addMethod("startDateM", function (value, element) {
    var startDate = $("#m_policy_end_dt").val();
    var date1 = (new Date(startDate.replace(/^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/, '$1-$2-$3 $4:$5:$6'))).getTime();
    var date2 = (new Date(value.replace(/^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/, '$1-$2-$3 $4:$5:$6'))).getTime();
    return date1 >= date2 || value == "";
}, "Start date must be before end date");


$.validator.addMethod("greaterThan", function (value, element, params) {

    if (!/Invalid|NaN/.test(new Date(value))) {
        return new Date(value) > new Date($(params).val());
    }

    return isNaN(value) && isNaN($(params).val())
        || (Number(value) > Number($(params).val()));
}, 'Must be greater than {0}.');

$("#form1").validate({
    rules: {
        "c_policy_name": {
            required: true,
        },
        "c_policy_start_dt": {
            required: true,
            startDateC: true
        },
        "c_policy_end_dt": {
            required: true,
            endDateC: true,
        },
        "c_doing_time": {
            required: true,
            number: true,
        },
        "c_policy_type": {
            required: true,

        },
    },
});

$("#form2").validate({
    rules: {
        "m_policy_name": {
            required: true,
        },
        "m_policy_start_dt": {
            required: true,
            startDateM: true
        },
        "m_policy_end_dt": {
            required: true,
            endDateM: true,
        },
        "m_doing_time": {
            required: true,
            number: true,
        },
        "m_policy_type": {
            required: true,

        },
    },
});
