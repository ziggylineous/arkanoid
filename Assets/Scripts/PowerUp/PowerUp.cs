using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public abstract class PowerUp : MonoBehaviour {
	private int DieLayer;
	private int ShipLayer;

	void Start() {
		DieLayer = LayerMask.NameToLayer ("Die");
		ShipLayer = LayerMask.NameToLayer ("Ship");
		GetComponent<Rigidbody2D> ().angularVelocity = 180.0f;
	}

	void OnCollisionEnter2D(Collision2D collision) {

		if (enabled) {
			if (collision.gameObject.layer == DieLayer) {
				Destroy (gameObject, 1.0f);
				enabled = false;
			} else if (collision.gameObject.layer == ShipLayer) {
				Affect (collision.gameObject);
				Destroy (gameObject);
				enabled = false;
			}
		}
	}

	protected abstract void Affect(GameObject ship);
}
