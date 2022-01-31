/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Map : MonoBehaviour {

    /* --- Properties --- */
    [SerializeField, ReadOnly] private int hexCount;
    [HideInInspector] private Hex[] hexes;

    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        Init();
    }

    /* --- Methods --- */
    private void Init() {
        hexes = Hex.FindAllHexes();
        hexCount = hexes.Length;
    }

}
