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
		if(photonView.isMine)
		{
			if (localPlayer == null)
				localPlayer = this;
		}

		myHammer = GetComponentInChildren<Hammer>();
	}

	private void Update()
	{
		if(photonView.isMine)
		{
			if(PInput.WhackButton)
			{
				PAnimator.SetBool("Whack", true);
			}
		}
	}

	public void EnableHammerHitbox()
	{
		//myHammer.Col.enabled = true;
		hammerCol.enabled = true;
	}

	public void DisableHammerHitbox()
	{
		//myHammer.Col.enabled = false;
		hammerCol.enabled = false;
	}

	/// <summary>
	/// Knocks the player down and become invincible for a short period of time
	/// </summary>
	public void Whacked()
	{
		isInvincible = true;
		PAnimator.SetBool("KnockedDown", true);

		//Vector3 otherPos = other.transform.position;
		//Vector3 direction = (transform.position - otherPos).normalized;
		//direction.y = 0;

		StartCoroutine("InvincibilityFlicker");
		StartCoroutine("ResetKnockedDown");
		//StartCoroutine("KnockAway", direction);
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
		//time is up, reset materials
		isInvincible = false;
		for (int i = 0; i < bodyMaterials.Length; i++)
		{
			bodyMaterials[i].color = startColors[i];
		}
	}

	private IEnumerator KnockAway(Vector3 dir)
	{
		float timer = 0;
		float inTime = .5f;
		Vector3 startPos = transform.position;
		float distance = 2f;
		Vector3 endPos = transform.position + (dir * distance);
		while(timer <= inTime)
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp(startPos, endPos, timer / inTime);
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator ResetKnockedDown()
	{
		float timer = 0;
		float inTime = .5f;
		while(timer <= inTime)
		{
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		PAnimator.SetBool("KnockedDown", false);
	}
}
