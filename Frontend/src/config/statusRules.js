export const statusTransitionRules = {
    // Đang học có thể chuyển thành:
    1: [4, 2, 3], // Tạm dừng học (4), Tốt nghiệp (2), Đã thôi học (3)
    // Bảo lưu có thể chuyển thành:
    4: [1, 3], // Quay lại Đang học (1) hoặc Đã thôi học (3)
    // Đã tốt nghiệp không thể chuyển đổi
    2: [],
    // Đã thôi học không thể chuyển đổi
    3: []
  };
  
  export const getStatusName = (id) => {
    const statusMap = {
      1: "Đang học",
      2: "Đã tốt nghiệp",
      3: "Đã thôi học",
      4: "Tạm dừng học"
    };
    return statusMap[id] || "Không xác định";
  };