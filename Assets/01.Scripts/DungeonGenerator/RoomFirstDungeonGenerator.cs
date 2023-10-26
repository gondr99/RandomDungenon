using System.Collections.Generic;
using UnityEngine;

public class RoomFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int _minRoomWidth = 8, _minRoomHeight = 8;

    [SerializeField]
    private int _dungeonWidth = 80, _dungeonHeight = 80;

    [SerializeField] [Range(0, 10)] 
    private int _offset = 2;

    //랜덤워크 알고리즘을 이용해서 방을 만들꺼냐? 아니면 정방형 방을 만들꺼냐?
    [SerializeField]
    private bool _randomWalkRooms = false;


    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        List<BoundsInt> roomBoundingList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt( (Vector3Int)_startPosition, new Vector3Int(_dungeonWidth, _dungeonHeight, 0)),
            _minRoomWidth, _minRoomHeight);

        HashSet<Vector2Int> floor = CreateSimpleRooms(roomBoundingList);
        
        _tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, _tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomBoundingList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (BoundsInt roomBound in roomBoundingList)
        {
            //x축, y축으로 사각형으로 쭉 그려서 넣는다.
            for (int col = _offset; col < roomBound.size.x - _offset; ++col)
            {
                for (int row = _offset; row < roomBound.size.y - _offset; ++row)
                {
                    Vector2Int position = (Vector2Int)roomBound.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }

        return floor;
    }
}
