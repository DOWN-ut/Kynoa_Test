using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration;

    [SerializeField] GameObject[] teamVisuals;

    public Vector2 MoveDirection { get; private set; }
    Vector3 MoveDirection3D { get { return ((Vector3.right * (TeamId == 0 ? -1 : 1) * MoveDirection.y) + (Vector3.forward * (TeamId == 0 ? 1 : -1) * MoveDirection.x)) * moveSpeed; } }
    Vector3 velocity;
    public int TeamId { get; private set; }
    public PlayerManager PlayerManager { get; private set; }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Setup(int team, PlayerManager pm)
    {
        TeamId = team;
        PlayerManager = pm;

        foreach (GameObject teamVisual in teamVisuals)
        {
            teamVisual.GetComponent<Renderer>().sharedMaterial = new Material(teamVisual.GetComponent<Renderer>().sharedMaterial);
            teamVisual.GetComponent<Renderer>().sharedMaterial.color = TeamManager.Instance.GetTeamColor(team);
        }
    }

    public void Move(Vector2 v)
    {
        MoveDirection = v;
    }

    private void FixedUpdate()
    {
        if (PlayerManager.IsOwner || PlayerManager.IsServer)
        {
            Movement();
        }
        Rotation();
    }
    void Movement()
    {
        velocity = Vector3.MoveTowards(velocity, MoveDirection3D, acceleration);

        Rigidbody.linearVelocity = velocity;//if (ArenaManager.Instance.Bounds.Contains(transform.position + velocity))
    }

    void Rotation()
    {
        if (MoveDirection3D != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection3D, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    Rigidbody Rigidbody { get; set; }
}
