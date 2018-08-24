using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;

namespace TumblrV2
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesProvider = GetServiceProvider();

            var runner = servicesProvider.GetRequiredService<Worker>();

            runner.Run(args);

            LogManager.Shutdown();
        }

        private static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddTransient<Worker>();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.AddLogging((builder) =>
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog(new NLogProviderOptions
            {
                CaptureMessageTemplates = true,
                CaptureMessageProperties = true
            });

            LogManager.LoadConfiguration("nlog.config");

            return serviceProvider;
        }





        //private const string APIKEY = 
        //    "bwdTHUgVUDcLONiPlS1DEgFRJYHfB2hCbUajHoedom8Mr9TlwJ";

        //private const string BLOGNAME =
        //    "funnyhalloffame.tumblr.com"; //"postcardtimemachine.tumblr.com";

        //static async Task Main(string[] args)
        //{
        //    var fetcher = new Fetcher(APIKEY);

        //    //var posts = await fetcher.GetPostsAsync(BLOGNAME);
        //}

        //private static void PrepDatabase()
        //{
        //    //using (var db = new LiteDatabase(@"Posts.db"))
        //    //{
        //    //    // Get customer collection
        //    //    var customers = db.GetCollection<Customer>("customers");

        //    //    // Create your new customer instance
        //    //    var customer = new Customer
        //    //    {
        //    //        Name = "John Doe",
        //    //        Phones = new string[] { "8000-0000", "9000-0000" },
        //    //        IsActive = true
        //    //    };

        //    //    // Insert new customer document (Id will be auto-incremented)
        //    //    customers.Insert(customer);

        //    //    // Update a document inside a collection
        //    //    customer.Name = "Joana Doe";

        //    //    customers.Update(customer);

        //    //    // Index document using a document property
        //    //    customers.EnsureIndex(x => x.Name);

        //    //    // Use Linq to query documents
        //    //    var results = customers.Find(x => x.Name.StartsWith("Jo"));
        //    //}
        //}
    }
}
