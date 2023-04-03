using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] public float      m_jumpForce = 8f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;


    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip runningAudio;
    [SerializeField] AudioSource effectSource;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip swordClip;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;

    public Transform attackPoint;
    public LayerMask enemyLayers;

	public float attackRange = 0.5f;
	public int attackDamage = 40;
    public float attackRate = 2f;
	private float attackTime = 0.0f;

    public int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] PhysicsMaterial2D Walls_noFriction;
    public bool isBlocking = false;
    public TextMeshProUGUI DeathText;


	// Use this for initialization
	void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        DeathText = FindObjectOfType<TextMeshProUGUI>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        currentHealth = maxHealth;

        audioSource.loop = true;
        audioSource.volume = 0.025f;
        audioSource.clip = runningAudio;
        audioSource.Play();

        
        
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

         // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (inputX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Move
        if (!m_rolling && !isBlocking )
        {
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
            
        } else
        {
            
        }

        if (inputX != 0 && m_grounded && !m_rolling)
        {
            audioSource.UnPause();
        } else
        {
            audioSource.Pause();
        }
            

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        /*   //Death
           if (Input.GetKeyDown("e") && !m_rolling)
           {
               m_animator.SetBool("noBlood", m_noBlood);
               m_animator.SetTrigger("Death");
           }

           //Hurt
           else if (Input.GetKeyDown("q") && !m_rolling)
               m_animator.SetTrigger("Hurt");*/

        //Attack
        if (Time.time >= attackTime)
        {
            if (Input.GetMouseButtonDown(0)  && !m_rolling)
            {
                effectSource.clip = swordClip;
                effectSource.volume = 0.07f;
                effectSource.Play();

                m_currentAttack++;

                // Loop back to one after third attack
                if (m_currentAttack > 3)
                    m_currentAttack = 1;

               

                // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                m_animator.SetTrigger("Attack" + m_currentAttack);

                Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(attackPoint.position, attackPoint.position, enemyLayers);

                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<BanditEnemy>().TakeDamage(attackDamage);
                    Debug.Log("We hit " + enemy.name);
                }
                // Reset timer
                attackTime = Time.time + 1f / attackRate;
            }
        }
        // Block
         if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            isBlocking=true;
            GetComponent<Collider2D>().sharedMaterial = null;
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1)){
        isBlocking=false;
        GetComponent<Collider2D>().sharedMaterial = Walls_noFriction;
            m_animator.SetBool("IdleBlock", false);
        }

        // Roll
         if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }


        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            effectSource.clip = jumpClip;
            effectSource.volume = 0.22f;
            effectSource.Play();

            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

        public void TakeDamage(int damage)
    {
           if (isBlocking) {
        // Player is blocking, so no damage is taken
        return;
    }

    currentHealth -= damage;
    m_animator.SetTrigger("Hurt");
    if(currentHealth <= 0)
    {
        Die();
    }
    }

    void Die()
    {
        Debug.Log("Player Died");
        m_animator.SetTrigger("Death");
        this.enabled=false;
         // Stop all bandit enemies from attacking
    BanditEnemy[] enemies = FindObjectsOfType<BanditEnemy>();
    foreach (BanditEnemy enemy in enemies) {
        enemy.SetPlayerAlive(false);
    }
     DeathText.enabled = true;
        Invoke("ResetLevel", 2.0f);
    }

	private void OnDrawGizmosSelected()
	{
        if(attackPoint == null)
			return;
		Gizmos.DrawSphere(attackPoint.position, attackRange);
	}

      void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
