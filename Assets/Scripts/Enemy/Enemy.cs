using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Animator animator;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage() {
        animator.SetTrigger("TakeDamage");
    }

}
