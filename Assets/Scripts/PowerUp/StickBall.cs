using UnityEngine;
using System.Collections;

public class StickBall : MonoBehaviour
{
	public int stickCount = 3;
	private int BallLayer;

	void Start() {
		BallLayer = LayerMask.NameToLayer ("Ball");
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer == BallLayer) {
			Debug.Log ("sticking ball");
			GetComponent<Ship> ().Stick (collision.gameObject);
			--stickCount;

			if (stickCount == 0) {
				Destroy (this);
			}
		}
	}
}

