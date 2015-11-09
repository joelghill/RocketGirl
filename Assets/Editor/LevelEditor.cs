using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditor : EditorWindow
{
	private Vector3[] prevPositions;
	private bool doSnap = false;
	private bool doCopy = false;
	private bool LevelEditorActive = true;
	private float snapValue = 1;
	
	[MenuItem( "Window/Level Editor %_l" )]
	static void Init()
	{
		var window = (LevelEditor)EditorWindow.GetWindow( typeof( LevelEditor ) );
		window.maxSize = new Vector2( 200, 100 );
	}

	void OnEnable(){
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	void OnDisable(){
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}
	
	public void OnGUI()
	{

		this.LevelEditorActive = EditorGUILayout.Toggle( "Level Editor Active", LevelEditorActive);

        ///Showing instructions....
        GUILayout.Label("###### ON WINDOWS ######");
        GUILayout.Label("Hold Left Control to Snap object to grid & erase");
		GUILayout.Label("Hold Left Ctrl & Space to draw with selection");
        GUILayout.Label("...");
        GUILayout.Label("###### ON MAC OSX ######");
        GUILayout.Label("Hold \"s\" to snap object to grid & erase");
        GUILayout.Label("Hold \"s\" & \"c\" to draw with selection");

    }

	public void OnSceneGUI(SceneView sceneView){
		if (!this.LevelEditorActive)
			return;
		Event e = Event.current;
		switch (e.type) {
		case EventType.KeyDown:
			if (Event.current.keyCode == (KeyCode.LeftControl) ||
                    Event.current.keyCode == (KeyCode.S)){
				doSnap = true;
				//EditorGUILayout.Toggle( "Auto Snap", doSnap);
			}
			if(Event.current.keyCode == (KeyCode.Space) ||
                    Event.current.keyCode == (KeyCode.C))
                {
				doCopy = true;
			}
			break;
			
		case EventType.keyUp:
			if (Event.current.keyCode == (KeyCode.LeftControl) ||
                    Event.current.keyCode == (KeyCode.S)) {
				doSnap = false;
			}
			if(Event.current.keyCode == (KeyCode.Space) ||
                    Event.current.keyCode == (KeyCode.C))
                {
				doCopy = false;
			}
			break;
		}

	}

	public void OnSelectionChange(){
		if (!this.LevelEditorActive)
			return;
		Debug.Log ("SELECTION CHANGED");
		if(Selection.activeTransform == null) return;
		if (Selection.transforms.Length > 0) {
			prevPositions = new Vector3[Selection.transforms.Length];
			Debug.Log("Current number of selected Transforms:  " + Selection.transforms.Length.ToString());
		}
		for (int i = 0; i < prevPositions.Length; i++){
			prevPositions [i] = Selection.transforms [i].position;
		}
	}

	public void Update()
	{
		if (!this.LevelEditorActive)
			return;
		if (prevPositions == null) {
			Debug.Log ("Previous Positions is null");
			return;
		}
		if ( doSnap
		    && !EditorApplication.isPlaying
		    && Selection.transforms.Length > 0
	    	&& Selection.transforms[0].position != prevPositions[0] )
		{
			Snap();
		}
	}
	private void createCopy(Transform item, Vector3 pos){
		if (item.position.Equals (getRoundedVector (item.position))) {
			Object o = Resources.Load(item.gameObject.name, typeof(GameObject));
			if(o != null){
				GameObject n = (GameObject)PrefabUtility.InstantiatePrefab(o);
				n.transform.position = pos;
				if(item.parent != null){

				}
				n.transform.SetParent(item.parent);
			}else{
				Debug.LogError("There is no resource by the name " + item.gameObject.name);
			}
		}
	}
	private void Snap()
	{
		for(int i = 0; i < Selection.transforms.Length; i++)
		{
			var t = Selection.transforms[i].position;
			t.x = Round( t.x );
			t.y = Round( t.y );
			t.z = Round( t.z );

			Selection.transforms[i].position = t;
			if(!Selection.transforms[i].position.Equals(prevPositions[i])){
				Collider[] newspotColliders = Physics.OverlapSphere(Selection.transforms[i].position, 0.4f);
				foreach(Collider c in newspotColliders){
					if(isTrile(c.gameObject) && 
					   	!c.gameObject.Equals( Selection.transforms[i].gameObject))
						DestroyImmediate(c.gameObject);
				}
                if(doCopy)
				    createCopy(Selection.transforms[i], prevPositions[i]);
			}
			prevPositions[i] = Selection.transforms[i].position;
		}
	}

	private bool isTrile(GameObject o){
		return (o.tag == "None" ||
			o.tag == "Solid" ||
			o.tag == "SemiSolid" ||
			o.tag == "Moveable");
	}

	private Vector3 getRoundedVector(Vector3 v){
		return new Vector3 (Round (v.x), Round (v.y), Round (v.z));
	}
	
	private float Round( float input )
	{
		return snapValue * Mathf.Round( ( input / snapValue ) );
	}
}
