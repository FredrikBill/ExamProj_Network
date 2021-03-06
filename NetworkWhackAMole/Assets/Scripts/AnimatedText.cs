﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[ExecuteInEditMode]
public class AnimatedText : MonoBehaviour {

	//Since Text Mesh Pro's fields doesn't work with animations-
	//this class has options for changing the text and color 

	public Color color;
	[SerializeField, Tooltip("The Text you want to animate, if left null, it will attempt to get text on the same game object")]
	private TextMeshProUGUI text;

	private string t;
	private Color32 tmpColor;

	private void Awake()
	{
		if (text == null)
			if (GetComponent<TextMeshProUGUI>() != null)
				text = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (text == null)
			return;

		tmpColor = text.color;
		tmpColor.a = (byte)(color.a * 255);
		tmpColor.r = (byte)(color.r * 255);
		tmpColor.g = (byte)(color.g * 255);
		tmpColor.b = (byte)(color.b * 255);

		text.color = tmpColor;
	}

	public void SetText(string s)
	{
		text.text = s;
	}
}
