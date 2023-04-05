using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanditEnemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float detectionRange = 5f;

    private Transform player;
    private float timeSinceLastAttack = 0f;
    private bool isPlayerAlive = true;
    private bool isWalkingTowardsPlayer = false;
    private SpriteRenderer spriteRenderer;

    public Canvas healthBarCanvas;
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = FindObjectOfType<HeroKnight>().transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
   void Update()
{
    if (!isPlayerAlive) {
        // Stop attacking if player is dead
        return;
    }
    //finds distance to player and if not in range moves to player
    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    if (distanceToPlayer <= attackRange)
    {
        if (timeSinceLastAttack >= attackCooldown)
        {
            Attack();
            timeSinceLastAttack = 0f;
        }
        else {
            isWalkingTowardsPlayer = false;
            
        }
    }
    else if (distanceToPlayer <= detectionRange) // Check if the player is within the detection range
    {
        if (!isWalkingTowardsPlayer) {
            animator.SetInteger("AnimState", 2);
            isWalkingTowardsPlayer = true;
        }
        float xDirection = player.position.x - transform.position.x;
        if (xDirection > 0f) {
            spriteRenderer.flipX = true;
        } else if (xDirection < 0f) {
            spriteRenderer.flipX = false;
        }
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), moveSpeed * Time.deltaTime);
    }
    else
    {
        animator.SetInteger("AnimState", 0);
        isWalkingTowardsPlayer = false;
    }

    timeSinceLastAttack += Time.deltaTime;
}

    void Attack()
    {
        animator.SetTrigger("Attack");
       Invoke("DamagePlayer", 0.5f); // Delay the damage to player by 2 seconds
        isWalkingTowardsPlayer = false;
        timeSinceLastAttack = 0f;
    }

    void DamagePlayer()
{
      if (isPlayerAlive && currentHealth > 0) {
    player.GetComponent<HeroKnight>().TakeDamage(attackDamage);
      }else return;
}

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        healthBar.fillAmount = currentHealth/100f;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died");
        animator.SetTrigger("Death");

        GetComponent<Collider2D>().enabled = false;
        //switch to kinematic to prevent body from falling through ground
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        healthBarCanvas.enabled = false;
        this.enabled = false;
    }

    public void SetPlayerAlive(bool alive) {
        isPlayerAlive = alive;
    }
}