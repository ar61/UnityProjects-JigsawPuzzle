using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class splitImage : MonoBehaviour {

	public int noOfRows;
	public int noOfColumns;
	Sprite image;
	public Transform spriteTransform;
    bool showInstructions = false;
    public GameObject text;

    private void Start()
    {
        text = GameObject.FindGameObjectWithTag("Instructions");
        text.GetComponent<Text>().enabled = false;
        text.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public void StartGame () {
		// get the gameObject of the main image to split up
		GameObject go = GameObject.FindGameObjectWithTag("image");
		image = go.GetComponent<SpriteRenderer>().sprite;

		Transform temp, tile;
		Color[] color;
		Texture2D tex;
		Sprite stemp;
		float width = image.texture.width;
		float height = image.texture.height;
		int tileWidth = (int)(width/noOfColumns);
		int tileHeight = (int)(height/noOfRows);

		for(int i=0 ; i<noOfRows ; i++){
			for(int j=0; j<noOfColumns; j++){
				// This specific number -3 + i*2 is the center of the 1st location of tile(bottomleft-most)
				// 2 is the distance between tiles centers
				tile = (Transform)Instantiate(spriteTransform, new Vector3(-3f + i*2 ,-3f + j*2, -2.5f), Quaternion.identity);
				tile.gameObject.AddComponent<BoxCollider>();
				finalTileValues s = tile.gameObject.AddComponent<finalTileValues>();
				s.row = i;
				s.column = j;
				// finalTile is the tag for all correct final position of the tiles
				tile.gameObject.tag = "finalTile";
			}
		}

		float randomVal = 0;
		float xOffset, yOffset;
		Quaternion quat = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
		// create count of sprites and assign texture with equal sizes
		for(int i=0 ; i<noOfRows ; i++){
			for(int j=0; j<noOfColumns; j++){

				// set a random rotation
				quat = quat * Quaternion.Euler(0f,0f,90*Random.Range(-1,2));

				// set randomly -ve or +ve for xOffset and yOffset
				randomVal = Random.Range(1.0f,2.0f);
				if(randomVal > 1.5){
					randomVal = -1;
				} else {
					randomVal = 1;
				}
				xOffset = randomVal*Random.Range(8,12);

				// set randomly -ve or +ve for yOffset
				randomVal = Random.Range(1.0f,2.0f);
				if(randomVal > 1.5){
					randomVal = -1;
				} else {
					randomVal = 1;
				}
				yOffset = randomVal*Random.Range(2,5);

				// create a sprite from the prefab
				temp = (Transform)Instantiate(spriteTransform, new Vector2(xOffset , yOffset), quat);

				// get pixels from the image and split it up into equal parts
				color = image.texture.GetPixels(i*tileWidth, j*tileHeight, tileWidth, tileHeight);
				tex = new Texture2D(tileWidth, tileHeight);
				tex.SetPixels(color);
				tex.Apply();
				stemp = Sprite.Create(tex,new Rect(0,0,tileWidth,tileHeight),new Vector2(0.5f,0.5f));

				// update the extracted sprite
				temp.gameObject.GetComponent<SpriteRenderer>().sprite = stemp;
				temp.gameObject.name = "tile-" + i + "," + j;
				temp.gameObject.AddComponent<tileInteraction>();
				tileInteraction script = temp.gameObject.GetComponent<tileInteraction>();
				temp.gameObject.tag = "tile";
				script.row = i;
				script.column = j;
			}
		}
		// unset the sprite on the original image
		SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
		renderer.sprite = null;
	}

    public void ShowInstructions()
    {
        showInstructions = !showInstructions;
        if (showInstructions)
        {
            text.GetComponent<Text>().enabled = true;
            text.GetComponentInChildren<SpriteRenderer>().enabled = true;
        } else
        {
            text.GetComponent<Text>().enabled = false;
            text.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }
}
