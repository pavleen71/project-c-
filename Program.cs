using System;

public class GameBoard
{
    private readonly char[,] board;

    public GameBoard()
    {
        board = new char[6, 7];
        InitializeBoard();
    }

    public void InitializeBoard()
    {
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

    public bool IsFull()
    {
        for (int col = 0; col < 7; col++)
        {
            if (board[0, col] == '-')
            {
                return false;
            }
        }
        return true;
    }
}

public abstract class Player
{
    public string Name { get; set; }
    public char Symbol { get; set; }

    public abstract int GetMove(GameBoard board);
}

public class HumanPlayer : Player
{
    public override int GetMove(GameBoard board)
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

public class AIPlayer : Player
{
    public override int GetMove(GameBoard board)
    {
        Random random = new Random();
        int column;
        do
        {
            column = random.Next(1, 8); // Randomly choose a column
        } while (!board.MakeMove(column, Symbol));

        return column;
    }
}

public class GameManager
{
    private readonly GameBoard board;
    private readonly Player player1;
    private readonly Player player2;
    private int player1Wins;
    private int player2Wins;
    private int totalGames;

    public GameManager(Player p1, Player p2)
    {
        board = new GameBoard();
        player1 = p1;
        player2 = p2;
    }

    public void StartGame()
    {
        Console.WriteLine("Connect Four Game");
        Console.WriteLine();

        player1.Name = GetPlayerName(player1.Name, 1);
        player2.Name = GetPlayerName(player2.Name, 2);

        board.DisplayBoard();

        Player currentPlayer = player1;
        while (true)
        {
            int column = currentPlayer.GetMove(board);
            board.DisplayBoard();

            if (board.CheckWin(currentPlayer.Symbol))
            {
                Console.WriteLine($"{currentPlayer.Name} wins!");
                if (currentPlayer == player1)
                {
                    player1Wins++;
                }
                else
                {
                    player2Wins++;
                }
                totalGames++;
                DisplayStatistics();
                if (PlayAgain())
                {
                    board.InitializeBoard();
                    board.DisplayBoard();
                    currentPlayer = player1;
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (board.IsFull())
            {
                Console.WriteLine("It's a draw!");
                totalGames++;
                DisplayStatistics();
                if (PlayAgain())
                {
                    board.InitializeBoard();
                    board.DisplayBoard();
                    currentPlayer = player1;
                    continue;
                }
                else
                {
                    break;
                }
            }

            currentPlayer = (currentPlayer == player1) ? player2 : player1;
        }
    }

    private string GetPlayerName(string defaultName, int playerNumber)
    {
        string defaultPlayerName = playerNumber == 1 ? "Player 1" : "Player 2";
        Console.Write($"Enter name for {defaultName ?? defaultPlayerName}: ");
        string name = Console.ReadLine();
        return string.IsNullOrWhiteSpace(name) ? defaultName ?? defaultPlayerName : name;
    }

    private bool PlayAgain()
    {
        while (true)
        {
            Console.Write("Do you want to play again? (Y/N): ");
            string input = Console.ReadLine().Trim().ToUpper();

            if (input == "Y" || input == "N")
            {
                return input == "Y";
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
            }
        }
    }

    private void DisplayStatistics()
    {
        Console.WriteLine("Game Statistics:");
        Console.WriteLine($"Total Games Played: {totalGames}");
        Console.WriteLine($"{player1.Name} Wins: {player1Wins}");
        Console.WriteLine($"{player2.Name} Wins: {player2Wins}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Connect Four Game");
        Console.WriteLine("Choose the type of game:");
        Console.WriteLine("1. Player vs Player");
        Console.WriteLine("2. Player vs AI (Random Move)");
        Console.Write("Enter your choice: ");

        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
            Console.Write("Enter your choice: ");
        }

        Player player1 = new HumanPlayer { Symbol = 'X' };
        Player player2;

        if (choice == 1)
        {
            player2 = new HumanPlayer { Symbol = 'O' };
        }
        else
        {
            player2 = new AIPlayer { Symbol = 'O' };
        }

        GameManager game = new GameManager(player1, player2);
        game.StartGame();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
