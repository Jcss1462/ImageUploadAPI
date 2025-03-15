# Image Upload API

API desarrollada en **.NET 9** para la gestión de imágenes con almacenamiento en **PostgreSQL**. Permite subir imágenes y consultarlas por nombre o número de dorsal.

## 🚀 Características

- 📂 **Subida de imágenes** con almacenamiento en base de datos.
- 🔍 **Búsqueda de imágenes** por nombre o número de dorsal.
- 📑 **Swagger** para documentación interactiva de la API.
- 🔒 **CORS configurado** para permitir solicitudes desde distintos orígenes.
- ✅ **Migraciones incluidas** para facilitar la configuración de la base de datos.

## 🛠️ Prerrequisitos

Antes de ejecutar el proyecto, asegúrate de tener instalados los siguientes componentes:

1. **.NET 9 SDK** - Descárgalo desde [aquí](https://dotnet.microsoft.com/download/dotnet/9.0)
2. **PostgreSQL** - Instalado y corriendo en tu sistema.
3. **Entity Framework Core CLI**\
   Ejecuta el siguiente comando:
   ```sh
   dotnet tool install --global dotnet-ef
   ```

## ⚙️ Configuración del Proyecto

### 1️⃣ Clonar el repositorio

```sh
git clone <URL_DEL_REPOSITORIO>
cd <NOMBRE_DEL_PROYECTO>
```

### 2️⃣ Configurar la conexión a PostgreSQL

En el archivo **appsettings.json**, actualiza la cadena de conexión con tus credenciales de PostgreSQL:

```json
"ConnectionStrings": {
  "PostgresConnection": "Host=localhost;Port=5432;Database=imagenesdb;Username=tu_usuario;Password=tu_contraseña"
}
```

### 3️⃣ Ejecutar migraciones&#x20;

Aplicalas a la base de datos:

```sh
dotnet ef database update
```

### 4️⃣ Ejecutar la API

```sh
dotnet run
```

La API estará disponible en `https://localhost:7015/swagger`.

## 🛠️ Endpoints Principales

### 📌 Subir una imagen

**POST** `/upload`

#### Parámetros

- `file`: Imagen a subir (**multipart/form-data**).
- `name`: Nombre de la imagen.
- `dorsal`: Número de dorsal asociado.

#### Respuesta

```json
{
  "message": "Imagen guardada exitosamente",
  "id": 1
}
```

---

### 🔍 Buscar imágenes

**GET** `/search`

#### Parámetros opcionales

- `name`: Nombre de la imagen.
- `dorsal`: Número de dorsal.

#### Respuesta

```json
[
  {
    "id": 1,
    "name": "ejemplo.png",
    "dorsal": 123,
    "data": "<base64>"
  }
]
```

## 🏗️ Contribuciones

Si deseas contribuir, crea un **fork** del repositorio y genera un **pull request** con tus cambios. ¡Toda ayuda es bienvenida! 🎉

✨ *Desarrollado con .NET 9 y PostgreSQL* ✨

