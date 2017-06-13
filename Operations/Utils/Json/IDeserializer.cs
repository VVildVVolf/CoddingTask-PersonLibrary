namespace Operations.Utils.Json{
    public interface IDeserializer<out T>{
        T Deserialize(string json);
    }
}