using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileRepository _repo;
        private readonly IUserService _userService;
        private readonly IAccessService _accessService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly ILogger<FileService> _logger;
        private readonly string _uploadedFilesPath;

        public FileService(IUnitOfWork unitOfWork, IConfiguration config, IUserService userService, IAccessService accessService,
            IUserIdProvider userIdProvider, ILogger<FileService> logger)
        {
            _unitOfWork = unitOfWork;
            _repo = unitOfWork.GetRepository<IFileRepository>();
            _userService = userService;
            _accessService = accessService;
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
            var user = await _userService.GetSingleUserAsync(currentUserId);
            if (user?.Role != UserRole.Admin && file.CreatorId != _userIdProvider.UserId) return ResponseDTO<UploadedFile>.Forbidden(Errors.ForbiddenFile);
            if (isImage && !file.IsImage()) return ResponseDTO<UploadedFile>.BadRequest(Errors.FileIsNotImage);

            return new ResponseDTO<UploadedFile>(file);
        }

        // Check is files with those fileIds available and accessible
        // Returns UploadedFile entities
        public async Task<ResponseDTO<List<UploadedFile>>> CheckFilesAccessAsync(int[] fileIds)
        {
            List<UploadedFile> files = await _repo.GetIntersectingAsync(fileIds);
            if (files.Count < fileIds.Length)
            {
                var nonExistentFileIds = fileIds.Except(files.Select(f => f.Id));
                return ResponseDTO<List<UploadedFile>>.BadRequest(string.Format(Errors.NotFoundFiles, string.Join(", ", nonExistentFileIds)));
            }

            var accessibleFiles = await _accessService.GetAccessibleEntitiesAsync(files);
            if (accessibleFiles.Count() < files.Count)
            {
                var inAccessibleFileIds = fileIds.Except(accessibleFiles.Select(f => f.Id));
                return ResponseDTO<List<UploadedFile>>.BadRequest(string.Format(Errors.ForbiddenFiles, string.Join(", ", inAccessibleFileIds)));
            }

            return new ResponseDTO<List<UploadedFile>>(files);
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
