using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Need to standardize enemy animations
/// EACH ENEMY NEEDS
/// idle
/// chase
/// attack
/// die
/// This way we can just set which one to play via state
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyStateManager))]
public class EnemyAnimation : MonoBehaviour
{
    private Animator animationManager;
    private EnemyStateManager stateManager;

    public void Start()
    {
        animationManager = GetComponent<Animator>();
        stateManager = GetComponent<EnemyStateManager>();
    }

    void Update()
    {
        animationManager.SetInteger("State", (int)stateManager.currentState);

    }
}
