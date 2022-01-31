/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[ExecuteInEditMode]
public class HexCreator : MonoBehaviour {

    /* --- Components --- */
    public Hex basicHex;

    /* --- Switches --- */
    public bool createBasicHex = false;

    /* --- Unity --- */
    // Runs once every frame.
    private void Update() {
        if (createBasicHex) {
            CreateHex(basicHex);
            createBasicHex = false;
        }
    }

    /* --- Methods --- */
    private void CreateHex(Hex hex) {
        Instantiate(hex.gameObject, Vector3.zero, Quaternion.identity, transform);
        hex.Init();
    }

}
