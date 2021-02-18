using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace ProgramChat
{
    public class AppChat : MonoBehaviour
    {
        public Text inputfieldIP;
        public Text inputfieldPort;
        public Text inputYourName;
        public Text nameDisplay;
        public GameObject panelConnect;
        public GameObject panelChat;
       
        public Text textP1;
        public Text textP2;
        public Text inputfieldChat;
        private WebSocket websocket;

        //public List<string> chatList = new List<string>();
        //public int tP1, tP2;
        //Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
            if ((inputfieldIP.text == "127.0.0.1") && (inputfieldPort.text == "5500"))
            {
                inputfieldIP.GetComponent<Text>().color = Color.green;
                inputfieldPort.GetComponent<Text>().color = Color.green;

            }

            else
            {
                inputfieldIP.GetComponent<Text>().color = Color.red;
                inputfieldPort.GetComponent<Text>().color = Color.red;
            }

            //ChatText();

        }

        private void OnDestroy()
        {
            if (websocket != null)
            {
                websocket.Close();
            }
        }

        private void OnMessage (object sender, MessageEventArgs messageEventArgs)
        {
            Debug.Log("Receive msg : " + messageEventArgs.Data);

            //if (messageEventArgs.Data != textP1.text)
            //{
            //    chatList.Add(messageEventArgs.Data);
            //    tP1 = tP2;
            //    tP2 += 1;


            //}
            //else
            //{
            //    chatList.Add(messageEventArgs.Data);
            //    tP2 = tP1;
            //    tP1 += 1;


            //}

        }


        public void GetConnect()
        {
            if ((inputfieldIP.text == "127.0.0.1") && (inputfieldPort.text == "5500"))
            {
                websocket = new WebSocket("ws://127.0.0.1:5500/");
                websocket.Connect();
                panelChat.SetActive(true);
                panelConnect.SetActive(false);
                nameDisplay.text = "Your Name : " + "<color=#00FCFF>" + inputYourName.text + "</color>";
            }

        }



        public void PlayChat()
        {

            if (inputfieldChat.text == "" || websocket.ReadyState == WebSocketState.Open)
            
                websocket.OnMessage += OnMessage;
                websocket.Send(inputfieldChat.text);
            
            

        }

        public void LeaveChat()
        {
            
            if (websocket != null)
            {
                websocket.Close();
            }
            panelChat.SetActive(false);
            panelConnect.SetActive(true);          
        }

        //public void ChatText()
        //{
        //    if (tP1 > tP2)
        //    {
        //        if (chatList.Count > 0)
        //        {
        //            textP1.text = chatList[chatList.Count - 1];
        //        }
        //    }
        //    else if (tP1 < tP2)
        //    {
        //        if (chatList.Count > 0)
        //        {
        //            textP2.text = chatList[chatList.Count - 1];
        //        }
        //    }
        //}
    }
}
