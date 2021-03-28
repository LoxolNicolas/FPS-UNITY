using UnityEngine;
using Mirror;

public class TankMotor : NetworkBehaviour
{
    public GameObject go;

    private const float LIMITE_BASSE_CANON = -8.0f;
    private const float LIMITE_HAUTE_CANON = 9.1f;

    public Transform Turret;
    public Transform Canon;

    [SerializeField]
    public float playerSpeed = 0;
    [SerializeField]
    private float playerRotation = 5.0f;
    [SerializeField]
    private float sensitivityX = 3.0f;
    [SerializeField]
    private float sensitivityY = 3.0f;

    [ClientRpc]
    void RpcMoveTank(Vector3 movement, Vector3 tankRotation)
    {
        go.GetComponent<Transform>().Rotate(tankRotation); // 
        go.GetComponent<Transform>().position += movement;
    }

    [ClientRpc]
    private void RpcMoveTurret(float turretElevation, float turretRotation)
    {
        Turret.rotation *= Quaternion.Euler(new Vector3(0, turretRotation, 0));
        Canon.Rotate(new Vector3(-turretElevation, 0, 0));
    }

    void Start()
    {
        Turret = go.transform.GetChild(2).GetComponent<Transform>();
        Canon = go.transform.GetChild(2).GetChild(0).GetComponent<Transform>();
    }

    [Command]
    void CmdMovePlayer(float vertical, float horizontal, float xMouse, float yMouse)
    {
        Vector3 playerMoveZ = go.transform.forward * vertical;
        Vector3 velocity = playerMoveZ.normalized * playerSpeed;
        Vector3 movement = velocity * Time.fixedDeltaTime;

        Vector3 tankRotation = new Vector3(0.0f, horizontal * playerRotation * Time.fixedDeltaTime, 0.0f);

        RpcMoveTank(movement, tankRotation);

        float turretRotation = xMouse * sensitivityX;
        float turretElevation = yMouse * sensitivityY;

        float currentelevation = Canon.localEulerAngles.x;
        if (currentelevation > 180)
        {
            currentelevation -= 360;
        }

        currentelevation *= -1;

        if ((currentelevation + turretElevation) > LIMITE_HAUTE_CANON)
        {
            turretElevation = LIMITE_HAUTE_CANON - currentelevation;
        }

        if ((currentelevation + turretElevation) < LIMITE_BASSE_CANON)
        {
            turretElevation = LIMITE_BASSE_CANON - currentelevation;
        }


        RpcMoveTurret(turretElevation, turretRotation);

    }

    void FixedUpdate()
    {
        if (NetworkClient.active && isLocalPlayer)
        {
            float vertical   = Input.GetAxisRaw("Vertical"); //Deplacement vertical [-1 : 1]
            float horizontal = Input.GetAxisRaw("Horizontal"); //Deplacement horizontal [-1 : 1]

            float xMouse = Input.GetAxisRaw("Mouse X"); //Deplacement souris sur l'axe X
            float yMouse = Input.GetAxisRaw("Mouse Y"); //Deplacement souris sur l'axe Y

            CmdMovePlayer(vertical, horizontal, xMouse, yMouse);
        }
    }
}
