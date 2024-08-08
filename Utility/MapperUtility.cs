using AutoMapper;
using System.Reflection;

namespace Utility;

public static class MapperUtility
{
    /// <summary>
    /// Attempts to convert an object of any type to a specified target type <typeparamref name="TResult"/> using AutoMapper.
    /// </summary>
    /// <typeparam name="TResult">The target type to which the object should be converted.</typeparam>
    /// <param name="source">The object to be converted.</param>
    /// <returns>An instance of the target type <typeparamref name="TResult"/> populated with the mapped values from the source object, 
    /// or null if the conversion fails.</returns>
    public static TResult To<TResult>(this object source) where TResult : new()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap(source.GetType(), typeof(TResult));
        });
        IMapper mapper = config.CreateMapper();
        return mapper.Map<TResult>(source);
    }

    public static IEnumerable<T> Linq<T>(this T _)
    {
        return new List<T> { _ };
    }

    public static TResult Mapping<TResult>(this object source) where TResult : class, new()
    {
        TResult result = new TResult();
        IEnumerable<PropertyInfo> inner = from p in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead
                                          select p;
        foreach (var item in from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             where p.CanWrite
                             select p into r
                             join s in inner on new { r.Name } equals new { s.Name }
                             select (r, s))
        {
            Mapping(source, item, ref result);
        }

        return result;
    }

    public static TResult Mapping<TResult>(this object source, Dictionary<string, object> exceptionHandlings) where TResult : class, new()
    {
        Dictionary<string, object> exceptionHandlings2 = exceptionHandlings;
        TResult result = new TResult();
        IEnumerable<PropertyInfo> inner = from p in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead && !Enumerable.Contains(exceptionHandlings2.Keys, p.Name)
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<PropertyInfo> enumerable = from r in outer
                                               join e in exceptionHandlings2 on r.Name equals e.Key
                                               select r;
        IEnumerable<(PropertyInfo, PropertyInfo)> enumerable2 = from r in outer
                                                                join s in inner on new { r.Name } equals new { s.Name }
                                                                select (r, s);
        foreach (PropertyInfo item in enumerable)
        {
            item.SetValue(result, exceptionHandlings2[item.Name]);
        }

        foreach (var item2 in enumerable2)
        {
            Mapping(source, item2, ref result);
        }

        return result;
    }

    public static TResult Mapping<TSource, TResult>(this TSource source, Dictionary<string, Func<TSource, object>> exceptionHandlings) where TResult : class, new()
    {
        Dictionary<string, Func<TSource, object>> exceptionHandlings2 = exceptionHandlings;
        TResult result = new TResult();
        IEnumerable<PropertyInfo> inner = from p in typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead && !Enumerable.Contains(exceptionHandlings2.Keys, p.Name)
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<PropertyInfo> enumerable = from r in outer
                                               join e in exceptionHandlings2 on r.Name equals e.Key
                                               select r;
        IEnumerable<(PropertyInfo, PropertyInfo)> enumerable2 = from r in outer
                                                                join s in inner on new { r.Name } equals new { s.Name }
                                                                select (r, s);
        foreach (PropertyInfo item in enumerable)
        {
            item.SetValue(result, exceptionHandlings2[item.Name](source));
        }

        foreach (var item2 in enumerable2)
        {
            Mapping(source, item2, ref result);
        }

        return result;
    }

    public static TResult Mapping<TResult>(this object source, params (string Key, object Value)[] exceptionHandlings) where TResult : class, new()
    {
        (string Key, object Value)[] exceptionHandlings2 = exceptionHandlings;
        TResult result = new TResult();
        IEnumerable<PropertyInfo> inner = from p in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead && !exceptionHandlings2.Any(((string Key, object Value) e) => e.Key == p.Name)
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<PropertyInfo> enumerable = from r in outer
                                               join e in exceptionHandlings2 on r.Name equals e.Key
                                               select r;
        IEnumerable<(PropertyInfo, PropertyInfo)> enumerable2 = from r in outer
                                                                join s in inner on new { r.Name } equals new { s.Name }
                                                                select (r, s);
        foreach (PropertyInfo p2 in enumerable)
        {
            p2.SetValue(result, exceptionHandlings2.First(((string Key, object Value) e) => e.Key == p2.Name).Value);
        }

        foreach (var item in enumerable2)
        {
            Mapping(source, item, ref result);
        }

        return result;
    }

    public static TResult Mapping<TSource, TResult>(this TSource source, params (string Key, Func<TSource, object> Value)[] exceptionHandlings) where TResult : class, new()
    {
        (string Key, Func<TSource, object> Value)[] exceptionHandlings2 = exceptionHandlings;
        TResult result = new TResult();
        IEnumerable<PropertyInfo> inner = from p in typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead && !exceptionHandlings2.Any(((string Key, Func<TSource, object> Value) e) => e.Key == p.Name)
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<PropertyInfo> enumerable = from r in outer
                                               join e in exceptionHandlings2 on r.Name equals e.Key
                                               select r;
        IEnumerable<(PropertyInfo, PropertyInfo)> enumerable2 = from r in outer
                                                                join s in inner on new { r.Name } equals new { s.Name }
                                                                select (r, s);
        foreach (PropertyInfo p2 in enumerable)
        {
            p2.SetValue(result, exceptionHandlings2.First(((string Key, Func<TSource, object> Value) e) => e.Key == p2.Name).Value(source));
        }

        foreach (var item in enumerable2)
        {
            Mapping(source, item, ref result);
        }

        return result;
    }

    public static IEnumerable<TResult> Mappings<TResult>(this IEnumerable<object> sources) where TResult : class, new()
    {
        if (sources != null && sources.Count() == 0)
        {
            yield break;
        }

        IEnumerable<PropertyInfo> inner = from p in sources.GetType().GenericTypeArguments.First().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<(PropertyInfo r, PropertyInfo s)> rs = from r in outer
                                                           join s in inner on new { r.Name } equals new { s.Name }
                                                           select (r, s);
        foreach (object source in sources)
        {
            TResult result = new TResult();
            foreach (var item in rs)
            {
                Mapping(source, item, ref result);
            }

            yield return result;
        }
    }

    public static IEnumerable<TResult> Mappings<TResult>(this IEnumerable<object> sources, Dictionary<string, object> exceptionHandlings) where TResult : class, new()
    {
        Dictionary<string, object> exceptionHandlings2 = exceptionHandlings;
        IEnumerable<PropertyInfo> inner = from p in sources.GetType().GenericTypeArguments.First().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead && !Enumerable.Contains(exceptionHandlings2.Keys, p.Name)
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<PropertyInfo> handlings = from r in outer
                                              join e in exceptionHandlings2 on r.Name equals e.Key
                                              select r;
        IEnumerable<(PropertyInfo r, PropertyInfo s)> rs = from r in outer
                                                           join s in inner on new { r.Name } equals new { s.Name }
                                                           select (r, s);
        foreach (object source in sources)
        {
            TResult result = new TResult();
            foreach (PropertyInfo item in handlings)
            {
                item.SetValue(result, exceptionHandlings2[item.Name]);
            }

            foreach (var item2 in rs)
            {
                Mapping(source, item2, ref result);
            }

            yield return result;
        }
    }

    public static IEnumerable<TResult> Mappings<TResult>(this IEnumerable<object> sources, params (string Key, object Value)[] exceptionHandlings) where TResult : class, new()
    {
        (string Key, object Value)[] exceptionHandlings2 = exceptionHandlings;
        IEnumerable<PropertyInfo> inner = from p in sources.GetType().GenericTypeArguments.First().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead && !exceptionHandlings2.Any(((string Key, object Value) e) => e.Key == p.Name)
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<PropertyInfo> handlings = from r in outer
                                              join e in exceptionHandlings2 on r.Name equals e.Key
                                              select r;
        IEnumerable<(PropertyInfo r, PropertyInfo s)> rs = from r in outer
                                                           join s in inner on new { r.Name } equals new { s.Name }
                                                           select (r, s);
        foreach (object source in sources)
        {
            TResult result = new TResult();
            foreach (PropertyInfo p2 in handlings)
            {
                p2.SetValue(result, exceptionHandlings2.First(((string Key, object Value) e) => e.Key == p2.Name).Value);
            }

            foreach (var item in rs)
            {
                Mapping(source, item, ref result);
            }

            yield return result;
        }
    }

    public static IEnumerable<TResult> Mappings<TSource, TResult>(this IEnumerable<TSource> sources, params (string Key, Func<TSource, object> Value)[] exceptionHandlings) where TResult : class, new()
    {
        (string Key, Func<TSource, object> Value)[] exceptionHandlings2 = exceptionHandlings;
        IEnumerable<PropertyInfo> inner = from p in typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead && !exceptionHandlings2.Any(((string Key, Func<TSource, object> Value) e) => e.Key.Contains(p.Name))
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<PropertyInfo> handlings = from r in outer
                                              join e in exceptionHandlings2 on r.Name equals e.Key
                                              select r;
        IEnumerable<(PropertyInfo r, PropertyInfo s)> rs = from r in outer
                                                           join s in inner on new { r.Name } equals new { s.Name }
                                                           select (r, s);
        foreach (TSource source in sources)
        {
            TResult result = new TResult();
            foreach (PropertyInfo p2 in handlings)
            {
                p2.SetValue(result, exceptionHandlings2.First(((string Key, Func<TSource, object> Value) e) => e.Key == p2.Name).Value(source));
            }

            foreach (var item in rs)
            {
                Mapping(source, item, ref result);
            }

            yield return result;
        }
    }

    public static IEnumerable<TResult> Mappings<TSource, TResult>(this IEnumerable<TSource> sources, Dictionary<string, Func<TSource, object>> exceptionHandlings) where TResult : class, new()
    {
        Dictionary<string, Func<TSource, object>> exceptionHandlings2 = exceptionHandlings;
        IEnumerable<PropertyInfo> inner = from p in typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanRead && !Enumerable.Contains(exceptionHandlings2.Keys, p.Name)
                                          select p;
        IEnumerable<PropertyInfo> outer = from p in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                          where p.CanWrite
                                          select p;
        IEnumerable<PropertyInfo> handlings = from r in outer
                                              join e in exceptionHandlings2 on r.Name equals e.Key
                                              select r;
        IEnumerable<(PropertyInfo r, PropertyInfo s)> rs = from r in outer
                                                           join s in inner on new { r.Name } equals new { s.Name }
                                                           select (r, s);
        foreach (TSource source in sources)
        {
            TResult result = new TResult();
            foreach (PropertyInfo item in handlings)
            {
                item.SetValue(result, exceptionHandlings2[item.Name](source));
            }

            foreach (var item2 in rs)
            {
                Mapping(source, item2, ref result);
            }

            yield return result;
        }
    }

    private static void Mapping<T>(object source, (PropertyInfo r, PropertyInfo s) p, ref T result)
    {
        if (p.r.PropertyType == p.s.PropertyType)
        {
            p.r.SetValue(result, p.s.GetValue(source));
        }
        else if (p.r.PropertyType.IsEnum && p.s.PropertyType == typeof(int))
        {
            if (Enum.TryParse(p.r.PropertyType, p.s.GetValue(source)?.ToString(), out object result2))
            {
                p.r.SetValue(result, result2);
            }
        }
        else if (p.r.PropertyType.IsEnum && p.s.PropertyType == typeof(byte))
        {
            if (Enum.TryParse(p.r.PropertyType, p.s.GetValue(source)?.ToString(), out object result3))
            {
                p.r.SetValue(result, result3);
            }
        }
        else if (p.r.PropertyType.IsEnum && p.s.PropertyType == typeof(short))
        {
            if (Enum.TryParse(p.r.PropertyType, p.s.GetValue(source)?.ToString(), out object result4))
            {
                p.r.SetValue(result, result4);
            }
        }
        else if (p.r.PropertyType == typeof(int) && p.s.PropertyType.IsEnum)
        {
            p.r.SetValue(result, (int)p.s.GetValue(source));
        }
        else if (Nullable.GetUnderlyingType(p.r.PropertyType) == p.s.PropertyType)
        {
            p.r.SetValue(result, p.s.GetValue(source));
        }
        else if (p.r.PropertyType == Nullable.GetUnderlyingType(p.s.PropertyType) && (p.s.GetValue(source) != null || !p.r.PropertyType.IsValueType))
        {
            p.r.SetValue(result, p.s.GetValue(source));
        }
    }
}
