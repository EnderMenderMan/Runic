using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    [SerializeField] private LayerMask gridCheckLayerMask;
    public static WorldData Instance { get; private set; }
    [field: SerializeField] public Grid WorldGrid { get; private set; }

    private void Awake()
    {
    }

    void OnEnable()
    {
        Instance = this;
    }

    public bool IsGridSpaceFree(Vector2 position)
    {
        position = (Vector3)WorldGrid.WorldToCell((Vector3)position);
        if (Physics2D.OverlapCircle(position, 0.1f, gridCheckLayerMask) != null)
            return false;
        return true;
    }
    public Vector3 GetCorrenctionToCellCenter(Vector3 position) => WorldGrid.GetCellCenterLocal(WorldGrid.WorldToCell(position));
}
