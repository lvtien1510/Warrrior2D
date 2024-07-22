using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;
    [SerializeField] private int enemyDamage = 1; // Sát thương của quái vật
    [SerializeField] private float jumpForce = 5f; // Lực nhảy của thỏ
    [SerializeField] private float jumpInterval = 2f; // Khoảng thời gian giữa các lần nhảy

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded = false;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Gọi hàm nhảy liên tục
        InvokeRepeating("Jump", 0f, jumpInterval);
    }

    private void Update()
    {
        // Kiểm tra nếu thỏ đang ở trên mặt đất
        isGrounded = CheckIfGrounded();
    }

    private bool CheckIfGrounded()
    {
        // Sử dụng một box collider để kiểm tra nếu thỏ đang ở trên mặt đất
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("jump");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerAttack player = collision.gameObject.GetComponent<PlayerAttack>();
            if (player != null)
            {
                //player.TakeDamage(enemyDamage);
                Debug.Log("Player took " + enemyDamage + " damage from enemy.");
            }
        }
    }

    void Die()
    {
        // Chèn logic khi kẻ thù chết
        Debug.Log("Enemy died.");
        Destroy(gameObject);
    }
}
