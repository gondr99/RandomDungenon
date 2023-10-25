using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            Debug.Log("End!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
