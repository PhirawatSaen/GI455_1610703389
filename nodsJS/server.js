var websocket = require('ws');

var callbackInitServer = ()=>{
    console.log("Server is running.");
}

var wss = new websocket.Server({port:5500},callbackInitServer);

var wsList = [];
var roomList = [];

wss.on("connection", (ws)=>{

    {
        //LobbyZone
        ws.on("message", (data)=>
        {
            console.log(data);

            var toJson = JSON.parse(data);

             if(toJson.eventName == "CreateRoom")
             {
                 console.log("Client request CreateRoom [" +toJson.data+ "]");
                 var isFoundRoom = false
                 for (var i = 0; i < roomList.length; i++) 
                 {
                    if(roomList[i].roomName == toJson.data)
                    {
                        isFoundRoom = true;
                        break;
                    }
                     
                 }

                 if(isFoundRoom == true)
                 {
                     console.log("Create room : Already has this room");

                     var resultData =
                     {
                         eventName: toJson.eventName,
                         data: "CreateroomFail"
                     }

                     var toJsonStr = JSON.stringify(resultData);

                     ws.send(toJsonStr);

                     console.log("client create room fail.");
 
                 }
                 else 
                 {
                     var newRoom = 
                     {
                        roomName: toJson.data,
                        wsList: []
                     }

                     newRoom.wsList.push(ws);

                     roomList.push(newRoom);
                   
                     var resultData =
                     {
                        eventName: toJson.eventName,
                        data: "CreateroomSoccess"
                    }

                    var toJsonStr = JSON.stringify(resultData);

                    ws.send(toJsonStr);

                    console.log("client create room success.");
                   
                   // console.log("Create room : room is not found");

                   // ws.send("CreateRoomSuccess");
                 }
             }

             //JoinRoom
            else if(toJson.eventName == "JoinRoom")
             {
                console.log("client request JoinRoom")
                var jIsFoundRoom = false
                 for (var i = 0; i < roomList.length; i++) 
                 {
                    if(roomList[i].roomName == toJson.data)
                    {
                        jIsFoundRoom  = true;
                        roomList[i].wsList.push(ws);
                        break;
                    }
                     
                 }
                 if(jIsFoundRoom == false)
                 {
                    
                     var resultData =
                     {
                         eventName: toJson.eventName,                         
                         data: "JoinroomFail"
                     }

                     var toJsonStr = JSON.stringify(resultData);

                     ws.send(toJsonStr);

                     console.log("client join room fail.");
                 }
                 else 
                 {
                    console.log("Join room : Room is Found");

                     var resultData =
                    {
                        eventName: toJson.eventName,
                        data: "JoinroomSuccess"
                    }

                    var toJsonStr = JSON.stringify(resultData);

                    ws.send(toJsonStr);
                    console.log("client join room success.");
                }
            }
            
             
            
            else if(toJson.eventName == "LeaveRoom")
             {
                 var isFound = false;
                 for (var i = 0; i < roomList.length; i++) 
                 {
                    for (var j = 0; j < roomList[i].length; j++) 
                    {
                        if(ws == roomList[i].wsList[j])
                        {
                            roomList[i].wsList.splice(j, 1);
                           
                            if(roomList[i].wsList.length <= 0)
                             {
                                roomList.splice(i, 1);
                             }
                            isFound = true;
                             break;
                        } 
                        
                    }
                     
                 }
                if(isFound)
                {
                
                   var resultData = {
                    eventName: toJson.eventName,
                    data:"success"
                   }
                    var toJsonStr = JSON.stringify(resultData);
                    ws.send(toJsonStr);

                     console.log("leave room success");
                }
                else
                {

                    var resultData = {
                    eventName: toJson.eventName,
                    data:"fail"
                    }
                    var toJsonStr = JSON.stringify(resultData);
                    ws.send(toJsonStr);

                    console.log("leave room fail");
                }
                 
             }
         });
     

    }

            //console.log("send form client " + data);
            //Boardcast(data); 
  

         
    console.log("client connected.");
    wsList.push(ws);

    /*ws.on("message", (data)=>{

        console.log("send form client " + data);
        Boardcast(data); 
    });*/
 
    ws.on("close",()=>{
      
        console.log("client disconnected.");
        //wsList = ArrayRemove(wsList, ws);
        for (var i = 0; i < wsList.length; i++) 
        {
            if(wsList[i] == ws)
            {
                wsList.splice;(i, 1);
                break;
            }
            
        }

        for (var i = 0; i < roomList.length; i++) 
        {
            for (var j = 0; j < roomList[i].length; j++) 
            {
                if(roomList[i].wsList[j] == ws)
                {
                    roomList[i].wsList.splice(j, 1);
                    break;
                }
                        
            }
                     
        }
    });

}); 

function ArrayRemove(arr, value)
{
    return arr.filter((element)=>{
        return element != value;
    })
}

function Boardcast(data) 
{
    for (var i = 0; i < wsList.length; i++) 
    {
        wsList[i].send(data);    
    }
}

