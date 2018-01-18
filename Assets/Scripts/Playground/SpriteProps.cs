using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteProps : MonoBehaviour {
	// Use this for initialization
	void Start () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();

		Bounds spriteBounds = spriteRenderer.sprite.bounds;

		Debug.Log ("pos = " + transform.position);
		Debug.Log ("bounds" + spriteBounds);
		Vector3 top = transform.position +
						new Vector3(0.0f, spriteBounds.extents.y * transform.localScale.y, 0.0f);
		Debug.Log ("top = " + top);
	}	
}
