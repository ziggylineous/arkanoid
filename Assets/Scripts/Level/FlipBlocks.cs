using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipBlocks : LevelCondition
{

    public FlipFlopBlock[] blocks;
    public bool state = true;

    protected override void Start() {
        base.Start();

        foreach (FlipFlopBlock flipFlopBlock in blocks) {
            flipFlopBlock.Toggled = CallCheckInformGiven;
        }
    }

    private void CallCheckInformGiven(bool flipFlopState) {
        CheckInformGiven();
    }

    public override bool IsGiven {
        get {
            return Array.TrueForAll(blocks, flipFlopBlock => flipFlopBlock.isOn == state);
        }
    }
}
