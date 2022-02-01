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
    public Hex enemyHex;
    public Hex cyclicHex;
    public Hex spawnHex;
    public Hex coinHex;

    /* --- Switches --- */
    public bool createBasicHex = false;
    public bool createEnemyHex = false;
    public bool createCyclicHex = false;
    public bool spawnHexExists = false;
    public bool coinHexExists = false;

    /* --- Unity --- */
    // Runs once every frame.
    private void Update() {

        if (createBasicHex) {
            CreateHex(basicHex);
            createBasicHex = false;
        }

        if (createEnemyHex) {
            CreateHex(enemyHex);
            createEnemyHex = false;
        }

        if (createCyclicHex) {
            CreateHex(cyclicHex);
            createCyclicHex = false;
        }

        spawnHexExists = CheckForSpawnHex();
        if (!spawnHexExists) {
            CreateHex(spawnHex);
        }

        coinHexExists = CheckForCoinHex();
        if (!coinHexExists) {
            CreateHex(coinHex);
        }
    }

    /* --- Methods --- */
    private void CreateHex(Hex hex) {
        Hex newHex = Instantiate(hex.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Hex>();
        newHex.Init();
    }

    private bool CheckForSpawnHex() {
        Hex[] hexes = Hex.FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            SpawnHex spawnHex = hexes[i].GetComponent<SpawnHex>();
            if (spawnHex != null) {
                return true;
            }
        }
        return false;
    }

    private bool CheckForCoinHex() {
        Hex[] hexes = Hex.FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            CoinHex coinHex = hexes[i].GetComponent<CoinHex>();
            if (coinHex != null) {
                return true;
            }
        }
        return false;
    }

}
