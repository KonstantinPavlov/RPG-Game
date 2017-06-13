using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] int enemyLayer = 9;
        [SerializeField] float currentHealthPoints = 100f;
        [SerializeField] float maxHealthPoints = 100f;

        [SerializeField] float meleeDamage = 10f;       

        [SerializeField] Weapon weaponInUse;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        CameraRaycaster cameraRaycaster;
        float lastHitTime;
        private Animator animator;

        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / maxHealthPoints;
            }
        }
        public void Start()
        {
            
            RegisterForMouseClick();
            lastHitTime = Time.time;
            PutWeaponInHand();
            SetUpRuntimeAnimator();
        }

        private void SetUpRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT_ATTACK"] = weaponInUse.GetWeaponAnimationClip();
        }

        private void PutWeaponInHand()
        {
            GameObject weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int number = dominantHands.Length;
            Assert.AreNotEqual(number, 0, "No DominantHand found on player, add it!");
            Assert.IsFalse(number > 1, "Multiply DominantHand's found on player, remove it!");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        private void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {

            if (layerHit.Equals(enemyLayer))
            {
                var enemy = raycastHit.collider.gameObject;                

                if (IsTargetInRange(enemy))
                {
                    AttackTarget(enemy);
                }                         
                
            }
        }

        private void AttackTarget(GameObject target)
        {
            var enemyComponent = target.GetComponent<Enemy>();
            if (Time.time - lastHitTime > weaponInUse.MinTimeBetweenHit)
            {
                animator.SetTrigger("Attack");
                enemyComponent.TakeDamage(meleeDamage);
                lastHitTime = Time.time;
            }

        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTraget = (target.transform.position - transform.position).magnitude;
            return distanceToTraget <= weaponInUse.MaxMeleeAttackRange;
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
