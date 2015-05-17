namespace MicheleCaggiano.MongoModel
{
  using MongoDB.Bson.Serialization.Attributes;

  public partial class Categoria
  {
    [BsonId]
    [BsonRequired]
    public string Nome { get; set; }
    [BsonRequired]
    public AuthInfo AuthInfo { get; set; }
  }
}
