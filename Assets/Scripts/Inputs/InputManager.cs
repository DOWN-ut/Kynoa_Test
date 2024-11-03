using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    PlayerManager localPlayer;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLocalPlayer(PlayerManager player)
    {
        localPlayer = player;
    }

    public void Move(CallbackContext cc)
    {
        if(localPlayer == null) return;

        localPlayer.Move(cc.ReadValue<Vector2>());
    }

    public static InputManager Instance { get; private set; }
}
