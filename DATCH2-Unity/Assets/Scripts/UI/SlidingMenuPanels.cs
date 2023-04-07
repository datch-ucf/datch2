using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Allows functionality of the condensed UI (panel with multiple pages instead of all menus displaying at once)
/// LOCATION: menuPanels GameObject
/// </summary>

public class SlidingMenuPanels : MonoBehaviour
{
    [SerializeField] private int currentPage;
    public GameObject menuPageTitle;
    public GameObject fileMenu;
    public GameObject toolsMenu;
    public GameObject attributesMenu;
    private GameObject menuPanels;
    public GameObject buttonLeft;
    public GameObject buttonRight;
    Vector3 menuPanelsStartPos;
    private bool slidePanelLeft = false;
    private bool slidePanelRight = false;    
    private float speed;

    // Expanding panel backplate (mainly for miniPegGrid) variables
    public GameObject panelBackplate;
    private Vector3 panelOrigPos;
    private Vector3 panelOrigScale;
    private bool expandedPanel = false;
    public Vector3 expandedPanelPos;
    public Vector3 expandedPanelScale;
    //public GameObject miniPegGridTextExp;
    public GameObject miniPegGridComponentsExp;






    // Start is called before the first frame update
    void Start()
    {
        menuPanels = gameObject;
        currentPage = 1; 
        speed = 12; // Speed of UI sliding
        panelOrigPos = panelBackplate.transform.localPosition;
        panelOrigScale = panelBackplate.transform.localScale;

        SlideMenuLeftToAttributes(); // Added to make menu start on line attributes 

    }

    // Update is called once per frame
    void Update()
    {
        // Slide menu panel to the left
        if (slidePanelLeft == true && (currentPage <=3 && currentPage >=1))
        {
            menuPanels.transform.localPosition = Vector3.Lerp(menuPanels.transform.localPosition, new Vector3(menuPanelsStartPos.x-0.13f, menuPanelsStartPos.y, menuPanelsStartPos.z), speed * Time.deltaTime);
            
            if(menuPanels.transform.localPosition == new Vector3(menuPanelsStartPos.x - 0.13f, menuPanelsStartPos.y, menuPanelsStartPos.z))
            {
                slidePanelLeft = false;
            }
        }

        // Slide menu panel to the right
        if (slidePanelRight == true && (currentPage <= 3 && currentPage >= 1))
        {
            menuPanels.transform.localPosition = Vector3.Lerp(menuPanels.transform.localPosition, new Vector3(menuPanelsStartPos.x + 0.13f, menuPanelsStartPos.y, menuPanelsStartPos.z), speed * Time.deltaTime);
            
            if(menuPanels.transform.localPosition == new Vector3(menuPanelsStartPos.x + 0.13f, menuPanelsStartPos.y, menuPanelsStartPos.z))
            {
                slidePanelRight = false;
            }
        }

        // Expand panel
        if(expandedPanel == true)
        {
            
            panelBackplate.transform.localPosition = Vector3.Lerp(panelBackplate.transform.localPosition, expandedPanelPos, speed * Time.deltaTime);
            panelBackplate.transform.localScale = Vector3.Lerp(panelBackplate.transform.localScale, expandedPanelScale, speed * Time.deltaTime);
        }
        else
        {
            panelBackplate.transform.localPosition = Vector3.Lerp(panelBackplate.transform.localPosition,panelOrigPos, speed * Time.deltaTime);
            panelBackplate.transform.localScale = Vector3.Lerp(panelBackplate.transform.localScale, panelOrigScale, speed * Time.deltaTime);

            // Disable elements associated with expanded miniPegs grid panel
            miniPegGridComponentsExp.SetActive(false);
            //miniPegGridTextExp.SetActive(false);

        }
    }

    // FUNCTIONS
    public void SlideMenuPanelBackward()
    {
        menuPanelsStartPos = menuPanels.transform.localPosition; // Get start position on each button press
        slidePanelLeft = false;
        slidePanelRight = true;

        currentPage--;
        // If current page goes below 1, keep at page 1 and don't move menu
        if(currentPage <1)
        {
            currentPage = 1;
            slidePanelRight = false;
        }

        UpdateCurrentPanel();
    }

    public void SlideMenuPanelForward()
    {
        slidePanelRight = false;
        menuPanelsStartPos = menuPanels.transform.localPosition; // Get start position on each button press
        slidePanelLeft = true;

        currentPage++;

        // If current page goes above 3, keep at page 3 and don't move menu
        if (currentPage > 3)
        {
            currentPage = 3;
            slidePanelLeft = false;
        }

        UpdateCurrentPanel();

    }

    public void SlideMenuLeftToAttributes()
    {
        SlideMenuPanelForward();
        //UpdateCurrentPanel();
        StartCoroutine(ProceedToNextPanel(currentPage));
    }
    // Displays current menu panel only and changes title to match current menu
    public void UpdateCurrentPanel()
    {
        if (currentPage == 1)
        {
            StartCoroutine(DelaySetInactive(true, false, false));
            menuPageTitle.transform.GetComponent<TMP_Text>().text = "FILE";
            // First page
            buttonLeft.SetActive(false); 
            buttonRight.SetActive(true);
            expandedPanel = false;

        }
        if (currentPage == 2)
        {
            StartCoroutine(DelaySetInactive(false, true, false));
            menuPageTitle.transform.GetComponent<TMP_Text>().text = "TOOLS";

            // Middle page
            buttonRight.SetActive(true);
            buttonLeft.SetActive(true);
            expandedPanel = false;

        }
        if (currentPage == 3)
        {
            StartCoroutine(DelaySetInactive(false, false, true));
            menuPageTitle.transform.GetComponent<TMP_Text>().text = "ATTRIBUTES";

            // Last page
            buttonRight.SetActive(false); 
            buttonLeft.SetActive(true);
        }
    }

    IEnumerator DelaySetInactive(bool fileActive, bool toolsActive, bool attributesActive)
    {
        yield return new WaitForSeconds(0.01f);
        fileMenu.SetActive(fileActive);
        toolsMenu.SetActive(toolsActive);
        attributesMenu.SetActive(attributesActive);
    }

    IEnumerator ProceedToNextPanel(int currentPage)
    {
        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => currentPage == 2);
        SlideMenuPanelForward();
        //UpdateCurrentPanel();

    }

    // Expand UI for miniPegGrid

    public void ExpandUI()
    {
        expandedPanel = true;
        expandedPanelPos = new Vector3(panelOrigPos.x + 0.088f, panelOrigPos.y, panelOrigPos.z);
        expandedPanelScale = new Vector3(panelOrigScale.x * 2.2f, panelOrigScale.y, panelOrigScale.z);
        miniPegGridComponentsExp.SetActive(true);
        //miniPegGridTextExp.SetActive(true);
    }
}
