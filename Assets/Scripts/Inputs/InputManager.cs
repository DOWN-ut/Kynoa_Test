using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Terresquall;

public class InputManager : MonoBehaviour
{
    PlayerManager localPlayer;

    [SerializeField] VirtualJoystick joystick;

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

        localPlayer.Move(cc.ReadValue<Vector2>().normalized);
    }

    private void Update()
    {
        if(Application.isMobilePlatform)
        {
            if(joystick.GetAxis() != Vector2.zero)
            {
                localPlayer.Move(joystick.GetAxis());
            }
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    public static InputManager Instance { get; private set; }
}
