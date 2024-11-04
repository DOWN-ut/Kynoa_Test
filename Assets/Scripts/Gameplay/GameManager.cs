using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform ballSpawnPoint;

    public GameObject CurrentBall { get; private set; }

    void Awake()
    {
        Instance = this;
    }


    void SpawnBall()
    {
        CurrentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        NetworkObject ballNetworkObject = CurrentBall.GetComponent<NetworkObject>();
        ballNetworkObject.Spawn();
    }

    public void RespawnBall()
    {
        CurrentBall.transform.position = ballSpawnPoint.position;
        CurrentBall.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (IsServer && CurrentBall == null && NetworkConnection.Instance.ServerReady)
        {
            SpawnBall();
        }
    }

    public static GameManager Instance { get; private set; }
}
