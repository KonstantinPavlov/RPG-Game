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
    [SerializeField]
    const int walkableLayerNumber = 8;
    [SerializeField]
    const int enemyLayerNumber = 9;

    [SerializeField]
    int walkable;

    private CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged;
	}
	
	// Change cursor, observer pattern
	void OnLayerChanged(int newLayer) {
        print("Use delegate");
        switch (newLayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                break;            
            default:
                Cursor.SetCursor(unusedCursor, cursorHotspot, CursorMode.Auto);               
                return;
        }           
       
    }
}
