using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExeptionsTasks
{
    public class Program
    {

        public static void Main()
        {
            try
            {
                string filePath = AskForExistingFile();

                List<VideoGame> games = ReadGamesFromJson(filePath);

                PrintGames(games);
            }
            catch (Exception ex)
            {
                Console.WriteLine(AppConstants.Fatal);
                LogException(ex);
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine(AppConstants.PressAnyKey);
                Console.ReadKey(intercept: true);
            }
        }

        static string AskForExistingFile()
        {
            while (true)
            {
                Console.WriteLine(AppConstants.PromptFileName);
                string? input = Console.ReadLine();

                if (input is null)
                {
                    Console.WriteLine(AppConstants.NullInput);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine(AppConstants.EmptyInput);
                    continue;
                }

                if (!File.Exists(input))
                {
                    Console.WriteLine(AppConstants.NotFound);
                    continue;
                }

                return input;
            }
        }

        static List<VideoGame> ReadGamesFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            try
            {
                var games = JsonSerializer.Deserialize<List<VideoGame>>(json, options);
                return games ?? new List<VideoGame>();
            }
            catch (JsonException ex)
            {
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format(AppConstants.InvalidJsonPrefix, filePath));
                Console.WriteLine(json);
                Console.ForegroundColor = prevColor;

                Console.WriteLine();
                Console.WriteLine(AppConstants.Fatal);

                LogException(ex);
                throw;
            }
        }

        static void PrintGames(List<VideoGame> games)
        {
            if (games is null || games.Count == 0)
            {
                Console.WriteLine(AppConstants.NoGames);
                return;
            }

            Console.WriteLine(AppConstants.LoadedGamesHeader);
            foreach (var g in games)
            {
                string title = string.IsNullOrWhiteSpace(g.Title) ? "<untitled>" : g.Title!;
                Console.WriteLine($"{title}, released in {g.ReleaseYear}, rating: {g.Rating:0.0}");
            }
        }

        static void LogException(Exception ex)
        {
            var line = $"[{DateTime.Now:MM/dd/yyyy h:mm:ss tt}], Exception message:{ex.Message}, Stack trace:    {ex.StackTrace}";
            try
            {
                File.AppendAllText(AppConstants.LogFile, line + Environment.NewLine + Environment.NewLine);
            }
            catch
            { 

            }
        }

    }

    public class VideoGame
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public double Rating { get; set; }
    }

    public static class AppConstants
    {

        public const string PromptFileName = "Enter the name of the file you want to read:";
        public const string NullInput = "File name cannot be null.";
        public const string EmptyInput = "File name cannot be empty.";
        public const string NotFound = "File not found.";
        public const string LoadedGamesHeader = "Loaded games are:";
        public const string NoGames = "No games are present in the input file.";
        public const string InvalidJsonPrefix = "JSON in the {0} was not in a valid format. JSON body:";
        public const string Fatal = "Sorry! The application has experienced an unexpected error and will have to be closed.";
        public const string PressAnyKey = "Press any key to close.";
        public const string LogFile = "log.txt";

    }


}
