# Actividad Corta de Laboratorio: De ADO.NET Tradicional a la Automatización con EF Core

## Parte 1: Diagnóstico Técnico y Brecha de Impedancia
Adrián Gilberto Quiñónez Recinos 202400527
**1. La Brecha de Impedancia**
La brecha de impedancia se refiere a las diferencias conceptuales entre cómo se estructuran los datos en el paradigma orientado a objetos (Dominio de Objetos en C#) y cómo se almacenan en el modelo relacional (Dominio Relacional en SQL). El mapeador resuelve esto con las siguientes equivalencias:
* **Clase Clásica (POCO)** -> Mapea a -> **Tabla**
* **Propiedad/Atributo** -> Mapea a -> **Columna**
* **Instancia de Objeto** -> Mapea a -> **Fila (Registro)**

**2. Mitigación de Vulnerabilidades**
En Entity Framework Core, la **parametrización automática** previene la Inyección SQL. Cuando LINQ traduce la consulta, los valores de las variables se envían como parámetros seguros de base de datos y no se concatenan como texto plano. En ADO.NET tradicional, el comando equivalente para mitigar esto era agregar los parámetros manualmente a la colección del comando, utilizando `cmd.Parameters.AddWithValue()`.

**3. Optimización de Infraestructura**
El uso del método `.AsNoTracking()` es una muestra de solidaridad computacional porque **apaga el rastreador de cambios** (Change Tracker). Al indicar que la consulta es de solo lectura, EF Core no crea copias de los objetos en memoria para monitorear si cambian en el futuro, lo que ahorra significativamente memoria RAM crítica y procesamiento en el hardware compartido de la universidad.

---

## Parte 2: Desafío de Refactorización de Código

**1. El Contexto (DbContext)**

```csharp
using Microsoft.EntityFrameworkCore;

public class UnidadAcademicaContext : DbContext
{
    public UnidadAcademicaContext(DbContextOptions<UnidadAcademicaContext> options)
        : base(options)
    {
    }

    //  para mapear la tabla de catedráticos
    public DbSet<Catedratico> Tbl_Catedraticos { get; set; }
}
```

**2. La Consulta LINQ**

```csharp
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public List<Catedratico> ObtenerCatedraticosModerno(UnidadAcademicaContext _context)
{
    return _context.Tbl_Catedraticos
                   .AsNoTracking() // Libera memoria RAM apagando el rastreador de cambios
                   .Where(c => c.Nombre.StartsWith("Ing.")) // Sustituye al LIKE 
                   .ToList();
}
```

---

## Parte 3: Referencias Bibliográficas

* Facultad de Ingeniería, USAC. (2026). Sesión 17: Conectividad con SQL Server. Acceso Estructurado a Datos mediante C# y ADO.NET. Laboratorio de Introducción a la Programación y Computación 2. Guatemala.
* Facultad de Ingeniería, USAC. (2026). Sesión 18: Mapeo de Objetos Relacionales. Persistencia Automatizada con Entity Framework Core. Laboratorio de Introducción a la Programación y Computación 2. Guatemala.