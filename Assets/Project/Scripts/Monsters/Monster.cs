using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector3 moveVec;

    [Header("몬스터 기본 설정")]
    [SerializeField, Tooltip("몬스터 이동속도")] private float moveSpeed;
    [SerializeField, Tooltip("몬스터 최대체력")] private float maxHp;
    [SerializeField, Tooltip("몬스터 현재체력")] private float curHp;

    [Space]
    [SerializeField, Tooltip("몬스터 랜덤 최소 이동하는 시간변경")] private float randomMoveMinTime;
    [SerializeField, Tooltip("몬스터 랜덤 최대 이동하는 시간변경")] private float randomMoveMaxTime;
    private float randomMoveTime; //랜덤 시간을 받아올 변수
    private float moveTimer; //움직임을 멈춰줄 타이머
    private bool moveOff = false; //움직임을 제어해줄 변수

    [Space]
    [SerializeField, Tooltip("몬스터 랜덤 최소 가만히 있는 시간변경")] private float randomIdleMinTime;
    [SerializeField, Tooltip("몬스터 랜덤 최대 가만히 있는 시간변경")] private float randomIdleMaxTime;
    private float randomIdleTime; //랜덤 시간을 받아올 변수
    private float idleTimer; //제자리에 멈춰줄 타이머
    private bool idleOn = false; //제자리에 있게 해줄 변수

    [Header("체크 콜라이더 설정")]
    [SerializeField] private CircleCollider2D checkColl;
    private bool playerTracking = false; //플레이어를 추적하는지 체크하기 위한 변수
    private bool attackStop = false; //몬스터가 공격할 때 멈추게 해주는 변수

    private void onTriggerCheck(Collider2D collision)
    {
        if (attackStop == true)
        {
            moveVec.x = 0f;
            rigid.velocity = moveVec;
            return;
        }
        else
        {
            moveVec.x = moveSpeed;
            moveVec.y = rigid.velocity.y;
            rigid.velocity = moveVec;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerTracking = true;

            Vector3 playerPos = collision.transform.position - transform.position;

            if (playerPos.x < -1f && transform.localScale.x < 0)
            {
                moveSpeed *= -1;

                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;

                moveVec.x = moveSpeed;
            }
            else if (playerPos.x > 1f && transform.localScale.x > 0)
            {
                moveSpeed *= -1;

                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;

                moveVec.x = moveSpeed;
            }
            else if(playerPos.x >= -1f && playerPos.x <= 1f)
            {
                moveVec.x = 0f;
            }

            moveVec.y = rigid.velocity.y;
            rigid.velocity = moveVec;
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        randomMoveTime = Random.Range(randomMoveMinTime, randomMoveMaxTime);
        randomIdleTime = Random.Range(randomIdleMinTime, randomIdleMaxTime);
    }

    private void Update()
    {
        playerCollCheck();

        if (playerTracking == true || attackStop == true)
        {
            return;
        }

        monsterTimers();
        monsterMove();
    }

    /// <summary>
    /// 플레이어를 체크하기 위한 함수
    /// </summary>
    private void playerCollCheck()
    {
        Collider2D playerColl = Physics2D.OverlapBox(checkColl.bounds.center,
                     checkColl.bounds.size, 0, LayerMask.GetMask("Player"));

        if (playerColl != null)
        {
            onTriggerCheck(playerColl);
        }
        else
        {
            playerTracking = false;
        }
    }

    /// <summary>
    /// 타이머들의 모음
    /// </summary>
    private void monsterTimers()
    {
        if (moveOff == false)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= randomMoveTime)
            {
                idleOn = true;
                randomMoveTime = Random.Range(randomMoveMinTime, randomMoveMaxTime);
                moveTimer = 0f;
                moveOff = true;
            }
        }

        if (idleOn == true)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= randomIdleTime)
            {
                moveOff = false;
                randomIdleTime = Random.Range(randomIdleMinTime, randomIdleMaxTime);
                monsterTurn();
                idleTimer = 0f;
                idleOn = false;
            }
        }
    }

    /// <summary>
    /// 몬스터의 움직임을 담당하는 함수
    /// </summary>
    private void monsterMove()
    {
        if (moveOff == true || idleOn == true || attackStop == true)
        {
            moveVec.x = 0f;
            rigid.velocity = moveVec;
            return;
        }
        else
        {
            moveVec.x = moveSpeed;
            moveVec.y = rigid.velocity.y;
            rigid.velocity = moveVec;
        }

        if (moveVec.x > 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
        else if (moveVec.x < 0 && transform.localScale.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }

        moveVec.x = moveSpeed;
        moveVec.y = rigid.velocity.y;
        rigid.velocity = moveVec;
    }

    /// <summary>
    /// 몬스터의 좌우변경을 담당하는 함수
    /// </summary>
    private void monsterTurn()
    {
        moveSpeed *= -1;

        if (moveVec.x > 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
        else if (moveVec.x < 0 && transform.localScale.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    /// <summary>
    /// 각자의 몬스터 스크립트에서 무브벡터 값을 받아오기 위해 만들어진 함수
    /// </summary>
    /// <returns></returns>
    public Vector3 MoveVecReturn()
    {
        return moveVec;
    }

    public void SetAttackStop(bool _attackstop)
    {
        attackStop = _attackstop;
    }
}
