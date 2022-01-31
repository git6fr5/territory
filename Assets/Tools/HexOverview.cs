/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Definitions --- */
using GridPosition = Hex.GridPosition;

/// <summary>
/// 
/// </summary>
[ExecuteInEditMode]
public class HexOverview : MonoBehaviour {

    /* --- Data Structures --- */
    [System.Serializable]
    public struct HexOverviewData {

        /* --- Properties --- */
        [SerializeField, ReadOnly] public int id;
        [SerializeField] public Vector2Int gridPosition;
        [SerializeField] public bool delete;

        /* --- Callbacks --- */
        public GridPosition m_GridPosition => new GridPosition(gridPosition.x, gridPosition.y);

        /* --- Constructor --- */
        public HexOverviewData(int id, GridPosition gridPosition) {
            this.id = id;
            this.gridPosition = new Vector2Int(gridPosition.col, gridPosition.row);
            this.delete = false;
        }
    }

    /* --- Parameters --- */
    [SerializeField] public bool edit = false;

    /* --- Properties --- */
    [SerializeField] private List<HexOverviewData> overview;

    /* --- Unity --- */
    // Runs once every frame.
    private void Update() {
        if (edit) {
            EditCollection();
        }
        RecollectHexes();
    }

    /* --- Methods --- */
    private void EditCollection() {
        for (int i = 0; i < overview.Count; i++) {
            Hex hex = Hex.FindByID(overview[i].id);
            if (hex != null) {
                hex.gridPosition = overview[i].m_GridPosition;
                if (overview[i].delete) {
                    DestroyImmediate(hex.gameObject);
                }
            }
        }
    }

    private void RecollectHexes() {
        Hex[] hexes = Hex.FindAllHexes();
        overview = new List<HexOverviewData>();
        for (int i = 0; i < hexes.Length; i++) {
            overview.Add(new HexOverviewData(hexes[i].id, hexes[i].gridPosition));
        }
    }

}
