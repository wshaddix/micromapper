namespace MicroMapper
{
    public class JsonMapper
    {
        private readonly object _destination;
        private readonly string _json;

        public JsonMapper(string json, object destination)
        {
            _json = json;
            _destination = destination;
        }

        public void Execute()
        {
        }
    }
}