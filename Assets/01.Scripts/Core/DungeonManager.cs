using System;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    
    public GameObject floorPrefab;
    public GameObject wallPrefab;

    private Transform _floorParent;
    private Transform _wallParent;
    
    [HideInInspector] public Rect sizeRect;
    
    private void Awake()
    {
        Instance = this;

        _floorParent = transform.Find("Floors");
        _wallParent = transform.Find("Walls");
    }


    public void SetFloorToPosition(Vector3 pos)
    {
        GameObject floor = Instantiate(floorPrefab, pos, Quaternion.identity);
        floor.transform.SetParent(_floorParent);
    }

    public void SetWallToPosition(Vector3 pos)
    {
        GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
        wall.transform.SetParent(_wallParent);
    }
}
