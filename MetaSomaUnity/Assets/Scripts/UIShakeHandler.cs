using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShakeHandler : MonoBehaviour
{
    private Vector3 orginalPosition;
    public static UIShakeHandler instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        orginalPosition = transform.position;
    }

    public void Shake(float intensity, float duration)
    {
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }
    
    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = orginalPosition + 
                                 new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * 
                                 intensity;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = orginalPosition;
    }
    
    // pre-made shake functions
    public void ShakeLow() => Shake(5f, 0.2f);
    public void ShakeStrong() => Shake(10f, 0.5f);
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
