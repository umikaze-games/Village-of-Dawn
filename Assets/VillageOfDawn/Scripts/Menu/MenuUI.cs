using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject[] panels;

    public void SwitchPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i==index)
            {
                panels[index].transform.SetAsLastSibling();
            }
        
        }
        EventHandler.CallPlaySEEvent("Confirm", AudioType.PlayerSE);
    
    }
    public void ExitGame()
    { 
        Application.Quit();
        Debug.Log("ExitGame");
    }

}
