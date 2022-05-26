
var dict = {
    "Routing Code": {
        vn: "Mã định tuyến",
        en: "Routing Code"
    },
    "Name": {
        vn: "Tên",
        en: "Name"
    },
    "Create": {
        vn: "Tạo",
        en: "Create"
    },
    "Search": {
        vn: "Tìm Kiếm",
        en: "Search"
    },
    "Create Date": {
        vn: "Ngày Tạo",
        en: "Create Date"
    },
    "Model": {
        vn: "Mô hình",
        en: "Model"
    },
    "Modify": {
        vn: "Sửa",
        en: "Modify"
    },
    "Delete": {
        vn: "Xóa",
        en: "Delete"
    },
    "Save": {
        vn: "Lưu Lại",
        en: "Save"
    },

    "Reset": {
        vn: "Cài đặt lại",
        en: "Save"
    },
    "Process No": {
        vn: "Quy trình",
        en: "Process No"
    },
    "Next Process": {
        vn: "Quy trình tiếp theo",
        en: "Next Process"
    },
    "Selected": {
    vn: "Chọn",
    en: "Selected"
},
    "PRINT": {
        vn: "IN",
        en: "PRINT"
    },
    "Close": {
        vn: "Đóng",
        en: "Close"
    },
    "Standard": {
        vn: "Tiêu chuẩn",
        en: "Standard"
    },

    "Process": {
        vn: "Quy trình Chi tiết",
        en: "Process"
    },
    "Yes": {
        vn: "Có",
        en: "Yes"
    },
    "No": {
        vn: "Không",
        en: "No"
    },
    "color": {
        vn: "Màu sắc",
        en: "color"
    },

    "color": {
        vn: "Màu sắc",
        en: "color"
    },
    "Routing Information": {
        vn: "Thông Tin Chuyền sản xuất",
        en: "Routing Information"
    },
    "Process Sequence": {
        vn: "Trình tự",
        en: "Process Sequence"
    },
    "Part Process Code": {
        vn: "Mã Công đoạn ",
        en: "Part Process Code"
    },
    "Part Process Name": {
        vn: "Tên Công Đoạn",
        en: "Part Process Name"
    },
    "Staff": {
        vn: "Nhân Viên",
        en: "Staff"
    },
    "Lead Time(EA/ h)": {
        vn: "Thời Gian Dẫn(EA/ h)",
        en: "Lead Time(EA/ h)"
    },
    "Process Type": {
        vn: "Loại Công Đoạn",
        en: "Process Type"
    },
    "QC Code": {
        vn: "Mã QC",
        en: "QC Code"
    },
    "RGB Code": {
        vn: "Mã Màu",
        en: "RGB Code"
    },
}

var nn = "vn";
var translator = $('body').translate({ lang: "en", t: dict });

function setLanguage() {
    translator.lang(nn); //change to Portuguese
    onchange_language(nn);
    if (nn == "vn") { nn = "en"; }
    else { nn = "vn"; }
   
}
$(document).ready(function () {
    $(".ui-jqgrid-title").addClass("trn");
});