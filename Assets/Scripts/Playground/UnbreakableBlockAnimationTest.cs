using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableBlockAnimationTest : MonoBehaviour {

    public UnbreakableBlock ublock;

	void Start () {
        DeactivateBlock();
	}

    private void DeactivateBlock() {
        ublock.DelayedDeactivate(1.0f);
        Invoke("ActivateBlock", 5.0f);
    }

    private void ActivateBlock() {
        ublock.DelayedActivate(1.0f);
        Invoke("DeactivateBlock", 5.0f);
    }
}
