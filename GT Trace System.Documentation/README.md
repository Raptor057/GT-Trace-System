[![LOGO-GT](/Sources/LOGO-GT.png)](https://generaltransmissions.com/en/)

# GT Trace System

# Documentacion Old

- [Documentation](https://francereducteurs.sharepoint.com/:f:/s/SIGroupe/Eu4qX7N7639GissQQmdW6KkB76s_mjo_HMwiTWoUhUJb0w?e=b9NthL) 

# Documentación del Proyecto

## Introducción

Este es el código fuente del sistema de trazabilidad y sus servicios.

también llamado Trace v2, creado en 2022 por el Ingeniero Marco J Vazquez.

## Estado del Proyecto

Descripción del estado actual del proyecto, incluyendo si está en desarrollo, finalizado o en otra etapa.

## Objetivos


## Tecnologías Utilizadas

- [Git](https://git-scm.com/download/win)
- [GitHub Desktop (Opcional)](https://desktop.github.com/)
- [Azure Data Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver16&tabs=redhat-install%2Credhat-uninstall)
- [Net .6](https://dotnet.microsoft.com/es-es/download/dotnet/6.0)
- [Net .8](https://dotnet.microsoft.com/es-es/download/dotnet/8.0)
- [NodeJS](https://nodejs.org/en)

## Arquitectura del Proyecto

- [Clean Architecture](https://medium.com/@msmdotnet/introducci%C3%B3n-a-clean-architecture-con-net-parte-1-9db30045f2f2)

[![Diagrama de Clean Architecture propuesto por Robert C. Martin](/Sources/GT-Trace-System/Clean_Architecture.jpeg)](https://medium.com/@msmdotnet/introducci%C3%B3n-a-clean-architecture-con-net-parte-1-9db30045f2f2)

## Libros de apoyo

- [Clean Architecture A Craftsman's Guide to Software Structure and Design](/Sources/GT-Trace-System/Clean-Architecture.pdf)

[![Portada](/Sources/GT-Trace-System/Clean-Architecture.jpg)](/Sources/GT-Trace-System/Clean-Architecture.jpg)

- [Clean Code A Handbook of Agile Software Craftsmanship](/Sources/GT-Trace-System/Clean-Code.pdf)

[![Portada](/Sources/GT-Trace-System/Clean-code.jpg)](/Sources/GT-Trace-System/Clean-code.jpg)

- [C# 11 and .NET 7 Modern Cross Platform Development Fundamentals Seventh Edition](/Sources/GT-Trace-System/C-11-and-.NET-7.pdf)

[![Portada](/Sources/GT-Trace-System/C#-11-and-.NET-7.jpg)](/Sources/GT-Trace-System/C#-11-and-.NET-7.jpg)


## Funcionalidades

- Registrar la trazabilidad de componentes por piezas

- [Carga de Materiales (Tabletas de materialista / almacen)](http://mxsrvapps.gt.local/gtt/logistics/materialloading/)

- [Empaque Linea O (Ejemplo)](http://mxsrvapps/gtt/mfg/packaging/?line=MXDT202001)

- [Escaneo de etiquetas de componentes Linea O (Ejemplo)](http://mxsrvapps/gtt/mfg/assembly/?line=LO)

- [Cambio de modelo Linea O (Ejemplo)](http://mxsrvapps/gtt/mfg/changeovers/?line=LO)

- [Enlaces a las todo lo instalado en el IIS (GT IIS Web Apps)](http://mxsrvapps/gtt/mfg/lineUI/Line.html)

## Instalación

- Se instala en el servidor web IIS como web apps en la siguinte direccion

` \\mxsrvapps\apps\web `

[![IIS](/Sources/GT-Trace-System/iis-web-server.png)]()

Cuando se publica una Web Api se hace en la carpeta 
` \\mxsrvapps\apps\web\gtt\services `

[![IIS](/Sources/GT-Trace-System/iis-web-server-services.png)]()

una vez publicado se debe crear una Application Pool solo para esa API

Cuando se publica un Web Client se hace en la carpeta 
` \\mxsrvapps\apps\web\gtt\mfg `

[![IIS](/Sources/GT-Trace-System/iis-web-server-mfg.png)]()

este web client puede ir en DefaultAppPool

[![IIS](/Sources/GT-Trace-System/iis-web-server-appliction-pools.png)]()

## Uso

Se inicia en automatico mediante el IIS una vez publicado.

## Contribución

Instrucciones para contribuir al proyecto, incluyendo cómo reportar errores, enviar solicitudes de extracción, etc.

## Licencia

N/A

## Contacto
