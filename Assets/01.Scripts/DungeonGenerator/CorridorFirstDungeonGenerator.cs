using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] 
    private int _corridorLength = 14, _corridorCount = 5;
    
    [FormerlySerializedAs("_rootPercent")]
    [SerializeField]
    [Range(0.1f, 1f)]
    private float _roomPercent = 0.6f;
    
    
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPosition = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CreateCorridors(floorPosition, potentialRoomPosition); //복도를 만든다.

        HashSet<Vector2Int> roomFloors = CreateRooms(potentialRoomPosition);
    
        //꼬다리에 방이 없으면 이상하니까 deadEnd를 찾아서 전부 채워주자.
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPosition);
        
        //꼬다리라면 걍 무조건 방 만들어라.
        CreateRoomAtDeadEnd(deadEnds, roomFloors);
        
        //방과 복도를 합친다.
        floorPosition.UnionWith(roomFloors);

        for (int i = 0; i < corridors.Count; ++i)
        {
            corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            floorPosition.UnionWith(corridors[i]);
        }

        _tilemapVisualizer.PaintFloorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, _tilemapVisualizer);
    }

    private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previewDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; ++i)
        {
            //이전위치로부터 진행방향을 알아내고 그것이 변경되었다면
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (previewDirection != Vector2Int.zero && directionFromCell != previewDirection)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }

                previewDirection = directionFromCell;
            }
            else //방향이 변경되지 않았다면 
            {
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i-1]); // i 가 1부터 시작하니까
                newCorridor.Add(corridor[i-1] + newCorridorTileOffset);
            }
        }

        return newCorridor;
    }

    public List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; ++i)
        {
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    newCorridor.Add(corridor[i-1] + new Vector2Int(x, y));
                }
            }
        }

        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int directionFromCell)
    {
        if (directionFromCell == Vector2Int.up)
            return Vector2Int.right;
        if (directionFromCell == Vector2Int.right)
            return Vector2Int.down;
        if (directionFromCell == Vector2Int.down)
            return Vector2Int.left;
        if (directionFromCell == Vector2Int.left)
            return Vector2Int.up;

        return Vector2Int.zero;
    }

    /// <summary>
    /// 꼬다리에 방만들어주는 매서드 만약 이미 방이 존재한다면 만들지 않는다.
    /// </summary>
    /// <param name="deadEnds">꼬다리 리스트</param>
    /// <param name="roomFloors">이미 만들어진 방의 바닥 해시셋</param>
    private void CreateRoomAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (Vector2Int position in deadEnds)
        {
            // deadend포지션에 방이 만들어져있지 않다면 생성
            if (roomFloors.Contains(position) == false)
            {
                var deadEndFloor = RunRandomWalk(_dungeonData, position);
                roomFloors.UnionWith(deadEndFloor);
                //기존룸에 합병시킨다.
            }
        }
    }

    /// <summary>
    /// 꼬다리에 아무것도 없이 남겨진 복도 끝을 찾아서 반환해주는 매서드
    /// </summary>
    /// <param name="floorPosition">복도위치를 알려주는 매서드</param>
    /// <returns>데드엔드 포지션 모아 둔 리스트 반환.</returns>
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPosition)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (Vector2Int position in floorPosition)
        {
            int neighborCount = 0; //이웃이 한개밖에 없으면 데드엔드야.
            foreach (Vector2Int direction in Direction2D.cardinalDirectionList)
            {
                //해시셋이라 빨라. 걱정 NO
                if (floorPosition.Contains(position + direction))
                {
                    ++neighborCount;
                }
                
                if(neighborCount >= 2) break;
            }
            //여기까지 왔는데 이웃의 수가 1보다 작거나 같다면 deadEnd
            if (neighborCount <= 1)
            {
                deadEnds.Add(position);
            }
        }

        return deadEnds; 
    }

    /// <summary>
    /// 방으로 생성될 가능성이 있는 꼭지점들을 가져와서 방으로 만들어주는 함수
    /// </summary>
    /// <param name="potentialRoomPosition">가능성 있는 Vector2Int 포인트들이다.</param>
    /// <returns>생성된 방을 만들어서 넣어준다.</returns>
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPosition)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();

        //꼭지점에서 몇개나 방으로 만들지를 결정.
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPosition.Count * _roomPercent);

        //개쩌는 Linq입니다.
        List<Vector2Int> roomToCreate = potentialRoomPosition.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        //만들어진 방들을 포지션에 합병
        foreach (var pos in roomToCreate)
        {
            HashSet<Vector2Int> roomFloor = RunRandomWalk(_dungeonData, pos);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    /// <summary>
    /// 복도를 만들어주는 함수로 절차적생성알고리즘에서 복도생성 알고리즘을 호출하여 corridorCount만큼 복도를 만든다.
    /// </summary>
    /// <param name="floorPosition">복도를 생성해서 넣어줄 해시셋으로 비어있는걸 넣으면 된다.</param>
    /// <param name="potentialRoomPosition">차후 방으로 만들어질 연결고리들을 넣어주는 곳으로 비어있는걸 넣어라.</param>
    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPosition, HashSet<Vector2Int> potentialRoomPosition)
    {
        Vector2Int currentPosition = _startPosition;

        //복도가 끝나는 마지막 지점마다 룸을 만들어줄 룸을 만들어줄 포인트를 만든다.
        potentialRoomPosition.Add(currentPosition);

        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        
        for (int i = 0; i < _corridorCount; ++i)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, _corridorLength);
            
            corridors.Add(corridor);
            
            currentPosition = corridor.Last();
            //마지막 위치를 시작위치로 이어준다.
            potentialRoomPosition.Add(currentPosition);
            floorPosition.UnionWith(corridor);
        }

        return corridors;
    }
}
