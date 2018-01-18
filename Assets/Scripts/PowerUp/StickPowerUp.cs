using UnityEngine;
using System.Collections;

public class StickPowerUp : PowerUp
{
	protected override void Affect (GameObject ship)
	{
		Debug.Log ("called Pow up Stick");
		ship.GetComponent<Ship> ().MakeSticky ();	
	}
}

