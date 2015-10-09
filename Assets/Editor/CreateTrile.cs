using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class CreateTrile : MonoBehaviour {

	[MenuItem("Assets/Create/Trile")]
	private static void GenerateTrile()
	{
		Debug.Log (Selection.activeObject.name);
		GameObject o = createTrile ((Sprite)Selection.activeObject);
		//o.name = Selection.activeObject.name;
		string prefabPath = "Assets/Resources/" + Selection.activeObject.name + ".prefab";
		Object toPrefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
		PrefabUtility.ReplacePrefab(o, toPrefab, ReplacePrefabOptions.Default);
		DestroyImmediate (o);
	}

	[MenuItem("Assets/Create/Trile Set")]
	private static void GenerateTrileSet()
	{
		Debug.Log (Selection.activeObject.name);
		//create subfolder in tilesets
		string path = AssetDatabase.CreateFolder ("Assets/Tilesets", Selection.activeObject.name);
		string resourcespath = AssetDatabase.CreateFolder ("Assets/Tilesets/"+Selection.activeObject.name, "Resources");
		//string resource
		Texture2D texture = (Texture2D)Selection.activeObject;
		string texturePath = UnityEditor.AssetDatabase.GetAssetPath(texture);
		Debug.Log (texturePath);
		//get all generated sprites from a sliced textured.
		Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath (texturePath);
		Debug.Log (sprites [0].GetType().ToString());
		foreach (Object o in sprites) {
			Sprite s = (Sprite)o;
			GameObject trile = createTrile(s);
			string prefabPath = "Assets/Tilesets/"+Selection.activeObject.name+"/Resources/"+ s.name + ".prefab";
			Object toPrefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
			PrefabUtility.ReplacePrefab(trile, toPrefab, ReplacePrefabOptions.Default);
			DestroyImmediate (trile);

		}
	}
	
	// Note that we pass the same path, and also pass "true" to the second argument.
	[MenuItem("Assets/Create/Trile", true)]
	private static bool GenerateTrileValidation()
	{
		// This returns true when the selected object is a Texture2D (the menu item will be disabled otherwise).
		return Selection.activeObject.GetType() == typeof(Sprite);
	}

	// Note that we pass the same path, and also pass "true" to the second argument.
	[MenuItem("Assets/Create/Trile Set", true)]
	private static bool GenerateTrileSetValidation()
	{
		// This returns true when the selected object is a Texture2D (the menu item will be disabled otherwise).
		return Selection.activeObject.GetType() == typeof(Texture2D);
	}

	private static GameObject createTrile(Sprite s){
		GameObject fromPrefab = Instantiate((GameObject)Resources.Load ("SpriteCube", typeof(GameObject)));
		Trile t = (Trile)fromPrefab.GetComponent("Trile");
		t.setAllSides (s);
		return fromPrefab;
	}
}
