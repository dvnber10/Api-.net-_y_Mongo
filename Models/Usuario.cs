using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("Name")]
        public string? Nombre { get; set; }
        public string? Email {get; set;}
        public string? Password {get; set;}
    }
}