using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{

    private Transform targetObject; 
    public Transform SetTargetObject { set { targetObject = value; } }


    private Rigidbody2D enemyRigidbody;
    private BoxCollider2D enemyBoxCollider;
    private SpriteRenderer enemyPixel;
    protected Animator enemyAnimator;

   [SerializeField] protected float moveSpeed;    // Unity 내부에서 값 처리
   [SerializeField] protected int damage;         // Unity 내부에서 값 처리
    public int getDamage { get { return damage; } }    
    private bool isSlowDown = false;
    private WaitForSeconds slowDownDuration = new WaitForSeconds(0.5f);


    // Start is called before the first frame update
    private void Start()
    {
        enemyBoxCollider = GetComponent<BoxCollider2D>();
        enemyAnimator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyPixel = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        TargetObjectTracking();
    }

    private void TargetObjectTracking()
    {
        if (targetObject != null && !dead)
        {
            Vector2 dir = (targetObject.transform.position - transform.position).normalized;
            enemyRigidbody.velocity = dir * moveSpeed;

            enemyPixel.flipX = enemyRigidbody.velocity.x < 0 ? true : false;
        }
    }

    public void SlowDownCoroutine()
    {
        if (!isSlowDown) StartCoroutine(SlowDown());
    }


    private IEnumerator SlowDown()
    {
        isSlowDown = true;
        float originalSpeed = moveSpeed;

        moveSpeed /= 2;
        yield return slowDownDuration;
        moveSpeed = originalSpeed;
        isSlowDown = false;
    }
    public void DeadAnimation()
    {
        enemyBoxCollider.enabled = false;
        enemyRigidbody.isKinematic = true;
        enemyRigidbody.velocity = Vector2.zero;
        enemyAnimator.Play("Death");
    }
}
