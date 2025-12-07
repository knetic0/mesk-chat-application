using MESK.ResponseEntity;

namespace MeskChatApplication.Application.Services;

public interface IFileStorageService
{
    Task<ResponseEntity<string>> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken = default);
}