
namespace Bookstore.WebApi
{
    public class Book
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        /*public string imgSrc { get; set; }*/
        public string category { get; set; }
        public double price { get; set; }
        public bool isFavorite { get; set; } 
    }
}
