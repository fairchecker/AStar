using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int xCoo;
    private int yCoo;

    public int XCoo { get { return this.xCoo; } set { this.xCoo = value; } }
    public int YCoo { get { return this.yCoo; } set { this.yCoo = value; } }

    private string spriteName;

    public bool isWalkable = true;
    private int movementSpeed;

    public int MovementSpeed {
        get { return this.movementSpeed; }
        set {this.movementSpeed = value; } 
        }
    
    /*public bool IsWalkable{
        get { return this.isWalkable; }
        set {this.isWalkable = value; }
    } */

    private string tileType;

    private string item = "0"; 
    
    public string GetTileSpriteName(){
        return spriteName;
    }
    public void SetTileSpriteName(string newName){
        spriteName = newName;
    }
}
