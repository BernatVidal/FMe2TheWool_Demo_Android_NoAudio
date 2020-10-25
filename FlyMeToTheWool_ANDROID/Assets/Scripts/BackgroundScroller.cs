using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{

    public float scrollSpeed = 10;

    private Vector3 startPosition;

    private float tileSize;
  
    void Start()
    {
        startPosition = transform.position;
        tileSize = this.gameObject.transform.GetChild(0).transform.position.y;
    }


    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSize);
        transform.position = startPosition + Vector3.down * newPosition;
    }
}
