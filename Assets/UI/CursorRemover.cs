using UnityEngine;

public class CursorRemover : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
