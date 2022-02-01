/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Player : Piece {
    
    /* --- Methods --- */
    public override void Init() {
        base.Init();
    }

    // Runs once every turn.
    public override bool MakeMove() {
        base.MakeMove();

        bool madeMove = false;

        // Find the player hex.
        Hex playerHex = Hex.FindHexWithPiece(this);
        // Find the hex that the mouse is over.
        Hex selectedHex = Hex.FindSelectedHex();
        // Check if they are adjacent.
        if (playerHex != null && selectedHex != null) {
            bool adjacency = Hex.IsAdjacent(playerHex, selectedHex);
            if (adjacency) {
                madeMove = Hex.MovePiece(playerHex, selectedHex);
            }
        }

        return madeMove;

    }

    public override bool Interact(Piece piece) {

        bool successfulInteraction = false;
        Coin coin = piece.GetComponent<Coin>();
        if (coin != null) {
            successfulInteraction = true;
            print("Won Game");
        }

        Enemy enemy = piece.GetComponent<Enemy>();
        if (enemy != null) {
            successfulInteraction = false;
            Debug.Log("Cannot Move Into Enemy");
        }

        return successfulInteraction;
    }

}
