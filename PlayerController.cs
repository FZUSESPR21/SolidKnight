using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;

    public GameObject sword;
    public GameObject below_sword;
    public GameObject up_sword;
    private GameObject currentSword;
    public GameObject hit;

    //蓄力时间
    private float attackTime;

    private float hp;
    private float mp;

    [Header("生命")]
    [SerializeField]
    private float maxHp;

    [Header("能量")]
    [SerializeField]
    private float maxMp;
    
    private float horizontalMove;

    [Header("速度")]
    [SerializeField]
    private float speed;
    [Header("跳跃力")]
    [SerializeField]
    private float jumpForce;
    [Header("地面检测")]
    [SerializeField]
    private Transform groundCheck;

    [Header("图层检测")]
    [SerializeField]
    private LayerMask ground;

    [Header("墙面检测")]
    [SerializeField]
    private Transform wallCheck;

    [Header("蹬墙跳能力")]
    [SerializeField]
    private bool climbAble;

    [Header("冲刺能力")]
    [SerializeField]
    private bool rushAble;

    [Header("二段跳能力")]
    [SerializeField]
    public bool doubleJumpAble;

    [Header("蹬墙回跳速度")]
    [SerializeField] private float speedX;
    private float currentSpeedX;

    //isJump区别攀爬和第二段跳跃
    private bool isGround, isJump, isDashing, isClimbing;

    bool jumpPressed;

    
    private int jumpCount;

    [Header("Better Jump重力系数")]
    [SerializeField]
    private float jumpPa;

    [SerializeField]
    [Header("盒子size")]
    private Vector2 boxSize;

    [Header("Dash参数")]
    public float dashTime;
    private float lastDash = -10f;
    public float dashInterval;
    private float dashTimeLeft;
    public float dashSpeed;
    private float tsped;
    [Header("CD的UI软件")]
    public Image cdImage;


    [Header("Hurt无敌")]
    public float hurtLength; //MARK 自定义计数器长度
    private float hurtCount;//MARK 计数器 每帧减少
    [HideInInspector] public bool isAttacked;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        attackTime = 0;
        /* sword = transform.Find("wavePosition/sword").gameObject;
         below_sword = transform.Find("wavePosition/below_sword").gameObject;
         up_sword= transform.Find("wavePosition/up_sword").gameObject;
         hit = transform.Find("wavePosition/attack").gameObject;*/
        hp = maxHp;
        mp = maxMp;
        currentSpeedX = 0;
        hurtCount = hurtLength;
        isAttacked = false;

        Debug.Log(rb);

    }

    // Update is called once per frame
    void Update()
    {
        //受击无敌
        if (isAttacked)
        {
            hurtCount -= Time.deltaTime;
            if (hurtCount <= 0)
            {
                isAttacked = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            wave();
        }

        if (Input.GetKeyDown(KeyCode.C) && jumpCount > 0)
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Time.time >= (dashInterval + lastDash))
                ReadyToDash();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.DownArrow)&&!isGround)
            {
                currentSword = below_sword;
               // Debug.Log("down");
            }
            else if(Input.GetKey(KeyCode.UpArrow))
            {
                currentSword = up_sword;
            }
            else
            {
                currentSword = sword;
            }          
            Attack();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            attackTime += Time.deltaTime;
        }
        else
        {
            if (attackTime > 1)
            {                
                hit.SetActive(true);
            }
            attackTime = 0;
        }

        cdImage.fillAmount -= 1.0f / dashInterval * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //Debug.Log(rb);
        tsped = rb.velocity.y;
        isGround = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, ground);
        isClimbing = Physics2D.OverlapCircle(wallCheck.position, .1f, ground);
        Debug.Log(isClimbing);

        if (isGround)
        {
            rb.drag = 0;
        }

        Dash();

        if (isDashing)
            return;

        GroundMovement();
        if (isClimbing)
        {
            if (climbAble)
                ClimbJump();
            else BetterJump();
        }
        else
        {
            BetterJump();
        }
        jumpPressed = false;

        if (rb.velocity.y <= -50)
        {
            rb.drag = -Physics2D.gravity.y / 8;
        }
        SwitchAnim();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
        Gizmos.color = Color.black;
    }

    void GroundMovement()
    { 
        horizontalMove = Input.GetAxisRaw("Horizontal");//只返回-1，0，1

        if (hit != null && hit.activeSelf)
        {
            horizontalMove = 0;
        }
        rb.velocity = new Vector2(horizontalMove * speed + currentSpeedX, rb.velocity.y);


        if (currentSword!=null&&currentSword.activeSelf)
        {
           /* if (transform.localScale.x != horizontalMove)
            {

            }*/
            //horizontalMove = 0;
        }
        else
        {
            if (horizontalMove != 0)
            {
                if (horizontalMove > 0)
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
                else
                {
                   
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
                }
            }
        }
        

        if (currentSpeedX > 0)
        {
            currentSpeedX -= 0.5f;
        }
        else if (currentSpeedX < 0)
        {
            currentSpeedX += 0.5f;
        }
        else
        {

        }

    }

    void Attack()
    {
        currentSword.SetActive(true);
    }
    void ClimbJump()
    {

        jumpCount = 2;
        isJump = false;

        if (jumpPressed && isClimbing)
        {
            if (isGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            }
            else
            {
                rb.velocity = new Vector2(speedX, jumpForce);
                currentSpeedX = speedX * transform.localScale.x;
                Debug.Log("current" + currentSpeedX);
            }
        }
        
        jumpPressed = false;

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }


        if (rb.velocity.y >= -50 && rb.velocity.y < 0)
        {
            //Debug.Log(rb.velocity.y);
            rb.velocity += Vector2.up * Physics2D.gravity.y * 2.5f * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.C))
        {
            //Debug.Log(rb.velocity.y);
            rb.velocity += Vector2.up * Physics2D.gravity.y * jumpPa * Time.deltaTime;
        }

    }

    void BetterJump()//跳跃
    {

        if (isGround)
        {
            jumpCount = 2;//可跳跃数量
            isJump = false;
        }

        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            if (doubleJumpAble)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }

        if (rb.velocity.y >= -50 && rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 2.5f * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.C))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * jumpPa * Time.deltaTime;
        }

    }

    void SwitchAnim()//动画切换
    {
        /*anim.Play("Idle");
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround&& Mathf.Abs(rb.velocity.x)!=0)
        {
            anim.Play("Run");
           

        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.Play("Jump");
            
        }
        else if (!isGround && rb.velocity.y < 0)
        {
            anim.Play("Fall");
            
        }*/
    }

    void ReadyToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        cdImage.fillAmount = 1.0f;
    }
    void Dash() 
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                if (rushAble)
                {
                    if (transform.localScale.x > 0)
                    {
                        rb.velocity = new Vector2(dashSpeed, 0);
                    }
                    else
                    {
                        rb.velocity = new Vector2(-dashSpeed, 0);
                    }
                }

                dashTimeLeft -= Time.deltaTime;
                if (rushAble)
                    ShadowPool.instance.GetFromPool();
            }
            else
            {
                isDashing = false;
            }
        }
    }

    void wave()
    {
        if (mp > 0)
        {
            mp -= 1;
            Transform a = Instantiate(transform.Find("wavePosition/wave"));
            a.gameObject.SetActive(true);
        }        
    }

    public void getAttacked(float damage)
    {
        if (!isAttacked&&!isDashing)
        {
            
            hp -= damage;
            maxHp = hp;

            if (hp <= 0)
            {
                //death
            }
            else
            {
                //hurt

            }

            hurtCount = hurtLength;
            isAttacked = true;
        }
        
    }
}