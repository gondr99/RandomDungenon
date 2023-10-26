using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap _floorTilemap, _wallTilemap;

    [SerializeField] private TileBase _floorTile, _wallTopTile;
    


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


    public void Clear()
    {
        _floorTilemap.ClearAllTiles();
        _wallTilemap.ClearAllTiles();
    }

    public void PaintSingleBasicWall(Vector2Int position)
    {
        //벽하나 칠하기.
        PaintSingleTile(position, _wallTilemap, _wallTopTile);
    }
}
