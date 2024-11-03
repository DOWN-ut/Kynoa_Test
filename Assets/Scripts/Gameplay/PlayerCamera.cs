using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    [SerializeField] Transform follow;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float moveRange = 1f;
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] float rotateSpeed = 1.0f;
    [SerializeField] float rotateRange = 10f;
    [SerializeField] AnimationCurve rotateCurve;

    [SerializeField] Vector2 offset;

    public void Setup()
    {
        transform.parent = transform.parent.parent; //Move the camera out of the player object, after it has been turned with it
    }

    void FixedUpdate()
    {
        Vector3 pos = follow.position - (transform.forward * offset.x) + (transform.up * offset.y);
        float posR = Mathf.Clamp(Vector3.Distance(pos, transform.position) / moveRange, 0, 1);
        transform.position = Vector3.MoveTowards(transform.position,pos, moveSpeed * moveCurve.Evaluate(posR));

        /*
        Quaternion rot = Quaternion.LookRotation(follow.forward, Vector3.up); 
        float rotR = Mathf.Clamp(Quaternion.Angle(rot, transform.rotation) / rotateRange, 0, 90) / 90f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation,rot,rotateSpeed * rotateCurve.Evaluate(rotR));
        */
    }

    private void Update()
    {
        gameObject.SetActive(playerManager.IsOwner);
    }
}
