/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[DefaultExecutionOrder(200)]
public class Map : MonoBehaviour {

    /* --- Data Structures --- */
    public enum Phase {
        PlayerPhase,
        EnemyPhase,
        Count
    }

    /* --- Static Variables --- */
    public static int TurnNumber;

    /* --- Components --- */
    [SerializeField] public Piece player;
    [SerializeField] public Piece coin;
    [SerializeField] public Piece enemy;

    /* --- Parameters --- */
    [SerializeField] private float enemyMoveInterval;

    /* --- Properties --- */
    [SerializeField, ReadOnly] private int turnNumber;
    [SerializeField, ReadOnly] private Phase phase;
    [SerializeField, ReadOnly] private int enemyIndex;
    [SerializeField, ReadOnly] private float enemyMoveTicks;
    [SerializeField, ReadOnly] private int hexCount;
    [HideInInspector] private Hex[] hexes;

    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        Debug.Log("Initializing Map");
        Init();
    }

    private void Update() {
        turnNumber = TurnNumber;
        if (phase == Phase.PlayerPhase) {
            if (Input.GetMouseButtonDown(0)) {
                PlayerPhase();
            }
        }
        else if (phase == Phase.EnemyPhase) {
            enemyMoveTicks += Time.deltaTime;
            if (enemyMoveTicks > enemyMoveInterval) {
                EnemyPhase();
                enemyMoveTicks = 0f;
            }
        }
    }

    /* --- Methods --- */
    private void Init() {
        hexes = Hex.FindAllHexes();
        hexCount = hexes.Length;

        SpawnHex spawnHex = Hex.FindSpawnHex();
        if (spawnHex != null) {
            SpawnPlayer(spawnHex);
        }

        CoinHex coinHex = Hex.FindCoinHex();
        if (coinHex != null) {
            SpawnCoin(coinHex);
        }

        EnemyHex[] enemyHexes = Hex.FindEnemyHexes();
        if (enemyHexes != null && enemyHexes.Length > 0) {
            for (int i = 0; i < enemyHexes.Length; i++) {
                SpawnEnemy(enemyHexes[i]);
            }
        }

        TurnNumber = 0;
        phase = Phase.PlayerPhase;
        enemyIndex = 0;

    }

    public void PlayerPhase() {
        Player player = Piece.FindPlayerPiece();
        bool madeMove = player.MakeMove();
        if (madeMove) {
            Debug.Log("Player Makes Their Move");
            NextPhase();
        }
    }

    public void EnemyPhase() {
        Enemy[] enemies = Piece.FindAllEnemyPieces();
        if (enemyIndex < enemies.Length) {
            bool madeMove = enemies[enemyIndex].MakeMove();
            if (madeMove) {
                enemyIndex += 1;
            }
            Debug.Log("Enemies Make Their Move");
        }
        if (enemyIndex >= enemies.Length) {
            NextPhase();
        }
    }

    private void NextPhase() {
        int nextPhase = (int)phase + 1;
        if (nextPhase == (int)Phase.Count) {
            TurnNumber += 1;
            nextPhase = 0;
        }

        phase = (Phase)nextPhase;
        if (phase == Phase.EnemyPhase) {
            enemyIndex = 0;
        }
    }

    private void SpawnPlayer(SpawnHex spawnHex) {
        Player newPlayer = Instantiate(player.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Player>();
        newPlayer.Init();
        spawnHex.SetPiece(newPlayer);
    }

    private void SpawnCoin(CoinHex coinHex) {
        Coin newCoin = Instantiate(coin.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Coin>();
        newCoin.Init();
        coinHex.SetPiece(newCoin);
    }

    private void SpawnEnemy(EnemyHex enemyHex) {
        Enemy newEnemy = Instantiate(enemy.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Enemy>();
        newEnemy.Init(enemyHex.order);
        enemyHex.SetPiece(newEnemy);
    }

}
