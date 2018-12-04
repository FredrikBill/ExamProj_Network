using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyChecker : Photon.MonoBehaviour {

	private static ReadyChecker instance;
	public static ReadyChecker Instance { get { return instance; } }

	[SerializeField]
	private GameObject playerPanelPrefab;

	private List<PlayerReadyPanel> playerPanels = new List<PlayerReadyPanel>();
	public List<PlayerReadyPanel> PlayerPanels { get { return playerPanels; } }

	private void Awake()
	{
		if (instance == null)
			instance = this;

		PhotonNetwork.AllocateViewID();
	}

	private void OnEnable()
	{
		//Spawn a panel over the network, each player in the room will spawn their own
		GameObject go = PhotonNetwork.Instantiate(this.playerPanelPrefab.name, Vector3.zero, Quaternion.identity, 0);
		go.transform.parent = transform;
	}

	//Sent by the player panel whenever it's in its ready state
	public void PlayerIsReady()
	{
		for (int i = 0; i < playerPanels.Count; i++)
		{
			if (playerPanels[i].IsReady == false)
				return;
		}

		print("ALL PLAYERS ARE READY, LET'S PLAY");
		//TIME TO START THE GAME
		if(PhotonNetwork.isMasterClient)
			photonView.RPC("LoadGameLevel", PhotonTargets.AllBufferedViaServer);
	}

	[PunRPC]
	private void LoadGameLevel()
	{
		PhotonNetwork.LoadLevel("Game");
	}

	private void OnDisable()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i));
		}

		playerPanels.Clear();
	}

}
