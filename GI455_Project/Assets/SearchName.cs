using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SearchName : MonoBehaviour
{
    public Text texts;
    public string sName ;
    public Text inputField;
    public GameObject textSearch;

    


    public void Searching()
    {
       
        if (sName == "Clock" )
        {

            sName = inputField.GetComponent<Text>().text;
            textSearch.GetComponent<Text>().text = sName + " is Found";
            texts.GetComponent<Text>().color = Color.green;
            
        }
        

        else if (sName == "Computer")
        {
            sName = inputField.GetComponent<Text>().text;
            textSearch.GetComponent<Text>().text = sName + " is Found";
            texts.GetComponent<Text>().color = Color.green;

        }       

        else if (sName == "Window")
        {
            sName = inputField.GetComponent<Text>().text;
            textSearch.GetComponent<Text>().text = sName + " is Found";
            texts.GetComponent<Text>().color = Color.green;
        }

        else if (sName == "Television")
        {
            sName = inputField.GetComponent<Text>().text;
            textSearch.GetComponent<Text>().text = sName + " is Found";
            texts.GetComponent<Text>().color = Color.green;
        }

        else if (sName == "Picture")
        {
            sName = inputField.GetComponent<Text>().text;
            textSearch.GetComponent<Text>().text = sName + " is Found";
            texts.GetComponent<Text>().color = Color.green;
        }
        else
        {
            sName = inputField.GetComponent<Text>().text;
            textSearch.GetComponent<Text>().text = sName + " is not Found";
            texts.GetComponent<Text>().color = Color.red;
            
        }
        
        

    }

}
