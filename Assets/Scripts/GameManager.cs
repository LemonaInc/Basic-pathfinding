using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Tile Tileprefab;

	public int gridSizeX;
	public int gridSizeY;

	public float stepDelay;



	// This is a jagged array
	// private Tile[][] tiles; 
	// This is a 2D array
	private Tile[,] tiles;

	private Queue<Tile> frontier; 


	// Disable a gameobject and renable it
	void OnEnable () 
	{
		// We ALWAYS add our event listeners in the OnEnable 
		Tile.onTileClicked += Tile_onTileClicked;
	}


	private void OnDisable() 


	{

		// You have to remove your event listeners in the OnDisable otherwise the connection between the event persists
		// That would mean that these objects cant be garbage collected and errors might occur.
		Tile.onTileClicked -= Tile_onTileClicked; 

	}


	private void Tile_onTileClicked(Tile clickedTile)
	{

		StartCoroutine (createPath (clickedTile));


	}


	private IEnumerator createPath(Tile clickedTile)
	{



		// We clear all the information from the tiles sowe can do multiple seaches. 
		foreach (Tile t in tiles) 
		{
			t.previousTile = null; 
			if (!t.isWall) 
				{
				t.GetComponent<SpriteRenderer> ().color = Color.white; 
			}
		}



		Tile startTile = tiles [0, 0]; 


		frontier = new Queue<Tile>(); 


		frontier.Enqueue (startTile);
		while (frontier.Count > 0)



		{
			// We get the first tile from our frontier 
		

			Tile current = frontier.Dequeue(); 


			current.GetComponent<SpriteRenderer> ().color = Color.cyan; 


			// Destination Reached 
			if (current == clickedTile) 
			
			{

				print ("Reached Tile");

				while (current != startTile) 
				{

					// We draw a path and color it green
					current.GetComponent<SpriteRenderer> ().color = Color.green; 
					current = current.previousTile; 

				}
				// This breaks out of the coroutine. Same retutn as in a regular method.
				yield break; 
		
			}

			yield return StartCoroutine (SearchSurroundingTiles(current)); 

		}
			
		// Destroy(clickedTile.gameObject);

		print("couldnt reach destination"); 

	}



	IEnumerator SearchSurroundingTiles (Tile origin)
	{

		yield return StartCoroutine (SearchAdjacentTile (origin, - 1, 0)); 
		yield return StartCoroutine (SearchAdjacentTile (origin,  1, 0)); 
		yield return StartCoroutine (SearchAdjacentTile (origin, 0, 1)); 
		yield return StartCoroutine (SearchAdjacentTile (origin, 0, -1)); 



	}



	IEnumerator SearchAdjacentTile (Tile origin, int dirX, int dirY)

	{
		int tileX = origin.x + dirX;
		int tileY = origin.y + dirY; 

		if (tileX < 0 || tileX >= gridSizeX || tileY < 0 || tileY >= gridSizeY)
			yield break; 

		// We get the adjacent tile from the array
		
		Tile adjacentTile = tiles[tileX, tileY];

		// We ignore the walls  or tiles that have been checked. We know this because we set the previous tile when we search it.
		if (adjacentTile.isWall || adjacentTile.previousTile != null)
			yield break; 


		// Each tile holds a reference to the tile that found it. We use this to backtrack our path in the end.
		adjacentTile.previousTile = origin;
		adjacentTile.GetComponent<SpriteRenderer> ().color = Color.magenta; 


		frontier.Enqueue (adjacentTile); 
	}



	// Use this for initialization
	void Start () {

		tiles = new Tile[gridSizeX, gridSizeY];

		for (int i = 0; i < gridSizeX; i++) {

			for (int j = 0; j < gridSizeY; j++) 
			
			{
				Tile tileClone = Instantiate (Tileprefab); 

				tileClone.x = i;
				tileClone.y = j;
				// We put each clone into the tiles array
				tiles[i, j] = tileClone; 
				tileClone.transform.position = new Vector2 (i, j);

			}

		}
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
