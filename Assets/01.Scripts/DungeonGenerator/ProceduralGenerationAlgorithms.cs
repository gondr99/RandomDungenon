using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    /// <summary>
    /// 간단하게 랜덤워킹하는 알고리즘 
    /// </summary>
    /// <param name="startPos">시작위치</param>
    /// <param name="walkLength">몇칸이나 걸을 것인지. 계속 중복되면 아예 못걷기도 함.</param>
    /// <returns>걸은 경로를 반환한다.</returns>
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
            path.Add(newPos); //중복값은 false리턴한다.
            prevPos = newPos;
        }

        return path;
    }

    
    /// <summary>
    /// 방과 방을 잇는 복도를 그려주는 함수로 방을 연결하는 던전을 생성할 때 사용한다.
    /// </summary>
    /// <param name="startPosition">연결을 시작할 방의 위치와</param>
    /// <param name="corridorLength">복도의 길이를 지정</param>
    /// <returns>연결된 복도 리스트 List<Vector2Int>를 반환함.</returns>
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        //한쪽 방향을 정하고 길이만큼 쭉 나간다. (그래서 중복이 없기 때문에 List로 만들어도 돼)
        Vector2Int direction = Direction2D.GetRandomDirection();
        Vector2Int currentPosition = startPosition;

        corridor.Add(currentPosition);
        for (int i = 0; i < corridorLength; ++i)
        {
            currentPosition += direction;
            corridor.Add(currentPosition); //중복제거를 안한다.
        }

        return corridor;
    }


    /// <summary>
    /// 공간 이중분할 함수. 공간을 이분할 해서 2개의 공간으로 분할된 애들의 리스트를 반환한다.
    /// </summary>
    /// <param name="spaceToSplit"> 분할할 원본공간. </param>
    /// <param name="minWidth">분할된 공간의 크기의 최소 너비</param>
    /// <param name="minHeight">분할된 공간의 크기의 최소 높이</param>
    /// <returns>분할된 공간의 Bound를 리턴</returns>
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue(); //하나씩 빼서 더이상 못쪼갤 때까지 쪼갠다.
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                //절반의 확률로 가로부터 할지 세로부터 할지 결정한다. 
                if (Random.value < 0.5f) 
                {
                    if (room.size.y > minHeight * 2)
                    {
                        SplitHorizontal(minHeight, roomsQueue, room);
                    }else if (room.size.x > minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x > minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }else if (room.size.y > minHeight * 2)
                    {
                        SplitHorizontal(minHeight, roomsQueue, room);
                    }else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        } //end of while

        return roomsList;
    }

    
    
    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        //int xSplit = Random.Range(minWidth, room.size.x - minWidth); 
        //1부터 크기 -1까지 랜덤하게 나눠서 넣는다.
        int xSplit = Random.Range(1, room.size.x);

        //바운딩 박스의 생성자에서 포지션은 좌측하단을 말한다. 
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y));
        BoundsInt room2 = new BoundsInt(
                            new Vector3Int(room.min.x + xSplit, room.min.y), 
                            new Vector3Int(room.size.x - xSplit, room.size.y) );
        
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontal(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        //int ySplit = Random.Range(minHeight, room.size.y - minHeight);
        //이걸 만약 1 ~ room.size.y -1 까지 하면 좀더 랜덤하게 빠게지는 던전을 볼 수 있다. 
        int ySplit = Random.Range(1, room.size.y);
        //바운딩 박스의 생성자에서 포지션은 좌측하단을 말한다. 
        BoundsInt room1 = new BoundsInt(room.min,
                                    new Vector3Int(room.size.x, ySplit));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit), 
                                    new Vector3Int(room.size.x, room.size.y - ySplit) );
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
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
