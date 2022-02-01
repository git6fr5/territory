using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour {

    // Instance.
    public static GameRules Instance;
    // Camera.
    public static UnityEngine.Camera MainCamera;
    public static Vector3 MousePosition => MainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

    /* --- Unity --- */
    // Runs once before the first frame.
    void Start() {
        Init();
    }

    /* --- Methods --- */
    private void Init() {
        // Set these static variables.
        MainCamera = Camera.main;
        Instance = this;
    }

}
