using Unity.VisualScripting;
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

    [SerializeField] private ParticleSystem footstep;
    private ParticleSystem.EmissionModule footEmission;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        jumpCounter = extraJumps;
        footEmission = footstep.emission;
    }

    // Update được gọi mỗi khung hình
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Lấy giá trị đầu vào ngang

        // Di chuyển nhân vật
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        animator.SetFloat("xVelocity", horizontalInput);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", IsGrounded());

        

        // Xử lý nhảy
        if (Input.GetKeyDown(KeyCode.Space))
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
            else return;
            
        }
        //Hiệu ứng di chuyển
        if (horizontalInput !=0 && IsGrounded())
        {
            footEmission.rateOverTime = 35f;
        }
        else
        {
            footEmission.rateOverTime = 0f;
        }
        

        // Xoay nhân vật khi di chuyển
        Flip();
        
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.2f, groundLayer);
        return raycastHit.collider != null;
    }
}
