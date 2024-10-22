using Nest;

namespace Elastichsearch.API.Model
{
    public class Product
    {
        [PropertyName("_id")]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime Created {  get; set; }
        public DateTime Updated { get; set; }
        public ProductFeature? Feature { get; set; }
    }
}
