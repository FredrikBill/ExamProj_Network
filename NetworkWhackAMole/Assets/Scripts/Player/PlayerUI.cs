using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : Photon.MonoBehaviour {

	[SerializeField]
	private TextMeshProUGUI nameText;
	[SerializeField]
	private TextMeshProUGUI scoreText;

	private int score = 0;

	private void Awake()
	{
		nameText.text = photonView.owner.NickName;
		scoreText.text = score.ToString();
	}
}
