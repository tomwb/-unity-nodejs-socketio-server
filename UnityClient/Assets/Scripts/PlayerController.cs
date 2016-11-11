using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using SimpleJSON;

public class PlayerController : MonoBehaviour {

	public GameObject player; 

	private float speed = 10f;

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate ()
	{
		if (player.activeSelf) {
		float moveHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
			float moveVertical = CrossPlatformInputManager.GetAxis ("Vertical");

			Vector2 movement = new Vector2 (moveHorizontal * speed, moveVertical * speed);
			player.GetComponent<Rigidbody2D> ().velocity = movement;

			if ( moveHorizontal != 0 || moveVertical != 0 ) {
				this.SendMessage ("setMyMovement", player.transform.position);
			}
		}
	}

	public void StartGamePlayer ( string data ) {
		Vector2 position = new Vector2( -15, 4 );
		if ( int.Parse(JSONNode.LoadFromBase64 (data)["POSITION"]) == 1 ) {
			position = new Vector2( 15, 4 );
		}
		player.transform.position = position;
		player.SetActive (true);
	}
}
