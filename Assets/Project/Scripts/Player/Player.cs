using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public class PlayerData //플레이어 데이터를 저장하기 위한 클래스
    {
        private int personality;
    }

    [Header("캐릭터 인격 설정")]
    [SerializeField, Tooltip("인격을 수동으로 변경할 수 있게 해주는 변수")] private bool usePersonalityChange = false;
    [SerializeField, Tooltip("인격"), Range(0, 2)] private int personality;
    [Space]
    [SerializeField, Tooltip("제임스")] private GameObject james;
    [SerializeField, Tooltip("헨리")] private GameObject henry;
    [SerializeField, Tooltip("로렌")] private GameObject lauren;
    [Space]
    [SerializeField, Tooltip("인격 변경 시간 최소")] private float changeTypeMin;
    [SerializeField, Tooltip("인격 변경 시간 최대")] private float changeTypeMax;
    [Space]
    [SerializeField, Tooltip("타이머를 제어하기 위한 변수")] private bool changeTimerStop;
    private bool isPersonality; //인격을 변경할 수 있는지를 확인하기 위한 변수
    [SerializeField, Tooltip("인격을 변경시키는 타이머")] private float personalityChangeTimer; //인격 변경을 위한 타이머
    private int randomPersonality; //랜덤으로 값을 받아 올 변수
    [Space]
    [SerializeField, Tooltip("인격을 변경딜레이 시간")] private float personalityDelay; //인격 변경을 위한 딜레이 타임
    private bool personalityChangeOff = false;
    private float personalityDelayTimer; //인격 변경을 위한 딜레이 타이머
    [SerializeField, Tooltip("인격을 변경 중 무적 시간")] private float personalityInvincibilityTimer; //인격 변경 시 무적 시간

    private Animator animJames; //제임스의 애니메이션
    private Animator animHenry; //헨리의 애니메이션
    private Animator animLauren; //로렌의 애니메이션

    private Rigidbody2D rigid; //플레이어에 컴포넌트된 리지드바디를 받아올 변수
    private BoxCollider2D playerBoxColl; //플레이어의 박스 콜라이더
    private Vector3 moveVec; //플레이어의 움직임에 값을 받고 넣어줄 벡터 변수
    private RaycastHit2D hit2D; //땅을 체크하기 위한 레이캐스트

    [Header("중력 설정")]
    [SerializeField, Tooltip("플레이어의 중력")] private float gravity;
    private float gravityVelocity; //y값을 이용할 변수
    private bool isGround; //땅을 체크하기 위한 변수

    [Header("플레이어 이동속도 설정")]
    [SerializeField, Tooltip("플레이어의 이동속도")] private float playerMoveSpeed;

    [Header("플레이어 점프 설정")]
    [SerializeField, Tooltip("플레이어의 점프 힘")] private float playerJumpPower;
    private bool isJump = false; //점프를 했는지 체크하기 위한 변수
    private bool isDoubleJump = false; //점프를 했는지 체크하기 위한 변수
    private bool doubleJumpTimerOn = false; //더블 점프를 하기 위한 타이머를 작동시키는 변수
    private float doubleJumpDelay; //더블 점프를 하기 위한 딜레이 시간
    private bool jumpAnim = false; //점프 애니메이션을 실행시키기 위한 변수

    [Header("플레이어 대쉬 설정")]
    [SerializeField, Tooltip("플레이어의 대쉬 힘")] private float playerDashPower;
    [SerializeField, Tooltip("플레이어의 대쉬 쿨타임")] private float playerDashCoolTime;
    private bool isDash = false; //대쉬를 사용했는지 체크하기 위한 변수
    private bool dashCoolTimerOn = false; //대쉬를 사용 후 쿨타임을 실행시켜주는 변수
    private float dashCoolTimer; //대쉬 쿨 타이머
    [SerializeField, Tooltip("대쉬가 지속되는 시간")] private float dashDuration;
    private bool dashDurationTimerOn; //대쉬가 지속되는 타이머를 실행시켜주는 변수
    private float dashDurationTimer; //대쉬가 지속되는 타이머

    [Header("제임스 기본 설정")]
    [SerializeField, Tooltip("제임스의 공격력")] private float jamesDamage;

    [Header("헨리 기본 설정")]
    [SerializeField, Tooltip("헨리의 공격력")] private float henryDamage;

    [Header("로렌 기본 설정")]
    [SerializeField, Tooltip("로렌의 공격력")] private float laurenDamage;

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
    /// 플레이어에게 적용되는 타이머의 모음
    /// </summary>
    private void playerTimers()
    {
        if (isPersonality == false && changeTimerStop == false) //인격을 변경하기 위한 타이머
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

        if (doubleJumpTimerOn == true && isJump == false) //더블점프를 하기 위한 타이머
        {
            doubleJumpDelay += Time.deltaTime;
            if (doubleJumpDelay >= 0.2f)
            {
                isDoubleJump = true;
                doubleJumpDelay = 0.0f;
            }
        }

        if (dashCoolTimerOn == true) //대쉬 쿨 타이머
        {
            dashCoolTimer -= Time.deltaTime;
            if (dashCoolTimer <= 0.0f)
            {
                dashCoolTimer = playerDashCoolTime;
                dashCoolTimerOn = false;
            }
        }

        if (dashDurationTimerOn == true) //대쉬가 지속되는 타이머
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
    /// 땅을 체크하기 위한 함수
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
            0.0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground")); //플레이어의 박스 콜라이더 크기만큼의 레이캐스트를 가짐

        if (hit2D.transform != null &&
            hit2D.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) //캐스트에 닿은 트랜스폼이 null이 아니고, 레이어가 Ground면 작동
        {
            isGround = true; //박스케스트에 닿은 오브젝트가 Ground레이어면 땅 체크 
            isJump = false; //박스케스트에 닿은 오브젝트가 Ground레이어면 점프 체크 
            isDoubleJump = false; //박스케스트에 닿은 오브젝트가 Ground레이어면 더블점프 체크 
            doubleJumpTimerOn = false; //박스케스트에 닿은 오브젝트가 Ground레이어면 더블점프 타이머 체크 
            jumpAnim = false; //박스케스트에 닿은 오브젝트가 Ground레이어면 점프 애니메이션 체크 
        }
    }

    /// <summary>
    /// 플레이어의 기본적인 움직임을 담당하는 함수
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
    /// 플레이어의 점프를 담당하는 함수
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
    /// 플레이어가 대쉬를 하기 위한 함수
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
    /// 플레이어의 중력을 담당하는 함수
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

            if (gravityVelocity > gravity) //중력을 받는 변수 값이 일정수준을 넘어가면 조정
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
    /// 인격을 변경해주는 함수
    /// </summary>
    private void playerPersonalityChange()
    {
        if (isPersonality == true && personalityChangeOff == false)
        {
            if (james.activeSelf == true) //제임스 오브젝트가 활성화중이면 헨리와 로렌 중 랜덤 활성화
            {
                int ranPersonality = Random.Range(1, 3);
                randomPersonality = ranPersonality;
            }
            else if (henry.activeSelf == true) //헨리 오브젝트가 활성화중이면 제임스와 로렌 중 랜덤 활성화
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
            else if (lauren.activeSelf == true) //로렌 오브젝트가 활성화중이면 제임스와 헨리 중 랜덤 활성화
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
    /// 플레이어의 애니메이션을 담당하는 함수
    /// </summary>
    private void playerAnimation()
    {
        if (james.activeSelf == true)
        {
            //제임스 애니메이션
            animJames.SetInteger("isWalk", (int)moveVec.x);
            animJames.SetBool("isJump", jumpAnim);
            animJames.SetBool("isGround", isGround);
            animJames.SetBool("isDash", isDash);
        }
        else if (henry.activeSelf == true)
        {
            //헨리 애니메이션
            animHenry.SetInteger("isWalk", (int)moveVec.x);
            animHenry.SetBool("isJump", jumpAnim);
            animHenry.SetBool("isGround", isGround);
            animHenry.SetBool("isDash", isDash);
        }
        else if (lauren.activeSelf == true)
        {
            //로렌 애니메이션
            animLauren.SetInteger("isWalk", (int)moveVec.x);
            animLauren.SetBool("isJump", jumpAnim);
            animLauren.SetBool("isGround", isGround);
            animLauren.SetBool("isDash", isDash);
        }
    }
}
