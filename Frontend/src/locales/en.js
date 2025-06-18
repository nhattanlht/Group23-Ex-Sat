export const en = {
  student: {
    title: 'Student List',
    add: 'Add Student',
    import_export: 'Import/Export',
    search: {
      placeholder: 'Search by name, student ID',
      department_placeholder: 'All departments',
      button: 'Search'
    },
    fields: {
      student_id: 'Student ID',
      full_name: 'Full Name',
      date_of_birth: 'Date of Birth',
      gender: 'Gender',
      department: 'Department',
      status: 'Status',
      school_year: 'School Year',
      study_program: 'Study Program',
      email: 'Email',
      phone: 'Phone Number',
      nationality: 'Nationality',
      permanent_address: 'Permanent Address',
      temporary_address: 'Temporary Address',
      registered_address: 'Registered Address',
      identification: 'Identification',
      actions: 'Actions'
    },
    form: {
      title: 'Edit Student Information',
      gender_options: {
        male: 'Male',
        female: 'Female',
        other: 'Other'
      },
      identification: {
        type: 'ID Type',
        number: 'ID Number',
        issue_date: 'Issue Date',
        expiry_date: 'Expiry Date',
        issued_by: 'Issued By',
        has_chip: 'Has Chip',
        issuing_country: 'Issuing Country',
        notes: 'Notes',
        types: {
          cmnd: 'National ID',
          cccd: 'Citizen ID',
          passport: 'Passport'
        }
      },
      address: {
        house_number: 'House Number',
        street_name: 'Street Name',
        ward: 'Ward',
        district: 'District',
        province: 'Province',
        country: 'Country'
      },
      buttons: {
        save: 'Save Changes',
        cancel: 'Cancel'
      }
    },
    actions: {
      edit: 'Edit',
      delete: 'Delete',
      add: 'Add',
      view: 'View',
      save: 'Save',
      cancel: 'Cancel'
    },
    tooltips: {
      edit: 'Click to edit student information',
      delete: 'Click to delete student',
      view: 'Click to view details',
      view_identification: "View identification details"
    },
    messages: {
      delete_confirm: 'Are you sure you want to delete this student?',
      delete_success: 'Student deleted successfully',
      delete_error: 'Error deleting student',
      add_success: 'Student added successfully',
      add_error: 'Error adding student',
      edit_success: 'Student information updated successfully',
      edit_error: 'Error updating student information',
      load_error: 'Error loading student list',
      no_data: 'No data found',
      required_field: 'This field is required',
      nationality_required: 'Nationality is required',
      invalid_email: 'Please enter a valid email address',
      invalid_phone: 'Please enter a valid phone number'
    },
    address: {
      not_available: 'Not available',
      house_number: 'House Number',
      street: 'Street',
      ward: 'Ward',
      district: 'District',
      province: 'Province',
      country: 'Country'
    },
    identification: {
      details: "Identification Details",
      type: "Identification Type",
      number: "Identification Number",
      issue_date: "Issue Date",
      expiry_date: "Expiry Date",
      issued_by: "Issued By",
      has_chip: "Has Chip",
      issuing_country: "Issuing Country",
      notes: "Notes",
      not_available: "Not available"
    },
  },
  menu: {
    title: 'Student Management',
    students: 'Students',
    departments: 'Departments',
    programs: 'Programs',
    statuses: 'Statuses',
    courses: 'Courses',
    classes: 'Classes',
    enrollment: 'Enrollment',
    grades: 'Grades',
    import_export: 'Import/Export',
    collapse: 'Collapse menu',
    expand: 'Expand menu'
  },
  common: {
    yes: "Yes",
    no: "No",
    close: "Close",
    loading: "Loading...",
    save: "Save",
    cancel: "Cancel",
    actions: "Actions",
    edit: "Edit",
    delete: "Delete",
    add: "Add",
    search: "Search",
    filter: "Filter",
    view: "View",
    back: "Back",
    next: "Next",
    previous: "Previous",
    confirm: "Confirm",
    success: "Success",
    error: "Error",
    warning: "Warning",
    info: "Information"
  },
  departments: {
    title: "Department List",
    label: "Department",
    fields: {
      name: "Department Name"
    },
    actions: {
      add: "Add",
      edit: "Edit",
      delete: "Delete"
    },
    messages: {
      add_success: "Department added successfully",
      add_error: "Error adding department",
      edit_success: "Department updated successfully",
      edit_error: "Error updating department",
      delete_success: "Department deleted successfully",
      delete_error: "Error deleting department",
      delete_confirm: "Are you sure you want to delete this department?"
    }
  },
  programs: {
    title: "Program List",
    label: "Study Program",
    fields: {
      name: "Program Name"
    },
    actions: {
      add: "Add",
      edit: "Edit",
      delete: "Delete"
    },
    messages: {
      add_success: "Program added successfully",
      add_error: "Error adding program",
      edit_success: "Program updated successfully",
      edit_error: "Error updating program",
      delete_success: "Program deleted successfully",
      delete_error: "Error deleting program",
      delete_confirm: "Are you sure you want to delete this program?"
    }
  },
  studentstatus: {
    title: "Student Status List",
    label: "Student Status",
    fields: {
      name: "Status"
    },
    actions: {
      add: "Add Student Status",
      edit: "Edit",
      delete: "Delete"
    },
    messages: {
      add_success: "Student status added successfully",
      add_error: "Error adding student status",
      edit_success: "Student status updated successfully",
      edit_error: "Error updating student status",
      delete_success: "Student status deleted successfully",
      delete_error: "Error deleting student status",
      delete_confirm: "Are you sure you want to delete this student status?"
    }
  },
  data_management: {
    title: "Data Management",
    export: {
      json_button: "Export JSON",
      csv_button: "Export CSV",
      exporting: "Exporting..."
    },
    import: {
      title: "Import Data",
      format_select: "-- Select file format --",
      json_option: "JSON",
      csv_option: "CSV",
      choose_file: "Choose file",
      no_file: "No file chosen",
      import_button: "Import",
      importing: "Importing...",
      select_format_first: "Please select a file format before choosing a file!",
      select_file: "Please select a file before importing!",
      success: "Import successful!",
      error: "Error importing data!"
    }
  },
  course: {
    title: 'Course List',
    add: 'Add Course',
    label: 'Course',
    fields: {
      courseCode: 'Course Code',
      name: 'Course Name',
      credits: 'Credits',
      department: 'Department',
      description: 'Description',
      prerequisiteCourse: 'Prerequisite Course',
      isActive: 'Active',
      actions: 'Actions'
    }
  },
  class: {
    title: 'Class List',
    add: 'Add Class',
    label: 'Class',
    fields: {
      classId: 'Class ID',
      courseCode: 'Course Name',
      academicYear: 'Academic Year',
      semester: 'Semester',
      teacher: 'Teacher',
      maxStudents: 'Max Students',
      schedule: 'Schedule',
      room: 'Room',
      cancelDeadline: 'Cancel Deadline',
      actions: 'Actions'
    }
  },
  enrollment: {
    title: 'Enrollment List',
    add: 'Add Enrollment',
    label: 'Enrollment',
    fields: {
      classId: 'Class',
      courseCode: 'Course',
      StudentId: 'Student',
      registeredAt: 'Registration Time',
      isCancelled: 'Cancelled',
      cancelReason: 'Cancel Reason',
      cancelDate: 'Cancel Time',
      actions: 'Actions'
    }
  },
  grade: {
    title: 'Grade List',
    add: 'Add Grade',
    label: 'Grade',
    fields: {
      classId: 'Class',
      courseCode: 'Course',
      studentId: 'Student ID',
      student: 'Student Name',
      score: 'Score',
      gradeLetter: 'Grade Letter',
      gpa: 'GPA',
      actions: 'Actions'
    },
    export: {
      title: 'Export Grade Sheet',
      guide: 'Enter Student ID to export grade sheet',
      button: 'Export Grade Sheet',
      success: 'Grade sheet exported successfully',
      error: 'Error exporting grade sheet',
      file_name: 'GradeSheet',
    }
  }
}; 