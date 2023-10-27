using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SimpleRandomWalkMapGenerator : AbstractDungeonGenerator
{
    [SerializeField] protected SimpleRandomWalkSO _dungeonData;
    [SerializeField] protected bool _fillRoomTile = false; //방을 채울꺼냐.
    [SerializeField] protected bool _smoothLine = false; //방의 경계가 부드럽게 나오게 할꺼냐.
    protected override void RunProceduralGeneration()
    {
        _tilemapVisualizer.Clear(); //모든 타일 지우고 시작
        
        HashSet<Vector2Int> floorPosition = RunRandomWalk(_dungeonData, _startPosition);
        _tilemapVisualizer.PaintFloorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, _tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameter, Vector2Int startPos)
    {
        Vector2Int currentPosition = startPos;

        HashSet<Vector2Int> floorPositions = new();

        
        for (int i = 0; i < parameter.iteration; ++i)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameter.walkLength);
            floorPositions.UnionWith(path); //새롭게 구한 경로와 합쳐 C#개쩔어!

            if (parameter.startRandomlyEachIteration)
            {
                //리스트가 아니기 때문에 Linq를 써야해.
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        if (_fillRoomTile)
        {
            floorPositions = ProceduralGenerationAlgorithms.FillRoomTile(floorPositions, _smoothLine);
        }
        
        return floorPositions;
    }

    
}
