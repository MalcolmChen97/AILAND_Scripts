using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class player_controller : MonoBehaviour {

	Rigidbody myrigidbody;
	Vector3 velocity;

	void Start () {
		myrigidbody = GetComponent<Rigidbody> ();
	}
	

	void FixedUpdate () {
		myrigidbody.MovePosition (myrigidbody.position + velocity * Time.fixedDeltaTime);

	}

	public void move(Vector3 _velocity){
		velocity = _velocity;

	}
}
