using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HospitalHandler : MonoBehaviour
{
    [SerializeField] GameObject blueHospitalButton;
    [SerializeField] GameObject redHospitalButton;
    [SerializeField] GameObject blueOverlayImage;
    [SerializeField] GameObject redOverlayImage;
    private PanelController panelController;

    private bool redActive;
    private bool hospitalPannelActive;

    void Start()
    {
        redActive = false;
        hospitalPannelActive = false;

        blueHospitalButton.SetActive(true);
        redHospitalButton.SetActive(false);


        panelController = transform.GetComponent<PanelController>();
        blueOverlayImage.SetActive(true);
        redOverlayImage.SetActive(false);
    }

    public void UpdateGUI(BodyData body)
    {
        bool isCritical = body.heart == 0 || body.brain == 0 || body.intestine == 0 || body.pulmon == 0 || body.muscles == 0;
        if (isCritical)
        {
            ToggleRed();
        } else
        {
            ToggleBlue();
        }
    }

    public void ToggleRed(bool force = false)
    {
        if (!redActive || force)
        {
            blueHospitalButton.SetActive(false);
            redHospitalButton.SetActive(true);
            blueOverlayImage.SetActive(false);
            redOverlayImage.SetActive(true);
            redActive = true;
        }
    }

    public void ToggleBlue(bool force = false)
    {
        if (redActive || force)
        {
            blueHospitalButton.SetActive(true);
            redHospitalButton.SetActive(false);
            blueOverlayImage.SetActive(true);
            redOverlayImage.SetActive(false);
            redActive = false;
        }
    }

    public void ToggleMainPannel()
    {
        if (!hospitalPannelActive)
        {

            panelController.Toggle("HospitalPanel");
            hospitalPannelActive = true;
            blueHospitalButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Leave hospital");
            redHospitalButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Leave hospital");
        }
        else
        {
            panelController.Toggle("PillsPanel");
            hospitalPannelActive = false;
            blueHospitalButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Go to hospital");
            redHospitalButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Go to hospital");
        }
    }


}
