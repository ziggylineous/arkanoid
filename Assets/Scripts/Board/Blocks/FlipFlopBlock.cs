using UnityEngine;
using System.Collections;

public class FlipFlopBlock : BaseBlock
{
	public Material onMaterial;
	public Material offMaterial;
	public bool isOn = false;
	private delegate void ToggleDelegate ();
	private ToggleDelegate Toggle;
    public DelegateTypes.VoidBool Toggled;

	void Start ()
	{
        if (isOn) On();
        else Off();
	}

	private void Off() {
		isOn = false;
		GetComponent<MeshRenderer> ().material = offMaterial;
		Toggle = On;
        if (Toggled != null) Toggled(false);
	}

	private void On() {
		isOn = true;
		GetComponent<MeshRenderer> ().material = onMaterial;
		Toggle = Off;
        if (Toggled != null) Toggled(true);
	}

	public override void BallCollision(GameObject ball) {
		Toggle ();
	}
}

