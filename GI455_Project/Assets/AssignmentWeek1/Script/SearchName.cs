using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SearchName : MonoBehaviour
{
    public Text textColor;  
    public Text inputField;
    public Text textSearch;

    


    public void Searching()
    {

        if ((inputField.text == "Clock") || (inputField.text == "Computer") || (inputField.text == "Window") || (inputField.text == "Television") || (inputField.text == "Picture"))
        {

            
            textSearch.GetComponent<Text>().text = inputField.text   + " is Found";
            textColor.GetComponent<Text>().color = Color.green;

        }
        

        else
        {
            
            textSearch.GetComponent<Text>().text =  inputField.text + " is not Found";
            textColor.GetComponent<Text>().color = Color.red;

        }
        
        

    }

}
