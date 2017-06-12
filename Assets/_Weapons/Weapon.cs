using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class Weapon : ScriptableObject
    {

        public Transform gripTransform;
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetWeaponAnimationClip()
        {
            ClearAnimationEvents();
            return attackAnimation;
        }
        // Clearing animation event list, if we importing some pack
        private void ClearAnimationEvents()
        {             
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}
