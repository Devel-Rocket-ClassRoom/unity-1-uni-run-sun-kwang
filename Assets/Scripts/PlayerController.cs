using System;
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
    public BoxCollider2D idleBoxCollider;
    public BoxCollider2D slideBoxCollider;
    private float maxHealth = 100f;
    private float _currentHealth;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private float hitInterval = 1f;
    private float hitTimer = 0f;
    private bool isHit = false;

    private float blinkInterval = 0.1f;
    private float blinkTimer = 0f;

    private int score = 0;

    public event Action<int> OnScoreChanged;
    public event Action<float> OnHpChanged;
    public event Action OnGameOver;

    public float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = Mathf.Clamp(value, 0f, maxHealth);
            OnHpChanged?.Invoke(_currentHealth / maxHealth);
            if (_currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        idleBoxCollider.enabled = true;
        slideBoxCollider.enabled = false;
        _currentHealth = maxHealth;
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
            hitTimer += Time.deltaTime;
            blinkTimer += Time.deltaTime;

            if (blinkTimer >= blinkInterval)
            {
                Color color = spriteRenderer.color;
                color.a = color.a == 1f ? 0f : 1f;
                spriteRenderer.color = color;
                blinkTimer = 0f;
            }

            if (hitTimer >= hitInterval)
            {
                isHit = false;
                hitTimer = 0f;
                blinkTimer = 0f;
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
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
            TakeDamage(30f);
            isHit = true;
        }
        if (other.CompareTag("Item"))
        {
            other.gameObject.SetActive(false);
            GetItem();
        }
    }
    
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // 물리적 상호작용(충돌, 중력 등)을 비활성화
        animator.SetBool("isDead", true);
        animator.SetTrigger("Die");
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        OnGameOver?.Invoke();
    }

    private void GetItem()
    {
        CurrentHealth += 1;
        score++;
        OnScoreChanged(score);
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
