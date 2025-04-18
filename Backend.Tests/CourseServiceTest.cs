using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using StudentManagement.Models;
using StudentManagement.Repositories;
using StudentManagement.Services;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _mockRepo;
    private readonly CourseService _service;

    public CourseServiceTests()
    {
        _mockRepo = new Mock<ICourseRepository>();
        _service = new CourseService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllCoursesAsync_ReturnsList()
    {
        // Arrange
        var mockCourses = new List<Course> { new Course { CourseCode = "CS101" } };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(mockCourses);

        // Act
        var result = await _service.GetAllCoursesAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("CS101", result[0].CourseCode);
    }

    [Fact]
    public async Task GetActiveCoursesAsync_ReturnsActiveCourses()
    {
        // Arrange
        var activeCourses = new List<Course> { new Course { IsActive = true } };
        _mockRepo.Setup(r => r.GetActiveCoursesAsync()).ReturnsAsync(activeCourses);

        // Act
        var result = await _service.GetActiveCoursesAsync();

        // Assert
        Assert.Single(result);
        Assert.True(result[0].IsActive);
    }

    [Fact]
    public async Task GetCourseByCodeAsync_ReturnsCourse()
    {
        // Arrange
        var course = new Course { CourseCode = "CS102" };
        _mockRepo.Setup(r => r.GetByCodeAsync("CS102")).ReturnsAsync(course);

        // Act
        var result = await _service.GetCourseByCodeAsync("CS102");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CS102", result.CourseCode);
    }

    [Fact]
    public async Task CreateCourseAsync_InvalidCredits_ReturnsFalse()
    {
        var course = new Course { Credits = 1 }; // invalid
        var result = await _service.CreateCourseAsync(course);

        Assert.False(result);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Course>()), Times.Never);
    }

    [Fact]
    public async Task CreateCourseAsync_InvalidPrerequisite_ReturnsFalse()
    {
        var course = new Course
        {
            Credits = 3,
            PrerequisiteCourseCode = "NONEXIST"
        };
        _mockRepo.Setup(r => r.GetByCodeAsync("NONEXIST")).ReturnsAsync((Course?)null);

        var result = await _service.CreateCourseAsync(course);

        Assert.False(result);
    }

    [Fact]
    public async Task CreateCourseAsync_ValidCourse_ReturnsTrue()
    {
        var course = new Course { Credits = 3 };
        _mockRepo.Setup(r => r.AddAsync(course)).Returns(Task.CompletedTask);

        var result = await _service.CreateCourseAsync(course);

        Assert.True(result);
        _mockRepo.Verify(r => r.AddAsync(course), Times.Once);
    }

    [Fact]
    public async Task HasStudentRegistrationsAsync_ReturnsTrue()
    {
        _mockRepo.Setup(r => r.HasStudentRegistrationsAsync("CS105")).ReturnsAsync(true);

        var result = await _service.HasStudentRegistrationsAsync("CS105");

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateCourseAsync_CourseNotFound_ReturnsFalse()
    {
        _mockRepo.Setup(r => r.GetByCodeAsync("CS106")).ReturnsAsync((Course?)null);

        var result = await _service.UpdateCourseAsync(new Course { CourseCode = "CS106" });

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateCourseAsync_ValidCourse_UpdatesAndReturnsTrue()
    {
        var existingCourse = new Course { CourseCode = "CS107", Name = "Old" };
        _mockRepo.Setup(r => r.GetByCodeAsync("CS107")).ReturnsAsync(existingCourse);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Course>())).Returns(Task.CompletedTask);

        var updated = new Course
        {
            CourseCode = "CS107",
            Name = "New",
            Description = "Updated",
            DepartmentId = 1
        };

        var result = await _service.UpdateCourseAsync(updated);

        Assert.True(result);
        Assert.Equal("New", existingCourse.Name);
    }

    [Fact]
    public async Task DeleteCourseAsync_CourseNotFound_ReturnsFalse()
    {
        _mockRepo.Setup(r => r.GetByCodeAsync("CS108")).ReturnsAsync((Course?)null);

        var result = await _service.DeleteCourseAsync("CS108");

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteCourseAsync_HasOpenClasses_DeactivatesAndReturnsTrue()
    {
        var course = new Course { CourseCode = "CS109", IsActive = true };
        _mockRepo.Setup(r => r.GetByCodeAsync("CS109")).ReturnsAsync(course);
        _mockRepo.Setup(r => r.HasOpenClassesAsync("CS109")).ReturnsAsync(true);
        _mockRepo.Setup(r => r.UpdateAsync(course)).Returns(Task.CompletedTask);

        var result = await _service.DeleteCourseAsync("CS109");

        Assert.True(result);
        Assert.False(!course.IsActive); // confirms IsActive = false
    }

    [Fact]
    public async Task DeleteCourseAsync_NoOpenClasses_DeletesAndReturnsTrue()
    {
        var course = new Course { CourseCode = "CS110", IsActive = true };
        _mockRepo.Setup(r => r.GetByCodeAsync("CS110")).ReturnsAsync(course);
        _mockRepo.Setup(r => r.HasOpenClassesAsync("CS110")).ReturnsAsync(false);
        _mockRepo.Setup(r => r.DeleteAsync(course)).Returns(Task.CompletedTask);

        var result = await _service.DeleteCourseAsync("CS110");

        Assert.True(result);
        _mockRepo.Verify(r => r.DeleteAsync(course), Times.Once);
    }
}
