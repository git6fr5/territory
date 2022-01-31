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
public class HexPositioner : MonoBehaviour {

    /* --- Parameters --- */
    [SerializeField] public bool edit = false;
    [SerializeField] public bool snap = true;

    /* --- Unity --- */
    // Runs once every frame.
    private void Update() {
        if (edit) {
            SetGridPositions();
        }
        if (snap) {
            SnapPositions();
        }
    }

    /* --- Methods --- */
    private void SetGridPositions() {
        Hex[] hexes = Hex.FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            hexes[i].gridPosition = Hex.GetGridPosition(hexes[i].transform.position);
        }
    }

    private void SnapPositions() {
        Hex[] hexes = Hex.FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            hexes[i].transform.position = Hex.GetWorldPosition(hexes[i].gridPosition);
        }
    }

}
