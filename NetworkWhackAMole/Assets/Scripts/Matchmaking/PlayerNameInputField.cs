using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{

	private static string playerNamePrefKey = "PlayerName";

	private void Start()
	{
		string defaultName = "";

		InputField inputField = GetComponent<InputField>();

		if(inputField != null)
		{
			if(PlayerPrefs.HasKey(playerNamePrefKey))
			{
				defaultName = PlayerPrefs.GetString(playerNamePrefKey);
				inputField.text = defaultName;
			}
		}

		PhotonNetwork.playerName = defaultName;
	}

	public void SetPlayerName(string value)
	{
		PhotonNetwork.playerName = value + " ";

		PlayerPrefs.SetString(playerNamePrefKey, value);
	}
}
