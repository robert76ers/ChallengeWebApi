N5 Challenge
Instrucciones y paso a paso del desarrollo

-Creación de proyecto tipo NetCore WebApi en .Net 7

-Creación de base de datos en Sql Server, enfoque Database first

-Implementación de Entity FrameWork Core en proyecto

	--Instalacion de paquetes:

		Entity Framework Core

		Entity Framework Tools

		Entity Framework Sql Server

	--Migracion de base de datos existente a EF

	  Scaffold-DbContext "Server=.\SQLExpress;Database=N5_UsersPermissions;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

-Creacion de Controller

	--Creacion de 3 servicios requeridos

		requestpermission

		modifypermission

		getpermissions

-Agregar logs con Serilog

	--Instalacionde paquetes

		Serilog

		Serilig.Sinks.File

	--Implementar logs en metodos de controller

-Crear proyecto ed tipo xUnit para pruebas unitarias

-Crear integracion con ElasticSearch

	--Instalacion de paquete:

		NEST (Elasticsearch.Net & NEST)

		Implementacion de Metodo Search par buscar por Surename y Forename en base a un string

-Crear estructura de mensajes para enviar a servidor Paache Kafka

	--Instalar paquetes requeridos

		Confluent.Kafka

	--Crear metodo pra envio de mensajes

-Montar la web api en un docker

	--Publicar el sitio

	--Crear el archivo Dockerfile

	--Obtener la imagen .Net de Docker Hub

	--Preparar el archivo Dockerfile para su implementación
