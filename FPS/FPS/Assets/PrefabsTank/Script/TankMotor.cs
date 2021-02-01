using UnityEngine;

public class TankMotor : MonoBehaviour
{
    private const float LIMITE_BASSE_CANON = 352.0f;
    private const float LIMITE_HAUTE_CANON = 9.1f;

    private Transform Turret;
    private Transform Canon;

    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;

    public void MovePlayer(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void RotatePlayer(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    private void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            GetComponent<Transform>().position += velocity * Time.fixedDeltaTime;
        }
    }

    private void PerformRotation()
    {
        Turret.rotation *= Quaternion.Euler(rotation);

        if(!((Canon.eulerAngles.x - cameraRotation.x) > LIMITE_HAUTE_CANON && (Canon.eulerAngles.x - cameraRotation.x) < LIMITE_BASSE_CANON))
        {
            Canon.Rotate(-cameraRotation);
        }
    }

    void Start()
    {
        Turret = transform.GetChild(2).GetComponent<Transform>();
        Canon = transform.GetChild(2).GetChild(0).GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
}
