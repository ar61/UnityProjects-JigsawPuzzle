using UnityEngine;
using System.Collections;

public class tileInteraction : MonoBehaviour {

	public bool attached = false;
	bool placed = false;
	bool setZback = false;
	bool switchTiles = false;
	bool firstTime = true;
	float rotZvalue;
	Vector3 curPos;
	public int row;
	public int column;
	Vector3 otherPos;
	Collider otherTile;


	void OnTriggerEnter(Collider other){
		// If this is not the first time picking up this tile and we have
		// collide with a fellow tile then remember to swap!
		if(other.tag == "tile" && !firstTime){
			if(!other.gameObject.GetComponent<tileInteraction>().placed){
				switchTiles = true;
				otherTile = other;
			}
		}
	}

	void OnTriggerExit(Collider other){
		// if we just touch the fellow tile and move away then dont swap!
		if(other.tag == "tile"){
			switchTiles = false;
		}
	}

	void OnMouseOver(){
		// If left mouse button is clicked
		if(Input.GetMouseButtonDown(0)){
			if(!placed){
				attached = !attached;
				// Switch tile reference to mouse
				if(switchTiles){
					attached = false;
					tileInteraction ti = otherTile.gameObject.GetComponent<tileInteraction>();
					ti.attached = true;
					switchTiles = false;
					setZback = true;
				}
			} else {
				// if the tile is already placed in the right position, no more movement necessary
				attached = false;
			}
		}
		// if right mouse button is clicked rotate the tile by 90 degrees
		if(Input.GetMouseButtonDown(1)){
			if(attached){
				Quaternion quatEnd = transform.rotation * Quaternion.Euler(Vector3.forward * -90);
				transform.rotation = Quaternion.Slerp(transform.rotation, quatEnd, 1f);
			}
		}
	}

	void OnTriggerStay(Collider other){
		// If this tile is tagged to mouse cursor and is not yet correctly placed
		// We find out if the tile is correctly placed in the right orientation
		if(!attached && !placed){
			if(other.tag == "finalTile"){
				// Calcutate difference in x,y values to make sure we are close enough to the target
				float xDiff = Mathf.Abs(other.transform.position.x - transform.position.x);
				float yDiff = Mathf.Abs(other.transform.position.y - transform.position.y);
				finalTileValues s = other.gameObject.GetComponent<finalTileValues>();

				// check if placed at the right spot
				if((s.row == row) && (s.column == column) && (xDiff <= 1 && yDiff <= 1)){
					// check if the orientation is correct
					Vector3 euler = transform.rotation.eulerAngles;
					if(euler.z%360 == 0)
					{
						transform.position = other.transform.position;
						placed = true;
						attached = false;
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		// If this tile set to move with mouse, move our tile along as we move our mouse
		if(attached){
			// 6.7f from cameras position, so z = -10f(Camera's z position) + 6.7f = -3.3f
			Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 6.7f);
			curPos = Camera.main.ScreenToWorldPoint(screenPoint);
			transform.position = curPos;
			firstTime = false;
			setZback = true;
		} else {
			if(setZback){
				// place the tile back to the z position found
				curPos = new Vector3(curPos.x, curPos.y, curPos.z + 0.3f);
				transform.position = curPos;
				setZback = false;
			}
		}
	}
}
