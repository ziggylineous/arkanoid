using UnityEngine;
using System.Collections;

public class WidthenPowerUp : PowerUp
{
	protected override void Affect (GameObject ship) {
		Debug.Log ("called Pow up Widthen");
		ship.GetComponent<Ship> ().Widthen ();
	}
}

