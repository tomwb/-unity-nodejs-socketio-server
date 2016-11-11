using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;
using SimpleJSON;

public class SocketIoController : MonoBehaviour {

	public SocketIOComponent socketIO;

	public string socket_id;

	// Use this for initialization
	void Start () {
		socketIO.On("WAIT_ROOM", WaitRoom);
		socketIO.On ("SET_ID", SetId);
		socketIO.On("START_GAME", StartGameRoom);
		socketIO.On("GAME_MOVEMENT", GameMovement);
		socketIO.On("DISCONNECT", DisconnectGame);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void JoinRoom () {
		socketIO.Emit("JOIN_ROOM");
	}
		
	// entrou na sala
	public void WaitRoom ( SocketIOEvent obj ) {

		var data = JSONNode.Parse (obj.data.ToString());
		this.SendMessage ("SetRoomName", data["ROOM"].ToString().Replace("\"",""));
		this.SendMessage ("SetTotalPlayers", data["TOTAL_PLAYERS"].ToString().Replace("\"","") );

		//if (socket_id != null) {
			//Application.LoadLevel(Application.loadedLevel);
		//}
	}

	public void SetId ( SocketIOEvent obj ) {
		var data = JSONNode.Parse (obj.data.ToString());
		socket_id = data["ID"].ToString ().Replace ("\"", "");
	}

	public void Ready () {
		Debug.Log ( "Estou pronto" );
		socketIO.Emit("PLAYER_READY");
	}

	// começou o jogo
	public void StartGameRoom ( SocketIOEvent obj ) {
		Debug.Log ( "Começou o jogo" );
		this.SendMessage ("StartGameHud");

		// inimigos
		var data = JSONNode.Parse (obj.data.ToString());
		for (int i = 0; i < data["PLAYERS"].Count; i++) {

			string id = data ["PLAYERS"] [i] ["ID"].ToString ().Replace ("\"", "");
			if (socket_id != id) {
				this.SendMessage ("createEnemy", data ["PLAYERS"] [i].SaveToBase64 ());
			} else {
				this.SendMessage ("StartGamePlayer", data ["PLAYERS"] [i].SaveToBase64 ());
			}
		}
	}

	public void setMyMovement ( Vector3 position ) {
		Debug.Log ( "Envio minha posicao" );
		JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
		data.AddField("x", position.x);
		data.AddField("y", position.y);
		data.AddField("z", position.z);
		socketIO.Emit("SET_MOVEMENT", data);
	}

	// recebo um movimento
	public void GameMovement ( SocketIOEvent obj ) {
		Debug.Log ( "recebi um movimento" );
		var data = JSONNode.Parse (obj.data.ToString());
		if (socket_id != data ["ID"].ToString ().Replace ("\"", "")) {
			this.SendMessage ("EnemyMovement", data.SaveToBase64 ());
		}
	}

	public void DisconnectGame ( SocketIOEvent obj ) {
		Application.LoadLevel(Application.loadedLevel);
	}
}