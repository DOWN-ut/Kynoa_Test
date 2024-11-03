using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] Vector2 arenaSize; public Vector2 ArenaSize { get { return arenaSize; } }
    public Bounds Bounds { get { return new Bounds(transform.position, new Vector3(arenaSize.x, 10, arenaSize.y)); } }

    void Awake()
    {
        Instance = this;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(arenaSize.x, 0, arenaSize.y));
    }

    public static ArenaManager Instance { get; private set; }
}
