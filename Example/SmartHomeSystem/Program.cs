using WebInterface.API;
using WebInterface.Response;
using WebInterface.HTTP;
using WebInterface.IO;

using System;

namespace SmartHomeSystem
{
    class Program
    {
        public static string serverName = "MADSC";

        public static Server server1;
        public static Server server2;
        public static Server server3;

        static void Main()
        {

            try
            {
                Console.WriteLine("Loading Config");

                ConfigManager.LoadSettings();
                ConfigManager.GetSettingInt("test");

                Console.Write("Initiating Response Interfaces");

                ResponseManager.InitiateResponseInterfaces();

                AccessManager.LoadFileAccessSettings();

                server1 = new Server();
                server2 = new Server();
                server3 = new Server();


                server1.Start(80, 4, serverName);

                server2.Start(250, 4, serverName);

                server3.Start(420, 4, serverName);


                while (true) { }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            while (true) { }
        }



    }
}

