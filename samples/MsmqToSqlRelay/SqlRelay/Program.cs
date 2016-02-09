using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

namespace SqlRelay
{
    class Program
    {

        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {

            #region sqlrelay-config
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EndpointName("SqlRelay");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.DisableFeature<AutoSubscribe>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=PersistenceForSqlTransport;Integrated Security=True");
            #endregion


            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
            try
            {
                Start(endpoint);
            }
            finally
            {
                await endpoint.Stop();
            }
        }


        static void Start(IEndpointInstance busSession)
        {
            Console.WriteLine("\r\nSqlRelay is running - This endpoint will relay all events received to subscribers. Press any key to stop program\r\n");
            Console.ReadKey();
        }

    }
}