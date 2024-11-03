using Unity.Netcode;
using Unity.Multiplayer;
using UnityEngine;

public class NetworkConnection : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    
    public int PlayerCount { get { return networkManager.ConnectedClientsList.Count; } }
    public bool ServerReady { get { return PlayerCount >= 2; } }

    void Start()
    {
        if (MultiplayerRolesManager.ActiveMultiplayerRoleMask == MultiplayerRoleFlags.Server)
        {
            Connect();
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
}
