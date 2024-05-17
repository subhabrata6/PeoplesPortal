using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeoplesPortalNUnitTest.DbAccess
{
    public class DbConnection
    {
        private IConfiguration _config;

        public DbConnection()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);
        }

        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }
    }
}
