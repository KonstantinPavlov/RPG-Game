using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable            
    };

    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit raycastHit;
    public RaycastHit hit
    {
        get { return raycastHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }

    public delegate void OnLayerChange(Layer newLayer); // declare new Delegate type 
    public event OnLayerChange onLayerChange; // instatiate an observer set
    
    void Start() {
        viewCamera = Camera.main;            
    }

    void Update(){
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                raycastHit = hit.Value;
                // if layer changed 
                if (layerHit != layer){
                    layerHit = layer;
                    onLayerChange(layer);
                }                
                return;
            }
        }
        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        if (layerHit != Layer.RaycastEndStop){
            onLayerChange(Layer.RaycastEndStop);
        }
        layerHit = Layer.RaycastEndStop;        
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition); // make ray from camera view to mouse point

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
   
}
