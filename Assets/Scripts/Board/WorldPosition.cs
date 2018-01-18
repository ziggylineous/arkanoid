using UnityEngine;
using System.Collections;

public static class WorldPosition
{
	public static Vector2 TopLeft {
		get {
			return Camera.main.ScreenToWorldPoint (
				new Vector2 (0.0f, Screen.height)
			);
		}
	}

	public static Vector2 BottomLeft {
		get {
			return Camera.main.ScreenToWorldPoint (
				new Vector2 (0.0f, 0.0f)
			);
		}
	}

	public static Vector2 BottomRight {
		get {
			return Camera.main.ScreenToWorldPoint (
				new Vector2 (Screen.width, 0)
			);
		}
	}
			
	public static Vector2 TopRight {
		get {
			return Camera.main.ScreenToWorldPoint (
				new Vector2 (Screen.width, Screen.height)
			);
		}
	}

	public static Vector2 BottomCenter {
		get {
			return Camera.main.ScreenToWorldPoint (
				new Vector2 (Screen.width * 0.5f, 0.0f)
			);
		}
	}
}

