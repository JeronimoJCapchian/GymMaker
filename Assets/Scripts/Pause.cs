using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject MenuPause;
    public bool pausa = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pausa == false)
            {
                MenuPause.SetActive(true);
                pausa = true;
                //Time.timeScale = 0;
                //Cursor.visible = true;
                //Cursor.lockState = CursorLockMode.None;
            }
            else if (pausa == true)
            {
                resume();
            }
        } 
      
    }
    public void resume()
    {
        MenuPause.SetActive(false);
        pausa = false;
        //Time.timeScale = 1;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
