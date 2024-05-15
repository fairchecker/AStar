using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.U2D.Aseprite;
using System.Numerics;
using UnityEngine;


class AStar {
    
    List<Node> closedList;

    List<Node> neighbours;
    bool isFound;

    Node currentPoint;
    float currentCost = 0f;
    private int i = 0;
    
    Node finalPoint;
    public List<Tile> FindPath(Tile start, Tile end, World world){
        List<Node> openList;
        Debug.Log("algo started!");
        i = 0;
        currentPoint = new Node(start.XCoo, start.YCoo);
        finalPoint = new Node(end.XCoo, end.YCoo);
        Debug.Log(finalPoint.X + " " + finalPoint.Y + "fin Point");

        Debug.Log(end.XCoo + " " + end.YCoo + " end hyle net");

        List<Tile> Path = new List<Tile>();
        Path.Add(world.tiles[currentPoint.X, currentPoint.Y]);

        closedList = new List<Node>();
        openList = new List<Node>();

        closedList.Add(currentPoint);
        
        
        Debug.Log("We are going to find neighbours");
        neighbours = FindNeigbours(currentPoint, openList, closedList, world);
        foreach(Node node in neighbours){
            node.recountCost(currentPoint.X, currentPoint.Y, currentCost);
            node.recountOpenCost(end.XCoo, end.YCoo);
            node.recountWeight();
            
            openList.Add(node);
        }
        Debug.Log("We found neighbours!");
        currentPoint = FindSmallestWeight(neighbours, new Node(start.XCoo, start.YCoo));
        Path.Add(world.tiles[currentPoint.X, currentPoint.Y]);
        Debug.Log(currentPoint.X + "currentX");

        closedList.Add(currentPoint);
        openList.Remove(currentPoint);
        Debug.Log(!(currentPoint.X == finalPoint.X && currentPoint.Y == finalPoint.Y) + " " + (i <= 1000));
        
        while(!(openList.Count != 0) && !closedList.Exists(x => x.X == end.XCoo && x.Y == end.YCoo)){
            i+=1;
            Debug.Log("cycle started");
            neighbours = FindNeigbours(currentPoint, openList, closedList, world);
            if(neighbours != null){
                foreach(Node n in neighbours){
                    n.recountCost(currentPoint.X, currentPoint.Y, currentCost);
                    n.recountOpenCost(end.XCoo, end.YCoo);
                    n.recountWeight();
                    Debug.Log(n.X + " " + n.Y + " checker" );
                    openList.Add(n);
                    /*var str = GameObject.Find("Tile/" + n.X + "/" + n.Y).GetComponent<SpriteRenderer>();
                    str.sprite = Resources.Load<Sprite>("frame"); */
                }
                currentPoint = FindSmallestWeight(neighbours, new Node(start.XCoo, start.YCoo));
                if(currentPoint.X == start.XCoo && currentPoint.Y == start.YCoo) Path = new List<Tile>();
            }

            else { closedList.Add(currentPoint); openList.Remove(currentPoint); 
                currentPoint.X = closedList[i-2].X; 
                currentPoint.Y = closedList[i-2].Y;
                Path.Remove(world.tiles[currentPoint.X, currentPoint.Y]); 
            }
            closedList.Add(currentPoint);
            openList.Remove(currentPoint);

            Path.Add(world.tiles[currentPoint.X, currentPoint.Y]);
        }

        

        return Path;
        
    }
    List<Node> FindNeigbours(Node current, List<Node> openList, List<Node> closedList, World world){
        List<Node> neigbours = new List<Node>();
        Tile tile;
        for(int i = -1; i <= 1; i++){
            for (int j = -1; j <= 1; j++){
                if(i == 0 && j == 0){
                    continue;
                }
                if(current.X + i < 100 && current.X + i >= 0 && current.Y + j < 100 && current.Y + j >= 0) { 
                    Debug.Log("Tile/" + System.Convert.ToString(current.X + i) + "/" + System.Convert.ToString(current.Y + j) + "Tile" + current.X);
                    tile = world.tiles[currentPoint.X + i, currentPoint.Y + j];
                    }
                else continue;

                if (tile.isWalkable 
                /*&& !openList.Any(n => n.X == current.X + i && n.Y == current.Y + j)*/ 
                && !closedList.Any(n => n.X == current.X + i && n.Y == current.Y + j)){ 
                    neigbours.Add(new Node(current.X + i, current.Y + j));
                    Debug.Log(System.Convert.ToString(current.X + i) + " " + System.Convert.ToString(current.Y + j) + "Added to neighbours");
                }
            }
        }
        return neigbours;
    }

    Node FindSmallestWeight(List<Node> listik, Node startPoint){
        float smallestWeight = 9000000f;
        Node res = startPoint;
        for(int i = 0; i < listik.Count(); i++){
            if(listik[i].Weight < smallestWeight){
                smallestWeight = listik[i].Weight;
                Debug.Log(listik[i].Weight + " smallest Weight");
                res.X = listik[i].X;
                res.Y = listik[i].Y;
            }
        }
        Debug.Log(res.X + " " + res.Y + " picked");
        return res;
    }
}

class Node{
    int x;
    int y;

    public int X { get { return this.x; } set { this.x = value; } }
    public int Y { get { return this.y; } set { this.y = value; } }
    float cost = 0f;
    float openCost = 0f;
    float weight = 0f;

    public float Weight { get { return this.weight; } }

    public void recountWeight(){
        this.weight = this.cost + this.openCost;
        Debug.Log(this.weight + "ok");
    }
    
    public void recountOpenCost(int finalPosX, int finalPosY){
        //this.openCost = (float)Math.Sqrt((((float)this.x - (float)finalPosX) * ((float)this.x - (float)finalPosX)) + (((float)this.y - (float)finalPosY) * ((float)this.y - (float)finalPosY)));
        this.openCost = Mathf.Abs(this.X - finalPosX) + Mathf.Abs(this.Y - finalPosY);
        if(this.openCost == 0) Debug.Log("Pizda"); 
        else Debug.Log(this.openCost + " openCost");
    }

    public void recountCost(int currentPosX, int currentPosY, float currentCost){
        this.cost = (float)Math.Sqrt((((float)y - (float)currentPosY) * ((float)y - (float)currentPosY)) 
        +(((float)x - (float)currentPosX) * ((float)x - (float)currentPosX )));
        Debug.Log(this.cost + " Cost");
    }

    public Node(int xC, int yC){
        this.x = xC;
        this.y = yC;
    }
}