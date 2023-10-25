using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkMapGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2Int _startPosition = Vector2Int.zero;
    [SerializeField] protected int _iteration = 10;
    [SerializeField] protected int _walkLength;
    [SerializeField] protected bool _startRandomlyEachIteration = true;

    [SerializeField] private TilemapVisualizer _tilemapVisualizer;
    public void RunProceduralGeneration()
    {
        _tilemapVisualizer.Clear(); //모든 타일 지우고 시작
        
        HashSet<Vector2Int> floorPosition = RunRandomWalk();
        _tilemapVisualizer.PaintFloorTiles(floorPosition);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        Vector2Int currentPosition = _startPosition;

        HashSet<Vector2Int> floorPositions = new();

        for (int i = 0; i < _iteration; ++i)
        {
            HashSet<Vector2Int> path = PreoceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, _walkLength);
            floorPositions.UnionWith(path); //새롭게 구한 경로와 합쳐 C#개쩔어!

            if (_startRandomlyEachIteration)
            {
                //리스트가 아니기 때문에 Linq를 써야해.
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }
}
