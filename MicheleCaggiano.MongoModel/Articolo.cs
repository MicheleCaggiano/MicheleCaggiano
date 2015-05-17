
namespace MicheleCaggiano.MongoModel
{
  using MongoDB.Bson;
  using MongoDB.Bson.Serialization.Attributes;
  using System.Collections.Generic;

  public class Articolo
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRequired]
    public string Titolo { get; set; }
    [BsonRequired]
    public string Testo { get; set; }
    [BsonRequired]
    public AuthInfo AuthInfo { get; set; }
    public ICollection<string> Categorie { get; set; }
    public bool Pubblicato { get; set; }
    public BsonDateTime DataPubblicazione { get; set; }

  }
}
