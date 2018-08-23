using System.Threading.Tasks;

namespace TumblrV2
{
    class Program
    {
        private const string APIKEY = 
            "bwdTHUgVUDcLONiPlS1DEgFRJYHfB2hCbUajHoedom8Mr9TlwJ";

        private const string BLOGNAME =
            "funnyhalloffame.tumblr.com"; //"postcardtimemachine.tumblr.com";

        static async Task Main(string[] args)
        {
            var fetcher = new Fetcher(APIKEY);

            var posts = await fetcher.GetPostsAsync(BLOGNAME);
        }

        private static void PrepDatabase()
        {
            //using (var db = new LiteDatabase(@"Posts.db"))
            //{
            //    // Get customer collection
            //    var customers = db.GetCollection<Customer>("customers");

            //    // Create your new customer instance
            //    var customer = new Customer
            //    {
            //        Name = "John Doe",
            //        Phones = new string[] { "8000-0000", "9000-0000" },
            //        IsActive = true
            //    };

            //    // Insert new customer document (Id will be auto-incremented)
            //    customers.Insert(customer);

            //    // Update a document inside a collection
            //    customer.Name = "Joana Doe";

            //    customers.Update(customer);

            //    // Index document using a document property
            //    customers.EnsureIndex(x => x.Name);

            //    // Use Linq to query documents
            //    var results = customers.Find(x => x.Name.StartsWith("Jo"));
            //}
        }
    }
}
