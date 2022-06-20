using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public static bool isMenu = false;
    public GameObject menu;
    public GameObject buttonPrefab;
    public InteractionsFromInventory interactions;
    private List<GameObject> buttons = new List<GameObject>();
    private GlobalVariables globalVariables;
    private int activeButton;
    public bool openTimeCapsule;

    private void Start()
    {
        interactions = FindObjectOfType<InteractionsFromInventory>();
        openTimeCapsule = false;
        menu = GameObject.FindGameObjectWithTag("InventoryMenu");
        buttons = new List<GameObject>();
        globalVariables = FindObjectOfType<GlobalVariables>();
        activeButton = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenu == true)
            SetPause();
        else
            Resume();

        if (Input.GetKeyDown(KeyCode.LeftArrow) && activeButton > 0)
        {
            Debug.Log(buttons.Count);
            buttons[activeButton].GetComponent<Animator>().SetBool("Selected", false);
            activeButton--;
            buttons[activeButton].GetComponent<Animator>().SetBool("Selected", true);

        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && activeButton < buttons.Count - 1)
        {
            buttons[activeButton].GetComponent<Animator>().SetBool("Selected", false);
            activeButton++;
            buttons[activeButton].GetComponent<Animator>().SetBool("Selected", true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //controllo se il menù è stato aperto per interagire con la capsula del tempo o per posizionare un oggetto
            if (openTimeCapsule)
            {
                string oldKey = buttons[activeButton].GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
                string newKey = globalVariables.agingSets[oldKey];
                globalVariables.inventoryAging(oldKey);
                setMenuFalse();
                buttons[activeButton].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = newKey;
            }
            else
            {
                Vector3 instPos = globalVariables.justin.transform.position + globalVariables.justin.transform.forward * 3f;
                Quaternion instRot = globalVariables.justin.transform.rotation;
                string buttonText = buttons[activeButton].GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
                string name = "Meshes/"+buttonText;
                GameObject goInstance = Instantiate(Resources.Load(name) as GameObject, instPos, instRot);
                goInstance.name = buttonText;
                Destroy(buttons[activeButton]);
                buttons.Remove(buttons[activeButton]);
                interactions.checkInteractions(goInstance, instPos );
                globalVariables.inventory.Remove(buttonText);
                setMenuFalse();

            }
            openTimeCapsule = false;
        }

        if (isActive() && Input.GetKeyDown(KeyCode.Escape))
            setMenuFalse();


        /*if (Input.GetKeyDown("return"))
        {
            buttons[buttonCounter].buttonAnimator.SetBool("Clicked", true);
        }*/

    }

    public void setMenuTrue()
    {
        isMenu = true;
        globalVariables.justin.enabled = false;
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = false;
        }
    }

    public void setMenuFalse()
    {
        isMenu = false;
        globalVariables.justin.enabled = true;
        foreach (simpleEnemy enemy in globalVariables.enemies)
        {
            enemy.GetComponent<simpleEnemy>().enabled = true;
        }
    }

    private void SetPause()
    {
        menu.SetActive(true);
        buttons[activeButton].GetComponent<Animator>().SetBool("Selected", true);
    }

    public void Resume()
    {
        menu.SetActive(false);
    }

    public bool isActive()
    {
        return isMenu;
    }

    public void setBehaviour()
    {
        openTimeCapsule = true;
}

    public void addButton(string name)
    {
        GameObject go = Instantiate(buttonPrefab);
        buttons.Add(go);
        go.transform.SetParent(menu.transform, false);
        Vector3 buttonPos = new Vector3(menu.transform.position.x - 960, menu.transform.position.y, menu.transform.position.z);
        buttonPos.x = buttonPos.x + buttons.Count * 1920 / 4;
        go.GetComponent<RectTransform>().position = buttonPos;   /**globalVariables.inventory.Keys.Count*/
        go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;

    }

   
}