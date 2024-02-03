using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector3 moveVec;

    [Header("몬스터의 중력")]
    [SerializeField] private float gravity;
    private float verticalVelocity; //임이의 중력 값을 만들기 위한 변수
    private bool isGround = false; //땅인지 아닌지 체크를 위한 함수

    [Header("몬스터 기본 설정")]
    [SerializeField, Tooltip("몬스터 이동속도")] private float moveSpeed;
    [SerializeField, Tooltip("몬스터 최대체력")] private float maxHp;
    [SerializeField, Tooltip("몬스터 현재체력")] private float curHp;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        monsterMove();
    }

    /// <summary>
    /// 몬스터의 움직임을 담당하는 함수
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
    /// 각자의 몬스터 스크립트에서 무브벡터 값을 받아오기 위해 만들어진 함수
    /// </summary>
    /// <returns></returns>
    public Vector3 MoveVecReturn()
    {
        return moveVec;
    }
}
