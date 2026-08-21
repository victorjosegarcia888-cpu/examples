// CoolingConfigLoader.cs
//
// Cargador de configuracion de refrigeracion desde JSON.

using System.IO;
using System.Text.Json;

namespace FFSC_PicoGK.Utils
{
    /// <summary>
    /// Cargador de configuracion de refrigeracion.
    /// </summary>
    public static class CoolingConfigLoader
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
