using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FinalMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;

    public float speed, jumpForce;
    private float horizontalMove;
    public Transform groundCheck;
    public LayerMask ground;
    public Transform wallCheck;

    public bool isGround, isJump, isDashing, isClimbing;

    bool jumpPressed;
    [SerializeField] private int jumpCount;


    [SerializeField]
    [Header("盒子size")]
    private Vector2 boxSize;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeedX = 0;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && jumpCount > 0)
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            
        }

        cdImage.fillAmount -= 1.0f / dashInterval * Time.deltaTime;
    }

    private void FixedUpdate()
    {     
        isGround = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, ground);   

        if (isGround)
        {
            rb.drag = 0;
        }
        Dash();
        if (isDashing)
            return;
        GroundMovement();

    }

    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");//只返回-1，0，1
        rb.velocity = new Vector2(horizontalMove * speed + currentSpeedX, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
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
}