using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip portalClip;
    [SerializeField] private AudioClip failClip;

    private AudioSource audioSource;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private bool grounded;
    private bool started;
    private bool jumping;
    private Scene scene;

    [SerializeField] GameObject endGameText;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        grounded = true;
        scene = SceneManager.GetActiveScene();
    }
    private void Start()
    {
       audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (started && grounded)
            {
                animator.SetTrigger("Jump");
                grounded = false;
                jumping = true;
                audioSource.clip = jumpClip;
                audioSource.Play();
            }
            else
            {
                animator.SetBool("GameStarted", true);
                started = true;
            }
        }

        animator.SetBool("Grounded", grounded);
    }

    private void FixedUpdate()
    {
        if (started)
        {
            rigidbody2D.linearVelocity = new Vector2(speed, rigidbody2D.linearVelocity.y);
        }

        if (jumping)
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, jumpForce);
            jumping = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
        if (other.gameObject.CompareTag("Engel"))
        {
            animator.SetBool("Fail", true);
            audioSource.clip = failClip;
            audioSource.Play();
            Invoke("resetTransform", 0.5f);
            
        }
        if (other.gameObject.CompareTag("Pportal"))
       {
            audioSource.clip = portalClip;
            audioSource.Play();
        }
    }
    public void resetTransform()
    {
        SceneManager.LoadScene(scene.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       endGameText.SetActive(true);
    }
}
