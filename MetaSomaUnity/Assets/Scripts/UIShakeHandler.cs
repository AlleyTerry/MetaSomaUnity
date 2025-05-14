using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShakeHandler : MonoBehaviour
{
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private RectTransform originalRectTransform;
    public static UIShakeHandler instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        originalRectTransform = GetComponent<RectTransform>();
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
            transform.position = originalPosition + 
                                 new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * 
                                 intensity;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        GetComponent<RectTransform>().transform.localPosition = originalRectTransform.localPosition;
        
        StartCoroutine(ImageCorrection());
    }

    private IEnumerator ImageCorrection()
    {
        yield return new WaitForEndOfFrame();
        
        // Correct the image position
        transform.position = originalPosition;
        GetComponent<RectTransform>().transform.localPosition = originalRectTransform.localPosition;
        
        Debug.Log("Image position corrected.");
    }
    
    // pre-made shake functions
    public void ShakeLow() => Shake(5f, 0.2f);
    public void ShakeStrong() => Shake(10f, 0.5f);
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
