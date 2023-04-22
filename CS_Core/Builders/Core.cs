namespace CS_Core
{
    public static class Core
    {
        public static string Serialize(object @object) => System.Text.Json.JsonSerializer.Serialize(@object);
        public static T? Deserialize<T>(string value) => System.Text.Json.JsonSerializer.Deserialize<T>(value);
    }
}
