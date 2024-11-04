using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform connectionUI;
    [SerializeField] TMP_Text broadcastReceiveText;

    [SerializeField] RectTransform serverUI;
    [SerializeField] TMP_Text ipDisplay;
    [SerializeField] TMP_Text serverPlayerCount;
    [SerializeField] TMP_Text serverWaiting;

    [SerializeField] RectTransform scoreUI;
    [SerializeField] TMP_Text team1Score;
    [SerializeField] TMP_Text team2Score;

    [SerializeField] RectTransform loadingScreen;

    [SerializeField] RectTransform welcomeScreen;

    [SerializeField] float loadingMinDuration; float loadingD;

    void Update()
    {
        ManageConnectionUI();
        ManageServerUI();
        ManageScoreUI();
        ManageLoadingScreen();
        ManageWelcomeUI();
    }

    void ManageLoadingScreen()
    {
        if(!NetworkManager.Singleton.IsConnectedClient)
        {
            loadingScreen.gameObject.SetActive(false);
        }
        else
        {
            loadingScreen.gameObject.SetActive(!NetworkConnection.Instance.ServerReady || loadingD > 0);
        }
        loadingD -= Time.deltaTime;
    }

    void BringLoadingScreen()
    {
        loadingD = loadingMinDuration;
    }

    void ManageScoreUI()
    {
        scoreUI.gameObject.SetActive(NetworkConnection.Instance.ServerReady);

        team1Score.text = TeamManager.Instance.GetTeamScore(0).ToString();
        team1Score.color = TeamManager.Instance.GetTeamColor(0);
        team2Score.text = TeamManager.Instance.GetTeamScore(1).ToString();
        team2Score.color = TeamManager.Instance.GetTeamColor(1);
    }

    void ManageServerUI()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (NetworkConnection.Instance.ServerReady)
            {
                serverPlayerCount.gameObject.SetActive(false);
                serverWaiting.gameObject.SetActive(false);
                ipDisplay.gameObject.SetActive(false);
            }
            else
            {
                serverPlayerCount.text = NetworkManager.Singleton.ConnectedClients.Count.ToString();
                ipDisplay.text = NetworkConnection.Instance.LocalIPAddress;
                ipDisplay.gameObject.SetActive(true);
                serverPlayerCount.gameObject.SetActive(true);
                serverWaiting.gameObject.SetActive(true);
            }
        }
        else
        {
            serverUI.gameObject.SetActive(false);
        }
    }

    void ManageConnectionUI()
    {
        if (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsServer)
        {
            connectionUI.gameObject.SetActive(false);
        }
        else
        { 
            UnityTransport up = NetworkManager.Singleton.GetComponent<UnityTransport>();
            connectionUI.gameObject.SetActive(true);
            broadcastReceiveText.text = NetworkConnection.Instance.ClientReceivedServerIP ? 
                ("Server found at : " + up.ConnectionData.Address + ":" + up.ConnectionData.Port) : 
                "Waiting for server ...";
        }
    }

    void ManageWelcomeUI()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            welcomeScreen.gameObject.SetActive(false);
        }
    }

    public void ValidateWelcomeScreen()
    {
        loadingD = loadingMinDuration;
        welcomeScreen.gameObject.SetActive(false);
        NetworkConnection.Instance.ClientReady = true;
    }
}
