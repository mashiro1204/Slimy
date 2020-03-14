﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public string id;
    public NavMeshAgent agent;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float HP_MAX;
    public AudioSource audioSource;
    public AudioClip hittingClip;
    public float hittingVolume;
    public RectTransform hitpoints;
    public GameObject hpBar;

    protected float hp;
    public bool isAlive;
    protected bool isInvokeSet;
    protected Vector2 direction;

    public void TakeDamage(HitEventData data)
    {
        if(data.victim == gameObject && isAlive)
        {
            //hit recover for 0.5s
            agent.isStopped = true;
            Invoke("agentStart",0.1f);

            //change the direction
            Vector3 damageVector = data.shooter.transform.position - transform.position;
            Vector2 damageDirection = new Vector2(damageVector.x, damageVector.z).normalized;
            animator.SetFloat("Horizontal", damageDirection.x);
            animator.SetFloat("Vertical", damageDirection.y);

            animator.SetTrigger("Hit");

            hp -= data.tama.GetComponent<Tama>().damage * GameObject.Find("Player").GetComponent<PlayerController>().ATK;
            hitpoints.localScale = new Vector3(hp / HP_MAX, 1, 1);

            audioSource.PlayOneShot(hittingClip, hittingVolume);
            if (hp <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        isAlive = false;
        animator.enabled = false;
        agent.speed = 0.0f;
        spriteRenderer.color = new Color(90f / 255f, 90f / 255f, 90f / 255f);
        hp = 0;
        hpBar.SetActive(false);
        GameManager.instance.enemies.Remove(gameObject);
        GameManager.instance.EnemiesCleared();//detect if all enemies cleared
        GetComponent<Collider>().isTrigger = true;         
    }
    public void Stroll()
    {
        float newX = Random.Range(-10, 10);
        float newZ = Random.Range(-10, 10);
        Vector3 offset = new Vector3(newX, 0f, newZ);
        offset.Normalize();
        Vector3 strollPosition = transform.position + offset*2.0f;

        agent.SetDestination(strollPosition);
    }

    public virtual void Gone()
    {
        Destroy(gameObject);
    }

    public virtual void Buff(){}

    public float GetHP_MAX()
    {
        return HP_MAX;
    }

    void agentStart()
    {
        agent.isStopped = false;
    }
}
