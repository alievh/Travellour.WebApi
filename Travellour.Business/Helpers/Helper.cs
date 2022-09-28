using Microsoft.AspNetCore.Http;

namespace Travellour.Business.Helpers;

public static class Helper
{
    public static async Task<string> FileSaveAsync(this IFormFile file, string root, params string[] folders)
    {
        string fileNewName = FileGuidName(file);
        string rootInFolder = folders.Aggregate((result, folder) => Path.Combine(result, folder));
        string path = Path.Combine(root, rootInFolder, fileNewName);
        using (FileStream fs = new(path, FileMode.Create))
        {
            await file.CopyToAsync(fs);
        }
        return fileNewName;
    }

    private static string FileGuidName(this IFormFile file)
    {
        string path = Path.Combine(Guid.NewGuid().ToString() + file.FileName);
        return path;
    }
}
