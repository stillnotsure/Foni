using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour {

    PlayerMovement playerScript;

    void Start() {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.transform.tag == "Enemy") {
            playerScript.enemyInAttackZone = true;
        }
    }

    void OnTriggerExit(Collider coll) {
        if (coll.transform.tag == "Enemy") {
            playerScript.enemyInAttackZone = false;
        }
    }
}
