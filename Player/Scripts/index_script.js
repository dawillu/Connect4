// url
const url = "ws://127.0.0.1:7890/Play";

// sign
var username;
var opponent_name;
var player_id;
var opponent_id;
var player_color;
var enemy_color;
var is_my_turn = true;
var roomId;
var isSearching = true;


// web socket
const ws = new WebSocket(url);

// on connect
ws.addEventListener('open', () => {
    console.log("in");
});

document.addEventListener("keydown", function(event) {
    if (event.key == "Enter" && document.getElementById("message").value != "") {
        var msg = document.getElementById("message").value;
        var box = document.getElementById("chat-container");
        var color = `background-color: ${player_color}`;
        box.innerHTML += `<div class="chat1" style="${color}"> ${msg} </div>`;
        box.scrollTop = box.scrollHeight;

        document.getElementById("message").value = "";

        var obj = new Object();
        obj.MessageType = "message";
        obj.PlayerId = player_id;
        obj.OpponentId = opponent_id;
        obj.PlayerColor = player_color;
        obj.MessageContent = msg;
        ws.send(JSON.stringify(obj));
    }
});

function Search() {
    username = document.getElementById("username").value;
    if (username == "") return;
    if (isSearching == false) return;

    loading_img = document.getElementById("loading");
    loading_img.setAttribute('style', 'visibility: visible');
    loading_text = document.getElementById("loading-text")
    loading_text.setAttribute('style', 'visibility: visible');

    var obj = new Object();
    obj.MessageType = "connection";
    obj.PlayerId = player_id;
    obj.Username = username;
    ws.send(JSON.stringify(obj));

    isSearching = false;
}

// on message
ws.addEventListener('message', e => {
    var obj = JSON.parse(e.data);
    
    if (obj.MessageType === "connection config") {
        player_id = obj.PlayerId;
    }

    if (obj.MessageType === "start config") {
        roomId = obj.RoomId;
        opponent_name = obj.OpponentUsername;
        opponent_id = obj.OpponentId;
        player_color = obj.PlayerColor;
        enemy_color = obj.OpponentColor;

        document.getElementById("chat").setAttribute('style', 'visibility: visible');

        document.getElementById("opponent").innerText = "Enemy: " + opponent_name;
        document.getElementById("opponent").setAttribute('style', `background-color: ${enemy_color}`);
        document.getElementById("player").innerText = "Username: " + username;
        document.getElementById("player").setAttribute('style', `background-color: ${player_color}`);


        document.getElementById("scoreboard").setAttribute('style', 'visibility: visible');

        document.getElementById("loading").setAttribute('style', 'visibility: hidden');
        document.getElementById("wait").setAttribute('style', 'visibility: hidden');
        document.getElementById("field").setAttribute('style', 'visibility: visible');
    }

    if (obj.MessageType === "message config") {
        var box = document.getElementById("chat-container");
        var color = `background-color: ${enemy_color}`;
        box.innerHTML += `<div class="chat2" style="${color}"> ${obj.MessageContent} </div>`;
        box.scrollTop = box.scrollHeight;
    }

    if (obj.MessageType === "move config full") return;
    if (obj.MessageType === "move config") {
        PutSign(obj);
        is_my_turn = obj.isYourTurn;
    }

    if (obj.MessageType === "Win") {
        if (obj.WhosWin == player_id) alert("YOU WIN!");
        else if (obj.WhosWin == opponent_id) alert("YOU LOST!");
        is_my_turn = false;
    }
});

function PutSign(obj) {
    var id = `${obj.Row} ${obj.Col}`;

    if (obj.WhosMove == player_id) simbol = player_color;
    else simbol = enemy_color;
    document.getElementById(id).style.backgroundColor = simbol;
    document.getElementById(id).classList.add('fall');
}

function PlayerMove(id) {
    var obj = new Object();
    obj.MessageType = "move";
    obj.RoomId = roomId;
    obj.PlayerId = player_id;
    obj.OpponentId = opponent_id;
    obj.PlayerColor = player_color;
    obj.Row = id.split(' ')[0];
    obj.Col = id.split(' ')[1];
    var jsonString= JSON.stringify(obj);

    if (is_my_turn === true && roomId != null) ws.send(jsonString);
}