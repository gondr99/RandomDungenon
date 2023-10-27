using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SimpleRandomWalkMapGenerator : AbstractDungeonGenerator
{
    [SerializeField] protected SimpleRandomWalkSO _dungeonData;
    [SerializeField] protected bool _fillRandomWalk = false;
    protected override void RunProceduralGeneration()
    {
        _tilemapVisualizer.Clear(); //모든 타일 지우고 시작
        
        HashSet<Vector2Int> floorPosition = RunRandomWalk(_dungeonData, _startPosition, _fillRandomWalk);
        _tilemapVisualizer.PaintFloorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, _tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameter, Vector2Int startPos, bool fillRoom = false)
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

        //룸의 내용물을 꽉 채우도록 설계되었다면.
        if (fillRoom && floorPositions.Count > 0)
        {
            Debug.Log("fill");
            List<Vector2Int> list = floorPositions.OrderBy(x => x.y).ToList();

            HashSet<Vector2Int> filledPosition = new HashSet<Vector2Int>(); //추가할 포지션
            
            int currentY = list[0].y;
            int minX = 0, maxX = 0;
            bool resetFlag = true;
            
            foreach (Vector2Int position in list)
            {
                if (resetFlag) //리셋이 된거라면
                {
                    minX = position.x;
                    maxX = position.x;
                    resetFlag = false;
                }

                if (position.y != currentY)  //줄이 바뀐 순간
                {
                    FillLine(filledPosition, minX, maxX, currentY);
                    currentY = position.y;
                    resetFlag = true;
                }
                else
                {
                    if (minX > position.x)
                    {
                        minX = position.x;
                        Debug.Log("min 교체");
                    }

                    if (maxX < position.x)
                    {
                        maxX = position.x;
                    }
                }
            }
            //마지막에 한번 실행.
            FillLine(filledPosition, minX, maxX, currentY);
            
            floorPositions.UnionWith(filledPosition);
        }

        return floorPositions;
    }

    private void FillLine(HashSet<Vector2Int> positions, int start, int end, int lineY)
    {
        Debug.Log($"{start}, {end}, {lineY}");
        for (int i = start; i <= end; ++i)
        {
            positions.Add(new Vector2Int(i, lineY));
        }
    }
}
