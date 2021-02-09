using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float fov;
    [SerializeField] float moveSpeed;
    [SerializeField] float sightRange;
    public float radius = 1f;

    float oneSideFov;

    private void Start()
    {
        oneSideFov = fov / 2;
    }

    void Update()
    {
        Vector3 playerDir = player.position - transform.position;
        float distance = Mathf.Sqrt(playerDir.x * playerDir.x + playerDir.y * playerDir.y + playerDir.z * playerDir.z);
        Vector3 playerDirN = playerDir / distance;
        float dotForward = playerDirN.x * transform.forward.x + playerDirN.y * transform.forward.y + playerDirN.z * transform.forward.z;
        float angle = Mathf.Acos(dotForward) * Mathf.Rad2Deg;

        if (angle < oneSideFov && distance < sightRange)
        {
            Debug.Log("Chasing");
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(playerDir);
        }
    }
}
