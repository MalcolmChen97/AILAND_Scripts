using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {
	public GameObject flashHolder;
	public float flashtime;
	public Sprite[] flashSprites;
	public SpriteRenderer[] spriteRenderers;


	void Start(){
		Deactivate ();
	
	}

	public void Activate(){
		flashHolder.SetActive (true);
		int flashSpriteIndex = Random.Range (0, flashSprites.Length);
		for (int i = 0; i < spriteRenderers.Length; i++) {
			spriteRenderers [i].sprite = flashSprites [flashSpriteIndex];
		
		}



		Invoke ("Deactivate", flashtime);
	}

	public void Deactivate(){
		flashHolder.SetActive (false);

	}
}
