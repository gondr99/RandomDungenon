using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPosition, TilemapVisualizer tilemapVisualizer)
    {
        
        HashSet<Vector2Int> basicWallPosition = FindWallsInDirections(floorPosition, Direction2D.cardinalDirectionList);

        foreach (var position in basicWallPosition)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPosition, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new();

        //십자 방향 위치로 벽생성.
        foreach (var position in floorPosition)
        {
            foreach (var direction in directionList)
            {
                //이웃위치 구하고 거기에 길이 없으면 벽생성.
                Vector2Int neighborPosition = position + direction;
                if (floorPosition.Contains(neighborPosition) == false)
                {
                    wallPositions.Add(neighborPosition);
                }
            }                
        }

        return wallPositions;
    }
}
