using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private bool _path3X3 = true;
    


    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        List<BoundsInt> roomBoundingList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt( (Vector3Int)_startPosition, new Vector3Int(_dungeonWidth, _dungeonHeight, 0)),
            _minRoomWidth, _minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        
        if (_randomWalkRooms)
        {
            floor = CreateRoomRandomly(roomBoundingList);
        }
        else
        {
            //룸들을 2분할로 만들고
            floor = CreateSimpleRooms(roomBoundingList);
        }

        //각 룸들을 신장트리로 연결시킨다.
        //각 룸들의 중심점을 구해라.
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (BoundsInt room in roomBoundingList)
        {
            roomCenters.Add( (Vector2Int) Vector3Int.RoundToInt(room.center) );
        }

        //연결통로 만들기
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        //그걸 방이랑 합치기
        floor.UnionWith(corridors);

        _tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, _tilemapVisualizer);
    }
    
    //룸들을 연결해주는 복도 만들기
    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        int targetIndex = Random.Range(0, roomCenters.Count);  //여기서 선택한 방이 시작방
        Vector2Int targetCenter = roomCenters[targetIndex];
        roomCenters.RemoveAt(targetIndex); //가져온 녀석은 제거

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = roomCenters.OrderBy(x => Vector2Int.Distance(x, targetCenter)).First();
            roomCenters.Remove(closest);

            HashSet<Vector2Int> newCorridor = CreateCorridor(targetCenter, closest);
            targetCenter = closest;
            
            //새로만들어진 복도를 결과값에 합쳐줌
            corridors.UnionWith(newCorridor);
        }
        //마지막으로 선택된 방이 마지막 방

        return corridors;
    }

    //2개의 룸을 연결하는 복도 만들기.
    private HashSet<Vector2Int> CreateCorridor(Vector2Int targetCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        Vector2Int position = targetCenter;

        corridor.Add(position);
        //목표에 도달할때까지 이동. 먼저 y축부터
        while (position.y != destination.y)
        {
            int delta = destination.y - position.y > 0 ? 1: -1;
            position += new Vector2Int(0, delta);
            corridor.Add(position);
            if (_path3X3)
            {
                corridor.Add(position + new Vector2Int(-1, 0));
                corridor.Add(position + new Vector2Int(1, 0));
            }
        }

        while (position.x != destination.x)
        {
            int delta = destination.x - position.x > 0 ? 1: -1;
            position += new Vector2Int(delta, 0);
            corridor.Add(position);
            
            if (_path3X3)
            {
                corridor.Add(position + new Vector2Int(0, -1));
                corridor.Add(position + new Vector2Int(0, 1));
            }
        }

        return corridor;
    }
    
   
    
    private HashSet<Vector2Int> CreateRoomRandomly(List<BoundsInt> roomBoundingList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        for (int i = 0; i < roomBoundingList.Count; ++i)
        {
            BoundsInt bound = roomBoundingList[i];

            Vector2Int roomCenter = new Vector2Int(Mathf.RoundToInt(bound.center.x), Mathf.RoundToInt(bound.center.y));
            HashSet<Vector2Int> roomFloor = RunRandomWalk(_dungeonData, roomCenter);

            foreach (Vector2Int position in roomFloor)
            {
                if ((position.x >= bound.xMin + _offset)
                    && (position.x <= bound.xMax - _offset)
                    && (position.y >= bound.yMin + _offset)
                    && (position.y <= bound.yMax - _offset))
                {
                    floor.Add(position);
                }
            }
        }

        return floor;
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
