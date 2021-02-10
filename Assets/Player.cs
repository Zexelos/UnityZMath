using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int MaxHP;
    [SerializeField] float attackRange;
    [SerializeField] Enemy enemy;
    [SerializeField] float radius = 2f;

    bool canAttack;
    int currentHP;

    void Start()
    {
        canAttack = true;
        currentHP = MaxHP;
    }

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
        else if (dotForward > 0 && dotRight > 0)
            Debug.Log("Enemy on the front-right");
        else if (dotForward < 0 && dotRight > 0)
            Debug.Log("Enemy on the back-right");

        if (distance <= radius + enemy.radius)
            Debug.Log($"Collision detected");

        if (canAttack && distance < attackRange)
            Attack();
    }

    void Attack()
    {
        Debug.Log("Player attack");

        enemy.TakeDamage(10);

        StartCoroutine(Cooldown(0.5f));
    }

    IEnumerator Cooldown(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage");

        if (currentHP <= 0)
        {
            Debug.Log($"{gameObject.name} died");
        }
    }
}
