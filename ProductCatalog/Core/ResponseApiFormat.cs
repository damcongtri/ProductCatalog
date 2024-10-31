namespace ProductCatalog.Core
{
    public class ResponseApiFormat
    {
        public int statusCode { get; set; }
        public string message { get; set; }

        public object data { get; set; }
        public bool success { get; set; }

        public List<string> errorMessages { get; set; } = [];
    }
}
