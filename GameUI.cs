using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {
	public Image fadeplane;

	public GameObject gameoverUI;
	// Use this for initialization
	void Start () {
		FindObjectOfType<player> ().OnDeath += OnGameOver;

	}

	IEnumerator Fade(Color from, Color to, float time){
		float speed = 1 / time;
		float percent = 0;
		while (percent < 1) {
			percent += Time.deltaTime * speed;
			fadeplane.color = Color.Lerp (from, to, percent);
			yield return null;
		
		}
	
	}

	void OnGameOver(){
		StartCoroutine (Fade (Color.clear, Color.black, 1));
		gameoverUI.SetActive (true);

	}

	//Ui input
	public void StartNewGame(){
		SceneManager.LoadScene("withTerrien");
	}

}
