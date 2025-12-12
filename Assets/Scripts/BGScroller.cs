using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float maxX = 5.5f;
    public float speed = 0.01f;
    public float currentPos = 0f;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentPos += speed;
        gameObject.transform.position = new Vector3(currentPos, 0, 0);
        if (currentPos > maxX) 
        {
            gameObject.transform.position = Vector3.zero;
            currentPos = 0;
        }
        
    }
}
