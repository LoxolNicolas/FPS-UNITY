using System.Collections;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private TankMotor tankMotor; //Pour appel des fonctions de la classe PlayerMotor sans passer par static

    private const float TANK_FASTER = 4.5f;
    private const float TANK_SLOWER = 3.0f;

    private const float CATERPILLAR_STOP = 0.0f;
    private const float CATERPILLAR_SLOW = 0.05f;
    private const float CATERPILLAR_FAST = 0.2f;

    [SerializeField]
    private float playerSpeed = TANK_SLOWER;

    [SerializeField]
    private float playerRotation = 5.0f;

    [SerializeField]
    private float sensitivityX = 3.0f;

    [SerializeField]
    private float sensitivityY = 3.0f;

    [SerializeField]
    private float caterpillarSpeedLeft = CATERPILLAR_STOP;

    [SerializeField]
    private float caterpillarSpeedRight = CATERPILLAR_STOP;

    private float offsetL = 0.0f;
    private float offsetR = 0.0f;

    private Renderer caterpillarLeft;
    private Renderer caterpillarRight;

    /*
    public AudioClip[] playlist;
    public AudioSource audioTank;
    public AudioSource audioSound;
    public AudioSource audioSoldier;
    */

    private GameObject sparkLeft; 
    private GameObject sparkRight;

    private GameObject smoke;

    //enum SoundTypeTank { START = 0, RUNNING = 6, TOURELLE = 7 , KLAXON = 8};

    void PlayerOnKeyboard()
    {
        bool isMoving = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow));
        bool isStatic = !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);

        float speedCaterpillarLeft = CATERPILLAR_STOP;
        float speedCaterpillarRight = CATERPILLAR_STOP;

        //sparkLeft.SetActive(true);
        //sparkRight.SetActive(true);

        //smoke.SetActive(true);

        if(Input.GetKey(KeyCode.LeftArrow) && isStatic || Input.GetKey(KeyCode.LeftArrow) && isMoving)
        {
            if(Input.GetKey(KeyCode.LeftArrow) && isStatic && !Input.GetKey(KeyCode.LeftShift))
            {
                speedCaterpillarLeft = CATERPILLAR_SLOW;
                speedCaterpillarRight = -CATERPILLAR_SLOW;
            }
            else if(Input.GetKey(KeyCode.LeftArrow) && isMoving && !Input.GetKey(KeyCode.LeftShift))
            {
                speedCaterpillarLeft = CATERPILLAR_SLOW / 2.0f;
                speedCaterpillarRight = CATERPILLAR_STOP;
            }
            else if(Input.GetKey(KeyCode.LeftArrow) && isMoving && Input.GetKey(KeyCode.LeftShift))
            {
                speedCaterpillarLeft = CATERPILLAR_FAST / 2.0f;
                speedCaterpillarRight = CATERPILLAR_STOP;
            }

            GetComponent<Transform>().Rotate(new Vector3(0.0f, (playerRotation + (speedCaterpillarLeft * 10.0f)) * Time.fixedDeltaTime, 0.0f));
        }
        else if(Input.GetKey(KeyCode.RightArrow) && isStatic || Input.GetKey(KeyCode.RightArrow) && isMoving)
        {
            if(Input.GetKey(KeyCode.RightArrow) && isStatic && !Input.GetKey(KeyCode.LeftShift))
            {
                speedCaterpillarLeft = -CATERPILLAR_SLOW;
                speedCaterpillarRight = CATERPILLAR_SLOW;
            }
            else if(Input.GetKey(KeyCode.RightArrow) && isMoving && !Input.GetKey(KeyCode.LeftShift))
            {
                speedCaterpillarLeft = CATERPILLAR_STOP;
                speedCaterpillarRight = CATERPILLAR_SLOW / 2.0f;
            }
            else if(Input.GetKey(KeyCode.RightArrow) && isMoving && Input.GetKey(KeyCode.LeftShift))
            {
                speedCaterpillarLeft = CATERPILLAR_STOP;
                speedCaterpillarRight = CATERPILLAR_FAST / 2.0f;
            }

            GetComponent<Transform>().Rotate(new Vector3(0.0f, (-playerRotation - (speedCaterpillarRight * 10.0f)) * Time.fixedDeltaTime, 0.0f));
        }
        else
        {
            speedCaterpillarLeft = CATERPILLAR_STOP;
            speedCaterpillarRight = CATERPILLAR_STOP;

            //sparkLeft.SetActive(false);
            //sparkRight.SetActive(false);

            //smoke.SetActive(false);
        }

        if(isMoving)
        {
            caterpillarSpeedLeft = CATERPILLAR_SLOW + speedCaterpillarLeft;
            caterpillarSpeedRight = CATERPILLAR_SLOW + speedCaterpillarRight;

            //sparkLeft.SetActive(true);
            //sparkRight.SetActive(true);

            //smoke.SetActive(true);
        }
        else if(isStatic)
        {
            caterpillarSpeedLeft = CATERPILLAR_STOP + speedCaterpillarLeft;
            caterpillarSpeedRight = CATERPILLAR_STOP + speedCaterpillarRight;
        }

        if(Input.GetKey(KeyCode.LeftShift) && isMoving)
        {
            playerSpeed = TANK_FASTER;

            caterpillarSpeedLeft = CATERPILLAR_FAST + speedCaterpillarLeft;
            caterpillarSpeedRight = CATERPILLAR_FAST + speedCaterpillarRight;

            //audioSound.pitch = 1.5f;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) && isMoving)
        {
            playerSpeed = TANK_SLOWER;

            caterpillarSpeedLeft = CATERPILLAR_SLOW + speedCaterpillarLeft;
            caterpillarSpeedRight = CATERPILLAR_SLOW + speedCaterpillarRight;

            //audioSound.pitch = 1.0f;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            /*
            audioTank.clip = playlist[(int)SoundTypeTank.KLAXON];
            audioTank.loop = false;
            audioTank.Play();
            */
        }
    }

    void MoveCaterpillar()
    {
        offsetL = (offsetL + Time.deltaTime * caterpillarSpeedLeft) % 1.0f;
        offsetR = (offsetR + Time.deltaTime * caterpillarSpeedRight) % 1.0f;

        caterpillarLeft.material.SetTextureOffset("_MainTex", new Vector2(offsetL, 0.0f));
        caterpillarRight.material.SetTextureOffset("_MainTex", new Vector2(offsetR, 0.0f));
    }

    void Start()
    {
        tankMotor = GetComponent<TankMotor>();

        caterpillarLeft = transform.GetChild(0).GetComponent<Renderer>();
        caterpillarRight = transform.GetChild(1).GetComponent<Renderer>();

        //sparkLeft = GameObject.Find(caterpillarLeft.name + "/SparkL");
        //sparkRight = GameObject.Find(caterpillarRight.name + "/SparkR");
       // smoke = GameObject.Find(caterpillarLeft.name + "/smoke_thin");

        /*
        audioTank = gameObject.AddComponent<AudioSource>();
        audioSoldier = gameObject.AddComponent<AudioSource>();
        audioSound = gameObject.AddComponent<AudioSource>();
        */

        //StartCoroutine(StartingRoundCoroutine());

        //StartCoroutine(SoldierVoiceCoroutine());
    }

    void Update()
    {
        float zMovement = Input.GetAxisRaw("Vertical"); //Deplacement vertical [-1 : 1]

        float yRotation = Input.GetAxisRaw("Mouse X"); //Deplacement souris sur l'axe X
        float xRotation = Input.GetAxisRaw("Mouse Y"); //Deplacement souris sur l'axe Y

        PlayerOnKeyboard();
        MoveCaterpillar();

        Vector3 playerMoveZ = transform.forward * zMovement;

        Vector3 velocity = playerMoveZ.normalized * playerSpeed;

        Vector3 rotation = new Vector3(0, yRotation, 0) * sensitivityX;
        Vector3 cameraRotation = new Vector3(xRotation, 0, 0) * sensitivityY;

        tankMotor.MovePlayer(velocity);
        tankMotor.RotatePlayer(rotation);
        tankMotor.RotateCamera(cameraRotation);
    }

    /*
    IEnumerator StartingRoundCoroutine()
    {
        yield return new WaitForSeconds(2);

        audioTank.clip = playlist[(int)SoundTypeTank.START];
        audioTank.Play();

        yield return new WaitForSeconds(2);

        audioSoldier.clip = playlist[Random.Range(1, 5)];
        audioSoldier.Play();

        audioSound.clip = playlist[(int)SoundTypeTank.RUNNING];
        audioSound.loop = true;
        audioSound.volume = 0.5f;
        audioSound.Play();

        yield return new WaitForSeconds(1);

        audioSoldier.Stop();
    }

    IEnumerator SoldierVoiceCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(30);

            if(!audioSoldier.isPlaying)
            {
                audioSoldier.clip = playlist[Random.Range(1, 5)];
                audioSoldier.Play();
            }
        }
    }
    */
}
