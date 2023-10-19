using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveDisplay : MonoBehaviour
{
    TextMeshProUGUI ObjectiveText;
    [SerializeField] SceneSwitcher[] switchers;
    public int switcherIndex = 1;
    void Start()
    {
        ObjectiveText = GetComponent<TextMeshProUGUI>();
    }

    public void DisplayObjective()
    {
        if (switchers[0].HasSwitched() == true)
        {
            ObjectiveText.text = "Look around";
        }
        else if (switchers[1].HasSwitched() == true)
        {
            ObjectiveText.text = "Clear the debris";
        }
        else if (switchers[2].HasSwitched() == true)
        {
            ObjectiveText.text = "Turn on the power";
        }
        else if (switchers[3].HasSwitched() == true)
        {
            ObjectiveText.text = "Look around";
        }
        else if (switchers[4].HasSwitched() == true)
        {
            ObjectiveText.text = "Find the exit";
        }
    }

    void Update()
    {
        DisplayObjective();
    }
}
