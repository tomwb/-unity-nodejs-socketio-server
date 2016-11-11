using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class EnemyController : MonoBehaviour {

	public GameObject enemy;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	void createEnemy ( string data ) {
		Vector2 position = new Vector2( -15, 4 );
		if ( int.Parse(JSONNode.LoadFromBase64 (data)["POSITION"]) == 1 ) {
			position = new Vector2( 15, 4 );
		}

		GameObject instance = Instantiate (enemy, position, transform.rotation) as GameObject;
		instance.name = JSONNode.LoadFromBase64 (data)["ID"];
	}

	void EnemyMovement ( string data ) {
		GameObject moveEnemy = GameObject.Find( JSONNode.LoadFromBase64 (data)["ID"] );
		moveEnemy.transform.position = new Vector2 ( float.Parse( JSONNode.LoadFromBase64 (data)["POSITION"]["x"] ), 
			float.Parse( JSONNode.LoadFromBase64 (data)["POSITION"]["y"] ) );
	}
}
