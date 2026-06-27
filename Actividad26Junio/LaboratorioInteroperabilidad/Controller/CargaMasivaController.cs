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