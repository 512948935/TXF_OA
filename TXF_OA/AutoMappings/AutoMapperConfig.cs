namespace TXF_OA.AutoMappings
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            new DomainModelToViewModelAutoMappings().Configure();
            new ViewModelToDomainModelAutoMappings().Configure();
        }
    }
}