using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {
	public Rigidbody myRididbody;
	public float forceMin;
	public float forceMax;

	float lifetime =4;
	float fadetime =2;
	// Use this for initialization
	void Start () {
		float force = Random.Range (forceMin, forceMax);
		myRididbody.AddForce (transform.right * force);	
		myRididbody.AddTorque (Random.insideUnitSphere * force);
		StartCoroutine (Fade ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Fade(){
		yield return new WaitForSeconds (lifetime);

		float percent = 0;
		float fadeSpeed = 1 / fadetime;
		Material mat = GetComponent<Renderer> ().material;
		Color initialColor = mat.color;
		while (percent < 1) {
			percent += Time.deltaTime * fadeSpeed;
			mat.color = Color.Lerp (initialColor, Color.clear, percent);
			yield return null;
		}

		Destroy (gameObject);
	}

}
