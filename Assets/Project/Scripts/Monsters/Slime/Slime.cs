using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Monster monsterSc; //���͵��� �⺻���� ��ũ��Ʈ

    private Animator anim; //�������� �ִϸ��̼��� �޾ƿ� ����

    [Header("������ �⺻ ����")]
    [SerializeField, Tooltip("���� ���� �ð�")] private float attackTime;
    [SerializeField, Tooltip("ȭ����� ������ð�")] private float fireCoolTime;
    [SerializeField] private float fireCoolTimer; //ȭ����� ��Ÿ�̸�
    [SerializeField] private bool useFireShot = false; //ȭ����縦 ����ߴ��� üũ�ϱ� ���� ����
    [SerializeField] private bool createFireShot = false; //ȭ���� �����ߴ��� üũ�ϱ� ���� ����

    [Header("üũ �ݶ��̴� ����")]
    [SerializeField] private CircleCollider2D checkColl;

    [Header("���� ������ ����")]
    [SerializeField, Tooltip("���� ������")] private GameObject attackPrefab;
    [SerializeField, Tooltip("���� �������� ������ ��ġ")] private Transform attackTrs;

    [SerializeField] private bool isAttack; //������ �ߴ��� üũ�ϱ� ���� ����
    [SerializeField] private bool attackContinuing = false; //������ ���������� üũ
    [SerializeField] private float continueTimer; //���� ���� �ð�

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
    /// �÷��̾ üũ�ϱ� ���� �Լ�
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
        if (attackContinuing == true) //ȭ����� ���� �� ���ڸ��� �ִ� �ð�
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

        if (useFireShot == true && attackContinuing == false) //ȭ����� ��Ÿ��
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
    /// �������� �ִϸ��̼��� ����ϴ� �Լ�
    /// </summary>
    private void slimeAnim()
    {
        anim.SetInteger("isWalk", (int)monsterSc.MoveVecReturn().x);
        anim.SetBool("isShot", attackContinuing);
    }
}
