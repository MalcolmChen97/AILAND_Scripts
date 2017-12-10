using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	float speed = 10f;
	public Color trailColor;
	public float damage =1;
	public LayerMask collisionMask;
	float lifetime=3;
	float skinlength=0.1f;

	void Start(){
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



	void OnHitObject(Collider c,Vector3 hitPoint){
		iDamagable damagableObject = c.GetComponent<iDamagable> ();
		if (damagableObject != null) {
			damagableObject.TakeHit (damage,hitPoint,transform.forward);

		}
		GameObject.Destroy (gameObject);
	}

	
}
