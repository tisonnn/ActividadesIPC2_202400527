# Actividad de Investigación y Práctica: Estructuras de Datos Avanzadas y APIs con ASP.NET Core

## Parte 1: Investigación Teórica

### 1. Estructuras de Datos Eficientes

#### Árboles Binarios de Búsqueda (ABB)

**Regla de ordenamiento**: En un Árbol Binario de Búsqueda, para cada nodo se cumple que:
- El subárbol izquierdo contiene únicamente valores menores que el valor del nodo padre.
- El subárbol derecho contiene únicamente valores mayores que el valor del nodo padre.

**Principal desventaja**: Cuando los datos se insertan en orden secuencial (por ejemplo, 1, 2, 3, 4, 5), el árbol se degenera en una estructura similar a una lista vinculada. En este caso, la altura del árbol se vuelve O(n) y las operaciones de búsqueda, inserción y eliminación pasan de tener complejidad O(log n) a O(n), perdiendo la eficiencia que justifica el uso de un árbol.

#### Árboles AVL

**Definición de árbol auto-balanceado**: Un árbol auto-balanceado es aquel que, después de cada operación de inserción o eliminación, realiza rotaciones para mantener su altura equilibrada, garantizando que la diferencia de altura entre los subárboles izquierdo y derecho de cada nodo sea acotada.

**Factor de balanceo**: Se define como:

Factor de Balanceo = Altura(subárbol izquierdo) - Altura(subárbol derecho)

En un árbol AVL, este factor debe ser -1, 0 o 1 para todos los nodos. Si algún nodo presenta un factor de balanceo fuera de este rango, se aplican rotaciones (simples o dobles) para reequilibrar el árbol.

**Complejidad O(log n)**: La complejidad de búsqueda, inserción y eliminación se mantiene siempre en O(log n) porque el árbol garantiza que su altura se mantiene proporcional al logaritmo del número de elementos, incluso en el peor de los casos.

### 2. Fundamentos de Web APIs

#### Concepto de API y modelo Cliente-Servidor

Una API (Application Programming Interface) es un conjunto de reglas y definiciones que permite que dos aplicaciones se comuniquen entre sí.

En el modelo Cliente-Servidor:
- El Cliente (navegador web, Postman, aplicación móvil) inicia la comunicación enviando una petición (Request).
- El Servidor (la API) procesa la petición y devuelve una respuesta (Response).

**Viaje de una petición HTTP**:
1. El cliente construye una petición HTTP que incluye: método (GET, POST, etc.), URL, cabeceras (headers) y opcionalmente un cuerpo (body).
2. La petición viaja a través de internet hasta el servidor.
3. El servidor interpreta la petición, ejecuta la lógica correspondiente y genera una respuesta.
4. La respuesta incluye: un código de estado (status code), cabeceras y opcionalmente un cuerpo (normalmente en formato JSON o HTML).
5. El cliente recibe la respuesta y la procesa.

#### Verbos HTTP

**GET**:
- Uso correcto: Recuperación o consulta de recursos existentes. No debe modificar el estado del servidor.
- Idempotencia: Es idempotente, lo que significa que realizar una misma petición GET múltiples veces produce el mismo resultado y no tiene efectos secundarios.

**POST**:
- Uso correcto: Creación de nuevos recursos en el servidor. Los datos del nuevo recurso se envían en el cuerpo de la petición.
- Idempotencia: No es idempotente, ya que enviar la misma petición POST múltiples veces puede crear múltiples recursos (por ejemplo, crear dos registros idénticos en lugar de uno).

### Pruebas realizadas

#### Prueba 1: Endpoint GET antes del POST

Se realizó una petición GET a http://localhost:5079/api/nodos.

Resultado esperado: Código 200 OK con el listado de nodos iniciales (dos nodos).

Captura de pantalla:

[GET antes del POST](./screenshots/01-get-antes.png)

#### Prueba 2: Endpoint POST

Se realizó una petición POST a http://localhost:5079/api/nodos con el siguiente cuerpo JSON:

{
    "Id": 99,
    "Valor": "Nodo de prueba"
}

Resultado esperado: Código 201 Created.

Captura de pantalla:

[POST exitoso](./screenshots/02-post.png)

#### Prueba 3: Endpoint GET después del POST

Al ejecutar nuevamente el GET, el nuevo elemento aparece listado en la colección (tres nodos).

Captura de pantalla:

[GET después del POST](./screenshots/03-get-despues.png)