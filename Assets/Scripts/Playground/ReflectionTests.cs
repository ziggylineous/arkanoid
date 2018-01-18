using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ReflectionTests : MonoBehaviour {

	// Use this for initialization
	void Start () {
		System.Type TypeOfThis = GetType ();
		Debug.Log (TypeOfThis.ToString ());


		TypeOfThis.InvokeMember ("PrivMeth", BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, this, new object[] { 1 });

		TypeOfThis.InvokeMember ("PubMeth", BindingFlags.InvokeMethod | BindingFlags.Public, null, this, new object[] { 2 });

	}
	
	private void PrivMeth(int algo) {

		Debug.Log (algo);

	}

	public void PubMeth(int algo) {

		Debug.Log (algo);

	}
}
