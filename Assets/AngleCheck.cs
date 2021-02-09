using UnityEngine;

public class AngleCheck : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] float radius = 2f;

    void Update()
    {
        Vector3 enemyDir = enemy.transform.position - transform.position;
        float distance = Mathf.Sqrt(enemyDir.x * enemyDir.x + enemyDir.y * enemyDir.y + enemyDir.z * enemyDir.z);
        Vector3 enemyDirN = enemyDir / distance;
        float dotForward = enemyDirN.x * transform.forward.x + enemyDirN.y * transform.forward.y + enemyDirN.z * transform.forward.z;
        float dotRight = enemyDirN.x * transform.right.x + enemyDirN.y * transform.right.y + enemyDirN.z * transform.right.z;

        float angle = Mathf.Acos(dotForward) * Mathf.Rad2Deg;
        Debug.Log($"Angle: {angle}");

        if (dotForward < 0 && dotRight < 0)
            Debug.Log("Enemy on the back-left");
        else if (dotForward > 0 && dotRight < 0)
            Debug.Log("Enemy on the front-left");
        else if(dotForward > 0 && dotRight > 0)
            Debug.Log("Enemy on the front-right");
        else if(dotForward < 0 && dotRight > 0)
            Debug.Log("Enemy on the back-right");

        if (distance <= radius + enemy.radius)
            Debug.Log($"Collision derected");
    }
}
