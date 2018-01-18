﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EdgeCollider2D collider = GetComponent<EdgeCollider2D>();

		if (!collider)
			collider = gameObject.AddComponent<EdgeCollider2D> ();

		collider.points = new Vector2[] {
			WorldPosition.BottomLeft,
			WorldPosition.BottomRight
		};
	}
}
