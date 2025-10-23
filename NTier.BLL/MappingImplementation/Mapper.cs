using System.Reflection;
using NTier.BLL.MappingInterface;

namespace NTier.BLL.MappingImplementation;

public class Mapper<TSource, TDest>(Func<TSource, TDest>? mapFunc = null) : IMapper<TSource, TDest> where TDest : new()
{
    private readonly Func<TSource, TDest>? _mapFunc = mapFunc;

    public TDest Map(TSource source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        // Use custom mapping function if provided, otherwise use reflection-based mapping
        return _mapFunc != null ? _mapFunc(source) : MapByProperties(source);
    }

    public IEnumerable<TDest> Map(IEnumerable<TSource> sources)
    {
        if (sources == null)
        {
            throw new ArgumentNullException(nameof(sources));
        }

        return sources.Select(Map);
    }

    /// <summary>
    /// Maps properties from source to an existing destination object.
    /// Only updates properties that exist in the source, preserving others.
    /// Perfect for partial updates where you want to keep unmapped properties intact.
    /// </summary>
    private static TDest MapByProperties(TSource source)
    {
        TDest dest = new TDest();
        var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destProperties = typeof(TDest).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceProp in sourceProperties)
        {
            var destProp = destProperties.FirstOrDefault(p =>
                p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
            if (destProp != null && destProp.CanWrite)
            {
                // var value = sourceProp.GetMethod?.Invoke(source, null);
                var value = sourceProp.GetValue(source);
                destProp.SetValue(dest, value);
            }
        }

        return dest;
    }

    public void MapToExisting(TSource source, TDest destination)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destProperties = typeof(TDest).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceProp in sourceProperties)
        {
            var destProp = destProperties.FirstOrDefault(p =>
                p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
            if (destProp != null && destProp.CanWrite)
            {
                var value = sourceProp.GetValue(source);
                // Skip nulls to avoid overwriting existing destination values during partial updates
                if (value is null)
                {
                    continue;
                }

                destProp.SetValue(destination, value);
            }
        }
    }
}