using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {


	public LivingEntity[] enemies;


	public int number;
	int control;


	void start(){
		control = number;
	}


	void Update(){
		
	}
	void OnTriggerEnter(Collider col){
		if (col.CompareTag ("Player") ) {
			
			for(int i=0;i<number;i++){
				int x = Random.Range (-3, 3);
				int z = Random.Range (-3, 3);
				int enemyindex = Random.Range (0, enemies.Length );

				LivingEntity spawnedEnemy = Instantiate (enemies[enemyindex], transform.position + new Vector3(x,0,z), Quaternion.identity) as LivingEntity;
				control--;
			}
				
		}
	}



}
