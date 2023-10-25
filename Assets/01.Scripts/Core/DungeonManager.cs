using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public enum Direction : ushort
{
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4
}

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    [SerializeField] private int _totalFloorCount = 100;
    [SerializeField] private TileBase _floorTile;
    [SerializeField] private TileBase _wallTile;
    [SerializeField] private GameObject _exitDoorPrefab;

    [SerializeField] private Transform _playerTrm;
    
    private Tilemap _floorTilemap;
    private Tilemap _wallTailmap;


    private List<Vector3Int> _floorList = new();
    
    private void Awake()
    {
        Instance = this;
        
        _floorTilemap = transform.Find("Tilemap/FloorTilemap").GetComponent<Tilemap>();
        _wallTailmap = transform.Find("Tilemap/WallTilemap").GetComponent<Tilemap>();
    }

    private void Start()
    {
        _playerTrm.position = _floorTilemap.CellToWorld(Vector3Int.zero);
        RandomWalker();
    }

    public void SetFloorToPosition(Vector3Int pos)
    {
        _floorTilemap.SetTile(pos, _floorTile);
        _wallTailmap.SetTile(pos, null);
    }
    
    public void SetWallToPosition(Vector3Int pos)
    {
        _wallTailmap.SetTile(pos, _wallTile);
    }

    private void Update()
    {
        if (Application.isEditor && Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    private void RandomWalker()
    {
        Vector3Int currentPos = Vector3Int.zero;

        
        while (_floorList.Count <= _totalFloorCount)
        {
            if (!_floorList.Any(x => x == currentPos))
            {
                _floorList.Add(currentPos);
                //현재 자리에 바닥 놓고
                SetFloorToPosition(currentPos);
                
                GenerateWall(currentPos);
            }

            //다음 자리를 선정
            var values = Enum.GetValues(typeof(Direction));
            int random = UnityEngine.Random.Range(0, values.Length);
            var dir = (Direction)values.GetValue(random);
            switch (dir)
            {
                case Direction.UP:
                    currentPos += Vector3Int.up;
                    break;
                case Direction.DOWN:
                    currentPos += Vector3Int.down;
                    break;
                case Direction.LEFT:
                    currentPos += Vector3Int.left;
                    break;
                case Direction.RIGHT:
                    currentPos += Vector3Int.right;
                    break;
            }
        }
        
        //타일생성이 모두 끝났다면
        CreateExitDoorWay();
    }

    private void CreateExitDoorWay()
    {
        Vector3Int doorPos = _floorList.Last();
        Vector3 worldPos = _floorTilemap.GetCellCenterWorld(doorPos);
        GameObject door = Instantiate(_exitDoorPrefab, worldPos, Quaternion.identity);
        door.transform.SetParent(transform);
    }

    private void GenerateWall(Vector3Int centerPos)
    {
        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                Vector3Int targetPos = centerPos + new Vector3Int(x, y);
                
                //바닥이나 벽에 아무것도 없다면 벽배치
                if (_floorTilemap.GetTile(targetPos) != null || _wallTailmap.GetTile(targetPos) != null)
                {
                    continue;
                }
                SetWallToPosition(targetPos);
            }
        }
    }

}
