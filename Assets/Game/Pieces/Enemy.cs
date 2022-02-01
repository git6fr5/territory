/* --- Libraries --- */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy : Piece {

    /* --- Properties --- */
    [SerializeField] public int order;

    /* --- Methods --- */
    public void Init(int order) {
        base.Init();
        this.order = order;
    }

    // Runs once every turn.
    public override bool MakeMove() {
        base.MakeMove();

        // Get the hex that this piece is on.
        Hex thisHex = Hex.FindHexWithPiece(this);

        // Get adjacent hexes.
        Hex[] adjacentHexes = Hex.FindAdjacentHexes(thisHex);
        List<Hex> pathableAdjacentHexes = new List<Hex>();
        // Move to the first adjacent one that it can.
        for (int i = 0; i < adjacentHexes.Length; i++) {
            bool canMove = Hex.CheckMove(thisHex, adjacentHexes[i]);
            if (canMove) {
                pathableAdjacentHexes.Add(adjacentHexes[i]);
            }
        }

        // Get the best move to make.
        Coin coin = Piece.FindCoinPiece();
        Hex coinHex = Hex.FindHexWithPiece(coin);

        int minDepth = 500;
        Hex bestHex = null;
        for (int i = 0; i < pathableAdjacentHexes.Count; i++) {

            if (pathableAdjacentHexes[i] == coinHex) {
                bestHex = pathableAdjacentHexes[i];
                minDepth = 1;
                break;
            }

            int adjancencyDepth = Hex.AdjacencyDepth(pathableAdjacentHexes[i], coinHex);
            if (adjancencyDepth < minDepth) {
                bestHex = pathableAdjacentHexes[i];
                minDepth = adjancencyDepth;
            }
        }
        Debug.Log("This enemy is " + minDepth.ToString() + " steps from the coin");

        // Make the move.
        if (bestHex != null) {
            bool madeMove = Hex.MovePiece(thisHex, bestHex);
            if (madeMove) {
                return true;
            }
        }

        // If no move was possible.
        Debug.Log("Enemy Got Angry, You Lose");
        return false;

    }

    public override bool Interact(Piece piece) {

        bool successfulInteraction = false;
        Coin coin = piece.GetComponent<Coin>();
        if (coin != null) {
            successfulInteraction = true;
            print("Lost Game");
        }

        Player player = piece.GetComponent<Player>();
        if (player != null) {
            successfulInteraction = false;
        }

        return successfulInteraction;
    }

    // Compare the order of the enemies.
    public static int Compare(Enemy enemyA, Enemy enemyB) {
        return enemyA.order.CompareTo(enemyB.order);
    }

}
