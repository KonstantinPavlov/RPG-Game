using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

   
    public float projectileSpeed;

    float damageCaused = 8f;

    public float DamageCaused
    {
        get
        {
            return damageCaused;
        }

        set
        {
            damageCaused = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Component damagableComponent=other.gameObject.GetComponent(typeof(IDamageable));
        if (damagableComponent) {
            (damagableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }
}
