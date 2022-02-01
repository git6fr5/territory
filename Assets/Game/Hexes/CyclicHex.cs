/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class CyclicHex : Hex {

    public int cycleLength;
    public int[] appearsOnIndices;

    public Text turnIndicator;

    protected override void Render() {
        base.Render();

        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        turnIndicator.gameObject.SetActive(true);

        if (cycleLength == 0) {
            cycleLength = 1;
        }

        bool render = false;
        int currIndex = Map.TurnNumber % cycleLength;
        for (int i = 0; i < appearsOnIndices.Length; i++) {
            if (appearsOnIndices[i] == currIndex) {
                render = true;
                break;
            }
        }

        int numberOfTurnsTillSomethingHappens = 0;
        List<int> L_Indices = new List<int>();
        for (int i = 0; i < appearsOnIndices.Length; i++) {
            L_Indices.Add(appearsOnIndices[i]);
        }

        for (int i = currIndex; i < cycleLength + currIndex; i++) {
            int index = i % cycleLength;
            if (!render && L_Indices.Contains(i)) {
                break;
            }
            else if (render && !L_Indices.Contains(i)) {
                break;
            }
            numberOfTurnsTillSomethingHappens += 1;
        }

        if (render) {
            spriteRenderer.material.SetFloat("_Opacity", 1f);
            hexCollider.enabled = true;
        }
        else {
            spriteRenderer.material.SetFloat("_Opacity", 0f);
            if (piece != null) {
                Destroy(piece.gameObject);
                piece = null;
            }
            hexCollider.enabled = false;
            mouseOver = false;

        }

        turnIndicator.text = numberOfTurnsTillSomethingHappens.ToString();

    }

}