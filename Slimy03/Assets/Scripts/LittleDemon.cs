﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDemon : Enemy
{
    //private Vector3 movementDirection;
    private bool isAttacking;
    private const float ATK = 2;
    private float attackRadius;
    public Transform attackPos;
    public Transform attackAnchor;
    public LayerMask playerLayer;
    private float buffHp;

    private void Awake()
    {
        //call TakeDamage() when hirEvent happens, also pass HitEventData at the same time
        SlimyEvents.hitEvent.AddListener(TakeDamage);
    }

    // Start is called before the first frame update
    void Start()
    {
        id = "LittleDemon";
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        agent.updateRotation = false;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        isAlive = true;
        isInvokeSet = false;
        isAttacking = false;
        HP_MAX = 9;
        hp = HP_MAX;
        attackRadius = 0.4f;
        buffHp = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameObject.Find("Player").transform.position - transform.position).magnitude < 6.0f)
        {
            CancelInvoke("Stroll");
            isInvokeSet = false;

            agent.SetDestination(GameObject.Find("Player").transform.position);
        }
        else
        {
            if (!isInvokeSet)
            {
                InvokeRepeating("Stroll", 1.0f, 3.0f);
                isInvokeSet = true;
            }
        }

        //start walking
        if (agent.desiredVelocity.magnitude > 0.1f)
        {
            direction = new Vector2(agent.desiredVelocity.x, agent.desiredVelocity.z).normalized;

            //animator.SetFloat("Horizontal", direction.x);
            //animator.SetFloat("Vertical", direction.y);

            attackPos.LookAt(GameObject.Find("Player").transform);

            CancelInvoke("Attack");
            isAttacking = false;
        }
        else if (isInvokeSet == false)
        {
            Vector3 vector = GameObject.Find("Player").transform.position - transform.position;
            Vector2 direction = new Vector2(vector.x, vector.z).normalized;
            //animator.SetFloat("Horizontal", direction.x);
            //animator.SetFloat("Vertical", direction.y);

            attackPos.LookAt(GameObject.Find("Player").transform);
        }

        //animator.SetFloat("Magnitude", agent.desiredVelocity.magnitude);

        //start attacking
        if (agent.desiredVelocity.magnitude < 0.1f && (GameObject.Find("Player").transform.position - transform.position).magnitude < 3.0f)
        {
            if (!isAttacking && isAlive == true)
            {
                InvokeRepeating("Attack", 1.0f, 3.0f);
                isAttacking = true;
            }
        }

    }

    void Attack()
    {
        //animator.SetTrigger("Attack");

        Collider[] hitColliders = Physics.OverlapSphere(attackAnchor.position, attackRadius, playerLayer);
        int i = 0;
        while (i < hitColliders.Length)
        {
            SlimyEvents.hitEvent.Invoke(new HitEventData(gameObject, hitColliders[i].gameObject, ATK));
            i++;
        }
    }

    public override void Buff()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().SetHp(buffHp);
    }

    public override void Die()
    {
        CancelInvoke("Attack");
        isAlive = false;
        animator.enabled = false;
        agent.speed = 0.0f;
        spriteRenderer.color = new Color(90f / 255f, 90f / 255f, 90f / 255f);
        hp = 0;
        GameManager.instance.enemies.Remove(gameObject);
        GameManager.instance.EnemiesCleared();//detect if all enemies cleared
        GetComponent<Collider>().isTrigger = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackAnchor.position, attackRadius);
    }
}
