namespace FinistBusinessLogic.Controllers
{
    public static class ConfigJSON
    {
        /// <summary>
        /// Создаёт конфиг, для работы с appsettings.json.
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot GetConfig()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");

            return builder.Build();
        }
    }
}
