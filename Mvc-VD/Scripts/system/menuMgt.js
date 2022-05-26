      $grid = $("#jqGrid").jqGrid({
          url: '/System/GetData',
          mtype: 'GET',
          datatype: 'json',
          colModel: [
  { label: 'ID', name: 'mnno', key: true, width: 60, align: 'right', hidden: true, key: true },
  { name: 'mn_cd', hidden: true },
  { name: 'up_mn_cd', hidden: true },
  { name: 'level_cd', hidden: true },
  { name: 'sub_yn', hidden: true },
  { name: 'mn_cd_full', hidden: true },
  { label: 'Menu Name', name: 'mn_nm', sortable: true, editable: true, editrules: { required: true, number: true }, width: 150, align: 'left' },
  { label: 'Full Name', name: 'mn_full', sortable: true, width: 350, editrules: { required: true, number: true }, align: 'left' },
  { label: 'URL', name: 'url_link', width: 220, align: 'left', sortable: true, editable: true, editrules: { required: true, number: true } },
  { label: 'USE YN', name: 'use_yn', width: 55, align: 'center', sortable: true, editrules: { required: true, number: true } },
  { label: 'Order No', name: 'order_no', width: 70, align: 'center', sortable: true, editable: true },
  { label: 'Reg Name', name: 'reg_id', sortable: true, },
  {
      label: 'Reg Date', name: 'reg_dt', sortable: true, formatter: "date",
      formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
  },

          ],
          loadonce: true,
          userDataOnFooter: true,
          pager: '#jqGridPager',
          rowNum: 20000,
          rownumbers: true,      
          rowList: [50, 100, 200],
          viewrecords: true,
          emptyrecords: "No data.",
          gridview: true,
          rownumbers: true,
          shrinkToFit: false,
          height: 800,
          jsonReader:
           {
               root: "rows",
               page: "page",
               total: "total",
               records: "records",
               repeatitems: false,
               Id: "0"
           },
          width: $(".box-body").width() - 5,
          autowidth: false,
          onSelectRow: function (rowid, status, e) {
              var selectedRowId = $("#jqGrid").jqGrid("getGridParam", 'selrow');
              row_id = $("#jqGrid").getRowData(selectedRowId);
              var mn_nm = row_id.mn_nm;
              var mnno = row_id.mnno;
              var mn_full = row_id.mn_full;
              var level_cd = row_id.level_cd;
              var url_link = row_id.url_link;
              var order_no = row_id.order_no;
              var use_yn = row_id.use_yn;

              var up_mn_cd = row_id.up_mn_cd;
              var mn_cd_full = row_id.mn_cd_full;
              $('#m_mn_nm').val(mn_nm);
              $('#m_url_link').val(url_link);
              $('#m_order_no').val(order_no);
              $('#m_mnno').val(mnno);
              $('#c_mnno').val(mnno);
              $('#c_mn_full').val(mn_full);
              $('#c_up_mn_cd').val(up_mn_cd);
              $('#c_mn_cd_full').val(mn_cd_full);

              $("#m_use_yn").val(use_yn);
                
              if (level_cd == "002") {
                  $("#c_mn_nm").prop("disabled", true);
                  $("#c_url_link").prop("disabled", true);
                  $("#c_save_but").prop("disabled", true);
                  $("#c_reset_but").prop("disabled", true);
              } else {
                  $("#c_mn_nm").prop("disabled", false);
                  $("#c_url_link").prop("disabled", false);
                  $("#c_save_but").prop("disabled", false);
                  $("#c_reset_but").prop("disabled", false);
              }

          },

      });

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#jqGrid").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "Menu Information.xlsx",
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 40,
                 onBeforeExport: null,
                 replaceStr: null
             }
         );
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $("#jqGrid").jqGrid('exportToHtml',
            options = {
                title: '',
                onBeforeExport: null,
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                tableClass: 'jqgridprint',
                autoPrint: false,
                topText: '',
                bottomText: '',
                fileName: "Menu Information  ",
                returnAsString: false
            }
         );
    });

});
$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#jqGrid").jqGrid('exportToPdf',
          options = {
              title: null,
              orientation: 'landscape',
              pageSize: 'A4',
              description: null,
              onBeforeExport: null,
              download: 'download',
              includeLabels: true,
              includeGroupHeader: true,
              includeFooter: true,
              fileName: "Menu.pdf",
              mimetype: "application/pdf"
          }
         );
    });

});

    $("#m_save_but").click(function () {
        var selRowId = $('#jqGrid').jqGrid("getGridParam", "selrow");
        console.log("selRowId : " + selRowId);
        if (selRowId == null) {
            alert("Please select the top menu on the grid.");
            return false;
        }

        if ($("#m_mn_nm").val().trim() == "") {
            alert("Please enter the menu Name");
            $('#m_mn_nm').focus();
            return false;
        }
        $.ajax({
            url: "/system/updateMenuMgt",
            type: "get",
            dataType: "json",
            data: {
                mn_nm: $('#m_mn_nm').val(),
                use_yn: $('#m_use_yn').val(),
                mn_nm: $('#m_mn_nm').val(),
                use_yn: $('#m_use_yn option:selected').val(),
                url_link: $('#m_url_link').val(),
                order_no: $('#m_order_no').val(),
                mnno: $("#m_mnno").val()
            },
            success: function (data) {
                $("#jqGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data }).trigger("reloadGrid");
                document.getElementById("form1").reset();
            }


        });

    });

    $(document).ready(function () {
        $('input[name=root_yn]').click(function () {
            if ($(this).prop("checked") == true) {
                $("#c_root_yn").val("1");
            }
            else if ($(this).prop("checked") == false) {
                $("#c_root_yn").val("");
            }
        });
    });
$("#c_save_but").click(function () {
    if ($('input[name=root_yn]').is(":checked")) {
        if ($("#c_mnno").val() == "") {
            $("#c_mnno").val("1");
        }

    } else {
        var selRowId = $('#jqGrid').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top Menu on the grid.");
            return false;
        }
    }
    if ($("#c_mn_nm").val().trim() == "") {
        alert("Please enter Location Name");
        $("#c_mn_nm").val("");
        $("#c_mn_nm").focus();
        return false;
    }
    if ($("#c_url_link").val().trim() == "") {
        alert("Please enter URL");
        $("#c_url_link").val("");
        $("#c_url_link").focus();
        return false;
    }


    else {
        var c_root_yn = $("#c_root_yn").val();
        $.ajax({
            url: "/system/insertMenu",
            type: "get",
            dataType: "json",
            data: {
                mn_nm: $('#c_mn_nm').val(),
                use_yn: $('#c_use_yn').val(),
                url_link: $('#c_url_link').val(),
                mnno: $('#c_mnno').val(),
                mn_full: $('#c_mn_full').val(),
                up_mn_cd: $('#c_up_mn_cd').val(),
                c_root_yn: $('#c_root_yn').val(),
            },
            success: function (data) {
                $("#jqGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data }).trigger("reloadGrid");
                document.getElementById("form2").reset();
                $("#c_root_yn").val("0");
            }

        });
    }
});
$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});
