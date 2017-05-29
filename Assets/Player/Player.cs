﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable{

    [SerializeField]
    float currentHealthPoints=100f;

    [SerializeField]
    float maxHealthPoints = 100f;

    public float healthAsPercentage { get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    public void TakeDamage(float damage) {
        currentHealthPoints =Mathf.Clamp(currentHealthPoints-damage,0f,maxHealthPoints);
    }
}
