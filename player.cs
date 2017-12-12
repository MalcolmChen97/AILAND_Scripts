using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(player_controller))]
[RequireComponent (typeof(gun_controller))]
public class player : LivingEntity {
	public Image healthbar;
	public Image energybar;
	public Text healthtext;
	public Text energytext;
	public event System.Action OnDeath;
	public GameObject gameoverUI;
	public float initialSpeed=5;
	public float moveSpeed = 5;
	public float runSpeed = 15;
	player_controller controller;
	gun_controller gunController;
	public float energy=50;
	// Use this for initialization
	public override void Start () {
		base.Start ();
		controller = GetComponent<player_controller> ();
		gunController = GetComponent<gun_controller> ();
		InvokeRepeating ("restore", 1, 2);
	}

	void restore(){
		addhealth (1);
		addenergy (2);
	}

	// Update is called once per frame
	void Update () {
		//UpdateUI
		updateUI();


		//Movement Input
		Vector3 moveInput=new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.move (moveVelocity);

		//Look Input
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;
		if(Physics.Raycast(camRay, out floorHit))
		{
			Vector3 targetPosition = new Vector3(floorHit.point.x, transform.position.y,
				floorHit.point.z);
			transform.LookAt(targetPosition);
			Debug.DrawLine (transform.position, targetPosition);
			gunController.Aim (targetPosition);
		}
		if (Input.GetKey (KeyCode.LeftShift)) {
			run ();
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			moveSpeed = initialSpeed;
		}

		//weapon input
		if(Input.GetMouseButton(0)){
			gunController.OnTriggerHold ();
		}
		if(Input.GetMouseButtonUp(0)){
			gunController.OnTriggerRelease ();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			gunController.Reload ();
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			gunController.nextGun ();
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			adddamage (1);
			Debug.Log ("current damage:"+gunController.currentDamage());
		}


	}

	void run(){
		if (energy > 0) {
			energy -= Time.deltaTime * 10;
			moveSpeed = runSpeed;
		} else {
			moveSpeed = initialSpeed;
		}
	}

	void updateUI(){
		healthbar.fillAmount = health / 100;
		energybar.fillAmount = energy / 100;
		healthtext.text = health + "/100";
		energytext.text = energy + "/100";
		if (health <= 0) {
			gameoverUI.SetActive (true);
		}
	}

	public void addhealth(int addhealth){
		if (health + addhealth <= 100) {
			health += addhealth;
		} else {
			health = 100;
		}
	}

	public void addenergy(float addenergy){
		if (energy + addenergy <= 100) {
			energy += addenergy;
		} else {
			energy = 100;
		}
	}

	public void adddamage(int adddamage){
		gunController.adddamage (adddamage);
	}

	public void addweapon(gun newgun){
		gunController.addweapon (newgun);
	}


}
