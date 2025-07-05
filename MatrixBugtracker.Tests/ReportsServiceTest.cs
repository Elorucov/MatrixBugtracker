using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Profiles;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.BL.Services.Implementations;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.DAL.Repositories.Implementations.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace MatrixBugtracker.Tests
{
    public class ReportsServiceTest
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductRepository> _productsRepoMock;
        private readonly Mock<ITagRepository> _tagsRepoMock;
        private readonly Mock<IReportRepository> _reportsRepoMock;
        private readonly Mock<IAccessService> _accessServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IUserIdProvider> _userIdProviderMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly IProductsService _productService;
        private readonly ITagsService _tagsService;
        private readonly IReportsService _service;

        public ReportsServiceTest()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _httpContextMock = new Mock<HttpContext>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productsRepoMock = new Mock<IProductRepository>();
            _tagsRepoMock = new Mock<ITagRepository>();
            _reportsRepoMock = new Mock<IReportRepository>();
            _accessServiceMock = new Mock<IAccessService>();
            _fileServiceMock = new Mock<IFileService>();
            _userServiceMock = new Mock<IUserService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _userIdProviderMock = new Mock<IUserIdProvider>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<IProductRepository>())
                .Returns(_productsRepoMock.Object);
            _unitOfWorkMock.Setup(uow => uow.GetRepository<ITagRepository>())
                .Returns(_tagsRepoMock.Object);
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

            _productService = new ProductsService(_unitOfWorkMock.Object, _accessServiceMock.Object, _fileServiceMock.Object,
                _userServiceMock.Object, _notificationServiceMock.Object, _userIdProviderMock.Object, _mapper);

            _tagsService = new TagsService(_unitOfWorkMock.Object, _mapper);

            _service = new ReportsService(_unitOfWorkMock.Object, _fileServiceMock.Object, _productService, _tagsService,
                _userServiceMock.Object, _userIdProviderMock.Object, _httpContextAccessorMock.Object, _mapper);
        }

        // Testing "access denied" error if user as tester is not a member of closed product for which the report is created
        [Fact]
        public async Task GetByIdAsync_ReportFromClosedProductIsNotMember_Forbidden()
        {
            // Arrange
            int currentUserId = 7;
            int reportId = 2;
            var report = Seed.Reports.SingleOrDefault(r => r.Id == reportId);
            var product = Seed.Products.SingleOrDefault(p => p.Id == report.ProductId);

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            _productsRepoMock.Setup(pr => pr.GetByIdAsync(report.ProductId))
                .ReturnsAsync(product);

            _reportsRepoMock.Setup(r => r.GetByIdWithIncludesAsync(reportId))
                .ReturnsAsync(report);

            // Act
            var response = await _service.GetByIdAsync(reportId);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status403Forbidden, response.HttpStatusCode);
            Assert.Equal(BL.Resources.Errors.ForbiddenProduct, response.ErrorMessage);
        }

        // Testing "access denied" error if user as tester is not a member of secret product for which the report is created
        [Fact]
        public async Task GetByIdAsync_ReportFromSecretProductAsTesterIsNotMember_Forbidden()
        {
            // Arrange
            int currentUserId = 7;
            int reportId = 1;
            var report = Seed.Reports.SingleOrDefault(r => r.Id == reportId);
            var product = Seed.Products.SingleOrDefault(p => p.Id == report.ProductId);

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            _productsRepoMock.Setup(pr => pr.GetByIdAsync(report.ProductId))
                .ReturnsAsync(product);

            _reportsRepoMock.Setup(r => r.GetByIdWithIncludesAsync(reportId))
                .ReturnsAsync(report);

            // Act
            var response = await _service.GetByIdAsync(reportId);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status403Forbidden, response.HttpStatusCode);
            Assert.Equal(BL.Resources.Errors.ForbiddenProduct, response.ErrorMessage);
        }


        // Testing access to report if user as tester is not a member of open product for which the report is created
        [Fact]
        public async Task GetByIdAsync_ReportFromOpenProductIsNotMember_Ok()
        {
            // Arrange
            int currentUserId = 7;
            int reportId = 3;
            var report = Seed.Reports.SingleOrDefault(r => r.Id == reportId);
            var product = Seed.Products.SingleOrDefault(p => p.Id == report.ProductId);

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            _productsRepoMock.Setup(pr => pr.GetByIdAsync(report.ProductId))
                .ReturnsAsync(product);

            _reportsRepoMock.Setup(r => r.GetByIdWithIncludesAsync(reportId))
                .ReturnsAsync(report);

            // Act
            var response = await _service.GetByIdAsync(reportId);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(product.Id, response.Data.ProductId);
        }

        // Testing access to report if user as tester is member of secret product for which the report is created
        [Fact]
        public async Task GetByIdAsync_ReportFromSecretProductAsTesterIsMember_Ok()
        {
            // Arrange
            int currentUserId = 7;
            int reportId = 1;
            var report = Seed.Reports.SingleOrDefault(r => r.Id == reportId);
            var product = Seed.Products.SingleOrDefault(p => p.Id == report.ProductId);
            var productMember = new ProductMember
            {
                ProductId = report.ProductId,
                MemberId = currentUserId,
                Status = ProductMemberStatus.Joined
            };

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            _productsRepoMock.Setup(pr => pr.GetProductMemberAsync(report.ProductId, currentUserId))
                .ReturnsAsync(productMember);

            _productsRepoMock.Setup(pr => pr.GetByIdAsync(report.ProductId))
                .ReturnsAsync(product);

            _reportsRepoMock.Setup(r => r.GetByIdWithIncludesAsync(reportId))
                .ReturnsAsync(report);

            // Act
            var response = await _service.GetByIdAsync(reportId);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(reportId, response.Data.Id);
        }

        // Testing creating a report for a product in which the user is not a member
        [Fact]
        public async Task CreateAsync_ToProductIsNotMember_Forbidden()
        {
            // Arrange
            int currentUserId = 7;
            int productId = 3;
            var product = Seed.Products.SingleOrDefault(p => p.Id == productId);

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            _productsRepoMock.Setup(pr => pr.GetByIdAsync(productId))
                .ReturnsAsync(product);

            ReportCreateDTO request = new ReportCreateDTO
            {
                ProductId = productId,
                Title = "Sample report",
                Severity = ReportSeverity.Low,
                ProblemType = ReportProblemType.FunctionNotWorking
            };

            // Act
            var response = await _service.CreateAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status403Forbidden, response.HttpStatusCode);
            Assert.Equal(Errors.ForbiddenProduct, response.ErrorMessage);
        }

        // Testing creating a report with one of tags is archived
        [Fact]
        public async Task CreateAsync_WithArchivedTag_BadRequest()
        {
            // Arrange
            int currentUserId = 7;
            int productId = 3;
            var product = Seed.Products.SingleOrDefault(p => p.Id == productId);

            var productMember = new ProductMember
            {
                ProductId = productId,
                Product = product,
                MemberId = currentUserId,
                Status = ProductMemberStatus.Joined
            };
            product.ProductMembers = new List<ProductMember>
            {
                productMember
            };

            string[] tags = Seed.Tags.Select(t => t.Name).ToArray();

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            _productsRepoMock.Setup(pr => pr.GetProductMemberAsync(productId, currentUserId))
                .ReturnsAsync(productMember);

            _productsRepoMock.Setup(pr => pr.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _tagsRepoMock.Setup(tr => tr.GetIntersectingAsync(tags))
                .ReturnsAsync(Seed.Tags);

            ReportCreateDTO request = new ReportCreateDTO
            {
                ProductId = productId,
                Title = "Sample report",
                Severity = ReportSeverity.Low,
                ProblemType = ReportProblemType.FunctionNotWorking,
                Tags = tags
            };

            // Act
            var response = await _service.CreateAsync(request);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status400BadRequest, response.HttpStatusCode);
            Assert.Equal(Errors.NotFoundSomeTags, response.ErrorMessage);
        }

        // Testing creating a report with available tags
        [Fact]
        public async Task CreateAsync_WithAvailableTags_Ok()
        {
            // Arrange
            int currentUserId = 7;
            int productId = 3;
            var product = Seed.Products.SingleOrDefault(p => p.Id == productId);

            var productMember = new ProductMember
            {
                ProductId = productId,
                Product = product,
                MemberId = currentUserId,
                Status = ProductMemberStatus.Joined
            };
            product.ProductMembers = new List<ProductMember>
            {
                productMember
            };

            string[] tags = Seed.Tags.Where(t => !t.IsArchived).Select(t => t.Name).ToArray();

            _userIdProviderMock.Setup(uip => uip.UserId).Returns(currentUserId);

            _userServiceMock.Setup(us => us.GetSingleUserAsync(currentUserId))
                .ReturnsAsync(Seed.Users.SingleOrDefault(u => u.Id == currentUserId));

            _productsRepoMock.Setup(pr => pr.GetProductMemberAsync(productId, currentUserId))
                .ReturnsAsync(productMember);

            _productsRepoMock.Setup(pr => pr.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _tagsRepoMock.Setup(tr => tr.GetIntersectingAsync(tags))
                .ReturnsAsync(Seed.Tags.Where(t => tags.Contains(t.Name)).ToList());

            ReportCreateDTO request = new ReportCreateDTO
            {
                ProductId = productId,
                Title = "Sample report",
                Severity = ReportSeverity.Low,
                ProblemType = ReportProblemType.FunctionNotWorking,
                Tags = tags
            };

            // Act
            var response = await _service.CreateAsync(request);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(0, response.Data);
        }
    }
}
