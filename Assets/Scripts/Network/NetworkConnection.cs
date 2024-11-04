using Unity.Netcode;
using Unity.Multiplayer;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using Unity.Netcode.Transports.UTP;
using System.Text;
using System.Threading.Tasks;

public class NetworkConnection : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;

    [SerializeField] int listenPort = 47777; // Port for listening for broadcasts
    int serverPort = 7777; // Port for server connection
    string serverIp;

    bool exitedApp;

    public bool ClientReady { get; set; } bool startedClient;
    public bool ClientReceivedServerIP { get { return !string.IsNullOrEmpty(serverIp); } }

    public int PlayerCount { get { return networkManager.ConnectedClientsList.Count; } }
    public bool ServerReady { get { return PlayerCount >= 2; } }

    public string LocalIPAddress { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private void OnApplicationQuit()
    {
        exitedApp = true;
    }

    void Start()
    {
        LocalIPAddress = GetLocalIPAddress();
        Debug.Log("Local IP Address: " + LocalIPAddress);

        if (MultiplayerRolesManager.ActiveMultiplayerRoleMask == MultiplayerRoleFlags.Server)
        {
            networkManager.GetComponent<UnityTransport>().ConnectionData.Address = LocalIPAddress;

            Connect();

            StartBroadcast();
        }
        else
        {
            //StartListen();
        }
    }

    private void Update()
    {
        if (ClientReady && !startedClient)
        {
            startedClient = true;
            StartListen();
        }
    }

    public void Connect()
    {
        switch(MultiplayerRolesManager.ActiveMultiplayerRoleMask)
        {
            case MultiplayerRoleFlags.Server:
                Debug.Log("Starting server");
                networkManager.StartServer();
                break;
            case MultiplayerRoleFlags.Client:
                Debug.Log("Starting client");
                networkManager.StartClient();
                break;
            default:
                Debug.LogError("No multiplayer role selected");
                break;
        }
    }

    private async void StartBroadcast()
    {
        serverPort = networkManager.GetComponent<UnityTransport>().ConnectionData.Port;
        // Start broadcasting on a separate thread to prevent blocking
        await Task.Run(BroadcastServerIP);
    }

    private void BroadcastServerIP()
    {
        using (UdpClient udpClient = new UdpClient())
        {
            udpClient.EnableBroadcast = true;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, listenPort);

            while (true)
            {
                try
                {
                    string serverInfo = $"{GetLocalIPAddress()}:{serverPort}";
                    byte[] data = Encoding.UTF8.GetBytes(serverInfo);

                    udpClient.Send(data, data.Length, endPoint);
                    Debug.Log("Broadcasting server IP: " + serverInfo);

                    // Broadcast every 2 seconds (adjust as necessary)
                    System.Threading.Thread.Sleep(2000);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Broadcast error: " + e.Message);
                    break;
                }

                if (exitedApp) { return; }
            }
        }
    }

    private async void StartListen()
    {
        await Task.Run(ListenForBroadcast);

        // Attempt to connect to the server
        ConnectToServer(serverIp, serverPort);
        return;
    }

    private void ListenForBroadcast()
    {
        using (UdpClient udpClient = new UdpClient(listenPort))
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, listenPort);

            while (true)
            {
                try
                {
                    // Wait for broadcast from server
                    byte[] data = udpClient.Receive(ref endPoint);
                    string serverInfo = Encoding.UTF8.GetString(data);
                    Debug.Log("Received broadcast: " + serverInfo);

                    // Extract IP and port (assuming format is "ip:port")
                    string[] serverDetails = serverInfo.Split(':');
                    serverIp = serverDetails[0];
                    serverPort = int.Parse(serverDetails[1]);
                    return;
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Listening error: " + e.Message);
                    break;
                }

                if (exitedApp) { return; }
            }
        }
    }

    void ConnectToServer(string serverIP, int serverPort)
    {
        networkManager.GetComponent<UnityTransport>().ConnectionData.Address = serverIP;
        networkManager.GetComponent<UnityTransport>().ConnectionData.Port = (ushort)serverPort;

        Connect();
    }

    private string GetLocalIPAddress()
    {
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("Local IP Address Not Found!");
    }

    public static NetworkConnection Instance { get; private set; }
}
