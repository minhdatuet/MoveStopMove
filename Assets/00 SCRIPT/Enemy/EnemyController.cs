using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState = CONSTANT.PlayerState;

public class EnemyController : PlayerController
{
    [SerializeField] float moveDuration = 3.0f;
    [SerializeField] float idleDuration = 1.0f;

    Vector3 randomDirection;
    bool isMoving = true;

    void Start()
    {
        radiusAttack = attackRange.GetComponent<Renderer>().bounds.size.x * 0.5f;
        Debug.Log(radiusAttack);
        enemyLayer = LayerMask.GetMask("Enemy");
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Di chuyển trong khoảng thời gian moveDuration
            isMoving = true;
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            yield return new WaitForSeconds(moveDuration);

            // Đứng im trong khoảng thời igan idleDuration
            isMoving = false;
            yield return new WaitForSeconds(idleDuration);
        }
    }

    void Update()
    {
        if (isMoving)
        {
            Move();
        }
        CheckForEnemiesAndAttack();
        CheckState();
        _anim.UpdateAnimation(_state);
    }

    public override void Move()
    {
        Vector3 movement = randomDirection * speed * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            transform.position += movement;

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * 100 * Time.deltaTime);
        }
    }

    public override void CheckState()
    {
        if (isMoving)
        {
            _state = PlayerState.RUN;
            canAttack = true;
        }
        else
        {
            _state = PlayerState.IDLE;
        }
    }
}
