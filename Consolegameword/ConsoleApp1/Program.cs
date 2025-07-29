using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static readonly HashSet<string> usedWords = new();
    static readonly HttpClient client = new();
    static int maxTurns = 25;

    static async Task Main()
    {
        Console.WriteLine("=== Word Battle Game ===");

        Console.Write("Enter Player 1 name: ");
        string player1 = Console.ReadLine();

        Console.Write("Enter Player 2 name: ");
        string player2 = Console.ReadLine();

        var players = new[] { player1, player2 };
        var scores = new Dictionary<string, int> { [player1] = 0, [player2] = 0 };

        // Randomly choose starting player
        Random rand = new();
        int currentPlayerIndex = rand.Next(0, 2);
        Console.WriteLine($"{players[currentPlayerIndex]} starts and must enter a word starting with 'A'.");

        for (int turn = 1; turn <= maxTurns; turn++)
        {
            string currentPlayer = players[currentPlayerIndex];
            Console.WriteLine($"\nTurn {turn}: {currentPlayer}'s turn");

            Console.Write("Enter a word: ");
            string word = Console.ReadLine().Trim().ToLower();

            // First word must start with 'a'
            if (turn == 1 && word[0] != 'a')
            {
                Console.WriteLine($"{currentPlayer} didn't start with 'A'. {players[1 - currentPlayerIndex]} wins!");
                return;
            }

            if (usedWords.Contains(word))
            {
                Console.WriteLine($"'{word}' was already used. {players[1 - currentPlayerIndex]} wins!");
                return;
            }

            bool valid = await IsValidWord(word);
            if (!valid)
            {
                Console.WriteLine($"'{word}' is not a valid dictionary word. {players[1 - currentPlayerIndex]} wins!");
                return;
            }

            usedWords.Add(word);
            scores[currentPlayer] += word.Length;

            // Switch player
            currentPlayerIndex = 1 - currentPlayerIndex;
        }

        // Game over after 25 turns
        Console.WriteLine("\n=== Game Over ===");
        Console.WriteLine($"{player1} Score: {scores[player1]} letters");
        Console.WriteLine($"{player2} Score: {scores[player2]} letters");

        if (scores[player1] > scores[player2])
            Console.WriteLine($"{player1} wins!");
        else if (scores[player2] > scores[player1])
            Console.WriteLine($"{player2} wins!");
        else
            Console.WriteLine("It's a tie!");
    }

    static async Task<bool> IsValidWord(string word)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
