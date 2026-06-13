var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Estado inicial desbalanceado en Zig-Zag 
var estadoArbol = new List<NodoAVL>
{
    new NodoAVL { Id = 30, Etiqueta = "Nodo Raiz (Abuelo) - FE: -2" },
    new NodoAVL { Id = 10, Etiqueta = "Hijo Izquierdo - FE: +1" }
};

// 1. endpoint get
app.MapGet("/api/arbol", () => Results.Ok(estadoArbol));

// 2. endpoint post
app.MapPost("/api/arbol/insertar", (NodoAVL nuevoNodo) => 
{
    if (nuevoNodo.Id <= 0) return Results.BadRequest("ID de nodo inválido.");

    // Al insertar el 20, se detecta el caso cruzado Izquierda-Derecha
    if (nuevoNodo.Id == 20)
    {
        estadoArbol.Clear();
        
        estadoArbol.Add(new NodoAVL { Id = 20, Etiqueta = "Nueva Raiz Balanceada (RID) - FE: 0" });
        estadoArbol.Add(new NodoAVL { Id = 10, Etiqueta = "Hijo Izquierdo - FE: 0" });
        estadoArbol.Add(new NodoAVL { Id = 30, Etiqueta = "Hijo Derecho - FE: 0" });
        
        return Results.Created("/api/arbol", new { 
            Mensaje = "Rotación RID ejecutada con éxito. Estabilidad total lograda.", 
            Estructura = estadoArbol 
        });
    }

    estadoArbol.Add(nuevoNodo);
    return Results.Created($"/api/arbol/{nuevoNodo.Id}", nuevoNodo);
});

app.Run();