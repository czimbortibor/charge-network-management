namespace Domain.Entities
{
    public class Vertex
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string PartitionKey { get; set; } = "PartitionKey";
    }
}
