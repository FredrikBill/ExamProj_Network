using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartCountDown : Photon.MonoBehaviour
{
	private Animator anim;

	private void Awake()
	{
		PhotonNetwork.AllocateViewID();
		anim = GetComponent<Animator>();
		GameManager.Instance.onStartCountDown += StartCountingDown;
	}

	[PunRPC]
	private void StartCountingDown()
	{
		anim.SetBool("CountDown", true);
		if(PhotonNetwork.isMasterClient)
			photonView.RPC("StartCountingDown", PhotonTargets.AllBufferedViaServer);
	}
}
