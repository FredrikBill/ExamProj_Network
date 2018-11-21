using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerReadyPanel : Photon.MonoBehaviour, IPunObservable {

	[Header("Player Info")]

	[SerializeField]
	private TextMeshProUGUI playerNameText;

	[HideInInspector]
	public PhotonPlayer myPlayer;

	[Header("Ready Images")]

	[SerializeField]
	private Image readyImage;

	[SerializeField]
	private Sprite readyMark;
	[SerializeField]
	private Sprite notReadyMark;

	private bool isReady = false;
	public bool IsReady { get { return isReady; } }

	private void Awake()
	{
		transform.parent = ReadyChecker.Instance.transform;
		isReady = false;
		readyImage.sprite = notReadyMark;
		playerNameText.text = photonView.owner.NickName;
		ReadyChecker.Instance.PlayerPanels.Add(this);
	}

	private void Update()
	{
		if (photonView.isMine)
		{
			if (Input.GetButtonDown("Jump"))
			{
				isReady = !isReady;
				UpdateReadyState();
			}
		}
	}

	private void UpdateReadyState()
	{
		readyImage.sprite = (isReady) ? readyMark : notReadyMark;
		if(isReady)
		{
			ReadyChecker.Instance.PlayerIsReady();
		}
	}

	public void SetPlayerInfo(string newPlayerName)
	{
		playerNameText.text = newPlayerName;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			stream.SendNext(isReady);
		}
		else
		{
			isReady = (bool)stream.ReceiveNext();
			UpdateReadyState();
		}
	}
}
