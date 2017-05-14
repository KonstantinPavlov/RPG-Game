using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    float walkStopRadius = 0.2f;
    
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
        
    private void Start() {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>(); 
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate() {
        if (Input.GetMouseButton(0)) {
            print("Cursor raycast hit layer: " + cameraRaycaster.layerHit);
         
            switch (cameraRaycaster.layerHit) {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;                    
                    break;
                case Layer.Enemy:
                    print("NOT moving to enemy!");
                    break;
                default:
                    print("Unexpected layer found!");
                    return;
            }
        }

        var playerToClickPoint = currentClickTarget - transform.position;
        if(playerToClickPoint.magnitude >= walkStopRadius) {
          m_Character.Move(playerToClickPoint, false, false);
        }
        else {
            m_Character.Move(Vector3.zero, false, false);
        }            

    }
}

