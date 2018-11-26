using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.MonoBehaviour{

	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	public bool isOnline = true;

	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	private GameObject playerUiPrefab;

	private GameObject spawnedPlayer;

	public delegate void OnPlayerSpawned();
	public OnPlayerSpawned onPlayerSpawned;

	public delegate void OnStartGame();
	public OnStartGame onStartGame;

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
			StartGame();
		}
		else if(PhotonNetwork.isMasterClient)
		{
			StartGame();
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
}
