using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_controller : MonoBehaviour {
	gun equippedGun;
	public Transform weaponhold;

	public gun[] allGuns;
	public int startingGunIndex =0;
	int currentIndex;
	void Start(){
		currentIndex = startingGunIndex;
		if (allGuns != null) {
			EquipGun (allGuns[currentIndex]);
		}
	}

	public void EquipGun(gun gunToEquip){
		if (equippedGun != null) {
			Destroy (equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, weaponhold.position, weaponhold.rotation) as gun;
		equippedGun.transform.parent = weaponhold;
	}

	public void nextGun(){
		currentIndex++;
		if (currentIndex == allGuns.Length) {
			currentIndex = 0;
		}
		EquipGun (allGuns [currentIndex]);
	
	}




	public void OnTriggerHold(){
		if (equippedGun != null) {
			equippedGun.OnTriggerHold ();
		}
	}

	public void OnTriggerRelease(){
		if (equippedGun != null) {
			equippedGun.OnTriggerRelease ();
		}
	}
	public void Aim(Vector3 aimPoint){
		if (equippedGun != null) {
			equippedGun.Aim (aimPoint);
		}
	}

	public void Reload(){
		if (equippedGun != null) {
			equippedGun.Reload();
		}
	}




}
