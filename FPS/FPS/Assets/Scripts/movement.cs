using UnityEngine;
using Mirror;


public class movement : NetworkBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    const float degreesToRadians = Mathf.PI / 180;

    public GameObject camera;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float cosPlayer = Mathf.Cos(transform.eulerAngles.y * degreesToRadians);
        float sinPlayer = Mathf.Sin(transform.eulerAngles.y * degreesToRadians);

        Vector3 move = new Vector3( Input.GetAxis("Horizontal") * cosPlayer + Input.GetAxis("Vertical") * sinPlayer,
                                    0,
                                    -Input.GetAxis("Horizontal") * sinPlayer + Input.GetAxis("Vertical") * cosPlayer);

        CmdRequestMove(move);

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X"));
    }

    [ClientRpc]
    void RpcMove(Vector3 move)
    {
        controller.Move(move);
    }

    [Command]
    void CmdRequestMove(Vector3 move)
    {
        move.Normalize();
        RpcMove(move * Time.deltaTime * playerSpeed);
    }
}