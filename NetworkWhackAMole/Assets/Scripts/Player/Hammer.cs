using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour {

	private Collider col;
	public Collider Col { get { return col; } }

	private void Awake()
	{
		col = GetComponentInChildren<Collider>();
		col.enabled = false;
	}
}
