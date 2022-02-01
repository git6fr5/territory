/* --- Libraries --- */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[DefaultExecutionOrder(100)]
public class Piece : MonoBehaviour {

    /* --- Properties --- */
    // int id;

    /* --- Methods --- */
    public virtual void Init() {
        //
    }

    // Runs once every turn.
    public virtual bool MakeMove() {
        return false;
    }

    public virtual bool Interact(Piece piece) {
        return false;
    }

    /* --- Static Methods --- */
    public static Player FindPlayerPiece() {
        Piece[] pieces = Piece.FindAllPieces();
        for (int i = 0; i < pieces.Length; i++) {
            Player player = pieces[i].GetComponent<Player>();
            if (player != null) {
                return player;
            }
        }
        Debug.Log("Could not find player piece");
        return null;
    }

    public static Coin FindCoinPiece() {
        Piece[] pieces = Piece.FindAllPieces();
        for (int i = 0; i < pieces.Length; i++) {
            Coin coin = pieces[i].GetComponent<Coin>();
            if (coin != null) {
                return coin;
            }
        }
        Debug.Log("Could not find coin piece");
        return null;
    }

    public static Enemy[] FindAllEnemyPieces() {
        Piece[] pieces = Piece.FindAllPieces();
        List<Enemy> L_Enemies = new List<Enemy>();
        for (int i = 0; i < pieces.Length; i++) {
            Enemy enemy = pieces[i].GetComponent<Enemy>();
            if (enemy != null) {
                L_Enemies.Add(enemy);
            }
        }
        Enemy[] enemies = L_Enemies.ToArray();
        Array.Sort<Enemy>(enemies, new Comparison<Enemy>((enemyA, enemyB) => Enemy.Compare(enemyA, enemyB)));
        return enemies;
    }

    public static Piece[] FindAllPieces() {
        return (Piece[])GameObject.FindObjectsOfType(typeof(Piece));
    }

}
