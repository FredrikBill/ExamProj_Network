using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : PlayerBase {

	//Handles miscellaneous tasks, a center point for other components to access
	//Also currently takes care of animations

	[SerializeField]
	private AnimationCurve hurtCurve;
	[SerializeField]
	private Color hurtColor;

	[SerializeField]
	private float invincibilityTime
	{
		get
		{
			return hurtCurve.keys[hurtCurve.keys.Length - 1].time;
		}
	}

	private Hammer myHammer;
	public Hammer MyHammer { get { return myHammer; } }

	[SerializeField]
	private Collider hammerCol;

	private bool isInvincible;
	public bool IsInvincible { get { return isInvincible; } }

	private void Awake()
	{
		if (localPlayer == null)
			localPlayer = this;

		myHammer = GetComponentInChildren<Hammer>();
	}

	private void Update()
	{
		if(photonView.isMine)
			PAnimator.SetBool("Whack", PInput.WhackButton);
	}

	public void EnableHammerHitbox()
	{
		//myHammer.Col.enabled = true;
		hammerCol.enabled = true;
		print(hammerCol.enabled);
	}

	public void DisableHammerHitbox()
	{
		//myHammer.Col.enabled = false;
		hammerCol.enabled = false;
		print(hammerCol.enabled);
	}

	/// <summary>
	/// Makes the character take damage, becoming invincible and slowed down
	/// </summary>
	public void Whacked()
	{
		isInvincible = true;
		PMovement.targetSpeed = PMovement.StunnedMoveSpeed;
		StartCoroutine("InvincibilityFlicker");
	}

	//Not tested at all, going to be used as visual cue for taking damage
	private IEnumerator InvincibilityFlicker()
	{
		//Get all of the mesh renderers on the character
		MeshRenderer[] meshRends = GetComponentsInChildren<MeshRenderer>();
		//Get all of the materials on the meshes
		Material[] bodyMaterials = new Material[meshRends.Length];
		for (int i = 0; i < meshRends.Length; i++)
		{
			bodyMaterials[i] = meshRends[i].material;
		}
		//Set a reference to the materials base color
		Color[] startColors = new Color[bodyMaterials.Length];
		for (int i = 0; i < bodyMaterials.Length; i++)
		{
			startColors[i] = bodyMaterials[i].color;
		}

		float timer = 0;
		isInvincible = true;
		PAnimator.SetBool("Whacked", false);

		//Start blinking, time is determined by the last keyframe from the hurtCurve
		while (timer <= invincibilityTime)
		{
			timer += Time.deltaTime;

			for (int i = 0; i < bodyMaterials.Length; i++)
			{
				bodyMaterials[i].color = Color.Lerp(startColors[i], hurtColor, hurtCurve.Evaluate(timer));
			}
			yield return new WaitForEndOfFrame();
		}
		isInvincible = false;
		for (int i = 0; i < bodyMaterials.Length; i++)
		{
			bodyMaterials[i].color = startColors[i];
		}
	}
}
