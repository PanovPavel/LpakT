using System.Configuration;

namespace LpakBL.Controller
{
    /// <summary>
    /// класс для получения строки подключения к базе данных из конфигурационного файла
    /// </summary>
    public static class ConnectionStringFactory
    {
        /// <summary>
        /// Наиминование свойства в конфигурационном файле для подключения к базе 
        /// </summary>
        private static string GetNameConnection => "ConnectionStringDBLpakTesting";
        /// <summary>
        /// Возвращает строку подключения к базе данных из конфигура
        /// </summary>
        /// <returns>Строка подключения</returns>
        public static string GetConnectionString()
        {
            var connectionStringName = GetNameConnection;
            //запрос к конфигурационному файлу для получения строки подключения
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}