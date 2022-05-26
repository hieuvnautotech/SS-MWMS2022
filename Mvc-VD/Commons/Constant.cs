using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Commons
{
    public static class Constant
    {
        public const string ErrorSystem = "Lỗi hệ thống!!!";
        public const string DataExist = "Dữ liệu đã tồn tại!!!";
        public const string DataNoExist = "Dữ liệu không đã tồn tại!!!";
        public const string Success = "Thành công!!!";
        public const string Used = "Đã được sử dụng!!!";
        public const string NotFound = "Không tìm thấy!!!";
        public const string ScanAgain = "Vui lòng Scan Lại!!!";
        public const string ChooseAgain = "Vui lòng chọn  Lại!!!";
        public const string Exist = "Đã tồn tại!!!";
        public const string Status = "Đã chuyển trạng thái khác!!!";
        public const string CheckData = "Vui lòng kiểm tra lại dữ liệu!!!";
        public const string CannotDelete = "Không thể Xóa!!!";
        public const string Duplicate = "Trùng!!!";
        public const string Other = "Mã Lot này đã được chuyển qua công đoạn khác!";
        public const string OverloadMaximum = "Số lượng đã vượt giới hạn!";
        public const string OverloadError = "Sô lượng lỗi vượt giới hạn!";
        public const string Compare = "Số lượng không bằng số lượng đã chia trước đó!";
        public const string QCPass = "Đã kiểm tra QC!";
        public const string FindDK = "Không thể tìm thấy mã MT hoặc mã MT không đủ điều kiện để tiếp tục!";
        public const string QuantityInvalid = "Số lượng Không hợp lệ!";
        public const string NotEA = "Không phải dạng EA!";
        public const string NotRoll = "Không phải dạng ROLL!";
        public const string Tested = "Đã kiểm tra!";
        public const string CannotCancel = "Không thể hủy!";
        public const string EndLotSuccess = "Kết thúc Lot Thành công!";
        public const string MergeCodeCannotCancel = "Mã Merge không thể hủy !";
        public const string BackSuccess = "Trở về thành công!";
        public const string Divide = "Đã được chia ra trước đó!";
        public const string Started = "Đã bắt đầu vào công đoạn làm việc!";
        public const string CancelSuccess = "Hủy Thành công!";
        public const string CannotPass = "Không thể cho qua!";
        public const string QuantityNGInvalid = "Số lượng NG không hợp lệ!";
        public const string CannotMapping = "Không thể mapping!";
        public const string CannotRedo = "Không thể Làm lại!";
        public const string EndShift = "Ca này đã kết thúc!";
        public const string MappingBuyerSuccess = "Đã mapping với mã Buyer";
        public const string ErrorAuthorize = "Xin vui lòng đăng nhập.";
        public const string ErrorCancelEASearch = "Không tìm thấy composite vui lòng kiểm tra lại.";
    }


    class language
    {
        public const string vi = "vi";
        public const string kr = "kr";
        public const string en = "en";
    }
}