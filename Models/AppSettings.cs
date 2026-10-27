using System.IO;
using System.Text.Json;

namespace AntiStressPet.Models;

public sealed class AppSettings
{
    public bool SoundEnabled { get; set; } = true;
    public double Volume { get; set; } = 0.7;
    public double LastLeft { get; set; } = double.NaN;
    public double LastTop { get; set; } = double.NaN;
    public bool PetVisible { get; set; } = true;

    private static string FilePath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AntiStressPet",
            "settings.json");

    public static AppSettings Load()
    {
        try
        {
            var path = FilePath;
            if (!File.Exists(path))
                return new AppSettings();

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void Save()
    {
        try
        {
            var path = FilePath;
            var dir = Path.GetDirectoryName(path)!;
            Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
        catch
        {
            // Local settings only; ignore write failures.
        }
    }
}
