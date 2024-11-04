using UnityEngine;
using Unity.Netcode;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform connectionUI;

    [SerializeField] RectTransform serverUI;
    [SerializeField] TMP_Text serverPlayerCount;

    [SerializeField] RectTransform scoreUI;
    [SerializeField] TMP_Text team1Score;
    [SerializeField] TMP_Text team2Score;

    void Update()
    {
        ManageConnectionUI();
        ManageServerUI();
        ManageScoreUI();
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
            serverUI.gameObject.SetActive(true);
            serverPlayerCount.text = NetworkManager.Singleton.ConnectedClients.Count.ToString();
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
            connectionUI.gameObject.SetActive(true);
        }
    }
}
