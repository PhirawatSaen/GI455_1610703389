const sqlite3 = require('sqlite3').verbose();

let database = new sqlite3.Database('./database/chatDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE, (err)=>
{
    if(err) throw err;

    console.log("Connected to database")
  

//Update //////////////////////
    /*var dataFromClientAdd = {
        evenName: "AddMoney",
        data: "test1111#100"
    }*/

    //var splitStr = dataFromClientAdd.data.split("#")
    //var userID = splitStr[0];
    //var AddedMoney = parseInt(splitStr[1]);

    //var sqlUpdate = "UPDATE UserDate SET Money= '200' WHERE UserID='"+userID+"' "
    
    /*db.all("SELECT Money FROM UserData WHERE UserID='"+userID+"' ", (err,rows)=>
    {
        if(err)
        {
            var callbackMsg = {
                evenName: "AddMoney",
                data: "Fail"
            }
            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("[6]" +toJsonStr);
        }
        else
        {           
            console.log(rows);
            if(rows.length > 0)
            {
                var currentMoney = rows[0].Money;
                currentMoney += AddedMoney;

                db.all("UPDATE UserData SET Money='"+currentMoney+"' WHERE UserID='"+userID+"'", (err,rows)=>
                {
                    if(err)
                    {
                        var callbackMsg = {
                            evenName: "AddMoney",
                            data: "Fail"
                        }
                        var toJsonStr = JSON.stringify(callbackMsg);
                        console.log("[7]" +toJsonStr);
                    }
                    else
                    {
                        var callbackMsg = {
                            evenName: "AddMoney",
                            data:currentMoney.toString()
                        }
                        var toJsonStr = JSON.stringify(callbackMsg);
                        console.log("[8]" +toJsonStr);
                    }
                });
            }
            else
            {
            var callbackMsg = 
            {
                evenName: "AddMoney",
                data: "Fail"
            }
            
            var toJsonStr = JSON.stringify(callbackMsg);
            console.log("[9]" +toJsonStr);
            
            }           
        }
        
    });*/

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
                    data:"LeaveRoomFail"
                   }
                    var toJsonStr = JSON.stringify(resultData);
                    ws.send(toJsonStr);

                     console.log("leave room fail");
                }
                else
                {

                    var resultData = {
                    eventName: toJson.eventName,
                    data:"LeaveRoomSuccess"
                    }
                    var toJsonStr = JSON.stringify(resultData);
                    ws.send(toJsonStr);

                    console.log("leave room success");
                }
                 
             }

             else if(toJson.eventName == "Login")
             {
                //Login ///////////////////////
                var dataFromClientLogin = {
                evenName: "Login",
                 data: toJson.data   
                }
                var splitStr = dataFromClientLogin.data.split("#")
                 var userID = splitStr[0];
                 var password = splitStr[1];

                 var sqlSelect = "SELECT * FROM UserData WHERE UserID= '"+userID+"' AND Password= '"+password+"' ";//Login
    
                //Login
                database.all(sqlSelect, (err, rows)=>
                {
                 if(err)
                     {
                      console.log("[2]" + err);
                     }
                 else
                     {
                        if(rows.length > 0)
                        {
                            console.log("--------[3]--------")
                            console.log(rows);
                            console.log("--------[3]--------")
                            var callbackMsg = {
                            evenName: "Login",
                            data: "LoginSuccess"
                        }

                             var toJsonStr = JSON.stringify(callbackMsg);
                             ws.send(toJsonStr);
                            console.log("[4]" +toJsonStr);
                        }
                        else
                         {
                            var callbackMsg = {
                            evenName: "Login",
                            data: "LoginFail"
                         }
                            var toJsonStr = JSON.stringify(callbackMsg);
                            ws.send(toJsonStr);
                            console.log("[5]" +toJsonStr);
                         }
            
                    }
                });
            }
            else if(toJson.eventName == "Register")
            {
                //Register //////////////////////
                var dataFromClientRegister = 
                {
                 evenName: "Register",
                 data: toJson.data
                }
                var splitStr = dataFromClientRegister.data.split("#")
                var userID = splitStr[0];
                var password = splitStr[1];
                var name = splitStr[2];
    

                var sqlInsert = "INSERT INTO UserData (UserID, Password, Name, Money) VALUES ('"+userID+"', '"+password+"', '"+name+"', '0')";//Register
    

                //Register
                database.all(sqlInsert, (err, rows)=>
                {
                     if(err)
                    {
                         var callbackMsg = 
                        {
                        evenName: "Register",
                        data: "RegisterFail"
                        }
                    var toJsonStr = JSON.stringify(callbackMsg);
                    ws.send(toJsonStr);
                    console.log("[0]" +toJsonStr);
                    }
                    else
                    {
                        var callbackMsg = {
                        evenName: "Register",
                        data: "RegisterSuccess"
                         }   
                        var toJsonStr = JSON.stringify(callbackMsg);
                        ws.send(toJsonStr);
                        console.log("[1]" +toJsonStr);
            
                    }
                })
            }
            else if(toJson.eventName == "SendMessage")
            {
                Boardcast(ws, toJson.data);
            }
             
        });
     

    }
          
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

function Boardcast(ws, massage) 
{
    var selectRoomIndex = -1;

    for (var i = 0; i < roomList.length; i++) 
    {
          for(var j = 0; roomList[i].wsList.length; i++)
          {
             if(ws == roomList[i].wsList[j])
             {
                selectRoomIndex = i;
                break;
             }
          }
    }

    for(var i = 0 ; i < roomList[selectRoomIndex].wsList.length; i++)
    {
        var resultData = {
            eventName: "SendMessage",
            data:massage
        }

        roomList[selectRoomIndex].wsList[i].send(JSON.stringify(resultData));
    }
}

});
