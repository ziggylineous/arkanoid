using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Portal : MonoBehaviour {

	public Portal destinationPortal;
	private float raycastDistance;
	private const int HitCount = 4;
	private bool teleportEnabled;

	void Start () {
		Bounds colliderBounds = GetComponent<BoxCollider2D> ().bounds;
		raycastDistance = 2.0f;//colliderBounds.size.magnitude * 2;
        teleportEnabled = destinationPortal != null;

		MakeFourTriQuad ();
	}

	private void MakeFourTriQuad() {
		Mesh mesh = GetComponent<MeshFilter> ().mesh;

		Vector3[] verts = new Vector3[5] {
			new Vector3(0.5f, -0.5f), // right, bottom
			new Vector3(-0.5f, -0.5f), // left, bottom
			new Vector3(-0.5f, 0.5f), // left, top
			new Vector3(0.5f, 0.5f), // right, top
			new Vector3(0.0f, 0.0f), // center
		};

		int[] triangles = new int[12] {
			0, 1, 4,
			1, 2, 4,
			2, 3, 4,
			3, 0, 4
		};

		Vector2[] uvs = new Vector2[5] {
			new Vector2 (1.0f, 1.0f),
			new Vector2 (1.0f, 1.0f),
			new Vector2 (1.0f, 1.0f),
			new Vector2 (1.0f, 1.0f),
			new Vector2 (0.0f, 0.0f)
		};

		mesh.vertices = verts;
		mesh.triangles = triangles;
		mesh.uv = uvs;
	}
	
	void OnTriggerEnter2D(Collider2D ballCollider) {
		Debug.Assert (ballCollider.gameObject.layer == LayerMask.NameToLayer ("Ball"));

		if (!teleportEnabled) {
			teleportEnabled = true;
			CancelInvoke ("EnableTeleport");
			return;
		}

		Vector2 ballCenter = ballCollider.transform.position;
		Vector2 ballVelocity = ballCollider.GetComponent<Rigidbody2D> ().velocity;

		RaycastHit2D hitFromBall = Physics2D.Raycast(ballCenter, ballVelocity.normalized, raycastDistance, 1 << gameObject.layer);
        
		if (hitFromBall.collider != null) {
			Vector2 backOrigin = ballCenter + ballVelocity.normalized * raycastDistance;


            RaycastHit2D[] hitsFrombehind = Physics2D.RaycastAll(backOrigin, -ballVelocity.normalized, raycastDistance, 1 << gameObject.layer);
            RaycastHit2D hitFromBehind = Array.Find(hitsFrombehind, hit => hit.collider == GetComponent<Collider2D>());

            if (hitFromBehind.collider) {
                Vector2 v2Pos = (Vector2)transform.position;
				Vector2 relativeEnter = hitFromBall.point - v2Pos;
				Vector2 relativeExit = hitFromBehind.point - v2Pos;

				DisableTeleportationTemporarily ();
				destinationPortal.DisableTeleportationTemporarily ();

                /*Debug.LogFormat("hitIn.point = {0}, hitOut.point = {1}", hitFromBall.point, hitFromBehind.point);
                Debug.LogFormat("ball = {0}, origin = {1}, destination = {2}, ", ballCenter, v2Pos, destinationPortal.transform.position);
                Debug.LogFormat("relEnter = {0}, relExit = {1}", relativeEnter, relativeExit);*/

				Ball ball = ballCollider.GetComponent<Ball> ();
				ball.Teleport (this, destinationPortal, relativeEnter, relativeExit);

			}
		}
	}

	private void DisableTeleportationTemporarily () {
		teleportEnabled = false;
		Invoke ("EnableTeleport", 2.0f);
	}

	private void EnableTeleport() {
        teleportEnabled = destinationPortal != null;
	}
}
