using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : Photon.MonoBehaviour, IPunObservable {

	private static PlayerUI localPlayerUi;
	public static PlayerUI LocalPlayerUi { get { return localPlayerUi; } }

	private Animator anim;

	[SerializeField]
	private TextMeshProUGUI nameText;
	[SerializeField]
	private TextMeshProUGUI scoreText;
	[SerializeField]
	private TextMeshProUGUI scoreIncreaseText;

	private int score = 0;

	private void Awake()
	{
		//check if this ui belongs to this client
		if(PhotonNetwork.offlineMode)
		{
			if (localPlayerUi == null)
				localPlayerUi = this;

			nameText.text = "Off-liner";
		}
		else
		{
			if (localPlayerUi == null && photonView.isMine)
				localPlayerUi = this;

			nameText.text = photonView.owner.NickName;
		}
		anim = GetComponent<Animator>();
		//set parent to the ui holder
		PlayerUIHolder.Instance.PlayerUi.Add(this);
		transform.parent = PlayerUIHolder.Instance.transform;
		//set text values
		scoreText.text = score.ToString();
	}

	public void IncreaseScore(int value)
	{
		score += value;
		scoreText.text = score.ToString();
		scoreIncreaseText.text = value.ToString();
		anim.SetTrigger("IncreaseScore");
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			stream.SendNext(score);
		}
		else
		{
			score = (int)stream.ReceiveNext();

			scoreText.text = score.ToString();
		}
	}
}
