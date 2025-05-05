using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class characterController : MonoBehaviour
{
    //Stats
    public characterStats characterStats;
    //Moving
    [SerializeField] public float moveDirection;
    private Rigidbody2D characterRb;
    private SpriteRenderer characterRen;
    //Dashing
    private TrailRenderer characterTrailRen;
    private Collider2D characterCollider;
    private bool canDash = true;
    private bool isDashing;
    //Animations
    private Animator characterAnimator;
    //attacking
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask enemyLy;
    [SerializeField] private float damage;
    private Collider2D[] enemy;
    private Vector3 attackPointOffset;
    private bool facingRight;

    private void Awake()
    {
        characterRb = GetComponent<Rigidbody2D>();
        characterRen = GetComponent<SpriteRenderer>();
        characterTrailRen = GetComponent<TrailRenderer>();
        characterAnimator = GetComponent<Animator>();
        attackPointOffset = attackPoint.localPosition;
        characterCollider = GetComponent<Collider2D>();
    }
    void Start()
    {

    }

    private void FixedUpdate()
    {
        //Dash
        if(isDashing)
        {
            return;
        }

        characterRb.linearVelocity = new Vector2(characterStats.walkSpeed * moveDirection, characterRb.linearVelocity.y);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            characterAnimator.SetBool("isWalking", true);
        }
        //Dash
        if (isDashing)
        {
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {

            moveDirection = -1.0f;
            characterRen.flipX = true;

            //Gizmoyu döndürmek için.
            facingRight = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1.0f;
            characterRen.flipX = false;
            
            //Gizmoyu döndürmek için.
            facingRight = true;  
        }
        else
        {
            moveDirection = 0.0f;
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
            Debug.Log("bastý");
        }

        if (characterRb.linearVelocity == Vector2.zero)
        {
            characterAnimator.SetBool("isWalking", false);
        }
        else
        {
            characterAnimator.SetBool("isWalking", true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            characterAnimator.SetBool("isAttacking",true);
        }
      
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = characterRb.gravityScale;
        characterRb.gravityScale = 0f;
        characterRb.linearVelocity = new Vector2(characterStats.dashingPower * moveDirection, characterRb.linearVelocity.y);
        if(moveDirection != 0.0f)
        {
            characterCollider.enabled = false;
        }
        if (characterRb.linearVelocity != Vector2.zero)
        {
            characterAnimator.Play(Animator.StringToHash("bossCharacterDashAnim"));
        }
        characterTrailRen.emitting = true;
        yield return new WaitForSeconds(characterStats.dashingTime);
        characterTrailRen.emitting = false;
        characterRb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(characterStats.dashingCoolDown);
        characterCollider.enabled = true;
        canDash = true;
    }
    public void endAttack()
    {
        characterAnimator.SetBool("isAttacking", false);
    }
    public void characterAttack()
    {  
        enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLy);

        foreach (Collider2D colliderEnemy in enemy)
        {
            Debug.Log("Hit");
            colliderEnemy.GetComponent<health>().takeDamage(damage);
        }
    }
    private void OnDrawGizmos()
    {
        //buraya yine bak!
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }

    void LateUpdate()
    {
        // AttackPoint’i her frame, doðru tarafa taþý
        float x = Mathf.Abs(attackPointOffset.x) * (facingRight ? 1f : -1f);
        attackPoint.localPosition = new Vector3(
            x,
            attackPointOffset.y,
            attackPointOffset.z
        );
    }

}
