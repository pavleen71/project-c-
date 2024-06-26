using System;

// Interface for players
public interface IPlayer // Gursharandeep Singh
{
    string Name { get; set; }
    char Symbol { get; set; }
    int GetMove(GameBoard board);
}

// Represents the game board
public class GameBoard // Pavleen kaur
{
    private const int Rows = 6;
    public const int Cols = 7;

    private readonly char[,] board;

    public GameBoard()
    {
        board = new char[Rows, Cols];
        InitializeBoard();
    }

    // Initialize the game board with empty cells
    public void InitializeBoard() // Pavleen kaur
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                board[row, col] = '-';
            }
        }
    }

    // Display the current state of the game board
    public void DisplayBoard() // Gursharandeep Singh
    {
        Console.Clear(); // Clear the console
        Console.WriteLine("".PadLeft((Console.WindowWidth - "Connect Four Game".Length) / 2) + "Connect Four Game");
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine(" 1 2 3 4 5 6 7");
        for (int row = 0; row < Rows; row++)
        {
            Console.Write("|");
            for (int col = 0; col < Cols; col++)
            {
                Console.Write(board[row, col] + "|");
            }
            Console.WriteLine();
        }
        Console.WriteLine("---------------");
    }

    // Make a move in the specified column with the given symbol
    public bool MakeMove(int column, char symbol) // Gursharandeep Singh
    {
        // Check if the column number is valid
        if (column < 1 || column > Cols)
        {
            Console.WriteLine("Invalid column number. Please choose a column from 1 to 7.");
            return false;
        }

        int colIndex = column - 1;
        // Find the first available row in the column
        for (int row = Rows - 1; row >= 0; row--)
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

    // Check if the given symbol has won the game
    public bool CheckWin(char symbol) // Pavleen kaur and Gursharandeep Singh
    {
        // Check horizontally
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols - 3; col++)
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
        for (int row = 0; row < Rows - 3; row++)
        {
            for (int col = 0; col < Cols; col++)
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
        for (int row = 0; row < Rows - 3; row++)
        {
            for (int col = 0; col < Cols - 3; col++)
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
        for (int row = 3; row < Rows; row++)
        {
            for (int col = 0; col < Cols - 3; col++)
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

    // Check if the game board is full
    public bool IsFull() // Gursharandeep Singh
    {
        for (int col = 0; col < Cols; col++)
        {
            if (board[0, col] == '-')
            {
                return false;
            }
        }
        return true;
    }
}

// Represents a human player
public class HumanPlayer : IPlayer // Pavleen kaur
{
    public string Name { get; set; }
    public char Symbol { get; set; }

    // Get the human player's move from the console input
    public int GetMove(GameBoard board) // Pavleen kaur
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

// Represents an AI player
public class AIPlayer : IPlayer // Gursharandeep Singh
{
    public string Name { get; set; }
    public char Symbol { get; set; }

    // Constructor to set the name of the AI player
    public AIPlayer()
    {
        Name = "Computer";
    }

    // Get the AI player's move by choosing a random column
    public int GetMove(GameBoard board) // Gursharandeep Singh
    {
        Random random = new Random();
        int column;
        do
        {
            column = random.Next(1, GameBoard.Cols + 1); // Randomly choose a column
        } while (!board.MakeMove(column, Symbol));

        return column;

    }
}

// Manages the game flow
public class GameManager // Gursharandeep Singh and Pavleen kaur
{
    private readonly GameBoard board;
    private readonly IPlayer player1;
    private readonly IPlayer player2;
    private int player1Wins;
    private int player2Wins;
    private int totalGames;

    // Constructor to initialize the game manager with players
    public GameManager(IPlayer p1, IPlayer p2) // Gursharandeep Singh
    {
        board = new GameBoard();
        player1 = p1;
        player2 = p2;
    }

    // Start the game
    public void StartGame() // Gursharandeep Singh and Pavleen kaur
    {
        Console.WriteLine("".PadLeft((Console.WindowWidth - "Connect Four Game".Length) / 2) + "Connect Four Game");
        Console.WriteLine();
        Console.WriteLine();

        // Set player names
        player1.Name = GetPlayerName(player1.Name, 1);
        player2.Name = GetPlayerName(player2.Name, 2);

        board.DisplayBoard();

        IPlayer currentPlayer = player1;
        while (true)
        {
            int column = currentPlayer.GetMove(board);
            board.DisplayBoard();

            // Check if the current player wins
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
                // Ask if the players want to play again
                if (!PlayAgain())
                {
                    break;
                }
                ResetGame();
                currentPlayer = player1;
                continue;
            }

            // Check if the game is a draw
            if (board.IsFull())
            {
                Console.WriteLine("It's a draw!");
                totalGames++;
                DisplayStatistics();
                // Ask if the players want to play again
                if (!PlayAgain())
                {
                    break;
                }
                ResetGame();
                currentPlayer = player1;
                continue;
            }

            // Switch players
            currentPlayer = (currentPlayer == player1) ? player2 : player1;
        }
    }

    // Get the player's name from the console input
    private string GetPlayerName(string defaultName, int playerNumber) // Gursharandeep Singh
    {
        string defaultPlayerName = playerNumber == 1 ? "Player 1" : "Player 2";
        Console.Write($"Enter name for {defaultName ?? defaultPlayerName}: ");
        string name = Console.ReadLine();
        return string.IsNullOrWhiteSpace(name) ? defaultName ?? defaultPlayerName : name;
    }

    // Ask the players if they want to play again
    private bool PlayAgain() // Pavleen kaur
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

    // Display game statistics
    private void DisplayStatistics() // Gursharandeep Singh
    {
        Console.WriteLine("Game Statistics:");
        Console.WriteLine($"Total Games Played: {totalGames}");
        Console.WriteLine($"{player1.Name} Wins: {player1Wins}");
        Console.WriteLine($"{player2.Name} Wins: {player2Wins}");
    }

    // Reset the game
    private void ResetGame() // Pavleen kaur
    {
        board.InitializeBoard();
        board.DisplayBoard();
    }
}

// Main program  // Pavleen kaur and Gursharandeep Singh
class Program
{
    static void Main(string[] args)
    {
        StartNewGame();
    }

    static void StartNewGame()
    {
        Console.Clear();
        Console.WriteLine("".PadLeft((Console.WindowWidth - "LET'S PLAY CONNECT4GAME".Length) / 2) + "LET'S PLAY CONNECT4GAME");
        Console.WriteLine();
        Console.WriteLine("Choose the type of game:");
        Console.WriteLine("1. Player vs Player");
        Console.WriteLine("2. Player vs AI ");
        Console.Write("Enter your choice: ");

        int choice;
        // Get user input for game mode
        while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
            Console.Write("Enter your choice: ");
        }
        // Create players based on user choice
        IPlayer player1 = new HumanPlayer { Symbol = 'X' };
        IPlayer player2 = choice == 1 ? new HumanPlayer { Symbol = 'O' } : new AIPlayer { Symbol = 'O' };

        // Initialize game manager with players
        GameManager game = new GameManager(player1, player2);
        game.StartGame();

        // After the game ends, ask if the user wants to restart from the beginning
        bool validChoice = false;
        while (!validChoice)
        {
            Console.WriteLine("Do you want to restart from the beginning? (Y/N): ");
            string restartChoice = Console.ReadLine().Trim().ToUpper();
            if (restartChoice == "Y")
            {
                validChoice = true;
                StartNewGame(); // Restart the game
            }
            else if (restartChoice == "N")
            {
                validChoice = true;
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter 'Y' to restart or 'N' to exit.");
            }
        }
    }
}

