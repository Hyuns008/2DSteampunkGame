using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("카메라 속도")]
    [SerializeField] private float cameraSpeed;

    [Space]
    [SerializeField] private GameObject player;
    private Vector3 playerPos;
    private Vector3 myPos;

    private void Start()
    {
        myPos.z = -transform.position.z;
        transform.position = myPos;
    }

    private void FixedUpdate()
    {
        playerFollow();
    }

    private void playerFollow()
    {
        if (player == null)
        {
            return;
        }

        playerPos = player.transform.position;
        player.transform.position = playerPos;

        Vector3 vecDestination = transform.position;
        vecDestination.x = Mathf.Lerp(transform.position.x, playerPos.x, cameraSpeed * Time.deltaTime);
        vecDestination.y = Mathf.Lerp(transform.position.y, playerPos.y + 1.0f, cameraSpeed * Time.deltaTime);
        vecDestination.z = -10.0f;
        transform.position = vecDestination;
    }
}
