using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPosition, TilemapVisualizer tilemapVisualizer)
    {
        
        HashSet<Vector2Int> basicWallPosition = FindWallsInDirections(floorPosition, Direction2D.cardinalDirectionList);
        CreateBasicWall(tilemapVisualizer, basicWallPosition, floorPosition); //십자방향 벽 생성

        HashSet<Vector2Int> cornerWallPosition =
            FindWallsInDirections(floorPosition, Direction2D.diagonalDirectionList); //대각선 벽 생성
        CreateCornerWall(tilemapVisualizer, cornerWallPosition, floorPosition);
    }

    private static void CreateCornerWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPosition, HashSet<Vector2Int> floorPosition)
    {
        foreach (Vector2Int position in cornerWallPosition)
        {
            byte binaryType = 0;
            foreach (Vector2Int direction in Direction2D.eightDirectionList)
            {
                Vector2Int neighborPosition = position + direction;
                if (floorPosition.Contains(neighborPosition))
                {
                    binaryType = (byte)((binaryType << 1) + 1);
                }
                else
                {
                    binaryType = (byte)(binaryType << 1);
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, binaryType);
        }    
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPosition, HashSet<Vector2Int> floorPosition)
    {
        foreach (var position in basicWallPosition)
        {
            byte binaryType = 0;
            //string debugStr = "";
            foreach (Vector2Int direction in Direction2D.cardinalDirectionList)
            {
                Vector2Int neighborPosition = position + direction;
                if (floorPosition.Contains(neighborPosition))
                {
                    binaryType = (byte)((binaryType << 1) + 1);
                }
                else
                {
                    binaryType = (byte)(binaryType << 1);
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(position, binaryType);
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
