using UnityEngine;
using UnityEditor;
using System.Collections;
using Tiled;
using Blocks;

[InitializeOnLoad]
public class Tiled2DImport : MonoBehaviour {

	static Tiled2DImport(){
		Debug.Log ("Tiled Editor Script up and running");
	}
	// Add a menu item named "Import..." to  in the menu bar.

	/// <summary>
	/// Runs A quick debig test. Set code to whatever you need to test at the time.
	/// </summary>
	[MenuItem ("Tiled2D/Debug Test")]
	static void RunTest(){
		string filepath = EditorUtility.OpenFilePanel(
			"Select JSON file to import",
			"",
			"json");
		TiledLevel level;
		if (filepath.Length > 0) {
			level = new TiledLevel(filepath);
			Debug.Log (level.Tilesets[0].GetTileProperties(5).Bottom);

		} else {
			level = null;
		}


	}

	/// <summary>
	/// Imports the json file as selected, imports and saves as prefab.
	/// </summary>
	[MenuItem ("Tiled2D/Import...")]
	static void ImportJson () {

		TiledLevel level;
		GameObject map;

		AssetDatabase.CreateFolder("Assets/Meshes/", "TiledMeshes");
		AssetDatabase.CreateFolder("Assets/Prefabs/", "TiledPrefabs");

		string prefabPath = "Assets/Prefabs/TiledPrefabs/";
		AssetDatabase.DeleteAsset(prefabPath);

		string filepath = EditorUtility.OpenFilePanel(
			"Select JSON file to import",
			"",
			"json");

		if (filepath.Length > 0) {
			level = new TiledLevel (filepath);
		} else {
			level = null;
		}

		map = new GameObject ();


		if (level != null) {
			int z = 0;
			Debug.Log("There are " + level.Tilesets.Length + " tilesets");
			foreach(TiledLayer layer in level.Layers){
				int count = 0;
				for(int row = layer.Height-1; row >=0; row --){
					for(int column = 0; column < layer.Width; column++){
						int number = layer.Data[count] -1;
						if(number != -1){
							TileSet tileset = level.getTileSetForNumber(number);
							TileProperties properties = tileset.GetTileProperties(number);
							GameObject newobj = Instantiate (Resources.Load ("cube" ,typeof(GameObject)))as GameObject;
							setCollisionType(newobj, properties);
							newobj.transform.position = (new Vector3(column, row, z));
							setMaterials(properties, newobj, number, level, tileset);
							newobj.transform.SetParent(map.transform);
						}else{
							//Debug.Log("ZERO ENCOUNTERED");
						}
						count++;
					}
				}

				z++;
			}
		}
		prefabPath = prefabPath + getNameFromFilepath(filepath) + ".prefab";
		PrefabUtility.CreatePrefab(prefabPath, map);
		//AssetDataBase.Refresh ();
	}

	public static string SaveTileMaterial(string name, int number, string texture){
		string filename = name + number.ToString ();
		string path = "Assets/Materials/"+filename+".mat";
		if (AssetDatabase.LoadAssetAtPath (path, typeof(Material)) != null) {
			Debug.Log ("Material already exists....");
			return path;
		}

		Material tile = new Material(Shader.Find("Unlit/Texture"));
		tile.mainTexture =  
		                (Texture2D) AssetDatabase.LoadAssetAtPath(texture, typeof(Texture2D));
		if(tile.mainTexture == null) Debug.Log ("TEXTURE DID NOT LOAD");
		//tile.SetTexture (filename, texture);
		AssetDatabase.CreateAsset(tile, path);
		return path;
	}

	/// <summary>
	/// Saves the tile material.
	/// </summary>
	/// <returns>The file path to the tile material.</returns>
	/// <param name="name">Name.</param>
	/// <param name="number">Index of the current tile as given by the layer data.</param>
	/// <param name="texture">Filepath to the saved Texture.</param>
	/// <param name="tileset">Tileset.</param>
	public static string SaveTileMaterial(string name, int number, string texture, TileSet tileset){
		if (number < 0)
			return "";
		string filename = name + number.ToString ();
		Vector2 coordinates = tileset.GetCoordinates (number);
		string path = "Assets/Materials/"+filename+".mat";
		if (AssetDatabase.LoadAssetAtPath (path, typeof(Material)) != null) {
			Debug.Log ("Material already exists....");
			return path;
		}
		
		Material tile = new Material(Shader.Find("Unlit/Transparent Cutout"));
		tile.mainTexture =  
			(Texture2D) AssetDatabase.LoadAssetAtPath(texture, typeof(Texture2D));
		if(tile.mainTexture == null) Debug.Log ("TEXTURE DID NOT LOAD");
		//tile.SetTexture (filename, texture);
		Vector2 scale = new Vector2 ((float)1 / tileset.Columns, (float)1 / tileset.Rows);
		tile.mainTextureScale = scale;
		tile.mainTextureOffset = new Vector2((scale.x * coordinates.x), (scale.y * coordinates.y));
		AssetDatabase.CreateAsset(tile, path);
		return path;
	}

	public static string SaveTileMaterial(string name, int number, Texture2D texture){
		string filename = name + number.ToString ();
		string path = "Assets/Materials/"+filename+".mat";
		if (AssetDatabase.LoadAssetAtPath (path, typeof(Material)) != null) {
			Debug.Log ("Material already exists....");
			return path;
		}
		
		Material tile = new Material(Shader.Find("Unlit/Texture"));
		tile.mainTexture = texture;
		if(tile.mainTexture == null) Debug.Log ("TEXTURE DID NOT LOAD");
		//tile.SetTexture (filename, texture);
		AssetDatabase.CreateAsset(tile, path);
		return path;
	}
	

	public static string SaveTileTexture(Texture2D texture, string name, int number){
		//Material tile = new Material(Shader.Find("Diffuse"));
		string filename = name + number.ToString ();
		string path = "Assets/Textures/" + filename + ".asset";
		if (AssetDatabase.LoadAssetAtPath (path, typeof(Texture2D)) != null) {
			Debug.Log ("Texture already exists....");
			return path;
		}
		AssetDatabase.CreateAsset(texture, path);
		return path;
	}

	private static string getNameFromFilepath(string path){
		string[] files = path.Split("/".ToCharArray());
		return files [files.Length - 1];
	}

	private static void setMaterials(TileProperties properties, GameObject mapPrimitive, int number, TiledLevel level, TileSet set){
		MapComponent primitve = mapPrimitive.GetComponent<MapComponent> ();
		if (primitve == null)
			return;
		string name = set.Name;
		//Texture2D text = (Texture2D) level.Tilesets[0].Texture;
		string texturepath = "Assets/Textures/" + name + ".asset";

		string frontpath = SaveTileMaterial(name, number, texturepath, set);
		string backpath = SaveTileMaterial(name, properties.Back, texturepath, set);
		string toppath = SaveTileMaterial(name, properties.Top, texturepath, set);
		string bottompath = SaveTileMaterial(name, properties.Bottom, texturepath, set);
		string leftpath = SaveTileMaterial(name, properties.Left, texturepath, set);
		string rightpath = SaveTileMaterial(name, properties.Right, texturepath, set);

		if (!frontpath.Equals ("")) {
			primitve.SetFrontMaterial((Material) AssetDatabase.LoadAssetAtPath(frontpath, typeof(Material)));
		}
		if (!backpath.Equals ("")) {
			primitve.SetBackMaterial((Material) AssetDatabase.LoadAssetAtPath(backpath, typeof(Material)));
		}
		if (!toppath.Equals ("")) {
			primitve.SetTopMaterial((Material) AssetDatabase.LoadAssetAtPath(toppath, typeof(Material)));
		}
		if (!bottompath.Equals ("")) {
			primitve.SetBottomMaterial((Material) AssetDatabase.LoadAssetAtPath(bottompath, typeof(Material)));
		}
		if (!leftpath.Equals ("")) {
			primitve.SetLeftMaterial((Material) AssetDatabase.LoadAssetAtPath(leftpath, typeof(Material)));
		}
		if (!rightpath.Equals ("")) {
			primitve.SetRightMaterial((Material) AssetDatabase.LoadAssetAtPath(rightpath, typeof(Material)));
		}
		
		
	}

	private static void setCollisionType(GameObject o, TileProperties properties){
		CollisionType collisionType = o.GetComponent<CollisionType> ();
		if(collisionType != null){
			switch (properties.GetColliderType()) 
			{
				case "None":
					collisionType.Type = CollisionType.ColliderType.None;
					break;
				case "Solid":
					collisionType.Type = CollisionType.ColliderType.Solid;
					break;
				case "SemiSolid":
					collisionType.Type = CollisionType.ColliderType.SemiSolid;
					break;
				case "Moveable":
					collisionType.Type = CollisionType.ColliderType.Moveable;
					break;
				default:
					collisionType.Type = CollisionType.ColliderType.None;
					o.tag = "None";
					break;
			}
		}
		o.tag = properties.GetColliderType ();
	}

}
