using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataBuilder : MonoBehaviour {
	public class Tile {
		public string tileType;
		public int x;
		public int y;
		public List<Tile> tileNeighbors = new List<Tile>();

		public Tile(string type, int xValue, int yValue){
			tileType = type;
			x = xValue;
			y = yValue;
		}
	}

	private List<Tile> tileList;
	public Tile[] mapData;
//	public Tile[] MapData{
//		get { return mapData; }
//	}
	// Use this for initialization
	void Start () {
		transform.localPosition = transform.localPosition + new Vector3 (1f, 0f, 0f);
		tileList = new List<Tile> ();
		BuildTileList ();
		Debug.Log ("out of there");
		mapData = new Tile[tileList.Count];

		for (int i = 0; i < mapData.Length; i++) {
			mapData [i] = tileList [i];
		}

		tileList.Clear();
	}

	private Transform currentTile;
	void BuildTileList(){
		Debug.Log ("In here");
		while (currentTile != null) {
			tileList.Add(new Tile(currentTile.tag, 
				(int)currentTile.localPosition.x,
				(int)currentTile.localPosition.z));

			Debug.Log (tileList.Count.ToString());
			transform.localPosition = transform.localPosition + new Vector3 (1f, 0f, 0f);
		}
	}

	void OnCollisionStay(Collision col){
		Debug.Log ("In here");
		if (col.collider.CompareTag("Ground")) {
			currentTile = col.transform;
		} else {
			currentTile = null;
		}
	}
}
