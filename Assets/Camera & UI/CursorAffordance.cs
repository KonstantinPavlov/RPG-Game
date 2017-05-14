using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {
    
    [SerializeField]
    Vector2 cursorHotspot = new Vector2(0,0);
    [SerializeField]
    Texture2D walkCursor = null;
    [SerializeField]
    Texture2D attackCursor = null;
    [SerializeField]
    Texture2D endWalkCursor = null;
    [SerializeField]
    Texture2D unusedCursor = null;

    private CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.layerChangeObserver += OnLayerChange;
	}
	
	// Change cursor, observer pattern
	void OnLayerChange() {
        switch (cameraRaycaster.currentLayerHit)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(endWalkCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unusedCursor, cursorHotspot, CursorMode.Auto);
                Debug.LogError("Unexpeted Layer!");
                return;
        }            
       
    }
}
