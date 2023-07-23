using e_trade_api.application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace e_trade_api.Infastructure;

public class FileService : IFileService
{
    readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream =
                new(
                    path,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    1024 * 1024,
                    useAsync: true
                );

            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true;
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }

    private Task<string> FileRenameAsync(string fileName)
    {
        string extension = Path.GetExtension(fileName);
        string oldName = Path.GetFileNameWithoutExtension(fileName);
        DateTime now = DateTime.Now;
        string dateTime = now.ToString("yyyy-MM-dd-HH-mm-ss-fff-fffffff");

        string newFileName = $"{NameOperation.CharacterRegulatory(oldName)}-{dateTime}{extension}";
        return Task.FromResult(newFileName);
    }

    public async Task<List<(string fileName, string path)>> UploadAsync(
        string path,
        IFormFileCollection files
    )
    {
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        List<(string fileName, string path)> datas = new();
        List<bool> results = new();
        foreach (IFormFile file in files)
        {
            string fileNewName = await FileRenameAsync(file.FileName);

            bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
            datas.Add((fileNewName, $"{uploadPath} \\{fileNewName}"));
            results.Add(result);
        }

        if (results.TrueForAll(r => r.Equals(true)))
        {
            return datas;
        }
        return null;
    }
}
