using System;
using System.Linq;
using CommonEntities;
using Newtonsoft.Json;

namespace Operations.Utils.Json {
    //There is the trusted library, so I do not test it.
    public class CustomPersonDeserializer : IDeserializer<Person>
    {
        public Person Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<Person>(json);
        }
    }
}