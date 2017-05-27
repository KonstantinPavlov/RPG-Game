using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {


    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;
    AICharacterControl aiCharacterControl;
    GameObject walkTarget = null;

    // TODO solve Serialized or const fields
    [SerializeField]
    const int walkableLayerNumber = 8;
    [SerializeField]
    const int enemyLayerNumber = 9;

    private void Start() {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>(); 
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        aiCharacterControl = GetComponent<AICharacterControl>();
        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
        walkTarget = new GameObject("walkTarget");
    }

    private void ProcessMouseClick(RaycastHit raycastHit, int layerHit) {
    
        switch (layerHit){
            case enemyLayerNumber:
                // navigate to enemy 
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                // navigate to point on gorund                
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;
            default:
                Debug.LogWarning("Cant handle layer in ProcessMouseClick");
                break;
        }
            

    }
    // Fixed update is called in sync with physics
    private void FixedUpdate() {
       // ProcessMouseMovement();// Mouse movement

    }  

}

