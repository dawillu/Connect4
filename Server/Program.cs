using WebSocketSharp;
using Newtonsoft.Json;
using WebSocketSharp.Server;

// server info (web socket)
var url = "ws://127.0.0.1:7890";
var play_service = "/Play";

// web socket
WebSocketServer wssv = new(url);
// add web services (url extensions)
wssv.AddWebSocketService<Play>(play_service);
// start server
wssv.Start();
Console.WriteLine($"server started on {url}{play_service}");

// menu
do {
    Console.Clear();
    Console.WriteLine($"server started on {url}{play_service}");
    Console.WriteLine("[esc] - stop server"); 
} while (Console.ReadKey().Key != ConsoleKey.Escape);

// stop / close
wssv.Stop();

public class Play : WebSocketBehavior
{ 
    private static List<WebSocket> _clientSockets = new();
    private static List<int> _pendingClients = new();
    private static List<string> _clientNames = new();
    private static List<Room> _rooms = new();
    private static User data;
    private static string color1 = "#dd4b61";
    private static string color2 = "#f8d072";
    private static int playerId = 0;
    private static int count;
    private static int cols = 7;
    private static int rows = 6;

    protected override void OnOpen()
    {
        count = _clientSockets.Count + 1;
        WebSocket client = Context.WebSocket;
        Console.WriteLine($"\n a client tried to connect");

        _clientSockets.Add(client);
        Console.WriteLine($"< client {count} > succesfully connected");

        // initialize c# class istance
        data = new();
        // connection config
        data.MessageType = "connection config";
        // player id
        data.PlayerId = playerId++;
        // send config to the player
        Context.WebSocket.Send(JsonConvert.SerializeObject(data));

        _clientNames.Add("");
    }

    protected override void OnMessage(MessageEventArgs e) {
        var data = JsonConvert.DeserializeObject<User>(e.Data);

        // on message text
        if (data.MessageType == "message") {
            data.MessageType = "message config";
            _clientSockets[data.OpponentId].Send(JsonConvert.SerializeObject(data));
        }

        // on pending for game
        if (data.MessageType == "connection") {

            // the configured client is pending
            _pendingClients.Add(data.PlayerId);
            Console.WriteLine("client " + (data.PlayerId + 1) + " is pending");
            
            _clientNames[data.PlayerId] = data.Username;

            // initialize c# class istance
            data = new();
            // start game
            if (_pendingClients.Count == 2) {
                // add the two players in a room
                _rooms.Add(new(_pendingClients[0], _pendingClients[1], rows, cols));

                // start message
                data.MessageType = "start config";
                data.RoomId = _rooms[_rooms.Count - 1].RoomId;

                data.OpponentUsername = _clientNames[_pendingClients[1]];
                data.OpponentId = _pendingClients[1];   // opponent id
                data.OpponentColor = color2;            // opponent color
                data.PlayerColor = color1;              // player color
                // send config to player 1
                _clientSockets[_pendingClients[0]].Send(JsonConvert.SerializeObject(data));

                data.OpponentUsername = _clientNames[_pendingClients[0]];
                data.OpponentId = _pendingClients[0];   // opponent id
                data.OpponentColor = color1;            // opponent color
                data.PlayerColor = color2;              // player color
                // send config to player 2
                _clientSockets[_pendingClients[1]].Send(JsonConvert.SerializeObject(data));

                // playing player are not pending anymore
                _pendingClients.Clear();
            }
        }

        // on move by a client
        if (data.MessageType == "move") {
            data.Row = GetFirstFreeRowOfCol(data.Col, data.RoomId);

            if( data.Row == 6 ) {
                data.MessageType = "move config full";
                _clientSockets[data.PlayerId].Send(JsonConvert.SerializeObject(data));
            }
            else Move(data);
        }
    }

    void Move(User data) {

        data.MessageType = "move config";

        data.WhosMove = data.PlayerId;

        data.isYourTurn = true;
        _clientSockets[data.OpponentId].Send(JsonConvert.SerializeObject(data));

        data.isYourTurn = false;
        _clientSockets[data.PlayerId].Send(JsonConvert.SerializeObject(data));

        if (data.PlayerColor == color1) _rooms[data.RoomId].field[data.Row, data.Col] = "R";
        else if (data.PlayerColor == color2) _rooms[data.RoomId].field[data.Row, data.Col] = "Y";

        // if is win then, send the JSON with datas
        if (Check(data.Row, data.Col, data.RoomId)) {
            
            data.MessageType = "Win";

            data.WhosWin = data.WhosMove;
            _clientSockets[data.PlayerId].Send(JsonConvert.SerializeObject(data));
            _clientSockets[data.OpponentId].Send(JsonConvert.SerializeObject(data));

            Console.WriteLine("client " + (data.WhosWin + 1) + " won");

        }
    }

    bool Check(int row, int col, int roomId) {
        string colorToMatch = _rooms[roomId].field[row, col];
        if (colorToMatch == "") return false;
        
        // horizontal check
        int count_combo = 1;
        // check left
        for (int i = col - 1; i >= 0; i--) {
            if (_rooms[roomId].field[row, i] == colorToMatch) {
                count_combo++;
            } else {
                break;
            }
        }
        // check right
        for (int i = col + 1; i < cols; i++) {
            if (_rooms[roomId].field[row, i] == colorToMatch) {
                count_combo++;
            } else {
                break;
            }
        }
        if (count_combo >= 4) return true;

        // vertical check
        count_combo = 1;
        // check down
        for (int i = row + 1; i < rows; i++) {
            if (_rooms[roomId].field[i, col] == colorToMatch) {
                count_combo++;
            } else {
                break;
            }
        }
        if (count_combo >= 4) return true;

        // diagonal check is already working correctly
        return CompletesDiagonal(row, col, roomId);
    }

    // check for diagonals
    private bool CompletesDiagonal(int pieceRow, int pieceCol, int roomId)
    {
        var colorToMatch = _rooms[roomId].field[pieceRow, pieceCol]; // Board is a field[6, 7] array
        var matchingPieces = 1; // We will count the original piece as a match

        // Check diagonal '/' - forward

        // First check botton left
        for (int i = 1; i < 4; i++)
        {
            var row = pieceRow - i;
            var col = pieceCol - i;

            // Make sure we stay within our board
            if (row < _rooms[roomId].field.GetLowerBound(0) || col < _rooms[roomId].field.GetLowerBound(1)) { break; }

            if (_rooms[roomId].field[row, col] == colorToMatch)
            {
                matchingPieces++;
                if (matchingPieces == 4) return true;
            }
            else { break; }
        }

        // Next check upper right
        for (int i = 1; i < 4; i++)
        {
            var row = pieceRow + i;
            var col = pieceCol + i;

            // Make sure we stay within our board
            if (row > _rooms[roomId].field.GetUpperBound(0) || col > _rooms[roomId].field.GetUpperBound(1)) { break; }

            // Check for a match
            if (_rooms[roomId].field[row, col] == colorToMatch)
            {
                matchingPieces++;
                if (matchingPieces == 4) return true;
            }
            else { break; }
        }

        // Check diagonal '\' - forward
        matchingPieces = 1;

        // First check bottom right
        for (int i = 1; i < 4; i++)
        {
            var row = pieceRow - i;
            var col = pieceCol + i;

            // Make sure we stay within our board
        for (int i = 1; i < 4; i++)
        {
            var row = pieceRow + i;
            var col = pieceCol - i;

            // Make sure we stay within our board
            if (row > _rooms[roomId].field.GetUpperBound(0) || col < _rooms[roomId].field.GetLowerBound(1)) { break; }

            // Check for a match
            if (_rooms[roomId].field[row, col] == colorToMatch)
            {
                matchingPieces++;
                if (matchingPieces == 4) return true;
            }
            else { break; }
        }

        // If we've gotten this far, then we haven't found a match
        return false;
    }

    // get first empty row by a given col
    int GetFirstFreeRowOfCol(int col, int roomId)
    {
        for (int i = 0; i < rows; i++) {
            if (_rooms[roomId].field[rows - i - 1, col] == "")
                return (rows - i - 1);
        }
        return rows;
    }
}
