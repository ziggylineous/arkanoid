using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelCondition : MonoBehaviour {
    
    public delegate void Delegate(LevelCondition winCondition);
    public Delegate Given;
    public Delegate StopBeingGiven;
    protected bool wasGiven;

    protected virtual void Start() {
        wasGiven = IsGiven;
    }

    protected void CheckInformGiven() {
        if (IsGiven) {
			Given(this);
            wasGiven = true;         
        } else if (wasGiven) {
            wasGiven = false;
            StopBeingGiven(this);
        }
    }

	public abstract bool IsGiven{ get; }
}
