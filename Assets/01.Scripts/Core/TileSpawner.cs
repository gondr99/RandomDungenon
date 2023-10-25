using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsMap;
    
    private Transform _floorParent;
    private Transform _wallParent;
    
    private void Start()
    {
        DungeonManager.Instance.SetFloorToPosition(transform.position);
        StartCoroutine(DelayedWallGenerate());
    }

    private IEnumerator DelayedWallGenerate()
    {
        yield return null; //1프레임 대기후 
        Vector2 hitSize = Vector2.one * 0.8f;
        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                Vector2 targetPos = (Vector2)transform.position + new Vector2(x, y);

                Collider2D hit = Physics2D.OverlapBox(targetPos, hitSize, 0, _whatIsMap);

                if (!hit)
                {
                    DungeonManager.Instance.SetWallToPosition(targetPos);
                }
            }
        }
        
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
    #endif
}
