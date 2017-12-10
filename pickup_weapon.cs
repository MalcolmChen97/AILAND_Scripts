using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup_weapon : MonoBehaviour {
	public gun[] guntypes;
	public int assignedGun;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		if (col.CompareTag ("Player")) {
			player m_player = col.gameObject.GetComponent<player> ();
			m_player.addweapon (guntypes [assignedGun]);
			Destroy (gameObject);
		}
	}
}
