using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : PlayerBase {

	//Handles the players input

	private float horizontalInput;
	private float verticalInput;
	private Vector2 inputDir;
	private bool whackButton;

	public float HorizontalInput { get { return horizontalInput; } }
	public float VerticalInput { get { return verticalInput; } }
	public Vector2 InputDir { get { return inputDir; } }
	public bool WhackButton { get { return whackButton; } }

	private void Update()
	{
		if(photonView.isMine)
		{
			horizontalInput = Input.GetAxisRaw("Horizontal");
			verticalInput = Input.GetAxisRaw("Vertical");

			inputDir.x = horizontalInput;
			inputDir.y = verticalInput;
			inputDir.Normalize();

			whackButton = Input.GetKeyDown(KeyCode.Space);
		}
	}
}
