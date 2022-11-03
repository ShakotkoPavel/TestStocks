using AutoMapper;
using TestStocks.APIModels;
using TestStocks.DataAccess.Entities;

namespace TestStocks
{
    public class DbMapper
    {
        private static readonly MapperConfiguration MapperConfiguration = new(ConfigureModelsToEntities);

        private static void ConfigureModelsToEntities(IMapperConfigurationExpression configurationExpression)
        {
            configurationExpression.CreateMap<StocksResponse, StockEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            configurationExpression.CreateMap<Result, ResultEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }

        private static readonly IMapper Mapper = MapperConfiguration.CreateMapper();

        public static TDestination Map<TDestination>(object source)
        {
            return source == null ? default(TDestination) : Mapper.Map<TDestination>(source);
        }
    }
}
