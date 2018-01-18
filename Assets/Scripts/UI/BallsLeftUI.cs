using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallsLeftUI : MonoBehaviour {

	public Ship ship;
	public GameObject ballUI;
	public float pad = 2.0f;
	private RectTransform rectTransform;


	void Start () {
		if (ship == null) {
			GameObject shipGameObj = GameObject.Find ("Ship");
			Debug.Assert (shipGameObj != null);
			ship = shipGameObj.GetComponent<Ship> ();
		}

		Debug.Assert (ship != null);

		rectTransform = GetComponent<RectTransform> ();

		ship.BallsLeftChanged += RefreshBallsLeft;

		for (int i = 0; i != ship.ballsLeft; ++i) {
			AddBall ();
		}
	}
	
	void RefreshBallsLeft(int ballsLeftCount) {
		int ballDelta = ballsLeftCount - rectTransform.childCount;

		if (ballDelta < 0) {
			Debug.Assert (ballsLeftCount >= 0);
			Destroy (LastBall);
		} else {
			AddBall ();
		}
	}

	private void AddBall() {
		GameObject newBallIcon = Instantiate (ballUI);
		RectTransform ballRectTranform = newBallIcon.GetComponent<RectTransform> ();
		float w = ballRectTranform.rect.width;
		float positionX = (w + pad) * rectTransform.childCount;
		ballRectTranform.SetParent (rectTransform);
		ballRectTranform.anchoredPosition = new Vector3 (positionX, 0.0f, 0.0f);
	}

	private GameObject LastBall {
		get {
			return rectTransform.GetChild (rectTransform.childCount - 1).gameObject;
		}
	}
}
