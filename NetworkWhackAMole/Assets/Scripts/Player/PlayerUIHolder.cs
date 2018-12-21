using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHolder : MonoBehaviour {

	private static PlayerUIHolder instance;
	public static PlayerUIHolder Instance { get { return instance; } }

	private List<PlayerUI> playerUi = new List<PlayerUI>();
	public List<PlayerUI> PlayerUi { get { return playerUi; } }

	public PlayerUI winningPlayer { get { return GetPlayerHighestScore(); } }

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	public PlayerUI GetPlayerHighestScore()
	{
		PlayerUI player = new PlayerUI();
		for (int i = 0; i < playerUi.Count; i++)
		{
			if(playerUi[i].Score > player.Score)
			{
				player = playerUi[i];
			}
		}

		return player;
	}

	public void DisplayWinner()
	{
		
	}
}
