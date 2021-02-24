using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class TankSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    private Camera sceneCamera;

    [SyncVar] public string playerTeam;
    [SyncVar] public string playerName;

    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;


    void Start()
    {

        if (!isLocalPlayer)
        {
            for(int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            sceneCamera = Camera.main;

            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            playerUIInstance = Instantiate(playerUIPrefab);
            GetComponent<PlayerLogic>().healthBar = playerUIInstance.GetComponentInChildren<HealthBar>();
            GetComponent<PlayerLogic>().DeadText = playerUIInstance.transform.Find("DeadText").gameObject;
            GetComponent<PlayerLogic>().HealthText = playerUIInstance.transform.Find("HealthBar/HealthText").GetComponent<Text>();
            
            GetComponent<PlayerLogic>().OnStart();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        GetComponentInChildren<TextMeshPro>().text = playerName;

        gameObject.tag = playerTeam;
    }

    void OnDisable()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
        Destroy(playerUIInstance);
    }

    void Update()
    {
        
    }
}
