using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : PlayerBase {

	//Handles miscellaneous tasks, a center point for other components to access
	//Also currently takes care of animations

	[SerializeField]
	private float invincibilityTime = 2f;
	private float timer;

	private Hammer myHammer;
	public Hammer MyHammer { get { return myHammer; } }

	[SerializeField]
	private Collider hammerCol;

	private bool isInvincible;
	public bool IsInvincible { get { return isInvincible; } }

	private void Awake()
	{
		myHammer = GetComponentInChildren<Hammer>();
	}

	private void Update()
	{
		if(photonView.isMine)
			PAnimator.SetBool("Whack", PInput.WhackButton);

		ProcessInvincibility();
	}

	private void ProcessInvincibility()
	{
		if (isInvincible)
		{
			timer += Time.deltaTime;
			if (timer >= invincibilityTime)
				InvincibleTimeOver();
		}
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
		timer = 0;
		PMovement.targetSpeed = PMovement.StunnedMoveSpeed;
		StartCoroutine("InvincibilityFlicker");
	}

	public void InvincibleTimeOver()
	{
		timer = 0;
		isInvincible = false;
		PMovement.targetSpeed = PMovement.MoveSpeed;
	}

	//Not tested at all, going to be used as visual cue for taking damage
	private IEnumerator InvincibilityFlicker()
	{
		Material[] bodyMaterials = GetComponentsInChildren<Material>();
		Color[] startColors = new Color[bodyMaterials.Length];
		Color[] targetColors = new Color[bodyMaterials.Length];
		for (int i = 0; i < bodyMaterials.Length; i++)
		{
			startColors[i] = bodyMaterials[i].color;
			targetColors[i] = startColors[i];
			targetColors[i].a = 0;
		}
		float timer  = 0;
		float inTime = invincibilityTime;
		float currentValue;

		while(timer <= invincibilityTime)
		{
			currentValue = timer / inTime;
			currentValue = Mathf.Sin(currentValue * Mathf.PI * .5f);
			for (int i = 0; i < bodyMaterials.Length; i++)
			{
				bodyMaterials[i].color = Color.Lerp(startColors[i], targetColors[i], currentValue);
			}
			yield return new WaitForEndOfFrame();
		}
		for (int i = 0; i < bodyMaterials.Length; i++)
		{
			bodyMaterials[i].color = startColors[i];
		}
	}
}
