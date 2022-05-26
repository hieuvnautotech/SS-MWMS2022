
$(function () {
    var row_id, row_id2;
    $grid = $("#wpmtGrid").jqGrid
        ({
            url: "/StandardMgtWO/getWorkPolicyInfo",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wid', name: 'wid', width: 10, hidden: true },
                { label: 'Code', name: 'policy_code', width: 100, align: 'center' },
                { label: 'Name', name: 'policy_name', width: 200, align: 'left' },
                { label: 'Work Start', name: 'work_starttime', width: 100, align: 'center', formatter: fomartStrToHour2 },
                { label: 'Work End', name: 'work_endtime', width: 100, align: 'center', formatter: fomartStrToHour2 },

                { label: 'Lunch Start', name: 'lunch_start_time', width: 100, align: 'center', formatter: fomartStrToHour2 },
                { label: 'Lunch End', name: 'lunch_end_time', width: 100, align: 'center', formatter: fomartStrToHour2 },
                { label: 'Dinner Start', name: 'dinner_start_time', width: 100, align: 'center', formatter: fomartStrToHour2 },
                { label: 'Dinner End', name: 'dinner_end_time', width: 100, align: 'center', formatter: fomartStrToHour2 },

                { label: 'Work Hour', name: 'work_hour', width: 100, align: 'center', formatter: formartDecToHour2 },
                { label: 'Use Y/N', name: 'use_yn', width: 100, align: 'center' },
                { label: 'Description', name: 're_mark', width: 200, align: 'left' },
                { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            ],
            gridComplete: function () {
                var rows = $("#wpmtGrid").getDataIDs();
                for (var i = 0; i < rows.length; i++) {

                    var po_no = $("#wpmtGrid").getCell(rows[i], "wid");
                    if (po_no == $('#color_bg_line').val()) {
                        $("#wpmtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                    }
                }
            },

            onSelectRow: function (rowid, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#wpmtGrid").jqGrid("getGridParam", 'selrow');
                row_id = $("#wpmtGrid").getRowData(selectedRowId);

                var a = row_id.work_starttime,
                    b = row_id.work_endtime,
                    c = row_id.work_hour,
                    e = row_id.lunch_start_time,
                    f = row_id.lunch_end_time,
                    g = row_id.dinner_start_time,
                    h = row_id.dinner_end_time;


                $("#m_wid").val(row_id.wid);
                $("#m_policy_code").val(row_id.policy_code);
                $("#m_policy_name").val(row_id.policy_name);
                $("#m_work_starttime").val(row_id.work_starttime);
                $("#m_work_endtime").val(row_id.work_endtime);




                $("#m_lunch_start_time").val(row_id.lunch_start_time);
                $("#m_lunch_end_time").val(row_id.lunch_end_time);
                $("#m_dinner_start_time").val(row_id.dinner_start_time);
                $("#m_dinner_end_time").val(row_id.dinner_end_time);



                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false);

                $("#m_work_hour").val(row_id.work_hour);
                $("#m_use_yn").val(row_id.use_yn);
                $("#m_re_mark").val(row_id.re_mark);

                $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").removeClass("hidden");
                $("#tab_c1").addClass("hidden");
                $("#tab_c2").addClass("active");
            },

            pager: "#wpmtGridPager",
            pager: jQuery('#wpmtGridPager'),
            viewrecords: true,
            rowList: [50, 200, 500],
            height: 220,
            width: $(".box-body").width() - 5,
            rowNum: 50,
            caption: 'Work Policy Information',
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

        })

    function DateFormatter(cellvalue, options, rowObject) {
        return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6")
    };



    function fomartStrToHour2(cellvalue, options, rowObject) {

        return cellvalue.replace(/(\d{1,2})(\d{2})$/, "$1:$2");

    }


    function formartDecToHour2(cellvalue, options, rowObject) {
        var sign = cellvalue < 0 ? "-" : "",
            min = Math.floor(Math.abs(cellvalue)),
            sec = Math.floor((Math.abs(cellvalue) * 60) % 60);
        return sign + (min < 10 ? "" : "") + min + ":" + (sec < 10 ? "0" : "") + sec;
    }


    //function formartDecToHour(minutes) {
    //    var sign = minutes < 0 ? "-" : "",
    //        min = Math.floor(Math.abs(minutes)),
    //        sec = Math.floor((Math.abs(minutes) * 60) % 60);
    //    return sign + (min < 10 ? "" : "") + min + ":" + (sec < 10 ? "0" : "") + sec
    //}

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

    //Create Common main
    $("#c_save_but").click(function () {

        var policy_name = $("#c_policy_name").val(),
            b = $("#c_work_starttime").val(),
            c = $("#c_work_endtime").val(),

            e = $("#c_lunch_start_time").val(),
            f = $("#c_lunch_end_time").val(),
            g = $("#c_dinner_start_time").val(),
            h = $("#c_dinner_end_time").val(),
            d = $("#c_work_hour").val(),

            use_yn = $("#c_use_yn").val(),
            re_mark = $("#c_re_mark").val(),
            work_start = fomartHourToStr(b).replace(/\D/g, ""),
            work_end = fomartHourToStr(c).replace(/\D/g, ""),
            lunch_start = fomartHourToStr(e).replace(/\D/g, ""),
            lunch_end = fomartHourToStr(f).replace(/\D/g, ""),
            dinner_start = fomartHourToStr(g).replace(/\D/g, ""),
            dinner_end = fomartHourToStr(h).replace(/\D/g, ""),

            work_hour = formarHourToDec(d);


        $.ajax({
            url: "/StandardMgtWO/insertWorkPolicyInfo",
            type: "Post",
            dataType: "json",
            data: {
                policy_name: policy_name,
                work_start: work_start,
                work_end: work_end,
                lunch_start: lunch_start,
                lunch_end: lunch_end,
                dinner_start: dinner_start,
                dinner_end: dinner_end,
                work_hour: work_hour,
                use_yn: use_yn,
                re_mark: re_mark,
            },
            success: function (data) {
                if (data.result == true) {
                    $("#color_bg_line").val(data.wid);
                    jQuery("#wpmtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    document.getElementById("form1").reset();
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });

    });

    //Modifly Common main
    $("#m_save_but").click(function () {

        var policy_name = $("#m_policy_name").val(),
            wid = $("#m_wid").val(),
            b = $("#m_work_starttime").val(),
            c = $("#m_work_endtime").val(),
            e = $("#m_lunch_start_time").val(),
            f = $("#m_lunch_end_time").val(),
            g = $("#m_dinner_start_time").val(),
            h = $("#m_dinner_end_time").val(),

            d = $("#m_work_hour").val(),

            use_yn = $("#m_use_yn").val(),
            re_mark = $("#m_re_mark").val(),
            work_start = fomartHourToStr(b).replace(/\D/g, ""),
            work_end = fomartHourToStr(c).replace(/\D/g, ""),
            lunch_start = fomartHourToStr(e).replace(/\D/g, ""),
            lunch_end = fomartHourToStr(f).replace(/\D/g, ""),
            dinner_start = fomartHourToStr(g).replace(/\D/g, ""),
            dinner_end = fomartHourToStr(h).replace(/\D/g, ""),

            work_hour = formarHourToDec(d);
        $.ajax({
            url: "/StandardMgtWO/updateWorkPolicyInfo",
            type: "Put",
            dataType: "json",
            data: {
                wid: wid,
                policy_name: policy_name,
                work_start: work_start,
                work_end: work_end,
                work_hour: work_hour,
                lunch_start: lunch_start,
                lunch_end: lunch_end,
                dinner_start: dinner_start,
                dinner_end: dinner_end,
                use_yn: use_yn,
                re_mark: re_mark,
            },
            success: function (data) {
                if (data.result == true) {
                    $('#color_bg_line').val($('#m_wid').val());
                    jQuery("#wpmtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                } else {
                    ErrorAlert(data.message);
                }
            }
        });

    });

    //Delete Common main
    $("#del_save_but").click(function () {
        var wid = $("#m_wid").val();
        $.ajax({
            url: "/StandardMgtWO/deletetWorkPolicyInfo",
            type: "Delete",
            dataType: "json",
            data: {
                id: wid,
            },
            success: function (data) {
                if (data.result == true) {
                    $('#color_bg_line').val($('#m_wid').val());
                    jQuery("#wpmtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                } else {
                    ErrorAlert(data.message);
                }

            }
        });
    });

    //Search Common main
    $("#searchBtn").click(function () {
        var mt_cd = $('#mt_cd').val().trim();
        var mt_nm = $('#mt_nm').val().trim();
        var mt_exp = $('#mt_exp').val().trim();
        $.ajax({
            url: "/StandardMgtWO/searchWHSCommon",
            type: "GET",
            dataType: "json",
            data: {
                mt_cd: mt_cd,
                mt_nm: mt_nm,
                mt_nm: mt_nm
            },
            success: function (result) {

                $("#WHSCommonGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                $("#WHSCommonDtGrid").clearGridData();

                document.getElementById("form1").reset();
                $("#tab_2").removeClass("active");
                $("#tab_1").addClass("active");
                $("#tab_c2").removeClass("active");
                $("#tab_c1").removeClass("hidden");
                $("#tab_c2").addClass("hidden");
                $("#tab_c1").addClass("active");

                document.getElementById("form3").reset();
                $("#tab_d2").removeClass("active");
                $("#tab_d1").addClass("active");
                $("#tab_dc2").removeClass("active");
                $("#tab_dc1").removeClass("hidden");
                $("#tab_dc2").addClass("hidden");
                $("#tab_dc1").addClass("active");

            }
        });

    });

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
    $("#del_save_but").attr("disabled", true);
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});