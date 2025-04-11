using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParallaxSystem : MonoBehaviour
{
    // For background/foreground layers
    [System.Serializable] public class ParallaxLayer
    {
        public Transform parallaxLayer;     // Layer of background / foreground
        public float parallaxScale; // Scale of parallax moving
        public float layerWidth;    // Width of layer (for infinite looping)
        public bool isLooping;     // Is the layer looping?
    }

    public List<ParallaxLayer> layers; // All the background / foreground layers
    public float smoothingSpeed = 1.0f;
    
    public float multiply = 10.0f;
    
    // Camera
    [SerializeField] private Transform camera;
    private Vector3 previousCameraPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        // Wiring Camera
        if (Camera.main != null) camera = Camera.main.transform;
        previousCameraPosition = camera.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.isInBattle && 
            !CameraManager.instance.cinemachineBrain.IsBlending)
        {
            foreach (ParallaxLayer layer in layers)
            {
                // Parallax effect
                float parallax = (previousCameraPosition.x - camera.position.x) * layer.parallaxScale;
                float targetX = layer.parallaxLayer.position.x + parallax;
                Vector3 target =
                    new Vector3(targetX, layer.parallaxLayer.position.y, layer.parallaxLayer.position.z);

                layer.parallaxLayer.position =
                    Vector3.Lerp(layer.parallaxLayer.position, target, smoothingSpeed /* * Time.deltaTime*/);

                // Infinite scrolling
                // ... going right
                if (camera.position.x > layer.parallaxLayer.position.x + layer.layerWidth &&
                    layer.isLooping)
                {
                    Vector3 newLayerPosition =
                        new Vector3(
                            layer.parallaxLayer.position.x + 2 * layer.layerWidth,
                            layer.parallaxLayer.position.y,
                            layer.parallaxLayer.position.z);

                    layer.parallaxLayer.position = newLayerPosition;
                }
                // ... going left
                else if (camera.position.x < layer.parallaxLayer.position.x - layer.layerWidth &&
                         layer.isLooping)
                {
                    Vector3 newLayerPosition =
                        new Vector3(
                            layer.parallaxLayer.position.x - 2 * layer.layerWidth,
                            layer.parallaxLayer.position.y,
                            layer.parallaxLayer.position.z);

                    layer.parallaxLayer.position = newLayerPosition;
                }
            }

            // Update camera position
            previousCameraPosition = camera.position;
        }
    }
}
