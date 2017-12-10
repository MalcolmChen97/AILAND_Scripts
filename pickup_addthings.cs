using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup_addthings : MonoBehaviour {
	public int health_enegy_power;
	public int healthvalue;
	public float enegyvalue;
	public int powervalue;





	void OnTriggerEnter(Collider col){
		if (col.CompareTag ("Player")) {
			player m_player = col.gameObject.GetComponent<player> ();

			switch (health_enegy_power)
			{
			case 0:
				m_player.addhealth (healthvalue);
				break;
			case 1:
				m_player.addenergy (enegyvalue);
				break;
			case 2:
				m_player.adddamage (powervalue);
				break;
			default:
				break;
			}



			Destroy (gameObject);
		}
	}
}
