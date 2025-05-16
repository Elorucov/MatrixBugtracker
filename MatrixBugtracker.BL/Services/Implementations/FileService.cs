using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileRepository _repo;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly ILogger<FileService> _logger;
        private readonly string _uploadedFilesPath;

        public FileService(IUnitOfWork unitOfWork, IConfiguration config, IUserService userService, IUserIdProvider userIdProvider, ILogger<FileService> logger)
        {
            _unitOfWork = unitOfWork;
            _repo = unitOfWork.GetRepository<IFileRepository>();
            _userService = userService;
            _userIdProvider = userIdProvider;
            _logger = logger;
            _uploadedFilesPath = config["PathForUploadedFiles"];
        }

        private string GetUniqueFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            string fileNameWithoutExt = fileName.Substring(0, fileName.Length - fileExtension.Length - 1);
            return $"{fileNameWithoutExt}.{DateTimeOffset.Now.ToUnixTimeMilliseconds()}{fileExtension}";
        }

        public async Task<ResponseDTO<int>> SaveFileAsync(FileUploadDTO request)
        {
            if (!Directory.Exists(_uploadedFilesPath)) Directory.CreateDirectory(_uploadedFilesPath);

            IFormFile file = request.File;
            if (request.IsPhoto && !file.IsImage())
                return ResponseDTO<int>.BadRequest($"{Errors.InvalidFileFormat}: {file.ContentType}");

            string newFileName = GetUniqueFileName(file.FileName);
            string filePath = Path.Combine(_uploadedFilesPath, newFileName);

            _logger.LogInformation($"Saving uploaded file to {filePath} — Original file name: {file.FileName}, content type: {file.ContentType}, size: {file.Length} bytes");
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            UploadedFile newFile = new UploadedFile
            {
                OriginalName = file.FileName,
                Path = newFileName,
                MimeType = file.ContentType,
                Length = file.Length
            };

            await _repo.AddAsync(newFile);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<int>(newFile.Id);
        }

        public async Task<ResponseDTO<UploadedFile>> GetFileEntityAsync(int fileId, bool isImage)
        {
            UploadedFile file = await _repo.GetByIdAsync(fileId);
            if (file == null) return ResponseDTO<UploadedFile>.NotFound(Errors.NotFoundFile);

            // Admins can get all files, others can get only own files
            int currentUserId = _userIdProvider.UserId;
            var role = await _userService.GetUserRoleAsync(currentUserId);
            if (role != DAL.Enums.UserRole.Admin && file.CreatorId != _userIdProvider.UserId) return ResponseDTO<UploadedFile>.Forbidden(Errors.ForbiddenFile);
            if (isImage && !file.IsImage()) return ResponseDTO<UploadedFile>.BadRequest(Errors.FileIsNotImage);

            return new ResponseDTO<UploadedFile>(file);
        }

        public async Task<(byte[], string)> GetFileContentByPathAsync(string path)
        {
            string defaultContentType = "application/octet-stream";
            string filePath = Path.Combine(_uploadedFilesPath, path);
            if (!File.Exists(filePath)) return (new byte[0], defaultContentType);

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string contentType))
                contentType = defaultContentType;

            byte[] content = await File.ReadAllBytesAsync(filePath);

            return (content, contentType);
        }
    }
}
