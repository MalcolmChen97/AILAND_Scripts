using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class robotboss : LivingEntity {
	public GameObject deathEffect;
	public GameObject attackEffect;
	public GameObject shootEffect;
	public GameObject shootparticle;

	public enum State{
		Idle,Chasing,Attacking
	}

	public Projectile projectile;
	public Transform projectileSpawn;
	public float damge = 40;
	State currentState;
	NavMeshAgent pathfinder;
	Transform target;
	Light harmlight;
	public GameObject pickupgun;
	float shootDistanceThreshold = 13;
	float attackDistanceThreshold = 13;
	float timebetweenAttcks = 4;
	float nextAttackTime;
	LivingEntity targetEntity;
	bool hasTarget;
	Animator m_animater;
	int choose_attack = 0;
	bool whether_in_jinzhan=false;


	// Use this for initialization
	public override void Start () {
		base.Start ();

		pathfinder = GetComponent<NavMeshAgent> ();
		m_animater = GetComponent<Animator> ();
		pathfinder.enabled = false;

		pathfinder.Warp(transform.position);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		currentState = State.Idle;
	}

	// Update is called once per frame
	void Update () {
		
		if (hasTarget) {
			if (Time.time > nextAttackTime) {
				
				choose_attack++;
				float Threshold=0;
				if (choose_attack %2== 0) {
					Threshold = shootDistanceThreshold;
					float sqrDisToTarget = (target.position - transform.position).sqrMagnitude;
					if (sqrDisToTarget < Mathf.Pow (Threshold, 2)) {
						nextAttackTime = Time.time + timebetweenAttcks;
						StartCoroutine (shoot ());
					}
				} else {
					Threshold = attackDistanceThreshold;
					float sqrDisToTarget = (target.position - transform.position).sqrMagnitude;
					if (sqrDisToTarget < Mathf.Pow (Threshold, 2)) {
						nextAttackTime = Time.time + timebetweenAttcks;

						StartCoroutine (Attack());
					}

				}



			}
		}


	}


	IEnumerator shoot(){
		currentState = State.Attacking;
		m_animater.SetTrigger ("shoot");
		yield return new WaitForSeconds (2f);

	}

	IEnumerator Attack(){
		currentState = State.Attacking;
		m_animater.SetTrigger ("haha");

		yield return new WaitForSeconds (1);

	}

	void attackresult(){

		if (whether_in_jinzhan) {

			targetEntity.TakeDamage (damge);
			Destroy(Instantiate (attackEffect,targetEntity.transform.position,Quaternion.identity) as GameObject,2);
		}
		currentState = State.Chasing;
	}

	void shootresult(){



		StartCoroutine (finalshoot ());
		shootparticle.SetActive (true);
		Destroy(Instantiate (shootEffect,targetEntity.transform.position,Quaternion.identity) as GameObject,2);
		currentState = State.Chasing;
	}
	IEnumerator finalshoot (){
		
		for (int i = 0; i < 10; i++) {
			yield return new WaitForSeconds (0.1f);
			Projectile newProjectile = Instantiate (projectile, projectileSpawn.position, projectileSpawn.rotation) as Projectile;
			transform.LookAt(target.position);
			newProjectile.transform.LookAt (target.position);
			newProjectile.setSpeed (36f);
		}


		shootparticle.SetActive (false);
	}


	void OnTriggerStay(Collider col){
		if(col.CompareTag("Player")){
			whether_in_jinzhan = true;
			targetEntity.TakeDamage (1);
		}
	}
	void OnTriggerExit(Collider col){
		if(col.CompareTag("Player")){
			whether_in_jinzhan = false;
		}
	}

	void startBattle (){
		pathfinder.enabled = true;
		hasTarget = true;
		m_animater.SetBool ("run",true);
		currentState = State.Chasing;
		targetEntity = target.GetComponent<LivingEntity> ();
	
		StartCoroutine (UpdatePath ());
	}
	void OnTriggerEnter (Collider col){
		if (col.CompareTag ("Player") ) {
			startBattle ();
		}

	}

	void OnTargetDeath(){
		hasTarget = false;
		currentState = State.Idle;

	}
	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir){

		if (damage >= health) {
			Instantiate (pickupgun, transform.position, Quaternion.identity);
			Destroy(Instantiate (deathEffect,hitPoint,Quaternion.FromToRotation(Vector3.forward,hitDir)) as GameObject,2);
		}

		base.TakeHit ( damage, hitPoint,hitDir);


	}







	IEnumerator UpdatePath(){
		float refreshRate = 0.25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
				m_animater.SetBool ("run",true);




				if (!dead) {
					pathfinder.SetDestination (target.position);
				}
			}
			yield return new WaitForSeconds (refreshRate);

		}
	}

}
