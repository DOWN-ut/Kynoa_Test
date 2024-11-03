using UnityEngine;
using Unity.Netcode;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform connectionUI;

    [SerializeField] RectTransform serverUI;
    [SerializeField] TMP_Text serverPlayerCount;

    void Update()
    {
        ManageConnectionUI();
        ManageServerUI();
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
