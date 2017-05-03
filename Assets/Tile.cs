using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
// We clarify that we use unity engine random not system random
using Random = UnityEngine.Random; 

public class Tile : MonoBehaviour {

	// We create an event and use the actio class (which is a delegate) to define which parameters are sent with the event
	public static event Action<Tile> onTileClicked;

	public float wallChance; 

	internal Tile previousTile; 

	internal int x;
	internal int y; 

	internal bool isWall; 


	// Use this for initialization
	void Start () 
	{

		// If Random.Value is less than WallChance is wall = true
		isWall = Random.value < wallChance; 


		if (isWall) 
		
		
		{
			GetComponent<SpriteRenderer>().color = Color.red; 

		}
	}
	
	// This is called automatically when the object is clicked 
	void OnMouseDown () 
	{
	     // Every time before we call an event we should do a null check. Events are null when they have no listeners. 
		// We call an event just like the method
		if (onTileClicked != null)
		onTileClicked(this); 


	}
}
