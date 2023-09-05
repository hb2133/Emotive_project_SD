using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float HP;
    public float MaxHp = 15;
    public float distance = 5.0f;
    public float chaseSpeed = 2.0f;
    public LayerMask layersToHit;
    public HealthbarBehaviour healthbar;

    public float attackSpeed = 1.0f;
    private Coroutine attackRoutine;
    SkeletonAnimation spriter;
    Animator anim;
    Rigidbody2D rigid;
    public Transform player;

    public RaycastHit2D hit;
    public enum State { Idle, Walk, Attack, TakeDamage, Die };
    public State currentState = State.Idle;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = transform.Find("Enemy").GetComponent<SkeletonAnimation>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        HP = MaxHp;
        healthbar.SetHealth(HP, MaxHp);
    }

    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(transform.position, -transform.right, distance, layersToHit);

        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Walk:
                Walk();
                break;
            case State.Attack:
                Attack();
                break;
            case State.TakeDamage:
                TakeDamage(5);
                break;
            case State.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        spriter.skeleton.SetSkin("Front_1");
        spriter.skeleton.SetToSetupPose();
        anim.SetBool("Walk", false);
        Invoke("StartWalking", 1.0f);
    }
    private void StartWalking()
    {
        currentState = State.Walk;
    }

    private void Walk()
    {
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, hit.point - (Vector2)transform.position, Color.white);

            if (hit.collider.CompareTag("Player"))
            {
                currentState = State.Attack;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, -transform.right * distance, Color.black);
            spriter.skeleton.SetSkin("Front_2");
            spriter.skeleton.SetToSetupPose();
            anim.SetBool("Walk", true);
            Vector2 targetPosition = new Vector2(player.position.x, rigid.position.y);
            Vector2 nextPosition = Vector2.MoveTowards(rigid.position, targetPosition, chaseSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(nextPosition);
        }
    }

    private void Attack()
    {
        if (attackRoutine != null)
        {
            return;
        }
        attackRoutine = StartCoroutine(AttackPlayer());
    }

    IEnumerator AttackPlayer()
    {
        while (true)
        {
            if (hit.collider == null || !hit.collider.CompareTag("Player"))
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
                anim.SetTrigger("Idle");
                currentState = State.Idle;
                yield break;
            }

            spriter.skeleton.SetSkin("Front_2");
            spriter.skeleton.SetToSetupPose();
            anim.SetTrigger("Attack");

            yield return new WaitForSeconds(attackSpeed);
            player.GetComponent<Player>().TakeDamage(5);
            Debug.Log("데미지 줌");

        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        healthbar.SetHealth(HP, MaxHp);
        if (HP <= 0)
        {
            StopAllCoroutines();
            anim.SetTrigger("Die");
            currentState = State.Die;
        }
    }

    private void Die()
    {
        HP = 0;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
