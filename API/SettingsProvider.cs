using System;
using System.IO;
using System.Text.Json;

namespace Settings
{
    public class SettingsProvider<T> where T : class, ISettings, new()
    {
        public T Settings { get; private set; }

        public SettingsProvider()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            var filePath = AppContext.BaseDirectory + "settings.json";
            try
            {
                Settings = File.Exists(filePath) ? JsonSerializer.Deserialize<T>(File.ReadAllText(filePath)) : new T();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Can't read settings file at {filePath}\n\tMessage: {ex.Message}\n\tInner: {ex.InnerException?.Message}");
            }
            
            if (Settings != null && Settings.HasEmptyProperties())
            {
                throw new ApplicationException($"Some required settings are missing");
            }
        }
    }
}
