# Investigación Teórica y Análisis de Casos: Árboles AVL y Web APIs

## 1. El Límite de las Rotaciones Simples y Desbalanceo en "Zig-Zag"

### El Problema Cruzado
Las rotaciones simples (Rotación Simple a la Derecha - RRD o Rotación Simple a la Izquierda - RII) están diseñadas para resolver desbalances lineales, donde el peso del árbol crece en una sola dirección. Fallan cuando enfrentamos inserciones cruzadas (como 30, luego 10 y luego 20) porque el desbalance tiene forma de "zig-zag". Si aplicamos una rotación simple en este escenario, el nodo problemático simplemente cambia de lado, invirtiendo la inclinación pero manteniendo el Factor de Equilibrio (FE) fuera del rango permitido (-1, 0, 1).

Matemáticamente, este escenario cruzado que gatilla una Rotación Doble Izquierda-Derecha (RID) ocurre cuando se cumplen las siguientes condiciones estructurales simultáneamente:
* **FE del Nodo Padre (Abuelo):** Alcanza un valor de -2 (indicando una sobrecarga pesada hacia su subárbol izquierdo).
* **FE del Nodo Hijo Izquierdo:** Tiene un valor de +1 (indicando que la nueva inserción ocurrió en su subárbol derecho, creando la forma cruzada).

### Principio DRY (Don't Repeat Yourself)
Desde la perspectiva de la ingeniería de software, implementar operaciones compuestas (RID y RDI) reutilizando las primitivas de rotación simple es una aplicación directa del principio DRY. En lugar de reasignar hasta 6 o 7 punteros manualmente desde cero para una rotación doble, se manda a llamar primero a la rotación simple del hijo y luego a la rotación simple del padre. 
La ventaja es triple:
1. **Mantenibilidad:** Si hay un error en la lógica de rotación, solo se depuran los métodos simples.
2. **Seguridad:** Reduce la probabilidad de dejar punteros huérfanos o crear ciclos infinitos en memoria.
3. **Legibilidad:** El código se vuelve modular y auto-documentado, delegando la complejidad a las funciones base.

---

## 2. Fundamentos de Arquitectura Web y Protocolo HTTP

### El Modelo Cliente-Servidor en Arquitecturas Web
La transición hacia servicios web implica desacoplar la interfaz del usuario de la lógica de negocio. En este modelo:
* **El Cliente (Request):** Inicia la comunicación enviando una petición HTTP al servidor a través de la red. Esta petición contiene un Verbo (GET, POST), una URL (el endpoint específico, ej. `/api/arbol`), cabeceras (Headers) de control y, si es necesario, un Payload o cuerpo de datos serializado (usualmente en JSON).
* **El Servidor (Response):** La Web API recibe el *Request*, lo enruta al controlador o función manejadora correspondiente, ejecuta las operaciones en la estructura de datos en memoria (el árbol AVL) y devuelve un *Response*. Esta respuesta contiene un Código de Estado HTTP (ej. 200 OK, 201 Created) y el estado actualizado de la estructura.

### Semántica de Operaciones: GET vs POST
* **HTTP GET (Recuperación):** Es un método **idempotente** diseñado estrictamente para la lectura de recursos. En nuestra API, se utiliza para obtener la estructura física actual del árbol. Los datos se solicitan a través de la URL y no altera el estado del servidor ni muta la estructura de datos.
* **HTTP POST (Mutación/Inserción):** Es un método **no idempotente** diseñado para enviar datos al servidor con el fin de crear un nuevo recurso o gatillar un cambio de estado. Se utiliza para la inserción de nuevos elementos en el árbol AVL. El nuevo nodo (el Id y la etiqueta) viaja de forma segura encapsulado dentro del cuerpo (*Body*) de la petición, permitiendo que el servidor procese la inserción y ejecute las rotaciones necesarias.


## Pruebas realizadas

#### Prueba 1: Endpoint GET antes del POST
Se usó la extensión Thunder Client de VS Code para la parte práctica.
Se realizó una petición GET a http://localhost:5260/api/nodos.

Resultado esperado: Código 200 OK con el listado de nodos iniciales (dos nodos).

Captura de pantalla:

[GET antes del POST](./screenshots/01-get-antes.png)

#### Prueba 2: Endpoint POST

Se realizó una petición POST a http://localhost:5260/api/nodos con el siguiente cuerpo JSON:

{
    "Id": 20,
    "Etiqueta": "Nieto Derecho"
}

Resultado esperado: Código 201 Created.

Captura de pantalla:

[POST exitoso y Get actualizado](./screenshots/02-post.png)