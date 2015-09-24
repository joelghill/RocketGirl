/*
 * Joel Hill
 * joel.hill.87@gmail.com
 * created on September 7th, 2015
 * 
 * TiledLevel is a set of classes designed to read a JSON output from Tiled2D and create C# objects.
 * 
 * Uses simpleJson
 * 
 */

using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEditor;

namespace Tiled{
	/// <summary>
	/// Class to represent a level as imported from Tiled2D
	/// http://www.mapeditor.org/
	/// </summary>
	public class TiledLevel{

		private string jsonPath;
		private string texturePath;
		private JSONNode levelData;

		private int height;
		private int width;
		private string backgroundcolor;
		private int nextobjectid;
		private int tileheight;
		private int tilewidth;

		TiledLayer[] layers;
		TileSet[] tilesets;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tiled.TiledLevel"/> class.
		/// </summary>
		/// <param name="filePath">File path to .json file.</param>
		public TiledLevel(string filePath){

			this.jsonPath = filePath;

			//read file
			string raw = System.IO.File.ReadAllText(filePath);
			Debug.Log (raw);
			this.levelData = JSON.Parse(raw);

			//set layers
			JSONNode l = levelData["layers"];
			int layercount = l.AsArray.Count;
			this.layers = new TiledLayer [layercount];
			for (int i = 0; i < layercount; i++) {
				layers[i] = new TiledLayer(l[i]);
			}

			//set Tilesets
			JSONNode tilesetsnode = levelData ["tilesets"];
			int tilesetscount = tilesetsnode.AsArray.Count;
			this.tilesets = new TileSet[tilesetscount];
			for (int i = 0; i < tilesetscount; i++) {
				this.tilesets[i] = new TileSet(tilesetsnode[i]);

				if(HasTexture(this.tilesets[i].Name)) {
					string path = "Assets/Textures/" + this.tilesets[i].Name + ".asset";
					this.tilesets[i].Texture = (Texture2D)AssetDatabase.LoadAssetAtPath (path, typeof(Texture2D));
				}else{
					this.tilesets[i].Texture = LoadTextureFromPath(
						getTexturePath(this.jsonPath,
					               this.tilesets[i].Image));
					this.SaveTexture(this.tilesets[i].Texture, this.tilesets[i].Name);
				}
			}

			//set other properties...
			this.height = levelData ["height"].AsInt;
			this.width = levelData ["width"].AsInt;
			this.tileheight = levelData ["tileheight"].AsInt;
			this.tilewidth = levelData ["tilewidth"].AsInt;
			this.backgroundcolor = levelData["backgroundcolor"];
			this.nextobjectid = levelData ["nextobjectid"].AsInt;

		}

		private bool HasTexture(string name){
			string filename = name;
			string path = "Assets/Textures/" + filename + ".asset";
			if (AssetDatabase.LoadAssetAtPath (path, typeof(Texture2D)) != null) {
				return true;
			}
			return false;
		}

		private void SaveTexture(Texture2D tex, string name){
			string path = "Assets/Textures/" + name + ".asset";
			AssetDatabase.CreateAsset(tex, path);
		}
		/// <summary>
		/// Gets the height of the level in units of tiles.
		/// </summary>
		/// <value>The height.</value>
		public int Height {
			get {
				return this.height;
			}
		}

		/// <summary>
		/// Gets the width of the level in units of tiles.
		/// </summary>
		/// <value>The width.</value>
		public int Width {
			get {
				return this.width;
			}
		}

		/// <summary>
		/// Gets the layers contained in the level. Each layer contains tile positions and texture data.
		/// </summary>
		/// <value>The layers.</value>
		public TiledLayer[] Layers {
			get {
				return this.layers;
			}
		}

		/// <summary>
		/// Gets the tilesets.
		/// </summary>
		/// <value>The tilesets.</value>
		/// <remarks> WARNING: ONLY ONE TILESET IS SUPPORTED RIGHT NOW</remarks>
		public TileSet[] Tilesets {
			get {
				return this.tilesets;
			}
		}

		private static string getTexturePath(string jsonPath, string imagepath){
			string path = "";
			Debug.Log ("Image path is:  " + imagepath);
			string[] firsthalf = jsonPath.Split("/".ToCharArray());
			string[] secondhalf = imagepath.Split("/".ToCharArray());

			int updir = 0;

			for (int i = 0; i < secondhalf.Length; i++) {
				Debug.Log (secondhalf [i]);
				if(secondhalf[i].Equals("..")) updir++;
			}

			for (int i = 0; i < firsthalf.Length - updir - 1; i++) {
				path = path + firsthalf[i] + "/";
			}

			for (int i = updir; i < secondhalf.Length; i++) {
				path = path + secondhalf[i];
				if(i != secondhalf.Length - 1) path = path + "/";
			}
			return path;
		}

		private static Texture2D LoadTextureFromPath(string path){
			Texture2D tex = null;
			byte[] fileData;
			if (System.IO.File.Exists (path)) {
				fileData = System.IO.File.ReadAllBytes (path);
				tex = new Texture2D (2, 2);
				tex.LoadImage (fileData); //..this will auto-resize the texture dimensions.
				tex.filterMode = FilterMode.Point;
			} else {
				Debug.Log ("TEXTTURE FILE DOES NOT EXIST at " + path);
			}

			return tex;
		}
	}

	/// <summary>
	/// Tiled layer. Contains the actual tile position data and other information for display.
	/// </summary>
	public class TiledLayer{

		private JSONNode jsonsource;
		private int height;
		private int width;
		private string name;
		private float opacity;
		private string type;
		private bool visible;
		private int x;
		private int y;
		private int[] data;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tiled.TiledLayer"/> class.
		/// </summary>
		/// <param name="node">JSONNode containing layer data to extract.</param>
		public TiledLayer(JSONNode node){
			jsonsource = node;
			this.height = jsonsource ["height"].AsInt;
			this.width = jsonsource ["width"].AsInt;
			this.name = jsonsource ["name"];
			this.opacity = jsonsource ["opacity"].AsFloat;
			this.type = jsonsource ["type"];
			this.visible = jsonsource ["visible"].AsBool;
			this.x = jsonsource ["x"].AsInt;
			this.y = jsonsource ["y"].AsInt;

			//get array data....
			int count = jsonsource ["data"].AsArray.Count;
			this.data = new int[count];
			for (int i = 0; i < count; i++) {
				this.data[i] = jsonsource ["data"].AsArray[i].AsInt;
			}
		}

		/// <summary>
		/// Gets the height in tile units.
		/// </summary>
		/// <value>The height.</value>
		public int Height {
			get { return this.height;}
		}

		/// <summary>
		/// Gets the width in tile units.
		/// </summary>
		/// <value>The width.</value>
		public int Width {
			get {return this.width;}
		}

		public string Name {
			get {return this.name;}
		}

		public float Opacity {
			get {return this.opacity;}
		}

		public string Type {
			get {return this.type;}
		}

		public bool Visible {
			get {return this.visible;}
		}

		public int X {
			get {return this.x;}
		}

		public int Y {
			get {return this.y;}
		}

		public int[] Data {
			get {return this.data;}
		}

	}

	/// <summary>
	/// Tile set. All data needed to import the texture and set to materials in unity.
	/// </summary>
	public class TileSet{
		private JSONNode data;
		private int firstgid;
		private string image;
		private int imageheight;
		private int imagewidth;
		private int margin;
		private string name;
		private int spacing;
		private int tilecount;
		private int tileheight;
		private int tilewidth;
		private string transparentcolor;
		private Texture2D texture;

		//metadata
		private int columns;
		private int rows;
		private JSONNode tileproperties;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tiled.TileSet"/> class.
		/// </summary>
		/// <param name="jsondata">Jsondata. A JSONNode extracted from level json file.</param>
		public TileSet(JSONNode jsondata){
			this.data = jsondata;
			this.firstgid = this.data ["firstgid"].AsInt;
			this.image = this.data ["image"];
			this.imageheight = this.data ["imageheight"].AsInt;
			this.imagewidth = this.data ["imagewidth"].AsInt;
			this.margin = this.data ["margin"].AsInt;
			this.name = this.data ["name"];
			this.spacing = this.data ["spacing"].AsInt;
			this.tilecount = this.data ["tilecount"].AsInt;
			this.tileheight = this.data ["tileheight"].AsInt;
			this.tilewidth = this.data ["tilewidth"].AsInt;
			this.transparentcolor = this.data ["transparentcolor"];

			this.InitializeMetaData ();

			//this.GetTileTexture (39);
		}

		/// <summary>
		/// Gets the tile properties for a specified index.
		/// </summary>
		/// <returns>The tile properties.</returns>
		/// <param name="i">The index of the tile as retreived from the layer data.</param>
		/// <returns> An instance of TileProperties for the requested index.</returns>
		public TileProperties GetTileProperties(int i){
			string key = i.ToString ();
			JSONNode node = this.tileproperties [key];
			return new TileProperties (node);
		}

		private void InitializeMetaData(){
			//correct for margins
			int numberColumns;
			int actualHeight = this.imageheight - (2 * this.margin);
			int actualWidth = this.imagewidth - (2 * this.margin);
			
			if (this.tilewidth == actualWidth) {
				numberColumns = 1;
			} else if (((2 * this.tilewidth) + this.spacing) == actualWidth) {
				numberColumns = 2;
			} else {
				//reduce width by 2 tiles and one space.
				int reducedWidth = actualWidth - (2*this.tilewidth) - this.spacing;
				numberColumns = (reducedWidth/this.tilewidth) + 2;
			}

			this.columns = numberColumns;
			this.rows = this.tilecount / this.columns;
			Debug.Log ("There are " + this.rows);

			tileproperties = this.data ["tileproperties"];
		}

		/// <summary>
		/// Gets the tile texture.
		/// </summary>
		/// <returns>The tile texture from the tile atlas.</returns>
		/// <param name="i">The index.</param>
		public Texture GetTileTexture(int i){

			//convert map index to real image index
			//int index = i - this.firstgid;
			int index = i;
			double row = (System.Math.Floor ((double)index / (double)this.columns));
			Debug.Log ("Row of index 12(should be 1): " + row);

			int floor = this.imageheight - (this.margin + (this.rows * this.tileheight) + ((this.rows - 1) * this.spacing));

			double coloumn = index - (row * this.columns);
			Debug.Log ("Column of index 12 (should be 1): " + coloumn);

			double x = this.margin + (coloumn * this.tilewidth) + (coloumn * this.spacing);
			double y = (this.margin + ((this.rows - row -1) * this.tileheight) +((this.rows - row-1) * this.spacing)) + (double)floor;

			Color[] pix = this.texture.GetPixels((int)x, (int)y, this.tilewidth, this.tileheight);

			Texture2D destTex = new Texture2D(this.tilewidth, this.tileheight);
			destTex.SetPixels(pix);
			destTex.Apply();
			return destTex;
		}
		public Vector2 GetCoordinates(int index){
			double row = (System.Math.Floor ((double)index / (double)this.columns));
			row = this.rows - row - 1;
			double coloumn = index - (row * this.columns);
			return new Vector2((float)coloumn, (float)row);
		}

		public int Columns {
			get {
				return this.columns;
			}
		}

		public int Rows {
			get {
				return this.rows;
			}
		}

		public Texture2D Texture {
			get {
				return this.texture;
			}
			set {
				texture = value;
			}
		}
		int Firstgid {
			get {
				return this.firstgid;
			}
		}

		public string Image {
			get {
				return this.image;
			}
		}

		int Imageheight {
			get {
				return this.imageheight;
			}
		}

		int Imagewidth {
			get {
				return this.imagewidth;
			}
		}

		int Margin {
			get {
				return this.margin;
			}
		}

		public string Name {
			get {
				return this.name;
			}
		}

		int Spacing {
			get {
				return this.spacing;
			}
		}

		public int Tilecount {
			get {
				return this.tilecount;
			}
		}

		public int Tileheight {
			get {
				return this.tileheight;
			}
		}

		public int Tilewidth {
			get {
				return this.tilewidth;
			}
		}

		public string Transparentcolor {
			get {
				return this.transparentcolor;
			}
		}
	}

	/// <summary>
	/// Tile properties. Set of properties set to each tile from within Tiled2D.
	/// </summary>
	public class TileProperties {
		private int back;
		private int bottom;
		private int top;
		private int left;
		private int right;
		private string primitive;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tiled.TileProperties"/> class.
		/// </summary>
		/// <param name="jsondata">Jsondata. A JSONNode from the Tiled2D export. </param>
		public TileProperties(JSONNode jsondata){
			if (jsondata.ToString ().Equals( "")) {
				this.back = -1;
				this.bottom = -1;
				this.top = -1;
				this.left = -1;
				this.right = -1;
				this.primitive = "cube";
			} else {
				this.back = this.getProperty("back", jsondata);
				this.bottom = this.getProperty("bottom", jsondata);
				this.top = this.getProperty("top", jsondata);
				this.left = this.getProperty("left", jsondata);
				this.right = this.getProperty("right", jsondata);
				this.primitive = jsondata["primitive"];

				if(this.primitive == null) this.primitive = "cube";
			}

			Debug.Log ("Tile Properties Initialized");
			Debug.Log ("Back:  " + this.back);
		}

		private int getProperty(string key, JSONNode data){
			JSONNode prop = data [key];
			if (prop.ToString().Equals (""))
				return -1;
			return data [key].AsInt;
		}

		public string Primitive {
			get {
				return this.primitive;
			}
		}

		public int Back {
			get {
				return this.back;
			}
		}

		public int Bottom {
			get {
				return this.bottom;
			}
		}

		public int Top {
			get {
				return this.top;
			}
		}

		public int Left {
			get {
				return this.left;
			}
		}

		public int Right {
			get {
				return this.right;
			}
		}
	}

	/// <summary>
	/// Tile. Stub class, may not use
	/// </summary>
	public class Tile{
		public Tile(){
		}
	}

	/// <summary>
	/// Tiled object. Currently unused.
	/// </summary>
	public class TiledObject{
		public TiledObject(){

		}
	}
}

