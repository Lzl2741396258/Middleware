using System.Collections.Generic;
using AutoMapper;

namespace Ersa.Mes.Schnittstelle.Mapper;

public static class MapperHelper
{
	public static TDestination Fun_edcMapTo<TSource, TDestination>(this TSource source)
	{
		if (source == null)
		{
			return default(TDestination);
		}
		MapperConfiguration config = new MapperConfiguration(delegate(IMapperConfigurationExpression cfg)
		{
			cfg.CreateMap<TSource, TDestination>();
		});
		IMapper mapper = config.CreateMapper();
		return mapper.Map<TDestination>(source);
	}

	public static IEnumerable<TDestination> Fun_edcMapToList<TSource, TDestination>(this IEnumerable<TSource> source)
	{
		if (source == null)
		{
			return new List<TDestination>();
		}
		MapperConfiguration config = new MapperConfiguration(delegate(IMapperConfigurationExpression cfg)
		{
			cfg.CreateMap<TSource, TDestination>();
		});
		IMapper mapper = config.CreateMapper();
		return mapper.Map<List<TDestination>>(source);
	}
}
