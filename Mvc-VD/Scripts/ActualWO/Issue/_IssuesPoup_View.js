function OpenPopup() {
    $('.dialoglinedetail').dialog('open');
}

function getIssueView2(mdii, style_no, style_nm, fo_no, title2, process_nm, old_no, fi1, fi2, f3) {
    var html1 = "IS-" + fo_no + "-" + process_nm;
    $("#title1").html(style_no + " " + '[ ' + style_nm + ' ]');
    $("#serial").html(html1);
    $('#mdii_v').val(mdii);
    $('#barcode').append(html1);
    $('#barcode').barcode(html1, "code128", { barWidth: 1, barHeight: 30 });
    $(document).ready(function () {
        $('#dataTable').DataTable({
            "bServerSide": true,
            "ajax": {
                "url": "/ActualWO/getIssue2?mdii=" + mdii,
                "type": "GET",
                "datatype": "json"
            },
            "searching": false,
            "paging": false,
            "bInfo": false,
            "columns": [
                { "data": "fo" },
                { "data": "line_no" },
                { "data": "process_nm" },
                { "data": "start_dt" },
                { "data": "end_dt" },
                { "data": "issue" },
                { "data": "content" }
            ],
            'columnDefs': [
     {
         "targets": 0, // your case first column
         "className": "text-center",
         "width": "5%"
     },
     {
         "targets": 1, // your case first column
         "className": "text-center",
         "width": "5%"
     },
      {
          "targets": 2, // your case first column
          "className": "text-left",
          "width": "10%"
      },
      {
          "targets": 3, // your case first column
          "className": "text-center",
          "width": "10%"
      },
      {
          "targets": 4, // your case first column
          "className": "text-center",
          "width": "10%"
      },
      {
          "targets": 5, // your case first column
          "className": "text-left",
          "width": "15%"
      },
      {
          "targets": 6, // your case first column
          "className": "text-left",
          "width": "25%"
      },

            ],
            "bDestroy": true
        });

    });

    $.ajax({
        url: "/ActualWO/getIMG2?mdii=" + mdii,
        type: "get",
        dataType: "json",
        success: function (data) {
            console.log(data.length);

            if (data != null && data != undefined) {
                if (data.length == 2) {
                    var html = '';
                    $.each(data, function (key, item) {
                        html += '<div class="col-md-6 text-center"> <img src="../Images/Issue/' + item.photo_file + '" style="width:100%" /> </div>'
                        //html += '<img src="../images/Issue/' + item.photo_file + '" style="height:350px;max-height: 350px" />'
                        console.clear;
                    });

                    //html += '</div>';
                    $("#photo2").html(html);
                }

                if (data.length == 3) {
                    var html = '';
                    $.each(data, function (key, item) {
                        html += '<div class="col-md-4 text-center"> <img src="../images/Issue/' + item.photo_file + '" style="width:100%" /> </div>'
                        //html += '<img src="../images/Issue/' + item.photo_file + '" style="height:350px;max-height: 350px" />'
                        console.clear;


                    });

                    //html += '</div>';
                    $("#photo2").html(html);
                }

                if (data.length == 1) {
                    var html = '';
                    $.each(data, function (key, item) {
                        html += '<div class="class="col-md-4 text-center"> <img src="../images/Issue/' + item.photo_file + '" style="width:100%" /> </div>'
                        //html += '<img src="../images/Issue/' + item.photo_file + '" style="height:350px;max-height: 350px" />'
                        console.clear;


                    });

                    //html += '</div>';
                    $("#photo2").html(html);
                }
            }
        }

    });

    $.ajax({
        url: "/ActualWO/getFilePrint?mdli=" + mdii,
        type: "get",
        dataType: "json",
        success: function (rtnData) {
            var html = '';
            $.each(rtnData, function (dataType, data) {
                {
                    html += '<a href="./../../Content/ActualWO/Issue/' + data.attach_file + '" target="_blank" download=' + data.attach_file + ' >' + data.attach_file + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp </a>';
                }
            });
            $("#file").html("<h4>" + '&nbsp;&nbsp;&nbsp;&nbsp' + html + "</h4>");

        }
    });

};

$("#c_print").on("click", function () {
    var mdli = $("#mdii_v").val();
    window.open("/ActualWO/_IssuePrint?mdli=" + mdli, "PRINT", "width=1500, height=800, left=0, top=100, location=no, status=no,")
})

$(function () {
    $(".dialoglinedetail").dialog({

        width: '75%',
        height: 800,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        zIndex: 1000,



        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",

        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },

        close: function (event, ui) {
            $(".info2").empty();
            $('.info2').slick('unslick');
            jQuery("#issueGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
    });



    $('#closeinedetail').click(function () {
        $(".info2").empty();
        $('.info2').slick('unslick');
        $('.dialoglinedetail').dialog('close');
    });
});