namespace Greta.BO.Api.Entities.Interfaces;

/// <summary>
/// Descibe a Entity with property name unique
/// </summary>
public interface INameUniqueEntity
{
    /// <summary>
    /// Unique name Property
    /// </summary>
    public string Name { get; set; }
}