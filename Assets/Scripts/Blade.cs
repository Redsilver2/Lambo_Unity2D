using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void LateUpdate()
    {
        this.transform.Rotate(Vector3.forward * rotationSpeed);
    }
}
