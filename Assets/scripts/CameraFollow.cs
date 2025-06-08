using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    void LateUpdate()
    {
        if (Target != null)
            transform.position = new Vector3(Target.position.x, Target.position.y, -10f);
    }
}
