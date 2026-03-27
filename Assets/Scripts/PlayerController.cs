using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip;
    public float jumpForce = 5f;

    private int jumpCount = 0;
    private bool isGrounded = true;
    private bool isDead = false;
    private bool isSlide = false;

    private Rigidbody2D rb;
    private Animator animator;
    public BoxCollider2D idleBoxCollider;
    public BoxCollider2D slideBoxCollider;
    //private AudioSource audioSource;

    //public GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        idleBoxCollider = GetComponent<BoxCollider2D>();
        idleBoxCollider.enabled = true;
        slideBoxCollider.enabled = false;
        //audioSource = GetComponent<AudioSource>();
    }
    // Start()는 첫 번째 Update 직전에 호출
    // Awake()는 Instantiate()로 객체가 생성되고 바로 호출
    private void Update()
    {
        if (isDead)
            return;

        if (Input.GetButtonDown("Fire1") && jumpCount < 2)
        {
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //audioSource.Play();
            jumpCount++;
            idleBoxCollider.enabled = true;
            slideBoxCollider.enabled = false;
        }
        else if (Input.GetButtonUp("Fire1") && rb.linearVelocityY > 0)
        {
            rb.linearVelocityY *= 0.5f;
        }

        if (Input.GetButton("Fire2") && isGrounded)
        {
            isSlide = true;
            //transform.position = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
            idleBoxCollider.enabled = false;
            slideBoxCollider.enabled = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isSlide = false;
            //transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
            idleBoxCollider.enabled = true;
            slideBoxCollider.enabled = false;
        }
        

        animator.SetBool("Grounded", isGrounded);
        animator.SetInteger("JumpCount", jumpCount);
        animator.SetFloat("yVelocity", rb.linearVelocityY);
        animator.SetBool("Slide", isSlide);
        //animator.Set
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform") && collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
            rb.linearVelocityY = 0;
        }
    }
    // Normal, 법선: 표면의 수직 방향
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dead"))
        {
            Die();
        }
    }

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // 물리적 상호작용(충돌, 중력 등)을 비활성화
        Debug.Log("Player Dead");
        animator.SetTrigger("Die");
        //audioSource.PlayOneShot(deathClip);
        isDead = true;
        //gameManager.OnPlayerDead();
        GetComponent<Collider2D>().enabled = false;
    }
}
