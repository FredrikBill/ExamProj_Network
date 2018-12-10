using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameTimer : Photon.MonoBehaviour {

	[Header("References")]

	[SerializeField]
	private TextMeshProUGUI timeText;

	[Header("Settings")]

	[SerializeField]
	private int gameTime = 60;

	private bool gameStarted;
	private float t;

	private void Awake()
	{
		GameManager.Instance.onStartGame += SetGameStarted;
		timeText.text = gameTime.ToString();
		//allocate an id (target) for the photonView, otherwise we would need to instantiate the object over the network.
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

			//probably a bad idea, but wanted to see if it was possible
			//basically the master client keeps track of the timer but sends the result over the network
			//only send the rpc if the time is closer to a second below the previous, without this check the other player's timers started to stutter
			if(Mathf.RoundToInt(t) < t)
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
		t = time;
		timeText.text = t.ToString("f0");
	}

	private void SetGameStarted()
	{
		gameStarted = true;
		t = gameTime;
	}
}
