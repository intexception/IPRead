using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Console = Colorful.Console;
using System.Drawing;
using System.Threading;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace IPRead
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "IPReadtest"
            Console.SetWindowSize(50, 35);
            GetData();
        }

        static void PrntAscii()
        {
            string ascii = @"
    _________________               _ 
    |_   _| ___ \ ___ \             | |
      | | | |_/ / |_/ /___  __ _  __| |
      | | |  __/|    // _ \/ _` |/ _` |
     _| |_| |   | |\ \  __/ (_| | (_| |
     \___/\_|   \_| \_\___|\__,_|\__,_|
                                   
     Made by nquantum | https://github.com/intexception/IPRead                               
";
            Console.WriteLine(ascii, Color.CadetBlue);
        }

        public static string localip()
        {
            var hostaddr = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in hostaddr.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception();
        }

        public static System.Collections.Generic.List<String> net_adapters()
        {
            List<String> values = new List<String>();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                values.Add(nic.Name);
            }
            return values;
        }

        static void GetData()
        {
            
            PrntAscii();
            WebClient client = new WebClient();
            string data = client.DownloadString("https://wtfismyip.com/json");
            dynamic Jsonobject = JsonConvert.DeserializeObject<dynamic>(data);

            string ipv4 = Jsonobject.YourFuckingIPAddress;
            string isp = Jsonobject.YourFuckingISP;
            string country = Jsonobject.YourFuckingCountryCode;
            string location = Jsonobject.YourFuckingLocation;
            string hostname = Jsonobject.YourFuckingHostname;
            int hash = Jsonobject.GetHashCode();

            // tab; [    ]

            Console.WriteLine("     PUBLIC DATA");

            Console.WriteLine("     =======================\n");
            
            Console.Write("     IPv4 Address...: ");
            Console.WriteLine(ipv4, Color.SlateBlue);
            Console.Write("     ISP............: ");
            Console.WriteLine(isp, Color.SlateBlue);
            Console.Write("     Country........: ");
            Console.WriteLine(country, Color.SlateBlue);
            Console.Write("     Location.......: ");
            Console.WriteLine(location, Color.SlateBlue);
            Console.Write("     Hostname.......: ");
            Console.WriteLine(hostname, Color.SlateBlue);
            Console.Write("     Hashcode.......: ");
            Console.WriteLine(hash.ToString(), Color.SlateBlue);

            Console.WriteLine("\n\n     PRIVATE DATA");

            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] address = ipHostEntry.AddressList;


            Console.WriteLine("     =======================\n");
            Console.Write("     IPv4 Address....: ");
            Console.WriteLine(address[4].ToString(), Color.SlateBlue);
            Console.Write("     DNS hostname....: ");
            Console.WriteLine(strHostName, Color.SlateBlue);
            Console.Write("     Network adapters: ");
            Console.WriteLine(net_adapters().Count, Color.SlateBlue);
            Console.Write("     Hashcode........: ");
            Console.WriteLine(hash.ToString(), Color.SlateBlue);
            Console.Write("\n     Type anything to get network interfaces: $ ");
            

            Console.ReadLine();
            ShowNetworkInterfaces();
            Console.Write("\n     Type anything to go back: $ ");
            Console.ReadLine();
            GetData();


            
        }

        public static void ShowNetworkInterfaces()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                    computerProperties.HostName, computerProperties.DomainName);
            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return;
            }

            Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Console.WriteLine();
                Console.WriteLine(adapter.Description);
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                Console.WriteLine("  Physical Address ........................ : {0}",
                           adapter.GetPhysicalAddress().ToString());
                Console.WriteLine("  Operational status ...................... : {0}",
                    adapter.OperationalStatus);
                string versions = "";

                // Create a display string for the supported IP versions. 
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    versions = "IPv4";
                }
                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                    {
                        versions += " ";
                    }
                    versions += "IPv6";
                }
                Console.WriteLine("  IP version .............................. : {0}", versions);


                // The following information is not useful for loopback adapters. 
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    continue;
                }
                Console.WriteLine("  DNS suffix .............................. : {0}",
                    properties.DnsSuffix);

                string label;
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                    Console.WriteLine("  MTU...................................... : {0}", ipv4.Mtu);
                    if (ipv4.UsesWins)
                    {

                        IPAddressCollection winsServers = properties.WinsServersAddresses;
                        if (winsServers.Count > 0)
                        {
                            label = "  WINS Servers ............................ :";

                        }
                    }
                }

                Console.WriteLine("  DNS enabled ............................. : {0}",
                    properties.IsDnsEnabled);
                Console.WriteLine("  Dynamically configured DNS .............. : {0}",
                    properties.IsDynamicDnsEnabled);
                Console.WriteLine("  Receive Only ............................ : {0}",
                    adapter.IsReceiveOnly);
                Console.WriteLine("  Multicast ............................... : {0}",
                    adapter.SupportsMulticast);

            }
                Console.WriteLine();
            }

        }
}
