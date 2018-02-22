using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour {

	public float speed = 5.0f;
	private Rigidbody2D body;
	private int DieLayer;

	public int MaxPositions = 10;
	private LineRenderer line;
	private List<Vector3> positions = null;
	private int shrinkingPositionCount = 0;
	private Tween shrinkLastPosition = null;
    public RuntimeSet balls;

	public event DelegateTypes.VoidGameObj Died;

	void Start () {
		body = GetComponent<Rigidbody2D> ();
		DieLayer = LayerMask.NameToLayer ("Die");

		line = GetComponent<LineRenderer> ();

        balls.Add(gameObject);
	}

	public void Launching() {
		body = GetComponent<Rigidbody2D> ();
		line = GetComponent<LineRenderer> ();
		body.isKinematic = true;

		//enabled = false;
	}

	public void Launch(Vector2 dir) {
		//body = GetComponent<Rigidbody2D> ();
		body.isKinematic = false;
		body.velocity = dir * speed;

		if (positions == null) {
			line.startColor = Color.clear;
			line.endColor = Color.white;

			positions = new List<Vector3>();
			BeginLinePositions (transform.position, transform.position + Vector3.up);
		}

		//enabled = true;
	}

	private void BeginLinePositions(Vector3 firstPosition, Vector3 secPosition) {
		positions.Clear ();
		positions.Add (firstPosition);
		positions.Add (secPosition);
		line.positionCount = 2;
		line.SetPositions (positions.ToArray ());
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer == DieLayer) {
			Die ();
		}

		positions.Add (transform.position);

		int positionsCount = positions.Count;

		if (positionsCount > MaxPositions) {
			TryStartShrinkPosition ();
		}

		line.SetPositions (positions.ToArray ());
		line.positionCount = positionsCount;
	}

	private void TryStartShrinkPosition() {
		++shrinkingPositionCount;

		if (shrinkingPositionCount == 1)
			ShrinkPositionTween ();
	}

	private void ShrinkPositionTween () {
		
		shrinkLastPosition = DOTween.To(
			()=> positions[0],
			p=> positions[0] = p,
			positions[1],
			1.0f
		);

		shrinkLastPosition.OnComplete (ShrinkPositionCompleted);

	}

	private void ShrinkPositionCompleted() {
		--shrinkingPositionCount;

		shrinkLastPosition = null;

		positions.RemoveAt (0);
		line.SetPositions (positions.ToArray ());
		line.positionCount = positions.Count;

		if (shrinkingPositionCount > 0)
			ShrinkPositionTween ();
	}

	public void Teleport(Portal origin, Portal destination, Vector2 relativeEnter, Vector2 relativeExit) {
		Vector2 centerOffset = (relativeExit - relativeEnter) * 0.5f;

		Vector2 originPosV2 = new Vector2 (origin.transform.position.x, origin.transform.position.y);
		Vector2 destinationPosV2 = new Vector2 (destination.transform.position.x, destination.transform.position.y);

		Vector2 start = originPosV2 + relativeEnter;
		Vector2 startEnd = start + centerOffset;
		Vector2 exit = destinationPosV2 + relativeEnter + centerOffset;
		Vector2 exitEnd = destinationPosV2 + relativeExit;

        // Debug.LogFormat("start = {0}, startEnd = {1}, exit = {2}, exitEnd = {3}", start, startEnd, exit, exitEnd);

		Vector2 prevVel = body.velocity;

        // tween time calculation
		float currentSpeed = prevVel.magnitude;
		Vector2 displacement = relativeExit - relativeEnter;
		float length = displacement.magnitude;
		float displacementTime = length / speed;

		body.velocity = Vector2.zero;
		body.isKinematic = true;

		Sequence teleportSequence = DOTween.Sequence();

		Tweener insidePortal = DOTween.To (
           	() => body.position,
           	newPosition => body.MovePosition (newPosition),
			startEnd,
			displacementTime * 0.5f
		);

		Tweener outPortal = DOTween.To (
			() => exit,
			newPosition => body.MovePosition (newPosition),
			exitEnd,
			displacementTime * 0.5f
		);
			
		teleportSequence.Append(insidePortal)
			.AppendCallback(() => {
				body.MovePosition (exit);
				shrinkLastPosition.Kill();
				BeginLinePositions(exit, exit);
			})
			.Append(outPortal)
			.AppendCallback(() => { body.isKinematic = false; body.velocity = prevVel;});



	}

	void Update() {
		if (!body.isKinematic) {
			positions [positions.Count - 1] = transform.position;
			line.SetPosition (positions.Count - 1, transform.position);
			line.SetPosition (0, positions[0]);
		}
	}

	void OnDestroy() {
		if (shrinkLastPosition != null)
			shrinkLastPosition.Kill ();

        balls.Remove(gameObject);
	}

	private void Die() {
		Died (gameObject);
		Destroy (gameObject);
	}
}
