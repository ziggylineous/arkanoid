using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour {

	private int level = 1;

	// Use this for initialization
	void Start () {
		Debug.Log ("GameFlow::start()");

		DontDestroyOnLoad (gameObject);

		Invoke("PreloadToCurrentLevel", 3.0f);
	}

	private void PreloadToCurrentLevel() {
		LoadLevel (level);
	}

	private void LoadLevel(int levelNum) {
		Debug.Log ("SceneLoad: lading level " + levelNum.ToString());
		SceneManager.sceneLoaded += LevelLoaded;
		SceneManager.LoadScene ("Level" + levelNum.ToString (), LoadSceneMode.Single);
	}

	private void LevelLoaded(Scene scene, LoadSceneMode mode) {
		ObserveLevelCompletion ();
		SceneManager.sceneLoaded -= LevelLoaded;
	}

	private void ObserveLevelCompletion() {
		GameObject levelGameObject = GameObject.Find ("Level");
		Debug.Assert (levelGameObject != null);
		Level level = levelGameObject.GetComponent<Level> ();
		level.completed = NextLevel;
	}
	
	public void NextLevel() {
		++level;
		LoadLevel (level);

	}
}
