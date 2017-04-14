using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    public Animator animator;
    private Rigidbody rb;
    public GameObject bulletPrefab;
    private GameObject enemy;

    private enum states { idle, attacking, chargingDash};
    private const string performAttackString =  "PerformAttack";
    public bool ableToAttack { get; set; } //While this is true, pressing the attack button will set the animation attacking bool to true
    private states state;

    public bool enemyInAttackZone {get; set;}

    public float runSpeed = 0.5f;
    private float currentDashDistance;
    public float minDashDistance = 3f;
    public float maxDashDistance = 8f;
    private float dashChargeDuration = 1.3f;
    public float moveToEnemySpeed = 5f;

    private bool canFire;
    private float fireRate = 0.25f;
    private float fireRateTimer;

    void Start() {
        rb = GetComponent<Rigidbody>();
        enemy = GameObject.Find("Enemy");
        ableToAttack = true;
        canFire = true;
        fireRateTimer = fireRate;
    }

    void OnCollisionEnter(Collision coll) {
        Debug.Log(coll.transform.name);
    }

    void Update() {
        CheckInputs();
        if (!canFire) {
            fireRateTimer -= Time.deltaTime;
            if (fireRateTimer < 0) {
                canFire = true;
                fireRateTimer = fireRate;
            }
        }
    }

	void CheckInputs() {
        if (Input.GetButtonDown("Dash")) {
            currentDashDistance = minDashDistance;
            StartCoroutine(ChargeDash());
        } else 
        if (Input.GetButtonUp("Dash"))
            Dash();
        if (Input.GetButtonDown("Attack"))
            TryAttack();
    }
	
    void TryAttack() {
        if (ableToAttack) {
            state = states.attacking;
            animator.SetBool(performAttackString, true);
            ableToAttack = false;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (state==states.idle)
            transform.Translate((x * runSpeed) * Time.fixedDeltaTime, 0, (y * runSpeed) * Time.fixedDeltaTime, Space.World);
        if (x != 0 || y != 0) {
            transform.rotation = Quaternion.LookRotation(new Vector3(x, 0, y));
        }
        //Todo - Check that this is correct, the original might let you shoot and attack at the same time
        CheckShooting();
    }

    void CheckShooting() {
        float rX = Input.GetAxis("Right Horizontal");
        float rY = Input.GetAxis("Right Vertical");

        if (canFire && (rX != 0 || rY != 0)) {
            Shoot(rX, rY);
            canFire = false;
        }
    }

    void Shoot(float x, float y) {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(new Vector3(x, 0, -y));
        bullet.transform.SetParent(GameObject.FindGameObjectWithTag("Bullet Holder").transform);
    }

    public IEnumerator ChargeDash() {
        state = states.chargingDash;
        //Increase current dash distance by an amount depending on how much time has passed
        //Get percentage of time elapse
        while (Input.GetButton("Dash")) {
            float chargeIncrement = (maxDashDistance - minDashDistance) * (dashChargeDuration * Time.deltaTime);
            currentDashDistance = Mathf.Min(currentDashDistance + chargeIncrement, maxDashDistance);
            yield return null;
        }
        Debug.Log("Charged to " + currentDashDistance);
    }

    void Dash() {
        StopCoroutine("ChargeDash");
        transform.position += transform.forward * currentDashDistance;
        state = states.idle;
    }

    public void AttackImpact(int attackNumber) {
        if (enemyInAttackZone) {
            Debug.Log("Hit the enemy with attack : " + attackNumber);
            StartCoroutine(MoveToEnemy());
            enemy.GetComponent<Enemy>().TakeDamage();
        }
    }

    public IEnumerator MoveToEnemy() {
        Vector3 target = (transform.position + enemy.transform.position) / 2;
        while ((transform.position - target).magnitude > 0.75f) {
            transform.position = Vector3.MoveTowards(transform.position, target, moveToEnemySpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void FinishAttack() {
        state = states.idle;
    }

}
