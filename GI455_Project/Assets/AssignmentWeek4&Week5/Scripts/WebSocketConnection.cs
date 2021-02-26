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
        public InputField inputUserIDLogin;
        public InputField inputPasswordLogin;
        public InputField inputUserIDRegister;
        public InputField inputNameRegister;
        public InputField inputPasswordRegister;
        public InputField inputRePasswordRegister;
       
        public GameObject rootConnection;
        public GameObject rootMessenger;
        public GameObject rootCreateAndJoin;
        public GameObject rootCreateRoom;
        public GameObject rootJoinRoom;
        public GameObject rootAlreadyHas;
        public GameObject rootRoomisnotFound;
        public GameObject rootLogin;
        public GameObject rootRegister;
        public GameObject rootLoginFail;
        public GameObject rootRegisterFail;
        public GameObject rootPasswordNotMatch;
        public GameObject rootInputallField;

        public InputField inputText;
        public Text sendText;
        public Text receiveText;
        public Text textRoomName;
        private WebSocket ws;

        private string tempMessageString;

        private string loginUser;
        private string registerUser;

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
            rootLogin.SetActive(true);

            //CreateRoom("TestRoom01");
        }
        public void LoginUser()
        {
            Login(loginUser);
        }

        public void RegisterUser()
        {
            Register(registerUser);
        }

        public void ToRegister()
        {
            rootLogin.SetActive(false);
            rootRegister.SetActive(true);
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
        public void BackToLogin()
        {
            rootLoginFail.SetActive(false);
        }
        public void BackToLRegister()
        {
            rootRegisterFail.SetActive(false);
            rootPasswordNotMatch.SetActive(false);
        }
        public void BackToReAndLog()
        {
            rootInputallField.SetActive(false);
        }

        public void Login(string data)
        {
            if (ws.ReadyState == WebSocketState.Open)
            {
                if (inputUserIDLogin.text == "" || inputPasswordLogin.text == "")
                {
                    rootInputallField.SetActive(true);
                }
                SocketEvent socketEvent = new SocketEvent("Login", data);
                socketEvent.data = inputUserIDLogin.text + "#" + inputPasswordLogin.text;
                loginUser = socketEvent.data;
                string jsonStr = JsonUtility.ToJson(socketEvent);
                ws.Send(jsonStr);
            }
        }
        public void Register(string data)
        {
            if (ws.ReadyState == WebSocketState.Open)
            {
                if (inputPasswordRegister.text == inputRePasswordRegister.text)
                {
                    if(inputUserIDRegister.text == "" || inputPasswordRegister.text == "" || inputNameRegister.text == "")
                    {
                        rootInputallField.SetActive(true);
                    }
                    
                    SocketEvent socketEvent = new SocketEvent("Register", data);
                    socketEvent.data = inputUserIDRegister.text + "#" + inputPasswordRegister.text + "#" + inputNameRegister.text;
                    registerUser = socketEvent.data;
                    string jsonStr = JsonUtility.ToJson(socketEvent);
                    ws.Send(jsonStr);
                }
                else
                {
                    rootPasswordNotMatch.SetActive(true);
                }
            }
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
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

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
                newMessageData.username = inputNameRegister.text;
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
            if (string.IsNullOrEmpty(tempMessageString) == false)

            {
                MessageData receiveMessageData = JsonUtility.FromJson<MessageData>(tempMessageString);
                if (receiveMessageData.username == inputNameRegister.text)
                {
                    sendText.text += "<color=green>" + receiveMessageData.username + "</color>" + ": " + "<color=#00FCFF>" + receiveMessageData.message + "</color>" + "\n";
                    receiveText.text += "\n";
                }
                else
                {
                    receiveText.text += "<color=green>" + receiveMessageData.username + "</color>" + ": " + "<color=yellow>" + receiveMessageData.message + "</color>" + "\n";
                    sendText.text += "\n";
                }


                //tempMessageString = "";
            }


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
                else if (receiveMessageData.data == "LeaveRoomSuccess")
                {
                    rootMessenger.SetActive(false);
                    rootCreateAndJoin.SetActive(true);
                }
                else if (receiveMessageData.data == "LoginSuccess")
                {
                    rootLogin.SetActive(false);
                    rootCreateAndJoin.SetActive(true);
                }
                else if (receiveMessageData.data == "LoginFail")
                {
                    rootLoginFail.SetActive(true);
                }
                else if (receiveMessageData.data == "RegisterSuccess")
                {
                    rootLogin.SetActive(true);
                    rootRegister.SetActive(false);
                }
                else if (receiveMessageData.data == "RegisterFail")
                {
                    rootRegisterFail.SetActive(true);
                }

                tempMessageString = "";
            }

            //tempMessageString = "";

        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            tempMessageString = messageEventArgs.Data;
        }
    }
}


