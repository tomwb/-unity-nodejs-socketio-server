using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

	public GameObject canvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// edito o canvas para a etapa waitRoom
	public void JoinRoom () {
		canvas.transform.FindChild ("BtnJoinRoom").gameObject.SetActive (false);
		canvas.transform.FindChild ("PnlTopBar").gameObject.SetActive (true);
		canvas.transform.FindChild ("TxtWaitEnemy").gameObject.SetActive (true);
	}

	public void SetRoomName ( string newName ) {
		Text txtRoomName = canvas.transform.FindChild ("PnlTopBar/TxtRoom").gameObject.GetComponent<Text> ();
		txtRoomName.text = newName;
	}

	public void SetTotalPlayers ( string total ) {
		Text txtTotalPlayers = canvas.transform.FindChild ("PnlTopBar/TxtPlayers").gameObject.GetComponent<Text> ();
		txtTotalPlayers.text = total;

		if (int.Parse (total) >= 1) {
			canvas.transform.FindChild ("TxtWaitEnemy").gameObject.SetActive (false);
			canvas.transform.FindChild ("BtnReady").gameObject.SetActive (true);
		}
	}

	// edito o canvas para a etapa ready
	public void Ready () {
		canvas.transform.FindChild ("BtnReady").gameObject.SetActive (false);
		canvas.transform.FindChild ("TxtWaitEnemy").gameObject.SetActive (true);
	}

	public void StartGameHud () {
		canvas.transform.FindChild ("MobileSingleStickControl").gameObject.SetActive (true);
		canvas.transform.FindChild ("TxtWaitEnemy").gameObject.SetActive (false);
	}
}
