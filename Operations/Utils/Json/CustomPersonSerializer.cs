using System;
using System.Linq;
using System.Text;
using CommonEntities;
using Newtonsoft.Json;

namespace Operations.Utils.Json{
    //There is the trusted library, so I do not test it.
    public class CustomPersonSerializer : ISerializer<Person>
    {
        public string Serialize(Person person)
        {
            return JsonConvert.SerializeObject(person);
        }
    }
}