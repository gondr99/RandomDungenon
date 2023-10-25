using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PreoceduralGenerationAlgorithms
{
    //중복값을 허용하지 않으며 해시기반으로 돌아가서 빠르게 찾는다. Hash는 Vector2Int의 GetHashCode기반이다.
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        //시작위치 더하고
        path.Add(startPos);
        Vector2Int prevPos = startPos;

        for (int i = 0; i < walkLength; ++i)
        {
            Vector2Int newPos = prevPos + Direction2D.GetRandomDirection();
            path.Add(newPos);
            // if (!path.Add(newPos))
            // {
            //     --i; //만약 추가되지 못했다면 빼줘라.
            // }
            prevPos = newPos;
        }

        return path;
    }
}


public static class Direction2D
{
    //대각선은 넣지마.
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>()
    {
        new Vector2Int(0, 1), //UP
        new Vector2Int(0, -1), //DOWN
        new Vector2Int(-1, 0), //LEFT
        new Vector2Int(1, 0), //RIGHT
    };

    public static Vector2Int GetRandomDirection()
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
}