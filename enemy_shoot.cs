using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent))]
public class enemy_shoot : LivingEntity {

	public float damge = 1;

	public GameObject deathEffect;
	public enum State{
		Idle,Chasing,Attacking
	}

	State currentState;

	NavMeshAgent pathfinder;
	Transform target;
	public Transform projectileSpawn;
	public Projectile projectile;
	public float attackDistanceThreshold = 30f;
	public float muzzleVelocity=16f;
	float timebetweenAttcks = 1;
	float nextAttackTime;


	float myCollisionRadius;
	float targetCollisionRadius;

	LivingEntity targetEntity;
	bool hasTarget;


	Animator m_animater;
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



	void OnTriggerStay(Collider col){
		if(col.CompareTag("Player")){
			pathfinder.enabled = true;

			hasTarget = true;
			m_animater.SetBool ("run",true);
			currentState = State.Chasing;


			targetEntity = target.GetComponent<LivingEntity> ();
			targetEntity.OnDeath += OnTargetDeath;

			//Subject To CHANGE!!!!!!!
			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;
			StartCoroutine (UpdatePath ());
		}
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




	// Update is called once per frame
	void Update () {
		if (hasTarget) {
			if (Time.time > nextAttackTime) {

				float sqrDisToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDisToTarget < Mathf.Pow (attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) {
					nextAttackTime = Time.time + timebetweenAttcks;
					StartCoroutine (Attck ());
				}
			}
		}

	}
	public int bullet_number = 10;
	IEnumerator Attck(){
		currentState = State.Attacking;
		pathfinder.enabled = false;
		m_animater.SetBool("shooting",true);
		Vector3 attackPosition = target.position +new Vector3(0,1,0);
		for (int i=0;i<10;i++) {
			yield return new WaitForSeconds (100f / 1000);
			projectileSpawn.LookAt (attackPosition);
			Projectile newProjectile = Instantiate (projectile, projectileSpawn.position, projectileSpawn.rotation) as Projectile;
			newProjectile.setSpeed (muzzleVelocity);;
		
		}

		yield return null;
		m_animater.SetBool("shooting",false);

		pathfinder.enabled = true;
		currentState = State.Chasing;
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
