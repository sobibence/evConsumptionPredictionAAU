using Npgsql;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class SshTunnler
    {
        static void Init()
        {
            using (var client = new SshClient("210.130.90.110", "root", "pasword"))
            {
                client.Connect();

                if (!client.IsConnected)
                {
                    // Display error
                    Console.WriteLine("Client not connected!");
                }
                else
                {
                    Console.WriteLine("Client connected!");
                }

                var port = new ForwardedPortLocal("127.0.0.1", 15432, "210.130.90.110", 5432);
                client.AddForwardedPort(port);

                port.Start();

                using (var conn = new NpgsqlConnection("Server=127.0.0.1;Database=dbname;Port=15432;User Id=dbuser;Password=dbpassword;"))
                {
                    conn.Open();
                }

                port.Stop();
                client.Disconnect();
            }
        }
    }
}
