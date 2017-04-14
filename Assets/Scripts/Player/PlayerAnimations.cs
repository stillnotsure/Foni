using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    public PlayerMovement playerScript;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void StopPerformAttack() {
        animator.SetBool("PerformAttack", false );
    }

    public void SetAbleToAttack() {
        playerScript.ableToAttack = true;
    }

    public void AttackImpact(int attackNumber) {
        playerScript.AttackImpact(attackNumber);
    }

    public void FinishAttackState() {
        playerScript.FinishAttack();
    }
}
