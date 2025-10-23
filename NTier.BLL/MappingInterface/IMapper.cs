namespace NTier.BLL.MappingInterface;

public interface IMapper<TSource, TDest>
{
    TDest Map(TSource source);
    IEnumerable<TDest> Map(IEnumerable<TSource> sources);

    /// <summary>
    /// Maps properties from source to an existing destination object.
    /// Only updates properties that exist in the source, preserving others.
    /// Perfect for partial updates where you want to keep unmapped properties intact.
    /// </summary>
    void MapToExisting(TSource source, TDest destination);
}