using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.UtilityScripts;
public class GameManager : Photon.PunBehaviour{

	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	[Header("Settings")]

	public bool isOnline = true;

	[Header("Prefabs")]

	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	private GameObject playerUiPrefab;

	[Header("References")]

	[SerializeField]
	private Transform[] playerSpawnPoints;

	private GameObject spawnedPlayer;

	public delegate void OnPlayerSpawned();
	public OnPlayerSpawned onPlayerSpawned;

	public delegate void OnStartGame();
	public OnStartGame onStartGame;

	public delegate void OnStartCountDown();
	public OnStartCountDown onStartCountDown;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		isOnline = PhotonNetwork.connected;
		PhotonNetwork.offlineMode = !isOnline;
	}

	private void Start()
	{
		SpawnPlayer();
		SetAllPlayersCanMove(false);
		//Run some kind of intro
		if (PhotonNetwork.offlineMode)
		{
			if (onStartCountDown != null)
				onStartCountDown.Invoke();

		}
		else if(PhotonNetwork.isMasterClient)
		{
			if (onStartCountDown != null)
				onStartCountDown.Invoke();
		}
	}

	private void StartGame()
	{
		if (onStartGame != null)
			onStartGame.Invoke();
	}

	private void SpawnPlayer()
	{
		if (playerPrefab != null)
		{
			if (isOnline)
			{
				int spawnIndex = (int)PhotonNetwork.player.CustomProperties["PlayerNumber"];
				spawnedPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, playerSpawnPoints[spawnIndex].position, Quaternion.identity, 0);
				if (playerUiPrefab != null)
					PhotonNetwork.Instantiate(this.playerUiPrefab.name, Vector3.zero, Quaternion.identity, 0);
			}

			else
			{
				spawnedPlayer = Instantiate(this.playerPrefab, playerSpawnPoints[0].position, Quaternion.identity);
				if (playerUiPrefab != null)
					Instantiate(playerUiPrefab, Vector3.zero, Quaternion.identity);
			}

			if (onPlayerSpawned != null)
				onPlayerSpawned.Invoke();
		}
	}

	public void CountDownOver()
	{
		if(PhotonNetwork.offlineMode)
			StartGame();
		else if(PhotonNetwork.isMasterClient)
			StartGame();

		SetAllPlayersCanMove(true);
	}

	public void GameTimeOver()
	{
		SetAllPlayersCanMove(false);
		MoleManager.Instance.Stop();
		Debug.Log(PlayerUIHolder.Instance.winningPlayer.Score);
	}

	private void SetAllPlayersCanMove(bool state)
	{
		//Ugly but effective, should be fixed by storing or requesting a reference to all the player characters
		PlayerBase[] players = FindObjectsOfType<PlayerBase>();
		for (int i = 0; i < players.Length; i++)
		{
			if (PhotonNetwork.offlineMode)
				players[i].PMovement.SetMovementEnabled(state);
			else
				players[i].PMovement.photonView.RPC("SetMovementEnabled", PhotonTargets.AllBufferedViaServer, new object[] { state });
		}
	}
}