using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public bool isAttacking; // Biến để theo dõi trạng thái tấn công

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");
        isAttacking = true;
        StartCoroutine(ResetAttack());
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    // Reset trạng thái tấn công sau một khoảng thời gian
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(5); // Thời gian tấn công (1 giây)
        isAttacking = false;
    }
}
