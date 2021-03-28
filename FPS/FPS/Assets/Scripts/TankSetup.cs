using UnityEngine;
using Mirror;
using TMPro;

public class TankSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    private Camera sceneCamera;

    public string playerTeam;
    public string playerName;
    
    void Start()
    {
        GameObject go;

        if(playerTeam == "red")
        {
            gameObject.transform.Find("Cromwell_IV").gameObject.SetActive(false);

            go = gameObject.transform.Find("Panzer_VI_E").gameObject;
            gameObject.GetComponent<TankMotor>().go = go;
            gameObject.GetComponent<TankController>().go = go;
            gameObject.GetComponent<PlayerLogic>().go = go;
            gameObject.GetComponent<ShootBullet>().Player = go;
        }
        else
        {
            gameObject.transform.Find("Panzer_VI_E").gameObject.SetActive(false);

            go = gameObject.transform.Find("Cromwell_IV").gameObject;
            gameObject.GetComponent<TankMotor>().go = go;
            gameObject.GetComponent<TankController>().go = go;
            gameObject.GetComponent<PlayerLogic>().go = go;
            gameObject.GetComponent<ShootBullet>().Player = go;
        }

        if (isLocalPlayer)
        {
            sceneCamera = Camera.main;

            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            go.transform.Find("Turret/Camera").gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        GetComponentInChildren<TextMeshPro>().text = playerName;
    }

    void OnDisable()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

    void Update()
    { 
        
    }
}
