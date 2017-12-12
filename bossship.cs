using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bossship : LivingEntity {
	public GameObject deathEffect;
	public GameObject pickupgun;
	public GameObject shootEffect;
	public enum State{
		Idle,Chasing,Attacking
	}
	public float msBetweenshots = 100;
	public float muzzleVelocity=35f;
	public Projectile projectile;
	public Transform[] projectileSpawn;
	public float damge = 1;
	State currentState;
	NavMeshAgent pathfinder;
	Transform target;


	float shootDistanceThreshold = 31;

	float timebetweenAttcks = 3;
	float nextAttackTime;
	LivingEntity targetEntity;
	bool hasTarget;



	// Use this for initialization
	public override void Start () {
		base.Start ();

		pathfinder = GetComponent<NavMeshAgent> ();
		pathfinder.enabled = false;

		pathfinder.Warp(transform.position);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		currentState = State.Idle;
	}

	// Update is called once per frame
	void Update () {
		
		if (hasTarget) {
			transform.LookAt (target.position);
			if (Time.time > nextAttackTime) {



				float sqrDisToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDisToTarget < Mathf.Pow (shootDistanceThreshold, 2)) {
					nextAttackTime = Time.time + timebetweenAttcks;
					StartCoroutine (shoot ());
				}
				



			}
		}


	}
	void OnTriggerEnter (Collider col){
		if (col.CompareTag ("Player") ) {
			Debug.Log ("playerenter");
			startBattle ();
		}

	}

	IEnumerator shoot(){
		
		currentState = State.Attacking;
		Destroy(Instantiate (shootEffect,targetEntity.transform.position,Quaternion.identity) as GameObject,2);
		int projectileperMag =72;
		int shootround = 3;
		while (shootround > 0) {
			yield return new WaitForSeconds (msBetweenshots / 1000);
			for (int i = 0; i < projectileSpawn.Length; i++) {
				projectileSpawn [i].LookAt (target.position);
				if (projectileperMag == 0) {
					
					yield return null;
				}
				projectileperMag--;
				Projectile newProjectile = Instantiate (projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
				newProjectile.setSpeed (muzzleVelocity);;
			}
			shootround--;
		}

		currentState = State.Chasing;


	}









	void startBattle (){
		pathfinder.enabled = true;
		hasTarget = true;

		currentState = State.Chasing;
		targetEntity = target.GetComponent<LivingEntity> ();

		StartCoroutine (UpdatePath ());
	}


	void OnTargetDeath(){
		hasTarget = false;
		currentState = State.Idle;

	}
	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir){

		if (damage >= health) {

			Destroy(Instantiate (deathEffect,hitPoint,Quaternion.FromToRotation(Vector3.forward,hitDir)) as GameObject,2);
			Instantiate (pickupgun, transform.position, Quaternion.identity);
		}

		base.TakeHit ( damage, hitPoint,hitDir);


	}







	IEnumerator UpdatePath(){
		float refreshRate = 0.25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
				




				if (!dead) {
					pathfinder.SetDestination (target.position);
				}
			}
			yield return new WaitForSeconds (refreshRate);

		}
	}

}
