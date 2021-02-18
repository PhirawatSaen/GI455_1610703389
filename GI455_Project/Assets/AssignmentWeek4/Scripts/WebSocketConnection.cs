using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace ChatWebSocketWithJson
{
    public class WebSocketConnection : MonoBehaviour
    {
        public class MessageData 
        {
            public string username;
            public string message;
        }

        struct SocketEvent 
        {
            public string eventName;
            public string data;

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }

        public InputField inputCreateRoomName;
        public InputField inputJoinRoomName;
        public InputField inputUsername;
       
        public GameObject rootConnection;
        public GameObject rootMessenger;
        public GameObject rootCreateAndJoin;
        public GameObject rootCreateRoom;
        public GameObject rootJoinRoom;
        public GameObject rootAlreadyHas;
        public GameObject rootRoomisnotFound;

        public InputField inputText;
        public Text sendText;
        public Text receiveText;
        public Text textRoomName;
        private WebSocket ws;

        private string tempMessageString;
      

        public void Start()
        {
            rootConnection.SetActive(true);
        }

        public void Connect()
        {
            string url = $"ws://127.0.0.1:5500/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            
            rootConnection.SetActive(false);
            rootCreateAndJoin.SetActive(true);

            //CreateRoom("TestRoom01");
        }

        public void ConnectCreateRoom()
        {
            rootCreateAndJoin.SetActive(false);
            rootCreateRoom.SetActive(true);
        }

        public void ConnectJoinRoom()
        {
            rootCreateAndJoin.SetActive(false);
            rootJoinRoom.SetActive(true);
        }

        public void LeaveRoomChat()
        {
            rootMessenger.SetActive(false);
            rootCreateAndJoin.SetActive(true);

            LeaveRoom();

            inputJoinRoomName.text = "";

            textRoomName.text = "";
        }

        public void Create()
        {          
            CreateRoom("");

            inputCreateRoomName.text = "";
            
        }

        public void Join()
        {
            JoinRoom("");

            inputJoinRoomName.text = "";
        }

        public void BackToCreateAndJoin()
        {
            rootAlreadyHas.SetActive(false);
            rootRoomisnotFound.SetActive(false);
        } 
        public void BlackTo()
        {
            rootCreateAndJoin.SetActive(true);
            rootCreateRoom.SetActive(false);
            rootJoinRoom.SetActive(false);
        }

        public void CreateRoom(string roomName)
        {
           
            if (ws.ReadyState == WebSocketState.Open)
            {
                SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);
                socketEvent.data = inputCreateRoomName.text;
                roomName = socketEvent.data;
                textRoomName.text = ("Room : " + "[ " + inputCreateRoomName.text + " ]");

                string jsonStr = JsonUtility.ToJson(socketEvent);
                ws.Send(jsonStr);

                
            }

        }

        public void JoinRoom(string roomName)
        {
            if (ws.ReadyState == WebSocketState.Open)
            {
                SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName);
                socketEvent.data = inputJoinRoomName.text; 
                textRoomName.text = ("Room : " + "[ " + inputJoinRoomName.text + " ]");

                string jsonStr = JsonUtility.ToJson(socketEvent);
                ws.Send(jsonStr);
            }
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "Success");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }
        
        public void SendMessage()
        {
            if (inputText.text == "" || ws.ReadyState != WebSocketState.Open)
                return;

            MessageData newMessageData = new MessageData();
            newMessageData.username = inputUsername.text;
            newMessageData.message = inputText.text;

            string toJsonStr = JsonUtility.ToJson(newMessageData);

            ws.Send(toJsonStr);
            inputText.text = "";
        }

        private void OnDestroy()
        {
            if (ws != null)
                ws.Close();
        }

        private void Update()
        {
            //if (string.IsNullOrEmpty(tempMessageString ) == false)

            //{
            //    MessageData receiveMessageData = JsonUtility.FromJson<MessageData>(tempMessageString);
            //    if (receiveMessageData.username == inputUsername.text)
            //    {
            //        sendText.text += "<color=green>" + receiveMessageData.username + "</color>" + ": " + "<color=#00FCFF>" + receiveMessageData.message + "</color>" + "\n";
            //        receiveText.text += "\n";
            //    }
            //    else
            //    {
            //        receiveText.text += "<color=green>" + receiveMessageData.username + "</color>" + ": " + "<color=yellow>" + receiveMessageData.message + "</color>" + "\n";
            //        sendText.text += "\n";
            //    }


            //    tempMessageString = "";
            //}


            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);
               
                if (receiveMessageData.data == "CreateroomSoccess")
                   {                        
                     rootCreateRoom.SetActive(false);
                     rootMessenger.SetActive(true);
                   }
                else if (receiveMessageData.data == "CreateroomFail")
                   {
                     rootAlreadyHas.SetActive(true);
                   }
                else if (receiveMessageData.data == "JoinroomSuccess")
                   {
                     rootJoinRoom.SetActive(false);
                     rootMessenger.SetActive(true);

                   }
                else if(receiveMessageData.data == "JoinroomFail")
                   {
                    rootRoomisnotFound.SetActive(true);
                   }
           
                tempMessageString = "";
            }

        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            tempMessageString = messageEventArgs.Data;
        }
    }
}


