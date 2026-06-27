using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LaboratorioInteroperabilidad.Models;

namespace LaboratorioInteroperabilidad.Services
{
    public class AcademicoService
    {
        private readonly HttpClient _httpClient;

        public AcademicoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Alumno?> ObtenerDatosAlumnoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.usac.edu/v1/alumnos");
                response.EnsureSuccessStatusCode();

                var jsonPayload = await response.Content.ReadAsStringAsync();

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<Alumno>(jsonPayload, jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de red: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error de parseo JSON: {ex.Message}");
                throw;
            }
        }
    }
}