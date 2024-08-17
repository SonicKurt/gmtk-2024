using UnityEngine;

public class GoalGet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("You win!");
        }
    }
}
