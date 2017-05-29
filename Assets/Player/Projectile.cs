using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    float damageCaused;

    private void OnTriggerEnter(Collider other)
    {
        Component damagableComponent=other.gameObject.GetComponent(typeof(IDamageable));
        if (damagableComponent) {
            (damagableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }
}
