using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.Profiles;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.BL.Services.Implementations;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Moq;

namespace MatrixBugtracker.Tests
{
    public class ProductsServiceTest
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductRepository> _productsRepoMock;
        private readonly Mock<IReportRepository> _reportsRepoMock;
        private readonly Mock<IAccessService> _accessServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IUserIdProvider> _userIdProviderMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly IProductService _service;

        public ProductsServiceTest()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _httpContextMock = new Mock<HttpContext>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productsRepoMock = new Mock<IProductRepository>();
            _reportsRepoMock = new Mock<IReportRepository>();
            _accessServiceMock = new Mock<IAccessService>();
            _fileServiceMock = new Mock<IFileService>();
            _userServiceMock = new Mock<IUserService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _userIdProviderMock = new Mock<IUserIdProvider>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IProductRepository>())
                .Returns(_productsRepoMock.Object);
            _unitOfWorkMock.Setup(uow => uow.GetRepository<IReportRepository>())
                .Returns(_reportsRepoMock.Object);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DefaultProfile(_httpContextAccessorMock.Object));
            });
            _mapper = mapperConfig.CreateMapper();

            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IMapper)))
                .Returns(_mapper);
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IUserIdProvider)))
                .Returns(_userIdProviderMock.Object);
            _httpContextMock.Setup(hc => hc.RequestServices)
                .Returns(_serviceProviderMock.Object);
            _httpContextAccessorMock.Setup(hca => hca.HttpContext)
                .Returns(_httpContextMock.Object);

            _service = new ProductService(_unitOfWorkMock.Object, _accessServiceMock.Object, _fileServiceMock.Object,
                _userServiceMock.Object, _notificationServiceMock.Object, _userIdProviderMock.Object, _mapper);
        }

        // Testing "access denied" error if user as tester is not a member of secret product
        [Fact]
        public async Task GetByIdAsync_SecretProductAccessCheckAsTester_Forbidden()
        {
            // Arrange
            int currentUserId = 7;
            int productId = 3;

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            _productsRepoMock.Setup(r => r.GetByIdWithIncludesAsync(productId))
                .ReturnsAsync(Seed.Products.SingleOrDefault(p => p.Id == productId));

            // Act
            var response = await _service.GetByIdAsync(productId);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status403Forbidden, response.HttpStatusCode);
        }

        // Testing access to closed product
        [Fact]
        public async Task GetByIdAsync_ClosedProductAccessCheckAsTester_Ok()
        {
            // Arrange
            int currentUserId = 7;
            int productId = 2;

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            var counters = new Dictionary<byte, int>() {
                { byte.MaxValue, 5 }
            };

            _productsRepoMock.Setup(r => r.GetByIdWithIncludesAsync(productId))
                .ReturnsAsync(Seed.Products.SingleOrDefault(p => p.Id == productId));
            _reportsRepoMock.Setup(r => r.GetStatusCountersByProductAsync(productId))
                .ReturnsAsync(counters);

            // Act
            var response = await _service.GetByIdAsync(productId);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(productId, response.Data.Id);
        }

        // Testing product counters
        [Fact]
        public async Task GetByIdAsync_CountersCheck_Ok()
        {
            // Arrange
            int currentUserId = 7;
            int productId = 1;

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            var counters = new Dictionary<byte, int>() {
                { byte.MaxValue, 27 }, // total
                { (byte)ReportStatus.Open, 3 }, // open
                { (byte)ReportStatus.Reopened, 1 }, // open
                { (byte)ReportStatus.InProgress, 2 }, // working
                { (byte)ReportStatus.UnderReview, 3 }, // working
                { (byte)ReportStatus.Fixed, 1 }, // working & fixed
                { (byte)ReportStatus.ReadyForTesting, 1 }, // fixed
                { (byte)ReportStatus.Verified, 1 }, // fixed
            };

            _productsRepoMock.Setup(r => r.GetByIdWithIncludesAsync(productId))
                .ReturnsAsync(Seed.Products.SingleOrDefault(p => p.Id == productId));
            _reportsRepoMock.Setup(r => r.GetStatusCountersByProductAsync(productId))
                .ReturnsAsync(counters);

            // Act
            var response = await _service.GetByIdAsync(productId);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(productId, response.Data.Id);
            Assert.Equal(27, response.Data.Counters.TotalReports);
            Assert.Equal(4, response.Data.Counters.OpenReports);
            Assert.Equal(6, response.Data.Counters.WorkingReports);
            Assert.Equal(3, response.Data.Counters.FixedReports);
        }
    }
}