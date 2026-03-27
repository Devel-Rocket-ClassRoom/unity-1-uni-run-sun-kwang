using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip;
    public float jumpForce = 5f;

    private int jumpCount = 0;
    private bool isGrounded = true;
    private bool wasGrounded = true;
    private bool isDead = false;
    private bool isSlide = false;

    private Rigidbody2D rb;
    private Animator animator;
    public BoxCollider2D idleBoxCollider;
    public BoxCollider2D slideBoxCollider;
    private int maxHealth = 100;
    private int _currentHealth;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    public Slider hpBar;

    private float hitInterval = 1f;
    private float hitTImer = 0f;
    private bool isHit = false;

    private SpriteRenderer spriteRenderer;
    private float blinkInterval = 0.1f;
    private float blinkTimer = 0f;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
            if (hpBar != null)
                hpBar.value = _currentHealth / (float)maxHealth;
            if (_currentHealth <= 0)
            {
                hpBar.value = 0f;
                Die();
            }
        }
    }
    //private AudioSource audioSource;

    //public GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        idleBoxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        idleBoxCollider.enabled = true;
        slideBoxCollider.enabled = false;
        _currentHealth = maxHealth;
        //audioSource = GetComponent<AudioSource>();
    }
    // Start()는 첫 번째 Update 직전에 호출
    // Awake()는 Instantiate()로 객체가 생성되고 바로 호출
    private void Update()
    {
        if (isDead)
            return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (!wasGrounded && isGrounded)
        {
            jumpCount = 0;
        }
        if (Input.GetButtonDown("Fire1") && jumpCount < 2)
        {
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            idleBoxCollider.enabled = true;
            slideBoxCollider.enabled = false;
            isSlide = false;
        }
        else if (Input.GetButtonUp("Fire1") && rb.linearVelocityY > 0)
        {
            rb.linearVelocityY *= 0.5f;
        }

        if (Input.GetButton("Fire2") && isGrounded)
        {
            isSlide = true;
            idleBoxCollider.enabled = false;
            slideBoxCollider.enabled = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isSlide = false;
            idleBoxCollider.enabled = true;
            slideBoxCollider.enabled = false;
        }
        if (isHit)
        {
            hitTImer += Time.deltaTime;
            blinkTimer += Time.deltaTime;
            if (hitTImer >= hitInterval)
            {
                isHit = false;
                hitTImer = 0f;
            }
        }
        else
        {
            spriteRenderer.enabled = true;
        }
        
        if (blinkTimer >= blinkInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            blinkTimer = 0f;
        }
        

        animator.SetBool("Grounded", isGrounded);
        animator.SetInteger("JumpCount", jumpCount);
        animator.SetFloat("yVelocity", rb.linearVelocityY);
        animator.SetBool("Slide", isSlide);
        wasGrounded = isGrounded;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dead"))
        {
            CurrentHealth = 0;
        }
        else if (other.CompareTag("Hit") && !isHit)
        {
            CurrentHealth -= 30;
            isHit = true;
        }
        if (other.CompareTag("Item"))
        {
            other.gameObject.SetActive(false);
            GetItem();
        }
    }

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // 물리적 상호작용(충돌, 중력 등)을 비활성화
        animator.SetBool("isDead", true);
        animator.SetTrigger("Die");
        //audioSource.PlayOneShot(deathClip);
        isDead = true;
        //gameManager.OnPlayerDead();
        GetComponent<Collider2D>().enabled = false;
    }

    private void GetItem()
    {
        CurrentHealth += 1;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
