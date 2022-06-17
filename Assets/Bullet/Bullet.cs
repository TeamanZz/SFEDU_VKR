using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitParticles;
    public Rigidbody2D rb;
    public int damage = 40;
    public float speed = 20f;

    private void Start()
    {
        rb.velocity = transform.right * speed;
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Collision(hitInfo);
    }

    [PunRPC]
    private void Collision(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Instantiate(hitParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}