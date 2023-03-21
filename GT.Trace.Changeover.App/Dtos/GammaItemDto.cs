namespace GT.Trace.Changeover.App.Dtos
{
    /// <summary>
    /// Representa un registro Gamma en forma de objeto DTO.
    /// </summary>
    /// Crea una nueva instancia de la clase GammaItemDto con los datos proporcionados.
    /// <param name="pointOfUseCode">Código del punto de uso asociado al registro Gamma.</param>
    /// <param name="compNo">Número de componente asociado al registro Gamma.</param>
    /// <param name="compRev">Revisión del componente asociado al registro Gamma.</param>
    /// <param name="compRev2">Segunda revisión del componente asociado al registro Gamma.</param>
    /// <param name="compDesc">Descripción del componente asociado al registro Gamma.</param>

    public sealed record GammaItemDto(string PointOfUseCode, string CompNo, string CompRev, string CompRev2, string CompDesc);
}