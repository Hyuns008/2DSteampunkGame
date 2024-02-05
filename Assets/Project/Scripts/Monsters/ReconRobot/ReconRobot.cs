using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconRobot : MonoBehaviour
{
    private Monster monsterSc; //���͵��� �⺻���� ��ũ��Ʈ

    private Animator anim; //�������� �ִϸ��̼��� �޾ƿ� ����

    private TrashTrs trashTrs; //�� �� ���� ���� ������Ʈ�� ���� ��ġ

    [Header("�����κ� �⺻ ����")]
    [SerializeField, Tooltip("���� ���� ���ð�")] private float attackTime;
    private float attackTimer; //���� ��� �ð���
    [SerializeField, Tooltip("�̻��� ���ð�")] private float missileDelay;
    private float delayTimer; //�̻��� ������ Ÿ�̸�
    private bool isMissile; //�̻����� �߻��ߴ��� üũ

    [Header("üũ �ݶ��̴� ����")]
    [SerializeField] private CircleCollider2D checkColl;

    [Header("���� ������ ����")]
    [SerializeField, Tooltip("���� ������")] private GameObject attackPrefab;
    [SerializeField, Tooltip("Ÿ�� ������")] private GameObject targetPrefab;
    private Transform playerTrs;

    private bool isAttack; //������ �ߴ��� üũ�ϱ� ���� ����

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
    /// �����κ��� �ִϸ��̼��� ����ϴ� �Լ�
    /// </summary>
    private void reconRobotAnim()
    {
        anim.SetBool("isStop", isMissile);
    }
}
