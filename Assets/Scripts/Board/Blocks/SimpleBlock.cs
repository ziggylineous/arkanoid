using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBlock : BaseBlock {

	override public void BallCollision (GameObject ball) {
		BlockDestroy ();
	}

}
