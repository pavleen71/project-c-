using System;

public class GameBoard
{
    private char[,] board; // 2D array to represent the game board

    public GameBoard()
    {
        // Initialize the game board with empty spaces
        board = new char[6, 7];
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                board[row, col] = '-';
            }
        }
    }

    public void DisplayBoard()
    {
        Console.WriteLine("  1 2 3 4 5 6 7");
        for (int row = 0; row < 6; row++)
        {
            Console.Write("|");
            for (int col = 0; col < 7; col++)
            {
                Console.Write(board[row, col] + "|");
            }
            Console.WriteLine();
        }
        Console.WriteLine("---------------");
    }

    public bool MakeMove(int column, char symbol)
    {
        if (column < 1 || column > 7)
        {
            Console.WriteLine("Invalid column number. Please choose a column from 1 to 7.");
            return false;
        }

        int colIndex = column - 1;
        for (int row = 5; row >= 0; row--)
        {
            if (board[row, colIndex] == '-')
            {
                board[row, colIndex] = symbol;
                return true;
            }
        }

        Console.WriteLine("Column is full. Please choose another column.");
        return false;
    }

    public bool CheckWin(char symbol)
    {
        // Check horizontally
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (board[row, col] == symbol &&
                    board[row, col + 1] == symbol &&
                    board[row, col + 2] == symbol &&
                    board[row, col + 3] == symbol)
                {
                    return true;
                }
            }
        }

        // Check vertically
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                if (board[row, col] == symbol &&
                    board[row + 1, col] == symbol &&
                    board[row + 2, col] == symbol &&
                    board[row + 3, col] == symbol)
                {
                    return true;
                }
            }
        }

        // Check diagonally (down-right)
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (board[row, col] == symbol &&
                    board[row + 1, col + 1] == symbol &&
                    board[row + 2, col + 2] == symbol &&
                    board[row + 3, col + 3] == symbol)
                {
                    return true;
                }
            }
        }

        // Check diagonally (up-right)
        for (int row = 3; row < 6; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (board[row, col] == symbol &&
                    board[row - 1, col + 1] == symbol &&
                    board[row - 2, col + 2] == symbol &&
                    board[row - 3, col + 3] == symbol)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

public class Player
{
    public string Name { get; set; }
    public char Symbol { get; set; }

    public virtual int GetMove(GameBoard board)
    {
        while (true)
        {
            Console.Write($"{Name}, enter the column number to drop your symbol ({Symbol}): ");
            if (int.TryParse(Console.ReadLine(), out int column))
            {
                if (board.MakeMove(column, Symbol))
                {
                    return column;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid column number.");
            }
        }
    }
}

public class HumanPlayer : Player
{
    // No additional methods needed for human player
}

public class ComputerPlayer : Player
{
    public override int GetMove(GameBoard board)
    {
        Random random = new Random();
        int column = random.Next(1, 8); // Randomly choose a column
        while (!board.MakeMove(column, Symbol))
        {
            column = random.Next(1, 8); // Try again if column is full
        }
        return column;
    }
}

public class GameManager
{
    private GameBoard board;
    private Player player1;
    private Player player2;

    public GameManager(Player p1, Player p2)
    {
        board = new GameBoard();
        player1 = p1;
        player2 = p2;
    }

    public void StartGame()
    {
        Console.WriteLine("Connect Four Game");
        Console.WriteLine("Player 1 (X) vs. Player 2 (O)");
        Console.WriteLine();

        board.DisplayBoard();

        Player currentPlayer = player1;
        while (true)
        {
            int column = currentPlayer.GetMove(board);
            board.DisplayBoard();

            if (board.CheckWin(currentPlayer.Symbol))
            {
                Console.WriteLine($"{currentPlayer.Name} wins!");
                break;
            }

            if (IsBoardFull())
            {
                Console.WriteLine("It's a draw!");
                break;
            }

            currentPlayer = (currentPlayer == player1) ? player2 : player1;
        }
    }

    private bool IsBoardFull()
    {
        for (int col = 0; col < 7; col++)
        {
            if (board.MakeMove(col + 1, 'T')) // Try to make a move with 'T' symbol
            {
                board.MakeMove(col + 1, '-'); // Undo the move
                return false; // Board is not full
            }
        }
        return true; // Board is full
    }
}

class Program
{
    static void Main(string[] args)
    {
        HumanPlayer player1 = new HumanPlayer { Name = "Player 1", Symbol = 'X' };
        ComputerPlayer player2 = new ComputerPlayer { Name = "Computer", Symbol = 'O' };

        GameManager game = new GameManager(player1, player2);
        game.StartGame();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
