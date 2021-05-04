namespace Application.Groups.Models
{
    public class GremlinResult<TProperties>
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }

        public TProperties Properties { get; set; }
    }
}
