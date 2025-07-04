using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.Profiles;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.BL.Services.Implementations;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;

namespace MatrixBugtracker.Tests
{
    public class ProductsServiceTest
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductRepository> _repoMock;
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
            _repoMock = new Mock<IProductRepository>();
            _accessServiceMock = new Mock<IAccessService>();
            _fileServiceMock = new Mock<IFileService>();
            _userServiceMock = new Mock<IUserService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _userIdProviderMock = new Mock<IUserIdProvider>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IProductRepository>())
                .Returns(_repoMock.Object);

            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IMapper)))
                .Returns(_mapper);
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IUserIdProvider)))
                .Returns(_userIdProviderMock.Object);
            _httpContextMock.Setup(hc => hc.RequestServices)
                .Returns(_serviceProviderMock.Object);
            _httpContextAccessorMock.Setup(hca => hca.HttpContext)
                .Returns(_httpContextMock.Object);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DefaultProfile(_httpContextAccessorMock.Object));
            });
            _mapper = mapperConfig.CreateMapper();

            _service = new ProductService(_unitOfWorkMock.Object, _accessServiceMock.Object, _fileServiceMock.Object, 
                _userServiceMock.Object, _notificationServiceMock.Object, _userIdProviderMock.Object, _mapper);
        }

        // Testing "access denied" error if user as tester is not a member of secret product
        [Fact]
        public async Task GetByIdAsync_SecretProductAccessCheck_Forbidden()
        {
            // Arrange
            int currentUserId = 7;
            int productId = 5;

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            var currentUser = new User
            {
                Id = currentUserId,
                FirstName = "Sample",
                LastName = "User",
                Role = UserRole.Tester
            };
            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(currentUser);

            var product = new Product
            {
                Id = productId,
                Name = "Sample",
                Type = ProductType.MobileApp,
                AccessLevel = ProductAccessLevel.Secret,
                ProductMembers = new List<ProductMember>()
            };
            _repoMock.Setup(r => r.GetByIdWithIncludesAsync(productId))
                .ReturnsAsync(product);

            // Act
            var response = await _service.GetByIdAsync(productId);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status403Forbidden, response.HttpStatusCode);
        }
    }
}