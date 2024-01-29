using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public class PlayerData //�÷��̾� �����͸� �����ϱ� ���� Ŭ����
    {
        private int personality;
    }

    [Header("ĳ���� �ΰ� ����")]
    [SerializeField, Tooltip("�ΰ��� �������� ������ �� �ְ� ���ִ� ����")] private bool usePersonalityChange = false;
    [SerializeField, Tooltip("�ΰ�"), Range(0, 2)] private int personality;
    [Space]
    [SerializeField, Tooltip("���ӽ�")] private GameObject james;
    [SerializeField, Tooltip("�")] private GameObject henry;
    [SerializeField, Tooltip("�η�")] private GameObject lauren;
    [Space]
    [SerializeField, Tooltip("�ΰ� ���� �ð� �ּ�")] private float changeTypeMin;
    [SerializeField, Tooltip("�ΰ� ���� �ð� �ִ�")] private float changeTypeMax;
    [Space]
    [SerializeField, Tooltip("Ÿ�̸Ӹ� �����ϱ� ���� ����")] private bool changeTimerStop;
    private bool isPersonality; //�ΰ��� ������ �� �ִ����� Ȯ���ϱ� ���� ����
    [SerializeField, Tooltip("�ΰ��� �����Ű�� Ÿ�̸�")] private float personalityChangeTimer; //�ΰ� ������ ���� Ÿ�̸�
    private int randomPersonality; //�������� ���� �޾� �� ����
    [Space]
    [SerializeField, Tooltip("�ΰ��� ��������� �ð�")] private float personalityDelay; //�ΰ� ������ ���� ������ Ÿ��
    private bool personalityChangeOff = false;
    private float personalityDelayTimer; //�ΰ� ������ ���� ������ Ÿ�̸�
    [SerializeField, Tooltip("�ΰ��� ���� �� ���� �ð�")] private float personalityInvincibilityTimer; //�ΰ� ���� �� ���� �ð�

    private Animator animJames; //���ӽ��� �ִϸ��̼�
    private Animator animHenry; //��� �ִϸ��̼�
    private Animator animLauren; //�η��� �ִϸ��̼�

    private Rigidbody2D rigid; //�÷��̾ ������Ʈ�� ������ٵ� �޾ƿ� ����
    private BoxCollider2D playerBoxColl; //�÷��̾��� �ڽ� �ݶ��̴�
    private Vector3 moveVec; //�÷��̾��� �����ӿ� ���� �ް� �־��� ���� ����
    private RaycastHit2D hit2D; //���� üũ�ϱ� ���� ����ĳ��Ʈ

    [Header("�߷� ����")]
    [SerializeField, Tooltip("�÷��̾��� �߷�")] private float gravity;
    private float gravityVelocity; //y���� �̿��� ����
    private bool isGround; //���� üũ�ϱ� ���� ����

    [Header("�÷��̾� �̵��ӵ� ����")]
    [SerializeField, Tooltip("�÷��̾��� �̵��ӵ�")] private float playerMoveSpeed;

    [Header("�÷��̾� ���� ����")]
    [SerializeField, Tooltip("�÷��̾��� ���� ��")] private float playerJumpPower;
    private bool isJump = false; //������ �ߴ��� üũ�ϱ� ���� ����
    private bool isDoubleJump = false; //������ �ߴ��� üũ�ϱ� ���� ����
    private bool doubleJumpTimerOn = false; //���� ������ �ϱ� ���� Ÿ�̸Ӹ� �۵���Ű�� ����
    private float doubleJumpDelay; //���� ������ �ϱ� ���� ������ �ð�
    private bool jumpAnim = false; //���� �ִϸ��̼��� �����Ű�� ���� ����

    [Header("�÷��̾� �뽬 ����")]
    [SerializeField, Tooltip("�÷��̾��� �뽬 ��")] private float playerDashPower;
    [SerializeField, Tooltip("�÷��̾��� �뽬 ��Ÿ��")] private float playerDashCoolTime;
    private bool isDash = false; //�뽬�� ����ߴ��� üũ�ϱ� ���� ����
    private bool dashCoolTimerOn = false; //�뽬�� ��� �� ��Ÿ���� ��������ִ� ����
    private float dashCoolTimer; //�뽬 �� Ÿ�̸�
    [SerializeField, Tooltip("�뽬�� ���ӵǴ� �ð�")] private float dashDuration;
    private bool dashDurationTimerOn; //�뽬�� ���ӵǴ� Ÿ�̸Ӹ� ��������ִ� ����
    private float dashDurationTimer; //�뽬�� ���ӵǴ� Ÿ�̸�

    [Header("���ӽ� �⺻ ����")]
    [SerializeField, Tooltip("���ӽ��� ���ݷ�")] private float jamesDamage;

    [Header("� �⺻ ����")]
    [SerializeField, Tooltip("��� ���ݷ�")] private float henryDamage;

    [Header("�η� �⺻ ����")]
    [SerializeField, Tooltip("�η��� ���ݷ�")] private float laurenDamage;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerBoxColl = GetComponent<BoxCollider2D>();

        personalityChangeTimer = 300.0f;
        dashCoolTimer = playerDashCoolTime;
    }

    private void Start()
    {
        animJames = james.GetComponent<Animator>();
        animHenry = henry.GetComponent<Animator>();
        animLauren = lauren.GetComponent<Animator>();
    }

    private void Update()
    {
        playerTimers();
        playerGroundCheck();
        playerMove();
        playerDash();
        playerGravity();
        playerPersonalityChange();
        playerAnimation();
    }

    /// <summary>
    /// �÷��̾�� ����Ǵ� Ÿ�̸��� ����
    /// </summary>
    private void playerTimers()
    {
        if (isPersonality == false && changeTimerStop == false) //�ΰ��� �����ϱ� ���� Ÿ�̸�
        {
            personalityChangeTimer -= Time.deltaTime;
            if (personalityChangeTimer <= 0.0f)
            {
                float randomTimer = Random.Range(changeTypeMin, changeTypeMax);
                personalityChangeTimer = randomTimer;
                personalityChangeOff = true;
                isPersonality = true;
            }
        }

        if (personalityChangeOff == true)
        {
            personalityDelayTimer += Time.deltaTime;
            if (personalityDelayTimer >= personalityDelay)
            {
                personalityDelayTimer = 0.0f;
                personalityChangeOff = false;
            }
        }

        if (doubleJumpTimerOn == true && isJump == false) //���������� �ϱ� ���� Ÿ�̸�
        {
            doubleJumpDelay += Time.deltaTime;
            if (doubleJumpDelay >= 0.2f)
            {
                isDoubleJump = true;
                doubleJumpDelay = 0.0f;
            }
        }

        if (dashCoolTimerOn == true) //�뽬 �� Ÿ�̸�
        {
            dashCoolTimer -= Time.deltaTime;
            if (dashCoolTimer <= 0.0f)
            {
                dashCoolTimer = playerDashCoolTime;
                dashCoolTimerOn = false;
            }
        }

        if (dashDurationTimerOn == true) //�뽬�� ���ӵǴ� Ÿ�̸�
        {
            dashDurationTimer += Time.deltaTime;
            if (dashDurationTimer >= dashDuration)
            {
                isDash = false;
                dashDurationTimer = 0.0f;
                dashDurationTimerOn = false;
            }
        }
    }

    /// <summary>
    /// ���� üũ�ϱ� ���� �Լ�
    /// </summary>
    private void playerGroundCheck()
    {
        isGround = false;
        doubleJumpTimerOn = true;

        if (gravityVelocity > 0.0f)
        {
            return;
        }

        hit2D = Physics2D.BoxCast(playerBoxColl.bounds.center, playerBoxColl.bounds.size, 
            0.0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground")); //�÷��̾��� �ڽ� �ݶ��̴� ũ�⸸ŭ�� ����ĳ��Ʈ�� ����

        if (hit2D.transform != null &&
            hit2D.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) //ĳ��Ʈ�� ���� Ʈ�������� null�� �ƴϰ�, ���̾ Ground�� �۵�
        {
            isGround = true; //�ڽ��ɽ�Ʈ�� ���� ������Ʈ�� Ground���̾�� �� üũ 
            isJump = false; //�ڽ��ɽ�Ʈ�� ���� ������Ʈ�� Ground���̾�� ���� üũ 
            isDoubleJump = false; //�ڽ��ɽ�Ʈ�� ���� ������Ʈ�� Ground���̾�� �������� üũ 
            doubleJumpTimerOn = false; //�ڽ��ɽ�Ʈ�� ���� ������Ʈ�� Ground���̾�� �������� Ÿ�̸� üũ 
            jumpAnim = false; //�ڽ��ɽ�Ʈ�� ���� ������Ʈ�� Ground���̾�� ���� �ִϸ��̼� üũ 
        }
    }

    /// <summary>
    /// �÷��̾��� �⺻���� �������� ����ϴ� �Լ�
    /// </summary>
    private void playerMove()
    {
        if (personalityChangeOff == true)
        {
            moveVec.x = 0;
            rigid.velocity = moveVec;
            return;
        }

        if (isDash == true)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            moveVec.x = 0.0f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveVec.x = -playerMoveSpeed;
            if (moveVec.x < 0.0f && transform.localScale.x < 0.0f)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1.0f;
                transform.localScale = scale;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveVec.x = playerMoveSpeed;
            if (moveVec.x > 0.0f && transform.localScale.x > 0.0f)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1.0f;
                transform.localScale = scale;
            }
        }
        else
        {
            moveVec.x = 0.0f;
        }

        rigid.velocity = moveVec;
    }

    /// <summary>
    /// �÷��̾��� ������ ����ϴ� �Լ�
    /// </summary>
    private void playerJump()
    {
        if (personalityChangeOff == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround == true && isJump == false)
            {
                gravityVelocity = playerJumpPower;
                doubleJumpTimerOn = true;
                jumpAnim = true;
            }
            else if (isGround == false && isJump == false && isDoubleJump == true)
            {
                gravityVelocity = playerJumpPower;
                isJump = true;
                isDoubleJump = false;
                jumpAnim = true;
            }
        }
    }

    /// <summary>
    /// �÷��̾ �뽬�� �ϱ� ���� �Լ�
    /// </summary>
    private void playerDash()
    {
        if (personalityChangeOff == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCoolTimerOn == false)
        {
            isDash = true;

            dashDurationTimerOn = true;

            gravityVelocity = 0;

            if (transform.localScale.x < 0)
            {
                rigid.velocity = new Vector3(playerDashPower, 0.0f, 0.0f);
            }
            else if (transform.localScale.x > 0)
            {
                rigid.velocity = new Vector3(-playerDashPower, 0.0f, 0.0f);
            }

            dashCoolTimerOn = true;
        }
    }

    /// <summary>
    /// �÷��̾��� �߷��� ����ϴ� �Լ�
    /// </summary>
    private void playerGravity()
    {
        if (isDash == true)
        {
            return;
        }

        if (isGround == false)
        {
            gravityVelocity -= gravity * Time.deltaTime * 2.0f;

            if (gravityVelocity > gravity) //�߷��� �޴� ���� ���� ���������� �Ѿ�� ����
            {
                gravityVelocity = gravity;
            }
        }
        else
        {
            gravityVelocity = -1.0f;
        }

        playerJump();

        moveVec.y = gravityVelocity;
        rigid.velocity = moveVec;
    }

    /// <summary>
    /// �ΰ��� �������ִ� �Լ�
    /// </summary>
    private void playerPersonalityChange()
    {
        if (isPersonality == true && personalityChangeOff == false)
        {
            if (james.activeSelf == true) //���ӽ� ������Ʈ�� Ȱ��ȭ���̸� ��� �η� �� ���� Ȱ��ȭ
            {
                int ranPersonality = Random.Range(1, 3);
                randomPersonality = ranPersonality;
            }
            else if (henry.activeSelf == true) //� ������Ʈ�� Ȱ��ȭ���̸� ���ӽ��� �η� �� ���� Ȱ��ȭ
            {
                int ranPersonality = Random.Range(0, 2);
                if (ranPersonality == 0)
                {
                    randomPersonality = 0;
                }
                else if (ranPersonality == 1)
                {
                    randomPersonality = 2;
                }
            }
            else if (lauren.activeSelf == true) //�η� ������Ʈ�� Ȱ��ȭ���̸� ���ӽ��� � �� ���� Ȱ��ȭ
            {
                int ranPersonality = Random.Range(0, 2);
                randomPersonality = ranPersonality;
            }

            if (randomPersonality == 0 && james.activeSelf == false)
            {
                henry.SetActive(false);
                lauren.SetActive(false);
                james.SetActive(true);
                personality = 0;
                isPersonality = false;
            }
            else if (randomPersonality == 1 && henry.activeSelf == false)
            {
                james.SetActive(false);
                lauren.SetActive(false);
                henry.SetActive(true);
                personality = 1;
                isPersonality = false;
            }
            else if (randomPersonality == 2 && lauren.activeSelf == false)
            {
                henry.SetActive(false);
                james.SetActive(false);
                lauren.SetActive(true);
                personality = 2;
                isPersonality = false;
            }
        }

        if (usePersonalityChange == true)
        {
            if (personality == 0)
            {
                if (james.activeSelf == true)
                {
                    return;
                }

                henry.SetActive(false);
                lauren.SetActive(false);
                james.SetActive(true);
            }
            else if (personality == 1)
            {
                if (henry.activeSelf == true)
                {
                    return;
                }

                james.SetActive(false);
                lauren.SetActive(false);
                henry.SetActive(true);
            }
            else if (personality == 2)
            {
                if (lauren.activeSelf == true)
                {
                    return;
                }

                henry.SetActive(false);
                james.SetActive(false);
                lauren.SetActive(true);
            }
        }
    }

    /// <summary>
    /// �÷��̾��� �ִϸ��̼��� ����ϴ� �Լ�
    /// </summary>
    private void playerAnimation()
    {
        if (james.activeSelf == true)
        {
            //���ӽ� �ִϸ��̼�
            animJames.SetInteger("isWalk", (int)moveVec.x);
            animJames.SetBool("isJump", jumpAnim);
            animJames.SetBool("isGround", isGround);
            animJames.SetBool("isDash", isDash);
        }
        else if (henry.activeSelf == true)
        {
            //� �ִϸ��̼�
            animHenry.SetInteger("isWalk", (int)moveVec.x);
            animHenry.SetBool("isJump", jumpAnim);
            animHenry.SetBool("isGround", isGround);
            animHenry.SetBool("isDash", isDash);
        }
        else if (lauren.activeSelf == true)
        {
            //�η� �ִϸ��̼�
            animLauren.SetInteger("isWalk", (int)moveVec.x);
            animLauren.SetBool("isJump", jumpAnim);
            animLauren.SetBool("isGround", isGround);
            animLauren.SetBool("isDash", isDash);
        }
    }
}
