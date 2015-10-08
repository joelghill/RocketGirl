using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class CreateTrile : MonoBehaviour {

	[MenuItem("Assets/Create/Trile")]
	private static void DoSomethingWithTexture()
	{
		Debug.Log (Selection.activeObject.name);
	}
	
	// Note that we pass the same path, and also pass "true" to the second argument.
	[MenuItem("Assets/Create/Trile", true)]
	private static bool NewMenuOptionValidation()
	{
		// This returns true when the selected object is a Texture2D (the menu item will be disabled otherwise).
		return Selection.activeObject.GetType() == typeof(Sprite);
	}
}
