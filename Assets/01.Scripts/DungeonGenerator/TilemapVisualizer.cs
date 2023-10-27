using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap _floorTilemap, _wallTilemap;

    [SerializeField] private TileBase _floorTile, _wallTopTile, _wallSideRight, _wallSideLeft, _wallBottom, _wallFull;

    [SerializeField] private TileBase _wallInnerCornerDownLeft, _wallInnerCornerDownRight;

    [SerializeField] private TileBase _wallDiagonalCornerDownRight, _wallDiagonalCornerDownLeft,
                                        _wallDiagonalCornerUpRight, _wallDiagonalCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, _floorTilemap, _floorTile);
    }

    //재활용을 위한 코드
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (Vector2Int position in positions)
        {
            PaintSingleTile(position, tilemap, tile);
        }
    }

    private void PaintSingleTile(Vector2Int position, Tilemap tilemap, TileBase tile)
    {
        tilemap.SetTile((Vector3Int)position, tile);
    }



    //십자방향 타일 채울때는 4비트를 사용한다.
    public void PaintSingleBasicWall(Vector2Int position, int tileFlag)
    {
        //Debug.Log($"{position}, type: { tileFlag.ToString()}");
        //벽하나 칠하기.
        TileBase tile = null;

        if (WallTypesHelper.wallTop.Contains(tileFlag))
        {
            tile = _wallTopTile;
        }else if (WallTypesHelper.wallSideRight.Contains(tileFlag))
        {
            tile = _wallSideRight;
        }else if (WallTypesHelper.wallSideLeft.Contains(tileFlag))
        {
            tile = _wallSideLeft;
        }else if (WallTypesHelper.wallBottm.Contains(tileFlag))
        {
            tile = _wallBottom;
        }else if (WallTypesHelper.wallBottm.Contains(tileFlag))
        {
            tile = _wallFull;
        }
        
        if(tile != null)
            PaintSingleTile(position, _wallTilemap, tile);
    }

    //대각선 타일을 채울때는 8방향을 모두 써야 해
    public void PaintSingleCornerWall(Vector2Int position, int tileFlag)
    {
        //Debug.Log($"{position}, type: {tileFlag.ToString()}");
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(tileFlag))
        {
            tile = _wallInnerCornerDownLeft;
        }else if (WallTypesHelper.wallInnerCornerDownRight.Contains(tileFlag))
        {
            tile = _wallInnerCornerDownRight;
        }else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(tileFlag))
        {
            tile = _wallDiagonalCornerDownLeft;
        }else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(tileFlag))
        {
            tile = _wallDiagonalCornerDownRight;
        }else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(tileFlag))
        {
            tile = _wallDiagonalCornerUpLeft;
        }else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(tileFlag))
        {
            tile = _wallDiagonalCornerUpRight;
        }else if (WallTypesHelper.wallFullEightDirections.Contains(tileFlag))
        {
            tile = _wallFull;
        }else if (WallTypesHelper.wallBottmEightDirections.Contains(tileFlag))
        {
            tile = _wallBottom;
        }
        
        if(tile != null)
            PaintSingleTile(position, _wallTilemap, tile);
    }
    
    public void Clear()
    {
        _floorTilemap.ClearAllTiles();
        _wallTilemap.ClearAllTiles();
    }
}
