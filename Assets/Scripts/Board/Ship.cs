using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ship : MonoBehaviour {

	public float moveSpeed;
	private Rigidbody2D body;

	public int ballsLeft = 3;
	public List<GameObject> flyingBalls;

	public event DelegateTypes.VoidInt BallsLeftChanged;

	public float[] widths = new float[]{ 1.0f, 1.5f, 2.0f, 2.5f, 3.0f };
	public int defaultWidthIndex = 2;
	private int widthIndex;

	void Start () {
		flyingBalls = new List<GameObject> ();
		widthIndex = defaultWidthIndex;

		Vector2 bottomCenter = WorldPosition.BottomCenter;
		//Debug.Log ("bottomCenter = " + bottomCenter.ToString());
		transform.position = bottomCenter + Vector2.up * 1.0f;

		body = GetComponent<Rigidbody2D> ();

		EnterLaunchingBall ();
	}

	private void EnterLaunchingBall() {
		Debug.Assert (flyingBalls.Count == 0);
		ShipLaunching launching = GetComponent<ShipLaunching> ();
		launching.Enter ();
	}

	public void Launch(GameObject ballGameObj) {
		Ball ball = ballGameObj.GetComponent<Ball> ();

		ball.Launch (new Vector2(1.0f, 1.0f));
		ball.Died += BallDied;

		GetComponent<ShipLaunching>().Exit();
		flyingBalls.Add (ballGameObj);
	}

	void FixedUpdate() {
		HorizontalMovement ();
	}

	void HorizontalMovement() {
		float moveInput = Input.GetAxis ("Horizontal");
		//float vel = moveInput * moveSpeed * Time.fixedDeltaTime;
		//body.MovePosition (body.position + new Vector2(displacement, 0.0f));
		body.velocity = Vector2.right * moveSpeed * moveInput;
	}

	void BallDied(GameObject ball) {

		if (ballsLeft > 0) {
			--ballsLeft;
			Debug.Log ("BallDieds, " + ballsLeft.ToString () + " balls left");
			BallsLeftChanged (ballsLeft);
			flyingBalls.Remove (ball);

			if (flyingBalls.Count == 0) {
				EnterLaunchingBall ();
			} else {
				Debug.Log ("there's another ball flying; dont enter launching");
			}
		} else {
			Debug.Log ("Lose");
		}
	}

	public void MakeSticky() {
		gameObject.AddComponent<StickBall> ();
	}

	public void ChangeWidth(int dir) {
		CancelInvoke ("ResetWidth");
		int prevWidthIndex = widthIndex;
		widthIndex = Mathf.Clamp(widthIndex + dir, 0, widths.Length);
		/*Debug.Log (	"prev w = " + prevWidthIndex.ToString ()
					+ "\t" +
				   	"new w = " + widthIndex.ToString ());*/

		if (widthIndex != defaultWidthIndex) {
			if (widthIndex != prevWidthIndex)
				transform.DOScaleX (widths [widthIndex], 0.5f);
			
			Invoke ("ResetWidth", 10.0f);
		} else {
			ResetWidth ();
		}
	}

	public void ResetWidth() {
		widthIndex = defaultWidthIndex;
		transform.DOScaleX (widths [defaultWidthIndex], 0.5f);
	}

	public void Widthen() {
		ChangeWidth (1);
	}

	public void Shrink() {
		ChangeWidth (-1);
	}

	public void Stick(GameObject ball) {
		Debug.Assert (flyingBalls.Contains (ball));
		flyingBalls.Remove (ball);
		GetComponent<ShipLaunching> ().Enter (ball);
	}

}
