using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector3 moveVec;

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("���� �̵��ӵ�")] private float moveSpeed;
    [SerializeField, Tooltip("���� �ִ�ü��")] private float maxHp;
    [SerializeField, Tooltip("���� ����ü��")] private float curHp;

    [Space]
    [SerializeField, Tooltip("���� ���� �ּ� �̵��ϴ� �ð�����")] private float randomMoveMinTime;
    [SerializeField, Tooltip("���� ���� �ִ� �̵��ϴ� �ð�����")] private float randomMoveMaxTime;
    private float randomMoveTime; //���� �ð��� �޾ƿ� ����
    private float moveTimer; //�������� ������ Ÿ�̸�
    private bool moveOff = false; //�������� �������� ����

    [Space]
    [SerializeField, Tooltip("���� ���� �ּ� ������ �ִ� �ð�����")] private float randomIdleMinTime;
    [SerializeField, Tooltip("���� ���� �ִ� ������ �ִ� �ð�����")] private float randomIdleMaxTime;
    private float randomIdleTime; //���� �ð��� �޾ƿ� ����
    private float idleTimer; //���ڸ��� ������ Ÿ�̸�
    private bool idleOn = false; //���ڸ��� �ְ� ���� ����

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        randomMoveTime = Random.Range(randomMoveMinTime, randomMoveMaxTime);
        randomIdleTime = Random.Range(randomIdleMinTime, randomIdleMaxTime);
    }

    private void Update()
    {
        monsterTimers();
        monsterMove();
    }

    /// <summary>
    /// Ÿ�̸ӵ��� ����
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
    /// ������ �������� ����ϴ� �Լ�
    /// </summary>
    private void monsterMove()
    {
        if (moveOff == true || idleOn == true)
        {
            moveVec.x = 0f;
            rigid.velocity = moveVec;
            return;
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
    /// ������ �¿캯���� ����ϴ� �Լ�
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
    /// ������ ���� ��ũ��Ʈ���� ���꺤�� ���� �޾ƿ��� ���� ������� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector3 MoveVecReturn()
    {
        return moveVec;
    }
}
