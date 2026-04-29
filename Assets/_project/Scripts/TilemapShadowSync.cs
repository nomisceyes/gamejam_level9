using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapShadowSync : MonoBehaviour
{
    public Tilemap originalTilemap;
    public Tilemap shadowTilemap;
    
    private void Start()
    {
        SyncTiles();
    }
    
    public void SyncTiles()
    {
        if (originalTilemap == null || shadowTilemap == null) return;
        
        shadowTilemap.ClearAllTiles();
        
        BoundsInt bounds = originalTilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = originalTilemap.GetTile(pos);
            if (tile != null)
            {
                shadowTilemap.SetTile(pos, tile);
            }
        }
        
        shadowTilemap.RefreshAllTiles();
    }
}