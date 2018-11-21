using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkLauncher : Photon.PunBehaviour
{

	//Used in the main menu for matchmaking
	//Handles connecting to the server, a room and the some UI associated with it.

	private static NetworkLauncher instance;
	public static NetworkLauncher Instance { get { return instance; } }

	[Header("Game Settings")]

	[SerializeField]
	private byte maxPlayerPerRoom = 4;

	private static string gameVersion = "1";

	private bool isConnecting;

	[Header("Connection Visuals")]

	[SerializeField]
	private GameObject playButton;
	[SerializeField]
	private GameObject cancelButton;
	[SerializeField]
	private TextMeshProUGUI connectionText;

	[Header("Other")]
	[SerializeField]
	private ReadyChecker readyChecker;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

		connectionText.enabled = false;
		playButton.SetActive(true);
		cancelButton.SetActive(false);
		readyChecker.gameObject.SetActive(false);
		PhotonNetwork.sendRate = 64;
	}

	public void Connect()
	{
		isConnecting = true;
		playButton.SetActive(false);
		cancelButton.SetActive(true);

		if (PhotonNetwork.connected)
		{
			//PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			connectionText.enabled = true;
			connectionText.text = "Connecting to server...";
			PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
	}

	public void Disconnect()
	{
		isConnecting = false;
		playButton.SetActive(true);
		cancelButton.SetActive(false);
		connectionText.enabled = false;
		PhotonNetwork.Disconnect();
	}

	public override void OnConnectedToMaster()
	{
		if (isConnecting)
		{
			connectionText.text = "Connecting to room...";
			PhotonNetwork.JoinRandomRoom();
		}
	}

	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		Debug.Log("No room available, creating a new room");
		connectionText.text = "Creating a new room...";
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayerPerRoom }, null);
	}

	public override void OnJoinedRoom()
	{
		connectionText.text = "Looking for players...";
		if (PhotonNetwork.room.PlayerCount >= 2)
		{
			connectionText.enabled = false;
			ShowReadyCheck();
		}
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		if (PhotonNetwork.room.PlayerCount >= 2)
		{
			ShowReadyCheck();
		}
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		if (PhotonNetwork.room.PlayerCount < 2)
		{
			//Reset any ready checks
			HideReadyCheck();
		}
	}

	private void ShowReadyCheck()
	{
		//show hud element to show ready state
		playButton.SetActive(false);
		cancelButton.SetActive(false);
		readyChecker.gameObject.SetActive(true);
	}

	private void HideReadyCheck()
	{
		cancelButton.SetActive(true);
		readyChecker.gameObject.SetActive(false);
	}
}
