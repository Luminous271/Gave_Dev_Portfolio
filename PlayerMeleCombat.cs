using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleCombat : MonoBehaviour
{

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackThrust;
    public Vector2 rebound;
    Rigidbody2D rb;
    Movement playercontroller;
    public float coolDown;
     float cD;
    public int hitCount;
     int hC;
    int thrustDirection;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playercontroller = GetComponent<Movement>();
        hC = hitCount;
        cD = coolDown;
    }
    void Update()
    {
        if (playercontroller.facingLeft)
            thrustDirection = -1;
        if(playercontroller.facingRight)
            thrustDirection = 1;
        if(hC == 0)
        {
            cD = cD-Time.deltaTime;
        }
        if(cD <= 0)
        {
            cD = coolDown;
            hC = hitCount;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && hC > 0 && cD == coolDown)
        {
            Attack();
        }

    }
    void Attack()
    {
        rb.AddForce(new Vector2(thrustDirection * attackThrust, 0f), ForceMode2D.Impulse);
        hC--;

        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitenemies)
        {
            rebound.x = rebound.x * thrustDirection;
            enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(rebound, ForceMode2D.Impulse);
        }

    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
