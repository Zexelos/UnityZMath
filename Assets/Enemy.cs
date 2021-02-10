using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Roam,
    Chase
}

public class Enemy : MonoBehaviour
{
    public float radius = 1f;
    [SerializeField] Player player;
    [SerializeField] float fov;
    [SerializeField] float moveSpeed;
    [SerializeField] float sightRange;
    [SerializeField] int maxHP;
    [SerializeField] float attackRange;
    [SerializeField] List<Transform> path;

    float oneSideFov;
    Transform currentTarget;
    Transform currentPathTarget;
    Vector3 currentTargetDirection;
    float currentTargetDistance;
    State currentState;
    bool canAttack;
    int currentHP;

    void Start()
    {
        oneSideFov = fov / 2;
        currentPathTarget = path[Random.Range(0, path.Count)];
        currentTarget = currentPathTarget;
        currentState = State.Roam;
        canAttack = true;
        currentHP = maxHP;
    }

    void Update()
    {
        Vector3 playerDir = player.transform.position - transform.position;
        float playerDistance = Mathf.Sqrt(playerDir.x * playerDir.x + playerDir.y * playerDir.y + playerDir.z * playerDir.z);
        Vector3 playerDirN = playerDir / playerDistance;
        float dotForward = playerDirN.x * transform.forward.x + playerDirN.y * transform.forward.y + playerDirN.z * transform.forward.z;
        float playerAngle = Mathf.Acos(dotForward) * Mathf.Rad2Deg;

        switch (currentState)
        {
            case State.Roam:
                currentTarget = currentPathTarget;

                currentTargetDirection = currentTarget.position - transform.position;
                currentTargetDistance = Mathf.Sqrt(currentTargetDirection.x * currentTargetDirection.x + currentTargetDirection.y * currentTargetDirection.y + currentTargetDirection.z * currentTargetDirection.z);

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z), moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(new Vector3(currentTargetDirection.x, 0, currentTargetDirection.z));

                if (currentTargetDistance < 2f)
                {
                    currentPathTarget = path[Random.Range(0, path.Count)];
                    currentTarget = currentPathTarget;
                }

                if (playerDistance <= sightRange && playerAngle <= oneSideFov)
                    currentState = State.Chase;

                break;
            case State.Chase:
                currentTarget = player.transform;

                currentTargetDirection = currentTarget.position - transform.position;
                currentTargetDistance = Mathf.Sqrt(currentTargetDirection.x * currentTargetDirection.x + currentTargetDirection.y * currentTargetDirection.y + currentTargetDirection.z * currentTargetDirection.z);

                if (currentTargetDistance < attackRange)
                {
                    if (canAttack)
                        Attack();
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z), moveSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.LookRotation(new Vector3(currentTargetDirection.x, 0, currentTargetDirection.z));
                }

                if (playerDistance > sightRange || playerAngle > oneSideFov)
                    currentState = State.Roam;

                break;
        }

        //Debug.Log($"target: {currentTarget.name}, currDis: {currentTargetDistance}");
        //Debug.Log($"state: {currentState}");
    }

    void Attack()
    {
        Debug.Log("Enemy attack");

        player.TakeDamage(5);

        StartCoroutine(Cooldown(1f));
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
            Debug.Log($"{gameObject.name} died");
    }
}
