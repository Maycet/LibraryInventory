README.md (Español)

# Inventario de Biblioteca - Aplicación Web ASP.NET MVC

Esta es una aplicación web ASP.NET MVC desarrollada en .NET 6 que permite llevar el inventario de los libros en una biblioteca. Utiliza una base de datos SQL Server para almacenar la información.

## Características

- Registro y gestión de libros, autores y editoriales.

## Requisitos previos

Antes de ejecutar la aplicación, asegúrate de tener instalado lo siguiente:

- .NET 6 SDK: [Descargar e instalar](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server: Asegúrate de tener una instancia de SQL Server disponible o instala una versión local.

## Configuración

Sigue los pasos a continuación para configurar y ejecutar la aplicación:

1. Clona este repositorio o descarga los archivos.
2. Abre la solución en Visual Studio 2022 (o una versión posterior).
3. En el archivo `appsettings.json`, configura la cadena de conexión a tu base de datos SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Biblioteca;Trusted_Connection=True"
}
```

4. Abre la Consola del Administrador de Paquetes en Visual Studio (`Herramientas -> Administrador de Paquetes NuGet -> Consola del Administrador de Paquetes`) y ejecuta el siguiente comando para aplicar las migraciones y crear la base de datos (o simplemente sube el backup LibraryInventory.bak en tu gestor de base de datos):

```
Update-Database
```

5. Compila la solución y asegúrate de que no haya errores.
6. Ejecuta la aplicación presionando `Ctrl+F5` o seleccionando `Depurar -> Ejecutar sin depuración` en Visual Studio.

La aplicación web se ejecutará en el puerto 7051 y podrás acceder a ella a través de tu navegador web en la siguiente URL: [http://localhost:7051](http://localhost:7051)

## Contribución

Si deseas contribuir a este proyecto, siéntete libre de hacer una rama y enviar tus mejoras a través de pull requests. También puedes informar cualquier problema o solicitud de características utilizando el sistema de Issues del repositorio.

## Licencia

Este proyecto está bajo la [Licencia MIT](https://es.wikipedia.org/wiki/Licencia_MIT).

---

README.md (English)

# LibraryInventory - ASP.NET MVC Web Application

This is an ASP.NET MVC web application developed in .NET 6 that allows managing the book's inventory of a library. It uses a SQL Server database to store the information.

## Features

- Registration and management of books, authors, and publishers.

## Prerequisites

Before running the application, make sure you have the following installed:

- .NET 6 SDK: [Download and install](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server: Ensure you have a SQL Server instance available or install a local version.

## Configuration

Follow the steps below to configure and run the application:

1. Clone this repository or download the files.
2. Open the solution in Visual Studio 2022 (or later).
3. In the `appsettings.json` file, configure the connection string to your SQL Server database:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Biblioteca;Trusted_Connection=True"
}
```

4. Open the Package Manager Console in Visual Studio (`Tools -> NuGet Package Manager -> Package Manager Console`) and run the following command to apply migrations and create the database (or just upload the LibraryInventory.bak backup in your database manager):

```
Update-Database
```

5. Build the solution and ensure there are no errors.
6. Run the application by pressing `Ctrl+F5` or selecting `Debug -> Start Without Debugging` in Visual Studio.

The web application will run on port 5000, and you can access it through your web browser at the following URL: [http://localhost:7051](http://localhost:7051)

## Contribution

If you want to contribute to this project, feel free to fork it and submit your enhancements through pull requests. You can also report any issues or feature requests using the repository's Issue tracking system.

## License

This project is licensed under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).