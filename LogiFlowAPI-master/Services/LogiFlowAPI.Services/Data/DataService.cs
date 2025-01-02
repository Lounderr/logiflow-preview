namespace LogiFlowAPI.Services.Data
{
    using AutoMapper;

    using LogiFlowAPI.Services.Mapping;

    public abstract class DataService
    {
        public DataService()
        {
            this.Mapper = AutoMapperConfig.MapperInstance;
        }

        protected IMapper Mapper { get; private set; }

        protected TDestination Map<TDestination>(object source)
        {
            return this.Mapper.Map<TDestination>(source);
        }

        protected TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return this.Mapper.Map(source, destination);
        }
    }
}
