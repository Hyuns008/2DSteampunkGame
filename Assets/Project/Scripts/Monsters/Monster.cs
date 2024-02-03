using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector3 moveVec;

    [Header("������ �߷�")]
    [SerializeField] private float gravity;
    private float verticalVelocity; //������ �߷� ���� ����� ���� ����
    private bool isGround = false; //������ �ƴ��� üũ�� ���� �Լ�

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("���� �̵��ӵ�")] private float moveSpeed;
    [SerializeField, Tooltip("���� �ִ�ü��")] private float maxHp;
    [SerializeField, Tooltip("���� ����ü��")] private float curHp;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        monsterMove();
    }

    /// <summary>
    /// ������ �������� ����ϴ� �Լ�
    /// </summary>
    private void monsterMove()
    {
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
    /// ������ ���� ��ũ��Ʈ���� ���꺤�� ���� �޾ƿ��� ���� ������� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector3 MoveVecReturn()
    {
        return moveVec;
    }
}
