using UnityEngine;

public class SpinKey : MonoBehaviour
{
    public float bounceHeight = 0.05f;
    public float bounceSpeed = 0.5f;
    public float spinSpeed = 50f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), spinSpeed * Time.deltaTime);
        float yOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}
