using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	float speed = 10f;
	public Color trailColor;
	public int damage =1;
	int dealtdamage;
	public LayerMask collisionMask;
	float lifetime=3;
	float skinlength=0.1f;

	void Start(){
		dealtdamage = damage;
		Debug.Log ("inial projectiledamge"+dealtdamage+"dsasda:"+damage);
		Destroy (gameObject, lifetime);
		Collider[] initialCollisions = Physics.OverlapSphere (transform.position, 0.1f, collisionMask);
		if (initialCollisions.Length > 0) {
			OnHitObject (initialCollisions [0],transform.position);
		}
		GetComponent<TrailRenderer> ().material.SetColor ("_TineColor", trailColor);
	}

	public void setSpeed(float newSpeed){
		speed = newSpeed;
	}
	// Update is called once per frame
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		checkCollisions (moveDistance);
		transform.Translate (Vector3.forward * moveDistance);
	}

	void checkCollisions(float moveDistance){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit,moveDistance+skinlength,collisionMask,QueryTriggerInteraction.Ignore)){
			OnHitObject (hit.collider,hit.point);
		}

	}

	public void setdamage(int whatdamage){
		Debug.Log ("set damage before:"+dealtdamage);
		dealtdamage = whatdamage;
		Debug.Log ("set damage after:"+dealtdamage);
	}

	public int getInitialdamage(){
		return damage;
	}

	void OnHitObject(Collider c,Vector3 hitPoint){
		iDamagable damagableObject = c.GetComponent<iDamagable> ();
		if (damagableObject != null) {
			damagableObject.TakeHit (dealtdamage,hitPoint,transform.forward);
			Debug.Log ("dealt" + dealtdamage);

		}
		GameObject.Destroy (gameObject);
	}

	
}
