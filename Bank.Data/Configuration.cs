using Microsoft.Extensions.Configuration;

namespace Bank.Data
{
    internal class Configuration
    {
        private static Configuration _instance;

        /// <summary>
        /// Definição Singleton da classe, mantendo somente uma instância em todo o sistema.
        /// </summary>
        private static Configuration Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Configuration();

                return _instance;
            }
        }

        private IConfigurationRoot _config;

        /// <summary>
        /// Construtor estático que obtém as configurações do arquivo correto.
        /// </summary>
        private Configuration()
        {
            var builder = new ConfigurationBuilder()
#if DEBUG
                    .AddJsonFile("appsettings.Development.json");
#else
                    .AddJsonFile("appsettings.Production.json");
#endif

            _config = builder.Build();
        }

        /// <summary>
        /// Obtém toda a configuração.
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot GetConfig()
        {
            return Instance._config;
        }

        /// <summary>
        /// Obtém uma determinada Connection String do arquivo de configuração.
        /// </summary>
        /// <param name="connectionString">Nome da Connection String</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionString)
        {
            return Instance._config.GetConnectionString(connectionString);
        }

        /// <summary>
        /// Obtém um determinada secção do arquivo de configuração.
        /// </summary>
        /// <param name="sectionName">Nome da Secção</param>
        /// <returns></returns>
        public static IConfigurationSection GetConfigurationSection(string sectionName)
        {
            return Instance._config.GetSection(sectionName);
        }
    }
}
