using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameTimer : Photon.MonoBehaviour {

	[SerializeField]
	private TextMeshProUGUI timeText;
	[SerializeField]
	private int timeToEnd = 60;

	private bool gameStarted;
	private float t;

	private void Awake()
	{
		GameManager.Instance.onStartGame += SetGameStarted;
		timeText.text = timeToEnd.ToString();
		PhotonNetwork.AllocateViewID();
	}

	private void Update()
	{
		if (!gameStarted)
			return;

		if(PhotonNetwork.offlineMode)
		{
			UpdateTimer();
		}
		else if(PhotonNetwork.isMasterClient)
		{
			UpdateTimer();
			//TODO game timer needs to be instantiated over the network
			photonView.RPC("RPCUpdateTimer", PhotonTargets.AllBufferedViaServer, new object[] { t });
		}
	}

	private void UpdateTimer()
	{
		t -= 1 * Time.deltaTime;

		if (t <= 0)
		{
			t = 0;
			gameStarted = false;
			GameManager.Instance.GameTimeOver();
		}

		timeText.text = t.ToString("f0");
	}

	[PunRPC]
	private void RPCUpdateTimer(float time)
	{
		Debug.Log(time);
		t = time;
		timeText.text = t.ToString("f0");
	}

	private void SetGameStarted()
	{
		gameStarted = true;
		t = timeToEnd;
	}
}
