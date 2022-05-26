
function popupm_day(id) {
    $('.dialog').dialog('open');
    $.get("/QMS/selectdont_end?id=" + id, function (data) {
        if (data != null && data != undefined && data.length) {
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
            console.log(data);
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
                        html4 += '<td class="title_qty-' + item.icno + ' title_qty" width="5%">' + '<input type="checkbox" name="checkbox1" id="checkAll-' + item.icno + '" value="" onclick="myFunctionAll(' + item.icno + ')">' + '</td>';
                        html4 += '<td class="title_qty" width="60%">' + 'Content' + '</td>';
                        html4 += ' </tr> </table>';
                        stt++;
                    }

                    html4 += ' <table style="width:100%" class="table_con">';
                    html4 += ' <tr>';
                    html4 += '<td class="day_css title_qty" width="5%">' + ' <input type="checkbox" value="' + item.order_no + '"   data-valuetwo="' + item.check_name + '"  />' + '</td>';
                    html4 += '<td class="day_css" width="60%">' + item.check_name + '</td>';
                    html4 += ' </tr>';
                    html4 += ' </table>';
                }
                if (item.check_type == "photo"  || item.check_type == "PHOTO") {
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

            var html = html1 + html2 + html3 + html4 + html5;
            $("#view").html(html);

        } else {
            var html = "";

            $("#view").html(html);
        }
    });

}

$(function () {
    $(".dialog").dialog({
        width: '30%',
        height: 800,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '20%',
        minHeight: 450,
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
        open: function (event, ui) {

        },
    });
    $('#close').click(function () {
        $('.dialog').dialog('close');
    });

});
