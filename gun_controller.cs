using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_controller : MonoBehaviour {
	gun equippedGun;
	public Transform weaponhold;

	public gun[] allGuns=new gun[10];
	public int startingGunIndex =0;
	public int currentIndex;
	int numberofgun=1;
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
		if (currentIndex == numberofgun) {
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

	public void addweapon(gun newgun){
		allGuns [numberofgun] = newgun;
		numberofgun++;
	}

	public void adddamage(int damage){
		for (int i = 0; i < numberofgun; i++) {
			allGuns [i].adddamage (damage);
		}
	}
	public int currentDamage(){
		return allGuns [currentIndex].getdamage ();
	}

}
