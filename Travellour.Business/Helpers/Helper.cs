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

    public static string GetTimeBetween(this DateTime time)
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        var ts = new TimeSpan(DateTime.UtcNow.AddHours(4).Ticks - time.Ticks);
        double delta = Math.Abs(ts.TotalSeconds);

        if (delta < 1 * MINUTE)
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

        if (delta < 2 * MINUTE)
            return "a minute ago";

        if (delta < 45 * MINUTE)
            return ts.Minutes + " minutes ago";

        if (delta < 90 * MINUTE)
            return "an hour ago";

        if (delta < 24 * HOUR)
            return ts.Hours + " hours ago";

        if (delta < 48 * HOUR)
            return "yesterday";

        if (delta < 30 * DAY)
            return ts.Days + " days ago";

        if (delta < 12 * MONTH)
        {
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }
        else
        {
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
}
