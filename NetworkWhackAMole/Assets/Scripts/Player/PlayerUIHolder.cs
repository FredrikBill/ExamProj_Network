using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHolder : MonoBehaviour {

	private static PlayerUIHolder instance;
	public static PlayerUIHolder Instance { get { return instance; } }

	private List<PlayerUI> playerUi = new List<PlayerUI>();
	public List<PlayerUI> PlayerUi { get { return playerUi; } }

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);


	}
}
