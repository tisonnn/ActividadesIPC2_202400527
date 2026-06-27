using Microsoft.EntityFrameworkCore;
using LaboratorioInteroperabilidad.Models;

namespace LaboratorioInteroperabilidad.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Alumno> Alumnos { get; set; }
    }
}