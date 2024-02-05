using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconRobot : MonoBehaviour
{
    private Monster monsterSc; //몬스터들의 기본적인 스크립트

    private Animator anim; //슬라임의 애니메이션을 받아올 변수

    private TrashTrs trashTrs; //한 번 쓰고 버릴 오브젝트를 담을 위치

    [Header("정찰로봇 기본 설정")]
    [SerializeField, Tooltip("공격 재사용 대기시간")] private float attackTime;
    private float attackTimer; //공격 대기 시간초
    [SerializeField, Tooltip("미사일 대기시간")] private float missileDelay;
    private float delayTimer; //미사일 딜레이 타이머
    private bool isMissile; //미사일을 발사했는지 체크

    [Header("체크 콜라이더 설정")]
    [SerializeField] private CircleCollider2D checkColl;

    [Header("공격 프리팹 설정")]
    [SerializeField, Tooltip("공격 프리팹")] private GameObject attackPrefab;
    [SerializeField, Tooltip("타겟 프리팹")] private GameObject targetPrefab;
    private Transform playerTrs;

    private bool isAttack; //공격을 했는지 체크하기 위한 변수

    private bool missile1 = false;
    private bool missile2 = false;
    private bool missile3 = false;
    private bool missile4 = false;

    private void onTriggerCheck(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isAttack == false)
            {
                playerTrs = collision.transform;
                monsterSc.SetAttackStop(true);
                anim.SetTrigger("isAttack");
                Instantiate(targetPrefab, playerTrs.position + new Vector3(0f, 0.2f, 0f),
                    playerTrs.rotation, playerTrs);
                isMissile = true;
                isAttack = true;
            }
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        monsterSc = GetComponent<Monster>();
    }

    private void Start()
    {
        trashTrs = TrashTrs.instance;
    }

    private void Update()
    {
        playerCollCheck();
        reconRobotTimer();
        reconRobotAnim();
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
    }

    private void reconRobotTimer()
    {
        if (isAttack == true && isMissile == false)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackTime)
            {
                missile1 = false;
                missile2 = false;
                missile3 = false;
                missile4 = false;
                attackTimer = 0f;
                isAttack = false;
            }
        }

        if (isMissile == true)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= 0.5f && missile1 == false)
            {
                Instantiate(attackPrefab, playerTrs.position + new Vector3(0f, 10f, 0f),
                   Quaternion.Euler(0f, 0f, 90f), trashTrs.transform);
                missile1 = true;
            }
            else if (delayTimer >= 1f && missile2 == false)
            {
                Instantiate(attackPrefab, playerTrs.position + new Vector3(0f, 10f, 0f),
   Quaternion.Euler(0f, 0f, 90f), trashTrs.transform);
                missile2 = true;
            }
            else if (delayTimer >= 1.5f && missile3 == false)
            {
                Instantiate(attackPrefab, playerTrs.position + new Vector3(0f, 10f, 0f),
   Quaternion.Euler(0f, 0f, 90f), trashTrs.transform);
                missile3 = true;
            }
            else if (delayTimer >= missileDelay && missile4 == false)
            {
                Instantiate(attackPrefab, playerTrs.position + new Vector3(0f, 10f, 0f),
   Quaternion.Euler(0f, 0f, 90f), trashTrs.transform);
                monsterSc.SetAttackStop(false);
                missile4 = true;
                delayTimer = 0f;
                isMissile = false;
            }
        }
    }

    /// <summary>
    /// 정찰로봇의 애니메이션을 담당하는 함수
    /// </summary>
    private void reconRobotAnim()
    {
        anim.SetBool("isStop", isMissile);
    }
}
