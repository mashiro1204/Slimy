﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tama : MonoBehaviour
{
    public Vector3 velocity;
    public GameObject player;

    public int damage = 4;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position + velocity * Time.deltaTime;
        Vector3 direction = velocity;
        direction.Normalize();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit,(newPosition - transform.position).magnitude*7.0f))
        {
            GameObject other = hit.collider.gameObject;
            if (other != player)
            {
                if (other.CompareTag("Enemy"))
                {
                    SlimyEvents.hitEvent.Invoke(new HitEventData(player, other, gameObject));
                    Destroy(gameObject);
                    //Debug.Log(hit.transform.name + " " + transform.position);
                }
                if (other.CompareTag("Pot") || other.CompareTag("Rock"))
                {
                    Item item = other.GetComponent<Item>();
                    item.TakeDamage(damage);
                    Destroy(gameObject);
                    //Debug.Log(hit.transform.name + " " + transform.position);
                }
                if (other.CompareTag("OuterWall") || other.CompareTag("Exit") || other.CompareTag("Enter"))
                {
                    Destroy(gameObject);
                    //Debug.Log(other.name + " " + transform.position);
                }
            }
        }


        //RaycastHit[] hits;
        //hits = Physics.RaycastAll(transform.position, newPosition, (newPosition - transform.position).magnitude);
        //foreach (RaycastHit hit in hits)
        //{
        //    GameObject other = hit.collider.gameObject;

        //    if (other != player)
        //    {
        //        if (other.CompareTag("Enemy"))
        //        {
        //            Destroy(gameObject);
        //            Debug.Log(other.name+" "+transform.position);
        //            break;
        //        }
        //        if (other.CompareTag("OuterWall") || other.CompareTag("Exit") || other.CompareTag("Enter"))
        //        {
        //            Destroy(gameObject);
        //            Debug.Log(other.name + " " + transform.position);
        //            break;
        //        }
        //    }
        //}

        transform.position = newPosition;
    }
}
