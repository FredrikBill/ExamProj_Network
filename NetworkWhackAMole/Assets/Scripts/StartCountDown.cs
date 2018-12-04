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
		anim.SetTrigger("CountDown");
		if(PhotonNetwork.isMasterClient)
			photonView.RPC("StartCountingDown", PhotonTargets.OthersBuffered);
	}
}
