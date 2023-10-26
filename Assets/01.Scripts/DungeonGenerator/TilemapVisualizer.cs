using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap _floorTilemap, _wallTilemap;

    [SerializeField] private TileBase _floorTile, _wallTopTile, _wallSideRight, _wallSideLeft, _wallBottom, _wallFull;
    
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



    public void PaintSingleBasicWall(Vector2Int position, byte tileFlag)
    {
        Debug.Log($"{position}, type: { tileFlag.ToString()}");
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

    public void PaintSingleCornerWall(Vector2Int position, byte tileFlag)
    {
        Debug.Log($"{position}, type: {tileFlag.ToString()}");
    }
    
    public void Clear()
    {
        _floorTilemap.ClearAllTiles();
        _wallTilemap.ClearAllTiles();
    }
}
