using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace API.Repositories
{
    public class Contexto
    {
        private readonly IConfigurationRoot _config;
        public MongoClient? client;
        public IMongoDatabase? db;
        public Contexto (){
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();

            client = new MongoClient(_config.GetConnectionString("Base"));
            db = client.GetDatabase("Api");
        }
    }
}