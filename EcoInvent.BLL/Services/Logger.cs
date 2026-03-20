using System;
using System.IO;
using System.Text;

namespace EcoInvent.BLL.Services
{
    public class Logger
    {
        private static string _logPath = Path.Combine(AppContext.BaseDirectory, "ecoinvent.log");
        private static readonly object _lock = new();

        public static void SetLogFile(string path)
        {
            _logPath = path;
            string? dir = Path.GetDirectoryName(_logPath);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);
        }

        public static void Error(string message, Exception ex)
        {
            Write("ERROR", $"{message} | {ex.Message}{Environment.NewLine}{ex.StackTrace}");
        }

        public void LogInfo(string message)
        {
            Write("INFO", message);
        }

        public void LogWarning(string message)
        {
            Write("WARNING", message);
        }

        public void LogError(Exception ex, string source)
        {
            Write("ERROR", $"Source: {source} | Message: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
        }

        private static void Write(string level, string message)
        {
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(
                        _logPath,
                        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}",
                        Encoding.UTF8);
                }
            }
            catch
            {
            }
        }
    }
}