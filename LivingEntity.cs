using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour,iDamagable{
	public float startingHealth;
	protected float health;
	protected bool dead = false;
	public event System.Action OnDeath;

	public virtual void Start(){
		health = startingHealth;
	}


	public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir){


		TakeDamage (damage);
	}


	public virtual void TakeDamage(float damage){

		health -= damage;
		if (health <= 0 && !dead) {
			Die ();
		}

	}


	public void Die(){
		dead = true;
		if (OnDeath != null) {
			OnDeath ();
		}

		GameObject.Destroy (gameObject);
	}


}
