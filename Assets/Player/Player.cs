using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour, IDamageable{

    [SerializeField] int enemyLayer = 9;
    [SerializeField] float currentHealthPoints=100f;
    [SerializeField] float maxHealthPoints = 100f;

    [SerializeField] float meleeDamage = 10f;
    [SerializeField] float minTimeBetweenHit = 1f;
    [SerializeField] float maxMeleeAttackRange = 1.5f;

    [SerializeField] Weapon weaponInUse;
  
    CameraRaycaster cameraRaycaster;
    float lastHitTime;

    public float healthAsPercentage { get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }
    public void Start() {
        RegisterForMouseClick();
        lastHitTime = Time.time;
        PutWeaponInHand();
    }

    private void PutWeaponInHand() {
        GameObject weaponPrefab = weaponInUse.GetWeaponPrefab();
        GameObject dominantHand = RequestDominantHand();
        var weapon = Instantiate(weaponPrefab,dominantHand.transform);
        weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
        weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
    }

    private GameObject RequestDominantHand()
    {
        var dominantHands = GetComponentsInChildren<DominantHand>();
        int number = dominantHands.Length;
        Assert.AreNotEqual(number, 0, "No DominantHand found on player, add it!");
        Assert.IsFalse(number >1,"Multiply DominantHand's found on player, remove it!");
        return dominantHands[0].gameObject;
    }

    private void RegisterForMouseClick() {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }

    private void OnMouseClick(RaycastHit raycastHit, int layerHit) {

        if (layerHit.Equals(enemyLayer)){
            var enemy = raycastHit.collider.gameObject;         

            float distanceToTarget = Vector3.Distance(enemy.transform.position, transform.position);

            if (distanceToTarget <= maxMeleeAttackRange) {

                var enemyComponent = enemy.GetComponent<Enemy>();
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
