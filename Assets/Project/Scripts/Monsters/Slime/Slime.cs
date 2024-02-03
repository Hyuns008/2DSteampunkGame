using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Monster monsterSc; //몬스터들의 기본적인 스크립트

    private Animator anim; //슬라임의 애니메이션을 받아올 변수

    [Header("슬라임 기본 설정")]
    [SerializeField, Tooltip("공격 지속 시간")] private float attackTime;
    [SerializeField, Tooltip("화염방사 재사용대기시간")] private float fireCoolTime;
    [SerializeField] private float fireCoolTimer; //화염방사 쿨타이머
    [SerializeField] private bool useFireShot = false; //화염방사를 사용했는지 체크하기 위한 변수
    [SerializeField] private bool createFireShot = false; //화염을 생성했는지 체크하기 위한 변수

    [Header("체크 콜라이더 설정")]
    [SerializeField] private CircleCollider2D checkColl;

    [Header("공격 프리팹 설정")]
    [SerializeField, Tooltip("공격 프리팹")] private GameObject attackPrefab;
    [SerializeField, Tooltip("공격 프리팹이 생성될 위치")] private Transform attackTrs;

    [SerializeField] private bool isAttack; //공격을 했는지 체크하기 위한 변수
    [SerializeField] private bool attackContinuing = false; //공격을 지속중인지 체크
    [SerializeField] private float continueTimer; //공격 지속 시간

    private void onTriggerCheck(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isAttack == false)
            {
                if (useFireShot == false)
                {
                    monsterSc.SetAttackStop(true);
                    attackContinuing = true;
                    anim.SetTrigger("isFire");
                    useFireShot = true;
                    isAttack = true;
                }
            }
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        monsterSc = GetComponent<Monster>();
    }

    private void Update()
    {
        playerCollCheck();
        slimeTimer();
        slimeAnim();
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

    private void slimeTimer()
    {
        if (attackContinuing == true) //화염방사 공격 중 제자리에 있는 시간
        {
            continueTimer += Time.deltaTime;

            if (continueTimer >= 0.4f && createFireShot == false)
            {
                Instantiate(attackPrefab, attackTrs.position, attackTrs.rotation, attackTrs);
                createFireShot = true;
            }
            else if (continueTimer > attackTime)
            {
                monsterSc.SetAttackStop(false);
                isAttack = false;
                continueTimer = 0f;
                attackContinuing = false;
            }
        }

        if (useFireShot == true && attackContinuing == false) //화염방사 쿨타임
        {
            fireCoolTimer += Time.deltaTime;

            if (fireCoolTimer >= fireCoolTime)
            {
                createFireShot = false;
                fireCoolTimer = 0f;
                useFireShot = false;
            }
        }
    }

    /// <summary>
    /// 슬라임의 애니메이션을 담당하는 함수
    /// </summary>
    private void slimeAnim()
    {
        anim.SetInteger("isWalk", (int)monsterSc.MoveVecReturn().x);
        anim.SetBool("isShot", attackContinuing);
    }
}
