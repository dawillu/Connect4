public class User {
    public string MessageType { get; set; }
    public string Username { get; set; }
    public string OpponentUsername { get; set; }
    public int PlayerId { get; set; }
    public int OpponentId { get; set; }
    public string PlayerColor { get; set; }
    public string OpponentColor { get; set; }
    public int Col { get; set; }
    public int Row { get; set; }
    public bool isYourTurn { get; set; }
    public int WhosMove { get; set; }
    public int RoomId { get; set; }
    public string MessageContent { get; set; }
    public int WhosWin { get; set; }
}