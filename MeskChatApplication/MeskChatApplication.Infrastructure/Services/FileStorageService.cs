using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Exceptions;
using MeskChatApplication.Application.Services;
using MeskChatApplication.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MeskChatApplication.Infrastructure.Services;

public sealed class FileStorageService(IOptions<CloudinaryOptions> cloudinaryOptions) : IFileStorageService
{
    private readonly Cloudinary _cloudinary = new Cloudinary(cloudinaryOptions.Value.Url);
    private readonly string _imageFolderName = cloudinaryOptions.Value.ImageFolderName;

    public async Task<ResponseEntity<string>> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
    {
        var uploadParams = BuildImageUploadParams(stream, fileName);
        var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new ErrorWhileUploadPhotoException();
        }

        var secureUrl = result.SecureUrl.ToString();
        
        return ResponseEntity<string>
            .Succeeded()
            .WithData(secureUrl);
    }

    private ImageUploadParams BuildImageUploadParams(Stream stream, string fileName)
        => new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = _imageFolderName,
            PublicId = fileName,
            Overwrite = true,
            Invalidate = true,
            UniqueFilename = false,
            UseFilename = false
        };
}