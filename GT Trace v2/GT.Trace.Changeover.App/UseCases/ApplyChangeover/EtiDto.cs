using Serilog;
using System.ComponentModel;
using System;

namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /// <summary>
    /// El código define una clase sellada (sealed) llamada EtiDto que representa un objeto DTO (Data Transfer Object) que contiene información sobre una etiqueta electrónica de inventario (ETI). La clase solo tiene un campo de solo lectura de tipo string llamado Number, que representa el número de la etiqueta electrónica.
    ///Los objetos DTO se utilizan para transferir datos entre diferentes componentes del sistema.La creación de una clase DTO específica para la información de la etiqueta electrónica permite un manejo más sencillo de la información en el código y reduce la complejidad de las estructuras de datos que se pasan entre los diferentes componentes del sistema.
    /// </summary>
    /// <param name="Number"></param>
    public sealed record EtiDto(string Number);
}