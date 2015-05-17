using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MicheleCaggiano.MongoModel.Properties;

namespace MicheleCaggiano.MongoModel
{
  public class ModelContext
  {
    public MongoDatabase Database;

    public ModelContext(string connectionString, string databaseName)
    {
      //var client = new MongoClient(Settings.Default.MicheleCaggianoConnectionString);
      var client = new MongoClient(connectionString);

      //client.Settings.ConnectTimeout = new TimeSpan(0, 1, 0);
      //client.Settings.ConnectionMode = ConnectionMode.Automatic;
      //client.Settings.SocketTimeout = new TimeSpan(0, 1, 0);

      var server = client.GetServer();

      Database = server.GetDatabase(databaseName);//Database = server.GetDatabase(Settings.Default.MicheleCaggianoDatabaseName);
    }

    public MongoCollection<Articolo> Articoli
    {
      get
      {
        return Database.GetCollection<Articolo>("articoli");
      }
    }
    public MongoCollection<Categoria> Categorie
    {
      get
      {
        return Database.GetCollection<Categoria>("categoria");
      }
    }
  }
}
