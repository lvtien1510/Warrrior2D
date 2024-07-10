using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private bool isGrounded = false;
    private Rigidbody2D rb;
    private Animator anim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }
    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        // Di chuyen nhan vat
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        if (Input.GetKey(KeyCode.Space) && isGrounded)
            Jump();
        // Xoay nhan vat khi di chuyen
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // Animation
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded);

    }
    void Jump()
    {

        rb.velocity = new Vector2(rb.velocity.x, jump);
        anim.SetTrigger("jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
    }
}
