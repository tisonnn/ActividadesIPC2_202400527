# Actividad de Laboratorio: Interoperabilidad y Carga Masiva de Datos

## Parte 1: Evaluación Conceptual y Buenas Prácticas

### Formatos de Intercambio

| Formato | Ventajas | Desventajas |
| :--- | :--- | :--- |
| **CSV** | Muy ligero y rápido de procesar. Altamente compatible con herramientas de hojas de cálculo y bases de datos. Ideal para cargar grandes volúmenes de datos tabulares planos. | No soporta estructuras de datos jerárquicas o complejas. Puede presentar problemas de parseo si los delimitadores (como comas) están presentes dentro de los datos mismos y no se escapan correctamente. |
| **XML** | Soporta estructuras jerárquicas anidadas. Permite una validación estricta de esquemas mediante archivos XSD. Es muy descriptivo y fácil de entender por el ser humano. | Es muy verboso y "pesado" debido a la redundancia de las etiquetas de apertura y cierre para cada dato. Esto incrementa el uso de ancho de banda y exige mayor poder de procesamiento para el parseo. |

### 1. Diferenciación de Procesos (Serialización vs Deserialización)
Utilizando la librería `System.Text.Json` de C#, la diferencia técnica radica en la dirección del flujo de los datos:
* **Serialización:** Consiste en tomar un objeto estructurado en memoria (como una clase instanciada) y convertirlo en una cadena de texto en formato JSON (`JsonSerializer.Serialize()`). Se utiliza al preparar datos para enviarlos en el cuerpo de una petición.
* **Deserialización:** Es el proceso inverso. Transforma una cadena de texto JSON pura (por ejemplo, el payload recibido de una API) de vuelta en un objeto fuertemente tipado en la memoria de C# (`JsonSerializer.Deserialize<T>()`).

### 2. El Antipatrón del Rendimiento (N+1) y Estrategia Batching
El antipatrón "N+1" al leer archivos masivos ocurre cuando por cada línea o registro individual (N) que se lee, se efectúa una consulta, transacción o llamada a la base de datos (1). Esto colapsa el rendimiento y satura las conexiones debido a los constantes viajes de ida y vuelta al servidor.

**Estrategia de Optimización (Batching):** La solución radica en procesar los datos por lotes. En lugar de hacer una inserción por registro, se leen y transforman múltiples registros para almacenarlos en una lista temporal en memoria. Una vez procesado el bloque, se realiza la inserción a la base de datos en una única transacción masiva (usando métodos como `AddRange()`).

---

## Parte 2: Implementación Práctica en C#
primero que nada instale las dependencias usando:
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
A continuación se presenta la evidencia de la implementación práctica de los desafíos solicitados. El código fuente completo se encuentra alojado en el repositorio de este proyecto.

### Desafío 1: Consumo de Endpoints y Deserialización
Implementación del método asíncrono utilizando `HttpClient` para el consumo seguro de la API y la deserialización del payload a un objeto de tipo `Alumno`. Es el código de AcademicoService.cs

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

### Desafío 2: Endpoint para Carga Masiva CSV
Desarrollo del endpoint `[HttpPost]` para la recepción del archivo masivo, lectura asíncrona mediante `StreamReader` e inserción optimizada por lotes. Es el código de CargaMasivaController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaboratorioInteroperabilidad.Data;
using LaboratorioInteroperabilidad.Models;

namespace LaboratorioInteroperabilidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargaMasivaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CargaMasivaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("alumnos")]
        public async Task<IActionResult> ProcesarCSV(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("El archivo no es válido o está vacío.");
            }

            var listaAlumnos = new List<Alumno>();

            using (var stream = archivo.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                string? linea;
                
                while ((linea = await reader.ReadLineAsync()) != null)
                {
                    var columnas = linea.Split(',');

                    if (columnas.Length >= 2)
                    {
                        var nuevoAlumno = new Alumno
                        {
                            Carnet = columnas[0].Trim(),
                            Nombre = columnas[1].Trim()
                        };
                        listaAlumnos.Add(nuevoAlumno);
                    }
                }
            }

            _context.Alumnos.AddRange(listaAlumnos);
            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = $"Carga exitosa. Se insertaron {listaAlumnos.Count} registros." });
        }
    }
}

---

## Parte 3: Referencias Bibliográficas

* Facultad de Ingeniería, USAC. (2026). Sesión 20: Integración de Datos. Consumo de APIs Externas y Carga Masiva (CSV/XML). Laboratorio del curso Introducción a la Programación y Computación 2. Guatemala.

**Recursos y Documentación Complementaria:**
* Microsoft. (s.f.). *Serialización y deserialización de JSON en .NET*. Microsoft Learn. Recuperado de: https://learn.microsoft.com/es-es/dotnet/standard/serialization/system-text-json/overview
* Microsoft. (s.f.). *Realización de solicitudes HTTP con la clase HttpClient*. Microsoft Learn. Recuperado de: https://learn.microsoft.com/es-es/dotnet/fundamentals/networking/http/httpclient
* Microsoft. (s.f.). *Actualizaciones eficientes (Batching) en Entity Framework Core*. Microsoft Learn. Recuperado de: https://learn.microsoft.com/es-es/ef/core/performance/efficient-updating