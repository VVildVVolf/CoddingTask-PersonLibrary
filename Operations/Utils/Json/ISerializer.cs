namespace Operations.Utils.Json{
    public interface ISerializer<in T>{
        string Serialize(T person);
    }
}