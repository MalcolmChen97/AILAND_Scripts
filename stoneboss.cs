using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class stoneboss : LivingEntity {
	public GameObject deathEffect;
	public GameObject attackEffect;
	public GameObject shootEffect;
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

	float shootDistanceThreshold = 13;
	float attackDistanceThreshold = 13;
	float timebetweenAttcks = 3;
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
		if (Input.GetKeyDown (KeyCode.P)) {
			startBattle ();
			Debug.Log ("start");
		}
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
		pathfinder.enabled = false;
		m_animater.SetTrigger ("shoot");
		yield return new WaitForSeconds (2f);
		pathfinder.enabled = true;
	}

	IEnumerator Attack(){
		pathfinder.enabled = false;
		m_animater.SetTrigger ("haha");
		yield return new WaitForSeconds (1);
		pathfinder.enabled = true;
	}

	void attackresult(){
		
		if (whether_in_jinzhan) {
			
			targetEntity.TakeDamage (damge);
			Destroy(Instantiate (attackEffect,targetEntity.transform.position,Quaternion.identity) as GameObject,2);
		}
	}

	void shootresult(){
		


		StartCoroutine (finalshoot ());
		Destroy(Instantiate (shootEffect,targetEntity.transform.position,Quaternion.identity) as GameObject,2);
	}
	IEnumerator finalshoot (){
		yield return new WaitForSeconds (1);
		Projectile newProjectile = Instantiate (projectile, projectileSpawn.position, projectileSpawn.rotation) as Projectile;
		newProjectile.transform.LookAt (target.position);

		newProjectile.setSpeed (36f);
	}


	void OnTriggerStay(Collider col){
		if(col.CompareTag("Player")){
			whether_in_jinzhan = true;
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
		targetEntity.OnDeath += OnTargetDeath;
		StartCoroutine (UpdatePath ());
	}


	void OnTargetDeath(){
		hasTarget = false;
		currentState = State.Idle;

	}
	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir){

		if (damage >= health) {

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
