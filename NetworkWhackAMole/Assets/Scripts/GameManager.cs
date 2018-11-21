using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.MonoBehaviour{

	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	public bool isOnline = true;

	[SerializeField]
	private GameObject playerPrefab;

	private void Awake()
	{
		PhotonNetwork.offlineMode = !isOnline;
	}

	private void Start()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		if(playerPrefab != null)
		{
			if (isOnline)
				PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 5f, 0), Quaternion.identity, 0);
			else
				Instantiate(this.playerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
		}
	}
}
