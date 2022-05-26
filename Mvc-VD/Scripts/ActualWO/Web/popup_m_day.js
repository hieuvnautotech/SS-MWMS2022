function popupm_day(olddno) {
    $.get("/ActualWO/popupm_day?olddno=" + olddno, function (data) {
        if (data != null && data != undefined && data.length) {
            $.each(data, function (key, item) {
                var date = item.work_ymd.substring(0, 4) + "-" + item.work_ymd.substring(4, 6) + "-" + item.work_ymd.substring(6, 8);
                $('#fo_no').val(item.fo_no);
                $('#line_no').val(item.line_no);
                $('#process_no').val(item.process_no);
                $('#prounit_cd').val(item.prounit_cd);
                $('#date').val(date);
                $('#refer_qty').val(item.refer_qty);
                $('#olddno').val(item.olddno);
            });
        }
    });

    $.get("/ActualWO/selectdont_end?olddno=" + olddno, function (data) {
        if (data != null && data != undefined && data.length) {
            var name1 = "";
            var name2 = "";
            var name3 = "";
            var name4 = "";
            var name5 = "";
            var html1 = "";
            var html2 = "";
            var html3 = "";
            var html4 = "";
            var html5 = "";
            var item_vcd = '';
            var item_nm = '';
            var item_exp = '';
            var check_id = '';
            var check_cd = '';
            var input = 1;
            var l = 0;
            var tong_hop = '';
            var id = "";
            var stt = 1;
            $.each(data, function (key, item) {
                item_vcd = item.item_vcd;
                item_nm = item.item_nm;
                item_exp = item.item_exp;
                check_cd = item.check_cd;
                check_id = item.check_id;
                if (item.check_type == "text") {
                    if (id == "" || id != item.icno) {
                        id = item.icno;
                        no = item.icno;
                        html1 += '<h4>' + stt + '.&nbsp;&nbsp;&nbsp;' + item.check_subject + '</h4>';
                        stt++;
                    }
                    html1 += '<h4>' + item.check_name + '</h4>';
                    html1 += '<input type="text" name="input_text" id="text_' + input++ + '"  class="input-text form-control" value="">';

                }
                
                if (item.check_type == "select" || item.check_type == "SELECT") {
                    if (l == 0) {

                        html2 += '<select id="select_type" class="form-control">';
                        html2 += '<option value="" selected="selected">**</option>';
                        l++;
                    }
                    if (id == "" || id != item.icno) {
                        id = item.icno;
                        no = item.icno;
                        html2 += '<h4>' + stt + '.&nbsp;&nbsp;&nbsp;' + item.check_subject + '</h4>';
                        stt++;
                    }
                    html2 += '<option value="' + item.check_name + '">&nbsp;&nbsp;' + item.check_name + '</option>';
                }
                if (item.check_type == "radio" || item.check_type == "RADIO") {
                    if (id == "" || id != item.icno) {
                        id = item.icno;
                        no = item.icno;

                        html3 += '<h4>' + stt + '.&nbsp;&nbsp;&nbsp;' + item.check_subject + '</h4>';
                        stt++;
                    }
                    html3 += '<div id="radio-group"> ';
                    html3 += ' <input type="radio" name="radio" value="' + item.check_name + '"  />&nbsp;&nbsp;' + item.check_name + '</div>';
                }
                if ((item.check_type == "check" || item.check_type == "CHECK") || (item.check_type == "range") || item.check_type == "RANGE") {

                    if (id == "" || id != item.icno) {

                        id = item.icno;
                        no = item.icno;
                        html4 += '<h4><th class="day_css">' + stt + ".&nbsp;&nbsp;&nbsp;" + item.check_subject + '</th><h4>';
                        html4 += ' <table style="width:100%" class="table_con"><tr>';
                        //html4 += '<td class="title_qty-' + item.icno + ' title_qty" width="5%">' + '<input type="checkbox" name="checkbox1" id="checkAll-' + item.icno + '" value="" onclick="myFunctionAll(' + item.icno + ')">' + '</td>';
                        html4 += '<td class="title_qty" width="60%">' + 'Content' + '</td>';
                        html4 += ' </tr> </table>';
                        stt++;
                    }

                    html4 += ' <table style="width:100%" class="table_con">';
                    html4 += ' <tr>';
                    html4 += '<td class="day_css title_qty" width="5%">' + '  <input type="checkbox" name="checkbox" value="' + item.check_name + '"  />' + '</td>';
                    html4 += '<td class="day_css" width="60%">' + item.check_name + '</td>';
                    html4 += ' </tr>';
                    html4 += ' </table>';
                }
                //if (item.check_type == "check") {
                //    name4 = '<h4>4,' + item.check_subject + '</h4>';
                //    html4 += '<div id="radio-group"> ';
                //    html4 += ' <input type="checkbox" name="checkbox" value="' + item.check_name + '"  />' + item.check_name + '</div>';
                //}
                if (item.check_type == "photo" || item.check_type == "PHOTO") {
                    if (id == "" || id != item.icno) {
                        id = item.icno;
                        no = item.icno;
                        html5 += '<h4>' + stt + '.&nbsp;&nbsp;&nbsp;' + item.check_subject + '</h4>';
                        stt++;
                    }

                    html5 += '<h4>' + item.check_name + '</h4>';
                    html5 += '    <input type="file" name="photo" id="photo" accept="image/gif, image/jpeg, image/png">';
                }
            });
            var tang = 0;
            $('#item_vcd').val(item_vcd);
            $('#check_cd').val(check_cd);
            $('#input_text').val(input - 1);
            $('#check_id').val(check_id);
            $('#item_nm').val(item_nm);
            $('#item_exp').val(item_exp);
            $('#qc_no').val(item_vcd);
            var html = html1 + html2 + html3 + html4 + html5;
            $("#view").html(html);

        }
          else {
                var html = "";
            $("#view").html(html);
        }
    });

}
$(".dialog").dialog({
    width: '100%',
    height: 800,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 1000,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
        "ui-dialog-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function () {
        selRowIds = $("#list2").jqGrid("getGridParam", "selarrrow")
        row_id = $("#list2").getRowData(selRowIds);
        popupm_day(row_id.olddno);
    },
});
$('#close_Popup_facline').click(function () {
    $('.dialog').dialog('close');
});

$("#take_popup").click(function () {
    var giatri = $('#input_text').val();
    var m = parseInt(giatri);
    var mang = [];
    for (var i = 0; i < m; i++) {
        var number = i + 1
        var id = '#text_' + number + '';
        var x = $(id).val();
        mang.push(x);
    }
    var checkbox = document.getElementsByName('checkbox');
    for (var i = 0; i < checkbox.length; i++) {
        if (checkbox[i].checked === true) {
            mang.push(checkbox[i].value);
        }
    }
    var radio = document.getElementsByName('radio');
    for (var i = 0; i < radio.length; i++) {
        if (radio[i].checked === true) {
            mang.push(radio[i].value);
        }
    }
    mang.push($('#select_type').val());
    var formData = new FormData();
    if (files != null) {
        var files = $("#photo").get(0).files;
    }
   

    formData.append("file", files[0]);
    formData.append("olddno", $('#olddno').val());
    formData.append("item_vcd", $('#item_vcd').val());
    formData.append("item_nm", $('#item_nm').val());
    formData.append("item_exp", $('#item_exp').val());
    formData.append("check_id", $('#check_id').val());
    formData.append("check_cd", $('#check_cd').val());
    formData.append("check_qty", $('#refer_qty').val());
    $.ajax({
        type: 'POST',
        url: "/ActualWO/Insert_fl_mc?mang=" + mang,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            $('.dialog').dialog('close');
        }
    });
});