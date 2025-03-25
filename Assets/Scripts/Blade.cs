using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void LateUpdate()
    {
        this.transform.Rotate(Time.deltaTime * Vector3.forward * rotationSpeed);
    }
}
