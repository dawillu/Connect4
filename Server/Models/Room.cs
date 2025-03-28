public class Room {
    static int id = 0;
    public int RoomId { get; set; }

    public int Player1Id { get; set; }
    public int Player2Id { get; set; }

    public string[,] field { get; set; }

    public Room(int P1, int P2, int row, int col) {
        RoomId = id++;
        Player1Id = P1;
        Player2Id = P2;
        field = new string[row, col];
        ResetField(row, col);
    }
    void ResetField(int row, int col) {
        // set each cell as ""
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                field[i, j] = "";
    }
}