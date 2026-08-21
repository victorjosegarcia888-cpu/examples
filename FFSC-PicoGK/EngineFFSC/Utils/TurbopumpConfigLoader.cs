// TurbopumpConfigLoader.cs
//
// Cargador de configuracion de turbobomba desde JSON.

using System.IO;
using System.Text.Json;
using FFSC_PicoGK.Models;

namespace FFSC_PicoGK.Utils
{
    /// <summary>
    /// Cargador de configuracion de turbobomba.
    /// </summary>
    public static class TurbopumpConfigLoader
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static T Load<T>(string ruta) where T : class, new()
        {
            if (!File.Exists(ruta))
                return new T();

            string json = File.ReadAllText(ruta);
            return JsonSerializer.Deserialize<T>(json, Options) ?? new T();
        }
    }
}
