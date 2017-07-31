using UnityEngine;
using System.Collections;

public class DamageObject : MonoBehaviour
{
    float DamageAmount = 150f;
    public bool destroyOnCollision;

    public GameObject ExplosionEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();

        if (p)
        {
            p.TakeDamage(DamageAmount);
            if (destroyOnCollision)
            {
                Destroy(Instantiate(ExplosionEffect, collision.contacts[0].point, transform.rotation), 3f);
                Destroy(this.gameObject);
            }
        }
    }
}
