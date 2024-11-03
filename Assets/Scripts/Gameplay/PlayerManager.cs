using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] PlayerCamera playerCamera;

    [SerializeField] float positionInterpolationSpeed = 10f;

    Vector2 moveInput;

    void Start()
    {
        if (IsOwner)
        {
            InputManager.Instance.SetLocalPlayer(this);
        }

        int teamId = (int)GetComponent<NetworkObject>().OwnerClientId - 1;//Cause server is 0 

        TeamManager.Instance.JoinTeam(teamId, playerCharacter.gameObject);
        playerCharacter.Setup(teamId, this);
        playerCamera.Setup();
    }


    [Rpc(SendTo.Server)]
    void SendInputRpc(Vector2 input)
    {
        //Debug.Log("Input received on server: " + input);
        ProcessInputOnServer(input);
    }

    void ProcessInputOnServer(Vector2 input)
    {
        // Apply the input on the server’s player position
        playerCharacter.Move(input);

        //Debug.Log("Input processed on server: " + input);

        // After processing, send the updated position back to the client
        SendTransformToClientRpc(playerCharacter.transform.position, playerCharacter.transform.rotation);
    }

    [Rpc(SendTo.NotServer)]
    void SendTransformToClientRpc(Vector3 serverPosition,Quaternion serverRotation)
    {
        //Debug.Log("Position received on client: " + serverPosition);

        // Reconcile the client’s position with the server’s authoritative position
        ReconcilePosition(serverPosition);
        ReconcileRotation(serverRotation);
    }

    void ReconcilePosition(Vector3 serverPosition)
    {
        float threshold = 0.1f;
        if (Vector3.Distance(playerCharacter.transform.position, serverPosition) > threshold)
        {
            playerCharacter.transform.position = Vector3.Lerp(playerCharacter.transform.position, serverPosition,Time.deltaTime * positionInterpolationSpeed); // Correct position
            //ReplayStoredInputs(); // Reapply inputs after correction
        }
    }

    void ReconcileRotation(Quaternion serverRotation)
    {
        float threshold = 0.1f;
        if (Quaternion.Angle(playerCharacter.transform.rotation, serverRotation) > threshold)
        {
            playerCharacter.transform.rotation = serverRotation; // Correct position
            //ReplayStoredInputs(); // Reapply inputs after correction
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            playerCharacter.Move(moveInput);
            SendInputRpc(moveInput);
        }
    }

    public void Move(Vector2 v)
    {
        if (IsOwner)
        {
            moveInput = v;
        }
    }
}
