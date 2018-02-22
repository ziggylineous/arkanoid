using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBoardPaint : MonoBehaviour {

    public Material paintMaterial;
    private RenderTexture renderTex;
    private Transform ballPosition;
    private Camera thisCamera;
    public RuntimeSet balls;

	// Use this for initialization
	void Start () {
        Debug.Log("BALL BOARD PAINT START");
        renderTex = CreateRenderTexture();

		paintMaterial.SetVector("_HalfScreen", new Vector4(Screen.width * 0.5f, Screen.height * 0.5f));
        paintMaterial.SetTexture("_PrevFrameTex", renderTex);

        GetComponent<Renderer>().material.mainTexture = renderTex;

        balls.OnAdd += OnBallAdded;
        balls.OnRemove += OnBallRemoved;

        enabled = ballPosition != null;

        thisCamera = Camera.main;
	}

    private RenderTexture CreateRenderTexture() {
        RenderTexture rTex = new RenderTexture(Screen.width / 4, Screen.height / 4, 0, RenderTextureFormat.ARGB32);

        if (!rTex.IsCreated())
            rTex.Create();

        Debug.LogFormat("widt = {0}, high = {1}", rTex.width, rTex.height);

        return rTex;
    }
	
	void Update () {
        Vector3 ballScreenPos = thisCamera.WorldToScreenPoint(ballPosition.position);
        //Debug.LogFormat("position {0}", ballScreenPos);
        paintMaterial.SetVector("_Position", new Vector4(ballScreenPos.x, ballScreenPos.y, 0.0f, 0.0f));
        Graphics.Blit(renderTex, renderTex, paintMaterial);
	}

    private void OnBallAdded(GameObject ballGameObj) {
        BallPosition = ballGameObj.transform;
        Debug.LogFormat("onBallAdded; enabled = {0}", enabled);
    }

    private void OnBallRemoved(GameObject ballGameObj) {
        if (ballPosition == ballGameObj.transform) {
            ballPosition = null;
            enabled = false;
        }

        Debug.LogFormat("onBallRemoved; enabled = {0}", enabled);
    }

    public Transform BallPosition {
        get { return ballPosition; }
        set {
            ballPosition = value;
            enabled = ballPosition != null;
        }
    }

    public void OnDestroy()
    {
        Debug.Log("ball add/remove listener destroyed");
        balls.OnAdd -= OnBallAdded;
        balls.OnRemove -= OnBallRemoved;
    }
}
