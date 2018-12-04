using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	[Header("SceneReferences")]

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

		PhotonNetwork.offlineMode = !isOnline;
	}

	private void Start()
	{
		SpawnPlayer();
		//Run some kind of intro
		if(PhotonNetwork.offlineMode)
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
				spawnedPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity, 0);
				if (playerUiPrefab != null)
					PhotonNetwork.Instantiate(this.playerUiPrefab.name, Vector3.zero, Quaternion.identity, 0);
			}

			else
			{
				spawnedPlayer = Instantiate(this.playerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
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
	}

	public void GameTimeOver()
	{
		PlayerBase[] players = FindObjectsOfType<PlayerBase>();
		for (int i = 0; i < players.Length; i++)
		{
			if (PhotonNetwork.offlineMode)
				players[i].PMovement.SetMovementEnabled(false);
			else
				players[i].PMovement.photonView.RPC("SetMovementEnabled", PhotonTargets.AllBufferedViaServer, new object[] { false });
		}
	}
}
