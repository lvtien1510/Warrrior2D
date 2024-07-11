using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameter")]
    [SerializeField] private float speed;
    [SerializeField] private float jump;

    [Header("Multiple Jump")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [SerializeField] private LayerMask groundLayer;
    private float horizontalInput;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private PlayerAttack playerAttack;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerAttack = GetComponent<PlayerAttack>();
        jumpCounter = extraJumps; // Khởi tạo jumpCounter với giá trị extraJumps
    }

    // Update được gọi mỗi khung hình
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Lấy giá trị đầu vào ngang

        // Di chuyển nhân vật
        Move();

        // Xử lý nhảy
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            
            
        }

        // Xoay nhân vật khi di chuyển
        Flip();

        // Thiết lập animation
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", IsGrounded());
    }

    // Hàm di chuyển
    void Move()
    {
        float currentSpeed = speed;

        // Nếu nhân vật đang tấn công, giảm tốc độ di chuyển xuống 1/3
        if (playerAttack != null && playerAttack.IsAttacking())
        {
            currentSpeed /= 3;
        }

        rb.velocity = new Vector2(horizontalInput * currentSpeed, rb.velocity.y);
    }

    // Hàm nhảy
    void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            jumpCounter = extraJumps; // Reset jumpCounter khi chạm đất
            

        }
        else if (jumpCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            jumpCounter--; // Giảm jumpCounter khi thực hiện nhảy đôi

        }
        else
        {
            return;
        }
        animator.SetTrigger("jump");

    }

    // Xoay nhân vật khi di chuyển
    private void Flip()
    {
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Tạo một lazer box thẳng xuống kiểm tra nhân vật có va chạm với ground không
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
