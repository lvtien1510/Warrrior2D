using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private int comboStep = 0;
    private int comboCount = 0; // Đếm số lần phím H đã được nhấn
    private float lastAttackTime = 0f;
    private float nextAttackTime = 0f;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int attackDamage = 1; // Sát thương của mỗi đòn đánh
    [SerializeField] private float comboResetTime = 0.8f; // Thời gian để reset combo nếu không tấn công tiếp
    [SerializeField] private int maxComboCount = 4; // Số lần combo tối đa trước khi tạm dừng
    [SerializeField] private float attackRate = 1.5f; // Tốc độ tấn công
    [SerializeField] private float attackCooldown = 0.5f; // Thời gian nghỉ giữa các lần tấn công

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAttack();
    }

    void HandleAttack()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            ResetCombo();
        }

        if (Input.GetKeyDown(KeyCode.H) && Time.time >= nextAttackTime)
        {
            PerformAttack();
            comboCount++;

            if (comboCount >= maxComboCount)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                comboCount = 0; // Reset comboCount sau khi tạm dừng
            }
            else
            {
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void PerformAttack()
    {
        lastAttackTime = Time.time;
        comboStep = (comboStep % 2) + 1;

        animator.SetInteger("comboStep", comboStep);
        animator.SetTrigger("attack");

        // Xác định các kẻ thù trong phạm vi tấn công
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Gây sát thương cho từng kẻ thù bị trúng
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage); // Gọi hàm TakeDamage của kẻ thù
        }
    }

    void ResetCombo()
    {
        comboStep = 0;
        comboCount = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
