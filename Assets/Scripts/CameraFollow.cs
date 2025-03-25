using UnityEngine;

public class CameraFollow : MonoBehaviour
{


    //membuat variabel yang menjadi target
    //untuk diikuti oleh Camera
    private Transform target;
    
    private void Start()
    {
        PlayerController controller = FindObjectOfType<PlayerController>();

        if (controller != null)
            target = controller.transform;
        else
            Debug.Log("Error! No Player Controller Found For Target!");
    }


    private void LateUpdate()
    {
        this.transform.position = Vector3.right   * target.transform.position.x +
                                  Vector3.up      * target.transform.position.y +
                                  Vector3.forward * (target.transform.position.z - 10f);
    }
}