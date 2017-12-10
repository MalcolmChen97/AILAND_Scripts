using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

	public float damge = 1;

	public GameObject deathEffect;
	public enum State{
		Idle,Chasing,Attacking
	}

	State currentState;

	NavMeshAgent pathfinder;
	Transform target;
	Light harmlight;
	float attackDistanceThreshold = 0.5f;
	float timebetweenAttcks = 1;
	float nextAttackTime;

	float myCollisionRadius;
	float targetCollisionRadius;

	LivingEntity targetEntity;
	bool hasTarget;

	// Use this for initialization

	public override void Start () {
		base.Start ();
		pathfinder = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		if (GameObject.FindGameObjectWithTag ("Player")) {
			hasTarget = true;
			pathfinder.enabled = false;

			pathfinder.Warp(transform.position);
			currentState = State.Chasing;

			harmlight = transform.GetComponentInChildren<Light>();
			harmlight.enabled = false;
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
		harmlight.enabled = true;
		StartCoroutine (turnofflight ());

	}

	IEnumerator turnofflight(){
		
		yield return new WaitForSeconds (2);
		harmlight.enabled = false;
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

	IEnumerator Attck(){
		currentState = State.Attacking;
		pathfinder.enabled = false;


		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);

		bool hasAppliedDamage = false;
		float attackspeed = 1;
		float percent = 0;
		while (percent <= 1) {
			if (percent >= 0.5f && !hasAppliedDamage) {
				hasAppliedDamage = true;
				targetEntity.TakeDamage (damge);

			}


			percent += Time.deltaTime * attackspeed;
			float interpolation = (-percent * percent + percent) * 4;
			transform.position = Vector3.Lerp (originalPosition, attackPosition, interpolation);

			yield return null;
		}

		pathfinder.enabled = true;
		currentState = State.Chasing;
	}


	IEnumerator UpdatePath(){
		float refreshRate = 0.25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);


				if (!dead) {
					pathfinder.SetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds (refreshRate);

		}
	}

}
