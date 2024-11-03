using UnityEngine;
using Unity.Netcode;

public class ServerCamera : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] Transform camera;
    void Update()
    {
        if (networkManager.IsClient)
        {
            camera.gameObject.SetActive(false);
        }
        else 
        {
            camera.gameObject.SetActive(true);
        }
    }
}
