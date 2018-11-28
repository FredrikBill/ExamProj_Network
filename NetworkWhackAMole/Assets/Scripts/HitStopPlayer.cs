using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopPlayer : MonoBehaviour {

	[SerializeField]
	protected AnimationCurve hitstopCurve;

	private Animator anim;

	private IEnumerator hitStop;

	private float currentSpeed;
	public float CurrentSpeed { get { return currentSpeed; } }

	public delegate void UpdateSpeed(float value);
	public UpdateSpeed onUpdateSpeed;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void PlayHitStop()
	{
		hitStop = HitStop();
		StartCoroutine(hitStop);
	}

	private IEnumerator HitStop()
	{
		float timer = 0;
		float inTime = hitstopCurve.keys[hitstopCurve.keys.Length - 1].time;
		while (timer <= inTime)
		{
			timer += Time.deltaTime;
			currentSpeed = hitstopCurve.Evaluate(timer);
			anim.speed = currentSpeed;
			if (onUpdateSpeed != null)
				onUpdateSpeed.Invoke(currentSpeed);
			print(transform.name + " speed is: " + currentSpeed);
			yield return new WaitForEndOfFrame();
		}

		currentSpeed = 1;
		anim.speed = currentSpeed;
	}
}
