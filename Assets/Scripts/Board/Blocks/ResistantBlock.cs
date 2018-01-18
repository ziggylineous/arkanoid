using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResistantBlock : BaseBlock {

	public List<float> widths;
	public float maxOffset;

	private LineRenderer line;

	void Start() {
		// base.Start ();

		line = GetComponent<LineRenderer> ();
	}

	public override void BallCollision (GameObject ball) {
		float currentWidth = line.startWidth;
		int index = widths.IndexOf (currentWidth);

		bool destroyed = index == (widths.Count - 1);

		if (destroyed)
			BlockDestroy();
		else
			LoseResistance(index);
	}

	private void LoseResistance(int index) {
		float prevWidth = widths [index];
		float prevOffset = maxOffset - (prevWidth * 0.5f);

		float nextWidth = widths [index + 1];
		float newOffset = maxOffset - (nextWidth * 0.5f);

		// transform positions
		float offsetConversion = (1.0f / prevOffset) * newOffset;

		int positionCount = line.positionCount;
		Vector3[] positions = new Vector3[positionCount];
		line.GetPositions (positions);

		for (int i = 0; i != line.positionCount; ++i)
			positions[i] *= offsetConversion;

		line.startWidth = nextWidth;
		line.endWidth = nextWidth;
		line.SetPositions (positions);
	}
}
