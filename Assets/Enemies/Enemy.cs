using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float currentHealthPoints = 100f;
    [SerializeField] float maxHealthPoints = 100f;

    [SerializeField] float attackRadius = 4f;
    [SerializeField] float moveRadius = 5f;

    [SerializeField] float damagePerShot = 4f;
    [SerializeField] float secondsBetweenShots = 4f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSpawnPoint;
    [SerializeField] Vector3 aimOffset = new Vector3(0,1f,0);

    bool isAttacking = false;

    AICharacterControl aiCharacterControl = null;
    GameObject player=null;

    public float healthAsPercentage {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <= 0)
            Destroy(gameObject);
    }

    private void Start() {

        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();

    }

    private void Update() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(distanceToPlayer <= attackRadius && !isAttacking) {
            isAttacking = true;           
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots);            
        }
        if (distanceToPlayer >= attackRadius)
        {
            isAttacking = false;
            CancelInvoke();            
        }
        if (distanceToPlayer <= moveRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

    void SpawnProjectile()
    {        
        GameObject projectile = Instantiate(projectileToUse, projectileSpawnPoint.transform.position,Quaternion.identity,transform);
        projectile.GetComponent<Projectile>().DamageCaused = damagePerShot;
        var projectileSpeed =projectile.GetComponent<Projectile>().projectileSpeed;
        Vector3 unitVector = (player.transform.position + aimOffset - projectileSpawnPoint.transform.position).normalized;
        projectile.GetComponent<Rigidbody>().velocity = unitVector * projectileSpeed;
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
    }

}
