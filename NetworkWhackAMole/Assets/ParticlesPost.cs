using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPost : MonoBehaviour {

	[SerializeField]
	private RenderTexture rTex;

	[SerializeField]
	private Camera particleCamera;

	private void OnPreRender()
	{
		particleCamera.backgroundColor = new Color(0, 0, 0, 0);
		particleCamera.clearFlags = CameraClearFlags.Depth;

		particleCamera.cullingMask = 1 << LayerMask.NameToLayer("Particles");
		//rTex.depth = 0;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//rTex.width = source.width;
		//rTex.height = source.height;

		//Graphics.Blit(rTex, destination);
		//rTex.
	}
}
