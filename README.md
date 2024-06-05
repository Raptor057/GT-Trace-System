## GT Trace System
# Introduction 
Este es el código fuente del sistema de trazabilidad y sus servicios.
también llamado Trace v2, creado en 2022 por el Ingeniero MJV.

# Getting Started
- [Documentation](https://francereducteurs.sharepoint.com/:f:/s/SIGroupe/Eu4qX7N7639GissQQmdW6KkB76s_mjo_HMwiTWoUhUJb0w?e=b9NthL) 
- [Git](https://git-scm.com/download/win)
- [GitHub Desktop](https://desktop.github.com/)
- [Azure Data Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver16&tabs=redhat-install%2Credhat-uninstall)
- Extensions for VSC (Optional)
- .NET Extension Pack
- Activitus Bar
- Better Comments
- C/C++
- C/C++ Extension Pack
- C/C++ Themes
- C#
- CMake
- CMake Tools
- Debugger for Java
- Dev Containers
- Docker
- DotUML
- Dracula Official
- ES7+ React/Redux/React-Native snippets
- Extension Pack for Java
- Graphviz (dot) language support for Visual Studio Code
- IntelliCode
- IntelliCode API Usage Examples
- Ionide for F#
- isort
- Jupyter
- Jupyter Cell Tags
- Jupyter Keymap
- Jupyter Notebook Renderers
- Jupyter Slide Show
- Language Support for Java(TM) by Red Hat
- learn-markdown
- Live Preview
- Material Icon Theme
- Maven for Java
- Microsoft Edge Tools for VS Code
- MySQL
- NPM Audit
- Output Colorizer
- PlantUML
- Polacode
- Polyglot Notebooks
- Power Query / M Language
- PowerShell
- Project Manager for Java
- Pylance
- Python
- R
- R Debugger
- React Native Tools
- REST Client
- Sass (.sass only)
- Serial Monitor
- Simple React Snippets
- SQL Notebook
- Svelte 3 Snippets
- Svelte for VS Code
- Svelte Intellisense
- Svelte Preview
- Test Runner for Java
- Highlight
- Tokyo Night
- vscode-icons
- XML Tools
- Arduino

# Build and Test

- 
# Actualizaciones

- 16/03/2023: Copié el proyecto de la Colección DefaultCollection a la Colección General_Transmissions_Dev.

- 23/03/2023: Actualice el ensamblaje del cliente web, agregue un botón de desbloqueo de línea y agregue la escritura de los mensajes a la base de datos Gtt, a la tabla Historial de eventos de la base de datos GTT, utilizando una API web.

- 29/03/2023: Se agregó el método GetOrigenByCegid y se agregó Origen en la etiqueta printMaster en el archivo GT.Trace.Packaging.App.UseCases.PackUnit.

- 30/03/2023: Se corrigió Origen en etiquetas maestras.

- 04/04/2023: Eliminar carpetas y archivos de seguimiento en Git, Cambie "iniciar navegador": verdadero por "iniciar navegador": falso. en todas las aplicaciones del proyecto.

- 10/04/2023: Quitando carpetas obj del control de versiones.

- 12/04/2023: Actualizacion de Query.

- 24/04/2023: Se agregó Origen en json de Master Label y se corrigió el error en la impresión de la última etiqueta Jr antes de imprimir la etiqueta maestra.

- 02/05/2023: Agregar el cliente web JoinMotorsEZ2000.

- 05/05/2023: Se agregó la API JoinEZ2000 Motors temporalmente, para luego agregar la función en la API de empaquetado general.

- 08/05/2023: Se agregó una línea para guardar los etis eliminados en la tabla SaveRemoveEtis en la base de datos GTT, también se agregó que todos los etis se guardan automáticamente en pointofuseetisV2 utilizado por la Línea E.

- 16/05/2023: Actualizar Gt.Trace.JoinMotorsWebClient Punto final agregado en RecordProcess Se agregó un comentario de código de línea en el repositorio de estaciones, se actualizo GT Trace UI.

- 25/05/2023: Confirmar 27dd9c5d: - Se agrego bloqueo que evita escanear 2 veces el mismo motor en la API que se encarga de registrar los datos de los motores de frameless.

- Se agrego bloqueo que evita escanear si previamente no existe en otra tabla la unidad antes de empacar en la API que se encarga de trazar las etis.

- Se cambio el .gitignore por uno mejor.

- 01/06/2023: Ya está terminada la opción que muestra la imagen en el cuadro "Calidad" en la UI del paquete, esto en LoadPackStateHandler.
agregó la variable APKNPCECO2 en la clase UARTICLE en GT.Trace.Packaging.Infra.DataSources.Entities y asignó PalletSize2 como APKNPCECO2.
Se agregó el método GetQuantityFromLastMasterID en la clase TrazaSqlDB en GT.Trace.Packaging.Infra.DataSources.
La clase StationRepository fue modificada en GT.Trace.Packaging.Infra.Repositories (Leer comentarios en código).
Agregué algunas variables nuevas a LabelParserService en GT.Trace.Packaging.Infra.Services para agregar las API que hice extras en el paquete (pendiente de finalización).

- 13/06/2023: El nombre se especificó en la interfaz de JoinMotors, en Input.Svelte se modificó la expresión regular que lee el escaneo, en JoinmotorsEZ2000Endpoint se cambió la expresión regular como en svelte,
En PackUnit.Responses, se agregó #pragma advertencia deshabilitar CS8604 para deshabilitar esas advertencias.
A PrintWipLabelHandler solo se le eliminó 1 espacio después de TODO
Se agregó el proyecto de prueba unitaria Packaging.AppTests.
Se cambió el nombre de las expresiones regulares EZ y Frameless QR en LabelFormatRegExPatterns, TODO se eliminó de README.md
Se agregó la advertencia #pragma para desactivar CS8602 en GT.Trace.JoinMotors.HttpServices.EndPoints.Units.Lines.JoinMotorsEZ2000
Se agregó la advertencia #pragma para desactivar CS8602 en SqlEtiRepository
en GT.Trace.Etis.Infra.Repositories y GT.Trace.ProcessHistory.HttpServices.EndPoints.Units.Lines.Processes en [Route("/api/UnitID/{ezLabel}/Line/{LineCode}")]
Se agregó la advertencia #pragma para desactivar CS8604 en SqlEtiRepository en GT.Trace.EtiMovements.Infra.Repositories
Se agregó la advertencia #pragma para desactivar CS1998 en ParseEtiIDHandler en GT.Trace.Etis.App.UseCases.ParseEti

- 16/06/2023: Se solucionó el error de la operación de empaque cuando no había rango en cegid.
El rango en TRAZAB se actualiza automáticamente desde CEGID cuando se detecta que no existe en la base de datos.
todo esto excluyendo la línea E
Se corrigió el error donde se detecta si hay más de 1 orden activa. 

- 21/06/2023: Se agregaron entidades EZ2000MotrosQR y entidades FramelessMotorsQR, cambiado si (prod_unit.letter! = "LE") => if (prod_unit.letter! = "LE" || prod_unit.letter! = "LN") Se cambió la clase interna TrazaSqlDB a la clase pública TrazaSqlDB.

- 23/06/2023: Se agregaron puntos finales y casos de uso de la unión de componentes y la interfaz de usuario de la unión de componentes.

- 28/06/2023: Se agregó el botón Eliminar transmisiones en la interfaz Componentes Unirse, PackaginApi modificado en componentes unirse.

- 29/06/2023: Se agregó JoinEZMotorsJoin y se corrigió Join Motors Frameless en el controlador y el presentador.

- 05/07/2023: Se agregó restricción de cambio de modelo en cambios para evitar que se descarguen componentes cuando no hay información del Boom del modelo al que se desea correr, Se cambiaron varios TODOS a NOTA. Se agregó la función de comparación entre rangos CEGID y Trazab para que se actualice automáticamente.

- 06/07/2023: Se corrigió y optimizó el método Count Components Be Async.

- 10/07/2023: Se corrigió un error en el cambio de modelo causado por un intento de corregir otro error para reducir el tiempo de inactividad de TI.

- 11/07/2023: Motores JoinEZ fijos, Y rediseño del botón de desbloqueo de línea manual desde la interfaz de usuario web de Assembly, Se corrigieron los JoinEZMotors y el rediseño del botón de desbloqueo de línea manual desde la interfaz de usuario web de Assembly. 
  
- 24/07/2023: Cambio de PC de Lenovo a HP, Actualizacion en App.Svelte para Pokayoke de linea P.

- 31/07/2023: Borrado de empaque de EZ2000 viejo, actualizar JoinMotorsWebClient, Cargar configuración pubxml, Fusionar rama 'principal' de http://mxsrvdev/General_Transmissions_Dev/GT%20Trace%20System/_git/GT%20Trace%20System, Se agregó la API y la interfaz de usuario de WagonLoad, Se agregó un método para guardar etis usados con isdeleted = true en la tabla WagonLoad en gtt.

- 03/08/2023: Del pro_etis001.cs en WagonLoad, Dell pro_ats001.sys en carga de errores.

- 04/08/2023: Corregir y actualizar el error de carga de material del subensamblaje.

- 15/08/2023: Se modifica la información del QR de motores para los modelos EZ, y se cambia la expresión regular, adaptando los módulos afectados, y las entidades para que pueda capturar todos los datos, Se eliminó el poka yoke de actualización de Boom debido a un problema de rendimiento, Los módulos que imprimen y cargan las etiquetas han terminado de programar el subconjunto.

- 25/08/2023: El archivo TtraceSqlDB del espacio de nombres GT. A Trace.Packaging.Infra.DataSources se le dijo el método SetStationBlocked porque no se envía para llamar desde ningún lugar.
Se modificó en la clase SqlGetLineWorkOrderGateway en el espacio de nombres GT. Trace.Infra.Gateways después de la producción Entities.pro_production; donde si hay un NULL dará error y no permitirá realizar el cambio.

- 30/08/2023: Se agregó un nuevo punto final para la actualización de línea bom.

- 31/08/2023: Interfaz actualizada del nuevo punto final para la actualización de línea bom, Eliminar GT.Trace.Assembly.UI.MaterialLoadingSpa.

- 05/09/2023: Agregar comentarios y eliminar el proyecto antiguo, Actualizar orden de trabajo a Orden de trabajo. Número de pieza.

- 06/09/2023: Control de torquimetros para línea E.

- 13/09/2023: Se agregó el registro PalletQR con UnitID en DB desde EZ en UI, Se agregó registro PalletQR con UnitID en DB de EZ.

- 20/09/2023: Error solucionado en el registro de la tabla LineProductionSchedule.

- 26/09/2023: Se agregaron comentarios al codigo en general en GttSqlDB.cs y en StationRepository.cs, Botón agregado para actualizar gama en línea

- 27/09/2023: Se agregó UpdateGamaGtt en CommonApi.

- 17/10/2023: Se agregó Origen en json de Master Label y se corrigió el error en la impresión de la última etiqueta Jr antes de imprimir la etiqueta maestra, Error de pedido activo solucionado en gt-apps La expresión regular se modificó para poder leer 2 tipos de formatos QR diferentes para EZ Engines.

- 20/11/2023: 
1) The label method for Frameless motors was added to BL Motors
2) The enpoint put was added to the JoinMotors UI to register the Pallet and the transmission.

- 27/11/2023: Se agrego Endpoint de Save EZ Motors

- 20/12/2023: Se agrego el proyecto de Traceability Legacy Integration el cual lo conforman:
1. TraceabilityLegacyIntegration.App
2. TraceabilityLegacyIntegration.Domain
3. TraceabilityLegacyIntegration.Infra
4. LegacyIntegrationWebApi
Faltando el: LegacyIntegrationWebUI

- 1/04/2024: Se actualizo el valor de containerSize =  uarticle.ContainerSize en la clase StationRepository
  de Packaging, esto debido a que solo cambiaba el valor de la tarima y no el de el contenedor, este cambio originalmente fue realizado para iterar entre 8 y 12 el tipo de empaque para EZ en la linea E en ciertos modelos
pero ya esta adaptado para cualquier modelo que se quiera correr siempre y cuando se encuentre un valor en el tipo de standart pack 001bis en cegid.