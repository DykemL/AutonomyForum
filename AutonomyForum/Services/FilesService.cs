using AutonomyForum.Models.DbEntities;
using AutonomyForum.Repositories;
using File = AutonomyForum.Models.DbEntities.File;

namespace AutonomyForum.Services;

public class FilesService
{
    private readonly FilesRepository filesRepository;

    public FilesService(FilesRepository filesRepository)
        => this.filesRepository = filesRepository;

    public async Task<File> UploadFile(IFormFile formFile, string webRootPath)
    {
        var file = new File()
        {
            Name = formFile.FileName
        };
        var fileExtention = formFile.FileName.Split('.').LastOrDefault();
        file.Path = "/files/" + file.Id.ToString();
        if (fileExtention != null)
        {
            file.Path += "." + fileExtention;
        }
        await using var fileStream = new FileStream(webRootPath + file.Path, FileMode.Create);
        await formFile.CopyToAsync(fileStream);
        await filesRepository.AddFile(file);
        return file;
    }

    public async Task<File?> GetFile(Guid fileId)
        => await filesRepository.GetFile(fileId);
}