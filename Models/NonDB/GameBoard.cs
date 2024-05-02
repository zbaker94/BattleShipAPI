namespace BattleShipAPI.Models.NonDB;

public class GameBoard
{
    public int Rows { get; set; }
    public int Columns { get; set; }
    public GameBoardCell[,] BoardState { get; set; }

    public GameBoard(Board board){
        Rows = board.Rows;
        Columns = board.Columns;
        BoardState = new GameBoardCell[Rows, Columns];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                var shot =  board.BoardState.Where(s => s.Row == i && s.Column == j).FirstOrDefault();
                var ship = board.Ships.Where(s => s.Vertical && s.Row <= i && i < s.Row + s.ShipType.Size && s.Column == j).FirstOrDefault() ??
                           board.Ships.Where(s => !s.Vertical && s.Column <= j && j < s.Column + s.ShipType.Size && s.Row == i).FirstOrDefault();
                BoardState[i,j] = new GameBoardCell
                {
                    Row = i,
                    Column = j,
                    Shot = shot,
                    Ship = ship
                };
            }
        }
    }

    public string[,] ToArray(){
        string[,] result = new string[Rows, Columns];
        foreach(var cell in BoardState){
            result[cell.Row, cell.Column] = cell.Ship != null ? "S" : cell.Shot != null ? cell.Shot.Hit ? "X" : "O" : " ";
        }

        return result;
    }
}