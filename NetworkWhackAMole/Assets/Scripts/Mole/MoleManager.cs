using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleManager : MonoBehaviour {

	private static MoleManager instance;
	public static MoleManager Instance { get { return instance; } }

	[SerializeField, Tooltip("The point off screen where the moles will spawn")]
	private Transform moleSpawnPoint;
	[SerializeField, Tooltip("The holes from which the moles will appear from")]
	private Transform[] moleHoles;

	private BigMole bigMole;
	private DynamiteMole[] dynamiteMoles;
	[SerializeField, Tooltip("The max amount of dynamite moles spawned")]
	private int dynamiteMolesAmount;
	private int activeDynamiteMolesAmount = 1;

	public delegate void OnRaiseMoles();
	public OnRaiseMoles onRaiseMoles;

	public delegate void OnRetractMoles();
	public OnRetractMoles onRetractMoles;

	private void Awake()
	{
		if(PhotonNetwork.isMasterClient)
		{
			if (instance == null)
				instance = this;
			else
				Destroy(gameObject);

			//Spawn and set reference to the big mole
			bigMole = PhotonNetwork.Instantiate("BigMole", moleSpawnPoint.position, Quaternion.identity, 0).GetComponent<BigMole>();

			//Spawn the dynamite moles and add them to the array
			for (int i = 0; i < dynamiteMolesAmount; i++)
			{
				GameObject go = PhotonNetwork.Instantiate("DynamiteMole", moleSpawnPoint.position, Quaternion.identity, 0);
				dynamiteMoles[i] = go.GetComponent<DynamiteMole>();
			}
		}
	}

	//TODO Should listen for the game manager to when it should raise and retract moles;
	private void RaiseMoles()
	{
		SetMolesTargetHoles();

		if (onRaiseMoles != null)
			onRaiseMoles.Invoke();
		//for every time we raise the moles, we increase the amount of dynamite moles for the next time
		activeDynamiteMolesAmount++;
		activeDynamiteMolesAmount = Mathf.Clamp(activeDynamiteMolesAmount, 0, dynamiteMoles.Length);
	}

	/// <summary>
	/// Sets all of the moles target holes, priorities the big moles hole first to not repeat the same hole, then sets the dynamites holes
	/// </summary>
	private void SetMolesTargetHoles()
	{
		List<Transform> possibleHoles = new List<Transform>();
		for (int i = 0; i < moleHoles.Length; i++)
		{
			if (moleHoles[i] != bigMole.PreviousHole)
				possibleHoles.Add(moleHoles[i]);
		}

		int randomHoleIndex = Random.Range(0, possibleHoles.Count - 1);
		bigMole.SetTargetHole(possibleHoles[randomHoleIndex]);

		//Set the dynamite moles holes
		//First clear the possible holes and re-add all of the holes to the list except the big mole's
		possibleHoles.Clear();
		for (int i = 0; i < moleHoles.Length; i++)
		{
			if (moleHoles[i] != bigMole.TargetHole)
				possibleHoles.Add(moleHoles[i]);

		}
		//set the dynamite mole's target hole
		for (int i = 0; i < activeDynamiteMolesAmount; i++)
		{
			randomHoleIndex = Random.Range(0, possibleHoles.Count - 1);
			dynamiteMoles[i].SetTargetHole(possibleHoles[randomHoleIndex]);
			//remove the previous mole's hole so it can't be used again
			possibleHoles.RemoveAt(randomHoleIndex);
		}
	}

	private void RetractMoles()
	{
		if (onRetractMoles != null)
			onRetractMoles.Invoke();
	}
}
