using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;

public class GameController : MonoBehaviour {

	public SocketIOComponent socketIO;

	public string ROOM = "room1";  

	// Use this for initialization
	void Start () {
		socketIO.On("ENTER_ROOM", EnterRoom);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ( "Clicou o mouse" );

			// envio o sala
			JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
			data.AddField("ROOM", ROOM);
			socketIO.Emit("SET_ROOM", data);
		}
	}
		
	// entrou na sala
	public void EnterRoom ( SocketIOEvent obj ) {
		Debug.Log ( "chegou do socket" );

		string data = JsonToString(obj.data.GetField("ROOM").ToString(), "\"");
		Debug.Log (obj);
	}

	string  JsonToString( string target, string s){
		string[] newString = Regex.Split(target,s);
		return newString[1];
	}

	Vector3 JsonToVecter3(string target ){
		Vector3 newVector;
		string[] newString = Regex.Split(target,",");
		newVector = new Vector3( float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]));
		return newVector;
	}
}
