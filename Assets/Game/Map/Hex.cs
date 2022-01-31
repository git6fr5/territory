/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Hex : MonoBehaviour {

    /* --- Data Structures --- */
    [System.Serializable]
    public struct GridPosition {

        public int col;
        public int row;

        public GridPosition(int col, int row) {
            this.col = col;
            this.row = row;
        }

    }

    /* --- Parameters --- */
    [SerializeField] public GridPosition gridPosition;

    /* --- Properties --- */
    [SerializeField] public int id;

    /* --- Switches --- */
    [SerializeField, ReadOnly] private bool initialized;

    /* --- Unity --- */
    private void Start() {
        if (!initialized) {
            Init();
        }
    }

    /* --- Methods --- */
    public void Init() {
        id = (int)Random.Range((int)1e8, (int)(1e9 - 1));
        initialized = true;
    }

    /* --- Static Methods --- */
    public static GridPosition GetGridPosition(Vector3 position) {
        int row = (int)Mathf.Round(position.y);
        position.x = row % 2 == 0 ? position.x - 0.5f : position.x;
        int col = (int)Mathf.Round(position.x);
        return new GridPosition(col, row);
    }

    public static Vector3 GetWorldPosition(GridPosition gridPosition) {
        float xOffset = gridPosition.row % 2 == 0 ? 0.5f : 0f;
        float x = (float)gridPosition.col + xOffset;
        float y = (float)gridPosition.row;
        return new Vector3(x, y, 0);
    }

    public static Hex[] FindAllHexes() {
        Hex[] hexes = (Hex[])GameObject.FindObjectsOfType(typeof(Hex));
        return hexes;
    }

    public static Hex FindByID(int id) {
        Hex[] hexes = FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            if (hexes[i].id == id) {
                return hexes[i];
            }
        }
        Debug.Log("No hexes found with the id: " + id.ToString());
        return null;
    }

    //public static Hex[] FindAdjacentHexes(Hex hex) {
    //    Hex[] hexes = FindAllHexes();
    //    for (int i = 0; i < hexes.Length; i++) {
    //        if (hex.gridPosition == )
    //    }
    //    return hexes;
    //}

}
