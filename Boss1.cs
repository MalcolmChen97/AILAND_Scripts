using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Boss1 : LivingEntity {


	public float damge = 1;
	public GameObject deathEffect;
	public enum State{
		Idle,Attacking
	}

	public string[] Abilityies = new string[]{"shooting","boom","lasereye"};

	[Header("shooting variables")]
	public float muzzleVelocity=35f;
	public GameObject weaponARM;
	public Transform[] projectileSpawn;
	public Projectile projectile;
	public float msBetweenshots = 100;




	State currentState;


	Transform target;
	Light harmlight;

	float timebetweenAttcks = 4;
	float nextAttackTime;
	gun_controller gunController;
	LivingEntity targetEntity;
	bool hasTarget;

	// Use this for initialization

	public override void Start () {
		base.Start ();
		weaponARM.SetActive (false);
		currentState = State.Idle;
		gunController = GetComponent<gun_controller> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		if (GameObject.FindGameObjectWithTag ("Player")) {
			hasTarget = true;
			harmlight = transform.GetComponentInChildren<Light>();
			harmlight.enabled = false;
			targetEntity = target.GetComponent<LivingEntity> ();


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
		if (currentState == State.Idle) {
			transform.Rotate (new Vector3 (0, 1, 0));
		} else {
			transform.LookAt (target);
		}



	}

	void OnTriggerStay(Collider col){
		if(col.CompareTag("Player")){
			if (Time.time > nextAttackTime) {

				nextAttackTime = Time.time + timebetweenAttcks;
				StartCoroutine (Attck ());

			}
		}
	}


	IEnumerator Attck(){
		currentState = State.Attacking;

		Vector3 originalPosition = transform.position;

		Vector3 attackPosition = target.position;//new Vector3(target.position.x,transform.position.y,target.position.z);
		int usingab = 0;//Random.Range (0, Abilityies.Length-1);

		switch (usingab) 
		{
		case 0:
			int shootround = 6;
			int projectileperMag = 6 * 12;
			weaponARM.SetActive (true);
			while (shootround > 0) {
				yield return new WaitForSeconds (msBetweenshots / 1000);
				for (int i = 0; i < projectileSpawn.Length; i++) {
					projectileSpawn [i].LookAt (attackPosition);
					if (projectileperMag == 0) {
						yield return null;
					}
					projectileperMag--;

					Projectile newProjectile = Instantiate (projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
					newProjectile.setSpeed (muzzleVelocity);;
				}
				shootround--;
			}





			break;
		case 1:
			break;
		case 2:
			break;
			


		}






	}




}
