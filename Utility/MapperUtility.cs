using AutoMapper;

namespace Utility
{
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
    }
}
