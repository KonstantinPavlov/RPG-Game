using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter; // For testing purporses

        const float DESTROY_DELAY = 0.01f;
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

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }
        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != shooter.layer)
            {
                DealDamage(collision);
            }
        }
        private void DealDamage(Collision collision)
        {
            Component damagableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damagableComponent)
            {
                (damagableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}
