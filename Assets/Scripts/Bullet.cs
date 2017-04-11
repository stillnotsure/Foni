using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Vector3 moveDirection;
    public float speed = 1f;

    public void SetDirection(Vector3 direction) {
        moveDirection= direction.normalized;
    }
	
	void FixedUpdate() {
        transform.position += moveDirection * speed;
	}
}
