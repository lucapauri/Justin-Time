using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ValigettaEmpty : MonoBehaviour
{
    public NPCConversation conversation;
    public DialogCamera dialogCamera;
    private GlobalVariables globalVariables;
    private bool cameraOn;
    private Vector3 startPos;
    private GameObject justin;
    private Scritte scritte;
    private bool isActive;
    private bool isTaken;
    public GameObject valigetta;
    private GameObject camera1;
    private GameObject camera2;

    // Start is called before the first frame update
    void Start()
    {
        justin = GameObject.FindGameObjectWithTag("Player");
        scritte = FindObjectOfType<Scritte>();
        isActive = false;
        isTaken = false;
        camera1 = GameObject.FindGameObjectWithTag("MainCamera");
        camera2 = GameObject.FindGameObjectWithTag("SecondCamera");
        startPos = dialogCamera.transform.position;
        cameraOn = false;
        globalVariables = FindObjectOfType<GlobalVariables>();
    }

    // Update is called once per frame
    void Update()
    {
        if (justin != null && Vector3.Distance(gameObject.transform.position, justin.transform.position) < 15 && !isTaken)
        {
            scritte.setActive("Premi X per prendere la valigetta", valigetta);
            isActive = true;
        }
        if (scritte.active() && (Vector3.Distance(gameObject.transform.position, justin.transform.position) > 15 || isTaken))
        {
            scritte.setNotActive();
            isActive = false;
        }
        //parte dialogo e prendo valigetta
        if (isActive && Input.GetKeyDown(KeyCode.X) && !cameraOn)
        {
            isTaken = true;
            dialogSequence();
            cameraOn = true;
        }

        if (cameraOn && Input.GetKeyDown(KeyCode.Return))
        {
            ConversationManager.Instance.PressSelectedOption();
            endDialog();
            cameraOn = false;
        }
    }

    public bool taken()
    {
        return isTaken;
    }

    private void endDialog()
    {
        globalVariables.justin.GetComponent<Justin>().enabled = true;
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = true;
        }

        FindObjectOfType<UpCamera>().gameObject.GetComponent<Camera>().enabled = true;
        FindObjectOfType<DownCamera>().gameObject.GetComponent<Camera>().enabled = true;
        dialogCamera.GetComponent<Camera>().enabled = false;

        dialogCamera.transform.position = startPos;

        //sdoppio camera
        camera1.GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 1);
        camera2.GetComponent<Camera>().rect = new Rect(0, -0.5f, 1, 1);

    }

    private void dialogSequence()
    {
        globalVariables.justin.GetComponent<Justin>().enabled = false;
        globalVariables.justin.GetComponent<Animator>().SetBool("Walk", false);
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = false;
        }

        dialogCamera.GetComponent<Camera>().enabled = true;
        FindObjectOfType<UpCamera>().gameObject.GetComponent<Camera>().enabled = false;
        FindObjectOfType<DownCamera>().gameObject.GetComponent<Camera>().enabled = false;

        dialogCamera.moveCamera();

        StartCoroutine(conversationCoroutine(dialogCamera.moveTime));
    }

    private IEnumerator conversationCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        ConversationManager.Instance.StartConversation(conversation);


    }
}
