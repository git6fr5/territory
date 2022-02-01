/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
[DefaultExecutionOrder(100)]
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

    /* --- Components --- */
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public PolygonCollider2D hexCollider;

    /* --- Parameters --- */
    [SerializeField] public GridPosition gridPosition;

    /* --- Properties --- */
    [SerializeField, ReadOnly] public int id;
    [SerializeField, ReadOnly] public Piece piece;

    /* --- Switches --- */
    [SerializeField, ReadOnly] private bool initialized;
    [SerializeField, ReadOnly] public bool mouseOver;
    [SerializeField, ReadOnly] public bool mouseAdjacent;

    /* --- Unity --- */
    private void Start() {
        if (!initialized) {
            Init();
        }
    }

    void Update() {
        UpdatePiece();
        Render();
    }

    void LateUpdate() {
        LateRender();
    }

    private void OnMouseOver() {
        mouseOver = true;
    }

    private void OnMouseExit() {
        mouseOver = false;
    }

    /* --- Methods --- */
    public virtual void Init() {
        // Get a unique id. (not unique rn but very low chance of overlap).
        id = (int)Random.Range((int)1e8, (int)(1e9 - 1));

        // Set up the collider.
        List<Vector2> L_Points = new List<Vector2>();
        for (int i = 0; i < 6; i++) {
            L_Points.Add(Quaternion.Euler(0f, 0f, 60f * i) * Vector2.up / 2f);
        }
        hexCollider = GetComponent<PolygonCollider2D>();
        hexCollider.points = new Vector2[1];
        hexCollider.SetPath(0, L_Points.ToArray());

        // Set up the renderer.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Confirm the initialization.
        initialized = true;
    }

    private void UpdatePiece() {
        if (piece != null) {
            Vector3 position = transform.position;
            position.z = -1f;
            piece.transform.position = position;
        }
    }

    public void RemovePiece() {
        piece = null;
    }

    public void SetPiece(Piece piece) {
        this.piece = piece;
    }

    protected virtual void Render() {
        Vector4 scale = new Vector4(1f, 1f, 1f, 1f);
        if (mouseOver) {
            spriteRenderer.material.SetVector("_Stretch", scale * 1.25f);
            Hex[] adjacentHexes = Hex.FindAdjacentHexes(this);
            for (int i = 0; i < adjacentHexes.Length; i++) {
                adjacentHexes[i].mouseAdjacent = true;
            }
        }
        else {
            spriteRenderer.material.SetVector("_Stretch", scale * 1f);
        }

        Player player = Piece.FindPlayerPiece();
        Hex playerHex = Hex.FindHexWithPiece(player);

        if (playerHex != null && Hex.IsAdjacent(this, playerHex)) {
            spriteRenderer.material.SetColor("_AddColor", Color.red * 0.5f);
        }
        else {
            spriteRenderer.material.SetColor("_AddColor", Color.white * 0f);
        }
    }

    private void LateRender() {
        Vector4 scale = new Vector4(1f, 1f, 1f, 1f);
        if (mouseAdjacent) {
            //spriteRenderer.material.SetColor("_AddColor", Color.red * 0.25f);
            //mouseAdjacent = false;
        }
        else {
            //spriteRenderer.material.SetColor("_AddColor", Color.white * 0f);
        }
    }

    /* --- Static Methods --- */
    public static float HexOffset = 0.75f;
    public static GridPosition GetGridPosition(Vector3 position) {
        int row = (int)Mathf.Round(position.y / HexOffset);
        position.x = row % 2 == 0 ? position.x - 0.5f : position.x;
        int col = (int)Mathf.Round(position.x);
        return new GridPosition(col, row);
    }

    public static Vector3 GetWorldPosition(GridPosition gridPosition) {
        float xOffset = gridPosition.row % 2 == 0 ? 0.5f : 0f;
        float x = (float)gridPosition.col + xOffset;
        float y = (float)gridPosition.row * HexOffset;
        return new Vector3(x, y, 0);
    }

    public static bool MovePiece(Hex origin, Hex destination) {
        
        bool successfulMove = CheckMove(origin, destination);

        if (successfulMove) {
            destination.SetPiece(origin.piece);
            origin.RemovePiece();
        }

        return successfulMove;
    }

    public static bool CheckMove(Hex origin, Hex destination) {
        bool successfulMove = false;
        if (destination.piece != null) {
            successfulMove = origin.piece.Interact(destination.piece);
        }
        else {
            successfulMove = true;
        }

        return successfulMove;
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

    public static Hex FindHexWithPiece(Piece piece) {
        Hex[] hexes = Hex.FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            if (hexes[i].piece == piece) {
                return hexes[i];
            }
        }
        Debug.Log("Could not find the hex with the given piece");
        return null;
    }

    public static Hex FindSelectedHex() {
        Hex[] hexes = Hex.FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            if (hexes[i].mouseOver) {
                return hexes[i];
            }
        }
        Debug.Log("Not selecting any hexes");
        return null;
    }

    public static SpawnHex FindSpawnHex() {
        Hex[] hexes = Hex.FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            SpawnHex spawnHex = hexes[i].GetComponent<SpawnHex>();
            if (spawnHex != null) {
                return spawnHex;
            }
        }
        Debug.Log("No spawn hex");
        return null;
    }

    public static CoinHex FindCoinHex() {
        Hex[] hexes = Hex.FindAllHexes();
        for (int i = 0; i < hexes.Length; i++) {
            CoinHex coinHex = hexes[i].GetComponent<CoinHex>();
            if (coinHex != null) {
                return coinHex;
            }
        }
        Debug.Log("No coin hex");
        return null;
    }

    public static EnemyHex[] FindEnemyHexes() {
        Hex[] hexes = Hex.FindAllHexes();
        List<EnemyHex> L_EnemyHexes = new List<EnemyHex>();
        for (int i = 0; i < hexes.Length; i++) {
            EnemyHex enemyHex = hexes[i].GetComponent<EnemyHex>();
            if (enemyHex != null) {
                L_EnemyHexes.Add(enemyHex);
            }
        }
        return L_EnemyHexes.ToArray();
    }

    public static Hex[] FindAdjacentHexes(Hex hex) {
        Hex[] hexes = FindAllHexes();
        List<Hex> L_AdjacentHexes = new List<Hex>();
        for (int i = 0; i < hexes.Length; i++) {
            if (hexes[i].hexCollider.enabled) {
                if (Hex.IsAdjacent(hex, hexes[i])) {
                    L_AdjacentHexes.Add(hexes[i]);
                }
                if (L_AdjacentHexes.Count == 6) {
                    break;
                }
            }
        }
        return L_AdjacentHexes.ToArray();
    }

    public static bool IsAdjacent(Hex hexA, Hex hexB) {
        int colOffset = 1;
        if (hexA.gridPosition.row % 2 == 0) {
            colOffset = -1;
        }
        int colDistance = hexA.gridPosition.col - hexB.gridPosition.col;
        int rowDistance = hexA.gridPosition.row - hexB.gridPosition.row;
        if (Mathf.Abs(rowDistance) == 1 && (colDistance == 0 || colDistance == colOffset)) {
            return true;
        }
        else if (Mathf.Abs(colDistance) == 1 && rowDistance == 0) {
            return true;
        }
        return false;
    }

    public static int AdjacencyDepth(Hex hexA, Hex hexB, int depth = 1, int recursiveLimit = 5) {

        // return 1;

        if (depth > recursiveLimit) {
            return depth;
        }

        Hex[] adjacentHexes = Hex.FindAdjacentHexes(hexA);
        for (int i = 0; i < adjacentHexes.Length; i++) {
            if (adjacentHexes[i] == hexB) {
                return depth;
            }
        }

        int minDepth = recursiveLimit;
        for (int i = 0; i < adjacentHexes.Length; i++) {
            int foundDepth = AdjacencyDepth(hexA, hexB, depth + 1);
            if (foundDepth < minDepth) {
                minDepth = foundDepth;
            }
        }

        return minDepth;

    }

}
