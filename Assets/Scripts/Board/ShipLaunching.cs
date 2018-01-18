using UnityEngine;
using System.Collections;

public class ShipLaunching : MonoBehaviour
{
	public GameObject ballPrefab;
	private GameObject ballToLaunch;
	private Rigidbody2D ballBody;

	public void Enter() {
		GameObject ball = Instantiate (ballPrefab, BallLaunchPosition, Quaternion.identity);
		Enter (ball);
	}

	public void Enter(GameObject ball) {
		Debug.Log ("ShipLaunching::Enter()");
		ballToLaunch = ball;
		ballBody = ballToLaunch.GetComponent<Rigidbody2D> ();
		ballToLaunch.GetComponent<Ball>().Launching ();
		enabled = true;
	}

	public void Exit() {
		Debug.Log ("ShipLaunching::Exit()");
		ballToLaunch = null;
		ballBody = null;
		enabled = false;
	}

	private Vector3 BallLaunchPosition {
		get {
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
			Bounds spriteBounds = spriteRenderer.sprite.bounds;

			CircleCollider2D ballCollider = ballPrefab.GetComponent<CircleCollider2D> ();

			float yOffset = ballCollider.radius +
				spriteBounds.extents.y * transform.localScale.y;
			//Debug.Log ("pos = " + transform.position);
			//Debug.Log ("bounds" + spriteBounds);
			return 	transform.position +
				new Vector3(0.0f, yOffset, 0.0f);
		}
	}

	void FixedUpdate() {
		ballBody.MovePosition (BallLaunchPosition);

		if (Input.GetButton ("Fire1")) {
			LaunchBall();
		}
	}

	private void LaunchBall() {
		GetComponent<Ship>().Launch (ballToLaunch);
	}
}

