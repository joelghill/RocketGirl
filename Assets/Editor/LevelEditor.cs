using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditor : EditorWindow
{
	private Vector3[] prevPositions;
	private bool doSnap = true;
	private bool doCopy = false;
	private float snapValue = 1;
	
	[MenuItem( "Edit/Auto Snap %_l" )]
	
	static void Init()
	{
		var window = (LevelEditor)EditorWindow.GetWindow( typeof( LevelEditor ) );
		window.maxSize = new Vector2( 200, 100 );
	}
	
	public void OnGUI()
	{
		doSnap = EditorGUILayout.Toggle( "Auto Snap", doSnap );
		doCopy = EditorGUILayout.Toggle ("Create Copy", doCopy);
		snapValue = EditorGUILayout.FloatField( "Snap Value", snapValue );
	}

	public void OnSelectionChange(){
		if (Selection.transforms.Length > 0) {
			prevPositions = new Vector3[Selection.transforms.Length];
		}
		for (int i = 0; i < prevPositions.Length; i++){
			prevPositions [i] = Selection.transforms [i].position;
		}
	}

	public void Update()
	{
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
			if(!Selection.transforms[i].position.Equals(prevPositions[i]) 
			   && doCopy){
				Collider[] hitColliders = Physics.OverlapSphere(prevPositions[i], 0.4f);
				foreach(Collider c in hitColliders){
					DestroyImmediate(c.gameObject);
				}
				createCopy(Selection.transforms[i], prevPositions[i]);
			}
				
			//createCopy(transform);
			prevPositions[i] = Selection.transforms[i].position;
		}
	}

	private Vector3 getRoundedVector(Vector3 v){
		return new Vector3 (Round (v.x), Round (v.y), Round (v.z));
	}
	
	private float Round( float input )
	{
		return snapValue * Mathf.Round( ( input / snapValue ) );
	}
}
