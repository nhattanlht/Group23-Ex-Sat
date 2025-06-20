export const vi = {
  student: {
    title: 'Danh sách Sinh viên',
    add: 'Thêm Sinh viên',
    import_export: 'Nhập/Xuất',
    search: {
      placeholder: 'Tìm kiếm theo tên, mã số sinh viên',
      department_placeholder: 'Tất cả khoa',
      button: 'Tìm kiếm'
    },
    fields: {
      student_id: 'MSSV',
      full_name: 'Họ và Tên',
      date_of_birth: 'Ngày Sinh',
      gender: 'Giới Tính',
      department: 'Khoa',
      status: 'Trạng Thái',
      school_year: 'Khóa',
      study_program: 'Chương Trình',
      email: 'Email',
      phone: 'Số Điện Thoại',
      nationality: 'Quốc Tịch',
      permanent_address: 'Địa Chỉ Thường Trú',
      temporary_address: 'Địa Chỉ Tạm Trú',
      registered_address: 'Địa Chỉ Đăng Ký',
      identification: 'Giấy Tờ Tùy Thân',
      actions: 'Thao Tác'
    },
    form: {
      title: 'Chỉnh Sửa Thông Tin Sinh Viên',
      gender_options: {
        male: 'Nam',
        female: 'Nữ',
        other: 'Khác'
      },
      identification: {
        type: 'Loại Giấy Tờ',
        number: 'Số Giấy Tờ',
        issue_date: 'Ngày Cấp',
        expiry_date: 'Ngày Hết Hạn',
        issued_by: 'Nơi Cấp',
        has_chip: 'Có Gắn Chip',
        issuing_country: 'Quốc Gia Cấp',
        notes: 'Ghi Chú',
        types: {
          cmnd: 'CMND',
          cccd: 'CCCD',
          passport: 'Hộ Chiếu'
        }
      },
      address: {
        house_number: 'Số Nhà',
        street_name: 'Tên Đường',
        ward: 'Phường/Xã',
        district: 'Quận/Huyện',
        province: 'Tỉnh/Thành Phố',
        country: 'Quốc Gia'
      },
      buttons: {
        save: 'Lưu Thay Đổi',
        cancel: 'Hủy'
      }
    },
    actions: {
      edit: 'Sửa',
      delete: 'Xóa',
      add: 'Thêm',
      view: 'Xem',
      save: 'Lưu',
      cancel: 'Hủy'
    },
    tooltips: {
      edit: 'Nhấn để chỉnh sửa thông tin sinh viên',
      delete: 'Nhấn để xóa sinh viên',
      view: 'Nhấn để xem chi tiết',
      view_identification: "Xem thông tin giấy tờ"
    },
    messages: {
      delete_confirm: 'Bạn có chắc chắn muốn xóa sinh viên này?',
      delete_success: 'Xóa sinh viên thành công',
      delete_error: 'Lỗi khi xóa sinh viên',
      add_success: 'Thêm sinh viên thành công',
      add_error: 'Lỗi khi thêm sinh viên',
      edit_success: 'Cập nhật thông tin sinh viên thành công',
      edit_error: 'Lỗi khi cập nhật thông tin sinh viên',
      load_error: 'Lỗi khi tải danh sách sinh viên',
      no_data: 'Không có dữ liệu',
      required_field: 'Trường này là bắt buộc',
      nationality_required: 'Quốc tịch là bắt buộc',
      invalid_email: 'Vui lòng nhập địa chỉ email hợp lệ',
      invalid_phone: 'Vui lòng nhập số điện thoại hợp lệ'
    },
    address: {
      not_available: 'Không có',
      house_number: 'Số Nhà',
      street: 'Đường',
      ward: 'Phường/Xã',
      district: 'Quận/Huyện',
      province: 'Tỉnh/Thành Phố',
      country: 'Quốc Gia'
    },
    identification: {
      details: "Chi Tiết Giấy Tờ",
      type: "Loại Giấy Tờ",
      number: "Số Giấy Tờ",
      issue_date: "Ngày Cấp",
      expiry_date: "Ngày Hết Hạn",
      issued_by: "Nơi Cấp",
      has_chip: "Có Gắn Chip",
      issuing_country: "Quốc Gia Cấp",
      notes: "Ghi Chú",
      not_available: "Chưa có thông tin"
    },
  },
  menu: {
    title: 'Quản lý sinh viên',
    students: 'Sinh viên',
    departments: 'Khoa',
    programs: 'Chương trình',
    statuses: 'Tình trạng',
    courses: 'Khóa học',
    classes: 'Lớp học',
    enrollment: 'Đăng ký lớp học',
    grades: 'Điểm',
    import_export: 'Nhập/Xuất',
    collapse: 'Thu gọn menu',
    expand: 'Mở rộng menu'
  },
  common: {
    yes: "Có",
    no: "Không",
    close: "Đóng",
    loading: "Đang tải dữ liệu...",
    save: "Lưu",
    cancel: "Hủy",
    actions: "Thao tác",
    edit: "Sửa",
    delete: "Xóa",
    add: "Thêm",
    search: "Tìm kiếm",
    filter: "Lọc",
    view: "Xem",
    back: "Quay lại",
    next: "Tiếp theo",
    previous: "Trước",
    confirm: "Xác nhận",
    success: "Thành công",
    error: "Lỗi",
    warning: "Cảnh báo",
    info: "Thông tin",
    confirm_delete: "Bạn có chắc chắn muốn xóa mục này?",
  },
  departments: {
    title: "Danh sách khoa",
    label: "Khoa",
    fields: {
      name: "Tên khoa"
    },
    actions: {
      add: "Thêm",
      edit: "Sửa",
      delete: "Xóa"
    },
    messages: {
      add_success: "Thêm khoa thành công",
      add_error: "Lỗi khi thêm khoa",
      edit_success: "Cập nhật khoa thành công",
      edit_error: "Lỗi khi cập nhật khoa",
      delete_success: "Xóa khoa thành công",
      delete_error: "Lỗi khi xóa khoa",
      delete_confirm: "Bạn có chắc chắn muốn xóa khoa này?"
    }
  },
  programs: {
    title: "Danh sách chương trình",
    label: "Chương trình đào tạo",
    fields: {
      name: "Tên chương trình"
    },
    actions: {
      add: "Thêm",
      edit: "Sửa",
      delete: "Xóa"
    },
    messages: {
      add_success: "Thêm chương trình thành công",
      add_error: "Lỗi khi thêm chương trình",
      edit_success: "Cập nhật chương trình thành công",
      edit_error: "Lỗi khi cập nhật chương trình",
      delete_success: "Xóa chương trình thành công",
      delete_error: "Lỗi khi xóa chương trình",
      delete_confirm: "Bạn có chắc chắn muốn xóa chương trình này?"
    }
  },
  studentstatus: {
    title: "Danh sách tình trạng sinh viên",
    label: "Tình trạng sinh viên",
    fields: {
      name: "Tình trạng"
    },
    actions: {
      add: "Thêm",
      edit: "Sửa",
      delete: "Xóa"
    },
    messages: {
      add_success: "Thêm tình trạng sinh viên thành công",
      add_error: "Lỗi khi thêm tình trạng sinh viên",
      edit_success: "Cập nhật tình trạng sinh viên thành công",
      edit_error: "Lỗi khi cập nhật tình trạng sinh viên",
      delete_success: "Xóa tình trạng sinh viên thành công",
      delete_error: "Lỗi khi xóa tình trạng sinh viên",
      delete_confirm: "Bạn có chắc chắn muốn xóa tình trạng sinh viên này?"
    }
  },
  data_management: {
    title: "Quản lý dữ liệu",
    export: {
      json_button: "Xuất JSON",
      csv_button: "Xuất CSV",
      exporting: "Đang xuất..."
    },
    import: {
      title: "Nhập dữ liệu",
      format_select: "-- Chọn định dạng file --",
      json_option: "JSON",
      csv_option: "CSV",
      choose_file: "Chọn tập tin",
      no_file: "Chưa chọn tập tin",
      import_button: "Nhập",
      importing: "Đang nhập...",
      select_format_first: "Vui lòng chọn định dạng file trước khi chọn file!",
      select_file: "Vui lòng chọn file trước khi import!",
      success: "Import thành công!",
      error: "Lỗi khi import dữ liệu!"
    }
  },
  course: {
    title: 'Danh sách khóa học',
    add: 'Thêm Khóa Học',
    label: 'Khóa Học',
    fields: {
      courseCode: 'Mã Khóa Học',
      name: 'Tên Khóa Học',
      credits: 'Số Tín Chỉ',
      department: 'Khoa',
      description: 'Mô Tả',
      prerequisiteCourse: 'Môn Tiên Quyết',
      isActive: 'Hoạt Động',
      actions: 'Thao Tác'
    }
  },
  class: {
    title: 'Danh sách lớp học',
    add: 'Thêm Lớp Học',
    label: 'Lớp Học',
    fields: {
      classId: 'Mã Lớp Học',
      courseCode: 'Tên Khóa Học',
      academicYear: 'Năm Học',
      semester: 'Học Kỳ',
      teacher: 'Giảng Viên',
      maxStudents: 'Số Lượng Tối Đa',
      schedule: 'Lịch Học',
      room: 'Phòng Học',
      cancelDeadline: 'Thời gian hủy đăng ký',
      actions: 'Thao Tác'
    }
  },
  enrollment: {
    title: 'Danh sách đăng ký lớp học',
    add: 'Thêm Đăng Ký Lớp Học',
    label: 'Đăng Ký Lớp Học',
    fields: {
      classId: 'Lớp Học',
      courseCode: 'Khóa Học',
      StudentId: 'Sinh Viên',
      registeredAt: 'Thời gian đăng ký',
      isCancelled: 'Hủy',
      cancelReason: 'Lí do hủy',
      cancelDate: 'Thời gian hủy',
      actions: 'Thao Tác'
    }
  },
  grade: {
    title: 'Danh sách điểm',
    add: 'Thêm Điểm',
    label: 'Điểm',
    fields: {
      classId: 'Lớp Học',
      studentId: 'MSSV',
      student: 'Họ Tên Sinh Viên',
      score: 'Điểm',
      gradeLetter: 'Điểm Chữ',
      gpa: 'GPA',
      actions: 'Thao Tác'
    },
    export: {
      title: 'Xuất Bảng Điểm',
      guide: 'Nhập Mã Số Sinh Viên để xuất bảng điểm',
      button: 'Xuất',
      success: 'Xuất bảng điểm thành công!',
      error: 'Lỗi khi xuất bảng điểm!',
      file_name: 'BangDiem'
    }
  }
}; 