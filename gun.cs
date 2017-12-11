using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour {
	//weapon variation
	public enum FireMode{Auto, Burst, Single}
	public FireMode fireMode;
	bool triggerRelaeasedSinceLastShot;
	public int burstCount;
	int shotsRemainingInBurst;
	public Transform[] projectileSpawn;
	int damagenow;
	public Transform muzzle;
	public Projectile projectile;
	public float msBetweenshots = 100;
	public float muzzleVelocity = 35f;
	public int projectileperMag=20;
	public float reloadTime = 0.3f;

	[Header("Recoil")]
	public Vector2 kickMinMax= new Vector2(0.05f,0.2f);
	public Vector2 recoilAngleMinMax=new Vector2(3f,7f);
	public float recoilMoveSpeed=0.1f;
	public float recoilRotation=0.1f;
	[Header("Effect")]
	public AudioClip shootaudio;
	public AudioClip Reloadaudio;
	public Transform shell;
	public Transform shellEjection;
	float nextShotTime;
	MuzzleFlash muzzleflash;

	int projectilesRemainingInMag;
	bool isReloading=false;

	Vector3 recoilSmoothDampVelopcity;
	float recoilAngle;
	float recoilRotaSmootDampVelocity;
	// Use this for initialization
	void Start () {
		muzzleflash = GetComponent<MuzzleFlash> ();
		damagenow = projectile.getInitialdamage();
		Debug.Log ("starting damage"+damagenow);

		shotsRemainingInBurst = burstCount;
		projectilesRemainingInMag = projectileperMag;
	}

	public int getdamage(){
		return damagenow;
	}


	// Update is called once per frame
	void LateUpdate () {
		//animate recoil
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition,Vector3.zero,ref recoilSmoothDampVelopcity, recoilMoveSpeed);
		recoilAngle = Mathf.SmoothDamp (recoilAngle, 0, ref recoilRotaSmootDampVelocity, recoilRotation);
		transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;
		if(!isReloading && projectilesRemainingInMag==0){
			Reload();
		}
	}

	public void Aim(Vector3 aimPoint){
		if (!isReloading) {
			transform.LookAt (aimPoint);
		}

	
	}


	void Shoot(){
		
		if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag> 0) {
			if (fireMode == FireMode.Burst) {
				if (shotsRemainingInBurst == 0) {
					return;
				}
				shotsRemainingInBurst--;
			} else if (fireMode == FireMode.Single) {
				if (!triggerRelaeasedSinceLastShot) {
					return;
				}
			}

			for (int i = 0; i < projectileSpawn.Length; i++) {
				if (projectilesRemainingInMag == 0) {
					return;
				}
				projectilesRemainingInMag--;
				nextShotTime = Time.time + msBetweenshots / 1000;
				Projectile newProjectile = Instantiate (projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
				newProjectile.setSpeed (muzzleVelocity);
				projectile.setdamage (damagenow);

			}
			Instantiate (shell, shellEjection.position, shellEjection.rotation);
			muzzleflash.Activate ();
			transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x,kickMinMax.y);
			recoilAngle += Random.Range(recoilAngleMinMax.x,recoilAngleMinMax.y);
			recoilAngle = Mathf.Clamp (recoilAngle, 0, 30);

			AudioManager.instance.PlaySound (shootaudio, transform.position);
		}
			
	}

	public void Reload(){
		if (!isReloading && projectilesRemainingInMag != projectileperMag) {
			StartCoroutine (AnimateReload());
			AudioManager.instance.PlaySound (Reloadaudio, transform.position);
		}

	}

	IEnumerator AnimateReload(){
		isReloading = true;
		yield return new WaitForSeconds (0.2f);

		float percent = 0;
		float reloadSpeed=1/reloadTime;
		Vector3 initialRot = transform.localEulerAngles;
		float maxReloadAngle = 30;
		while (percent < 1) {
			percent += Time.deltaTime * reloadSpeed;
			float interpolation = (-percent * percent + percent) * 4;
			float reloadAngle = Mathf.Lerp (0, maxReloadAngle, interpolation);

			transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;
			yield return null;

		}


		isReloading = false;
		projectilesRemainingInMag = projectileperMag;
	}

	public void OnTriggerHold(){
		Shoot ();
		triggerRelaeasedSinceLastShot = false;
	}

	public void OnTriggerRelease(){
		triggerRelaeasedSinceLastShot = true;
	}

	public void adddamage(int damage){
		Debug.Log ("before: I am gun, my damage is:"+damagenow);
		damagenow += damage;

		Debug.Log ("after : I am gun, my damage is:"+damagenow);
	}

}
