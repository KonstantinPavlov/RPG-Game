using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable{

    [SerializeField] int enemyLayer = 9;
    [SerializeField] float currentHealthPoints=100f;
    [SerializeField] float maxHealthPoints = 100f;

    [SerializeField] float meleeDamage = 10f;
    [SerializeField] float minTimeBetweenHit = 1f;
    [SerializeField] float maxMeleeAttackRange = 1.5f;

    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;
    float lastHitTime;

    public float healthAsPercentage { get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }
    public void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        lastHitTime = Time.time;
    }

    private void OnMouseClick(RaycastHit raycastHit, int layerHit) {

        if (layerHit.Equals(enemyLayer)){
            currentTarget = raycastHit.collider.gameObject;            
            float distanceToTarget = Vector3.Distance(currentTarget.transform.position, transform.position);

            if (distanceToTarget <= maxMeleeAttackRange) {

                var enemyComponent = currentTarget.GetComponent<Enemy>();
                if (Time.time -lastHitTime > minTimeBetweenHit) {
                    enemyComponent.TakeDamage(meleeDamage);
                    lastHitTime = Time.time;
                }
            }                
        }
    }

    public void TakeDamage(float damage) {
        currentHealthPoints =Mathf.Clamp(currentHealthPoints-damage,0f,maxHealthPoints);
        if(currentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
