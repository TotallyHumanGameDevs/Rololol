using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    DiceSoundEffects sounds;

    public float speed;
    bool isMoving = false;
    int faceUpSide;
    [SerializeField] Transform UIDice;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask groundLayer;
    bool onRotator = false;
    bool falling = false;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        faceUpSide = FaceInDirection();
        audioSource = GetComponent<AudioSource>();
        sounds = GetComponent<DiceSoundEffects>();

        RaycastHit hit;
        if(Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 10, interactableLayer)){
            if(hit.transform.name == "Waypoint 01"){
                onRotator = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManagement.instance.RestartLevel();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManagement.instance.LoadLevel(0);
        }

        if(isMoving || falling) return;

        if (Input.GetKey(KeyCode.UpArrow)) {
            StartCoroutine(Roll(Vector3.forward, faceUpSide));
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            StartCoroutine(Roll(Vector3.back, faceUpSide));
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            StartCoroutine(Roll(Vector3.left, faceUpSide));
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            StartCoroutine(Roll(Vector3.right, faceUpSide));
        }
        else if (Input.GetKey(KeyCode.Space) && onRotator){
            StartCoroutine(Spin());
        }
    }

    IEnumerator Roll(Vector3 direction, int amount){
        RaycastHit hit;
        isMoving = true;

        for(int i = 0; i < amount; i++){
            float remainingAngle = 90;
            Vector3 rotationCentre = transform.position + direction / 2f + Vector3.down / 2f;
            Vector3 UIDiceCentre = UIDice.position + direction / 2f + Vector3.down / 2f;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
            bool playedClip = false;

            while(remainingAngle > 0f){
                if(remainingAngle < 25f && !playedClip){
                    if(Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 1f, interactableLayer)){
                        playedClip = true;
                        sounds.PlayMetal();

                        if(hit.transform.name == "Trap 01" && i == amount - 1){
                            hit.transform.GetComponent<AudioSource>().Play();
                        }
                    }
                    else if(Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 1f, groundLayer)){
                        playedClip = true;
                        if(hit.transform.name == "Grass 01"){
                            sounds.PlayGrass();
                        }
                        else if(hit.transform.name == "Bridge 01"){
                            sounds.PlayWood();
                        }
                    }
                }

                float rotationalAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
                transform.RotateAround(rotationCentre, rotationAxis, rotationalAngle);
                remainingAngle -= rotationalAngle;

                //Vector3 UIDiceDirection = direction - UIDice.parent.up;
                //Vector3 UIDiceRotationAxis = Vector3.Cross(UIDice.forward, direction);
                //UIDice.RotateAround(UIDice.position, UIDiceRotationAxis, rotationalAngle);
                UIDice.localRotation = transform.rotation;

                yield return null;
            }
            if(!Physics.Raycast(new Ray(transform.position, Vector3.down), 1f, groundLayer)){
                falling = true;
                StartCoroutine(Respawn());
                GetComponent<Rigidbody>().useGravity = true;
                break;
            }
        }

        isMoving = false;

        faceUpSide = FaceInDirection();

        onRotator = false;
        if(Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 10, interactableLayer)){
            if(hit.transform.name == "Waypoint 01"){
                onRotator = true;
            }
            else if(hit.transform.parent.name == "End"){
                StartCoroutine(Won());
                falling = true;
                GetComponent<Rigidbody>().velocity = new Vector3(0f, 0.4f, 0f);
                GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));
            }
            else if(hit.transform.name == "Trap 01"){
                StartCoroutine(hit.transform.GetComponent<TrapTrigger>().Triggered());
                Launch();
            }
        }
    }

    IEnumerator Won(){
        yield return new WaitForSeconds(2f);
        if(SceneManagement.instance.GetLevelNum() <= 6)
            PlayerPrefs.SetInt("levelReached", Mathf.Max(PlayerPrefs.GetInt("levelReached"), SceneManagement.instance.GetLevelNum() + 1));
        else{
            List<string> bonusLevelsCompleted = new List<string>();
            foreach(char ch in PlayerPrefs.GetString("bonusLevelsCompleted")){
                bonusLevelsCompleted.Add(ch.ToString());
            }
            if(!bonusLevelsCompleted.Contains((SceneManagement.instance.GetLevelNum() - 6).ToString()))
                PlayerPrefs.SetString("bonusLevelsCompleted", PlayerPrefs.GetString("bonusLevelsCompleted") + (SceneManagement.instance.GetLevelNum() - 6).ToString());
        }
        SceneManagement.instance.LoadLevel(0);
    }

    void Launch(){
        StartCoroutine(Respawn());
        falling = true;
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        GetComponent<Rigidbody>().useGravity = true;
    }

    IEnumerator Respawn(){
        yield return new WaitForSeconds(1.5f);
        SceneManagement.instance.RestartLevel();
    }

    IEnumerator Spin(){
        isMoving = true;

        float remainingAngle = 90;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, Vector3.up);

        while(remainingAngle > 0){
            float rotationalAngle = Mathf.Min(Time.deltaTime * speed * 1.2f, remainingAngle);
            transform.RotateAround(transform.position, Vector3.up, rotationalAngle);
            remainingAngle -= rotationalAngle;
            UIDice.localRotation = transform.rotation;
            yield return null;
        }

        isMoving = false;
    }

    public int FaceInDirection() {
        Vector3 local = transform.InverseTransformDirection(new Vector3(0f, 1f, 0f));
        int code = Mathf.RoundToInt(local.x + 2 * local.y + 3 * local.z);

        switch(code){
            case 3:
                return 1;
            case 2:
                return 2;
            case -1:
                return 3;
            case 1:
                return 4;
            case -2:
                return 5;
            case -3:
                return 6;
        }

        return 1;
    }
}
