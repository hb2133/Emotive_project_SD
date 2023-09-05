using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    float HP;
    public float MaxHp = 100;
    public Vector2 inputVec;
    public float speed;
    public LayerMask enemyLayer;
    public float distance = 5;
    public Player_HealthbarBehaviour healthbar;

    Rigidbody2D rigid;
    SkeletonAnimation spriter;
    Animator anim;

    bool Jumping;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = transform.Find("Player").GetComponent<SkeletonAnimation>();
        anim = GetComponent<Animator>();
        Jumping = false;
    }

    public void Start()
    {
        HP = MaxHp;
        healthbar.SetHealth(HP, MaxHp);
    }

    public void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jumping = true;
            OnSpacePressed();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Jumping = true;
            OnCtrlPressed();
        }

    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }


    [System.Obsolete]
    private void LateUpdate()
    {
        if (inputVec.x != 0)
        {
            spriter.skeleton.FlipX = inputVec.x > 0;
            spriter.skeleton.SetSkin("Front/Front_2");
            anim.SetBool("Walk", true);
            anim.SetFloat("Walk_speed", inputVec.magnitude * speed);
        }
        else
        {
            if(Jumping == false)
            {
                spriter.skeleton.SetSkin("Front/Front_1");
                anim.SetBool("Walk", false);
                anim.SetFloat("Walk_speed", 1.0f);
            }
        }
    }

    private void OnCtrlPressed()
    {
        if(Jumping == true)
        {
            spriter.skeleton.SetSkin("Front/Front_2");
            spriter.skeleton.SetToSetupPose();
            anim.SetTrigger("Jump");
            Invoke("SetJumpFalse", 2.0f); // Adjust the time according to your jump animation duration.
        }
    }
    public void SetJumpFalse()
    {
        Jumping = false;
    }

    private void OnSpacePressed()
    {
        Vector2 rayStart = transform.position;
        Vector2 rayDirection = transform.right;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, rayDirection, distance, enemyLayer);

        spriter.skeleton.SetSkin("Front/Front_2");
        spriter.skeleton.SetToSetupPose();
        anim.SetTrigger("Attack");

        if (hit.collider != null)
        {
            Debug.Log("Hit an enemy: " + hit.collider.name);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(5);
            }
        }
        else
        {
            Debug.Log("No enemy detected.");
        }
    }

    public void ResetToSetupPose()
    {
        Debug.Log("Event");
        spriter.skeleton.SetSkin("Front/Front_1");
        spriter.skeleton.SetToSetupPose();
        anim.SetBool("Walk", false);
        anim.SetFloat("Walk_speed", 1.0f);
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        healthbar.SetHealth(HP, MaxHp);
        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        HP = 0;
        Debug.Log("Enemy Die");
        Destroy(gameObject);
    }
}
