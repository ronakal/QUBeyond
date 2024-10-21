using System;
using System.Collections.Generic;

namespace WordFinderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example matrix
            var matrix = new List<string>
            {
                "abcdc",
                "fgwio",
                "chill",
                "pqnsd",
                "uvdxy"
            };

            // Example word stream
            var wordStream = new List<string> { "chill", "cold", "wind", "snow","cold"};

            // Instantiate WordFinder with the matrix
            var wordFinder = new WordFinder(matrix);

            // Find the top 10 words from the word stream in the matrix
            var foundWords = wordFinder.Find(wordStream);

            // Output the result
            Console.WriteLine("Found Words:");
            foreach (var word in foundWords)
            {
                Console.WriteLine(word);
            }
        }
    }

    public class WordFinder
    {
        private readonly string[] matrix;
        private readonly int rows;
        private readonly int cols;

        // Constructor 
        public WordFinder(IEnumerable<string> matrix)
        {
            // Validate matrix input
            if (matrix == null || !matrix.Any() || matrix.Any(row => string.IsNullOrEmpty(row)))
            {
                throw new ArgumentException("Matrix cannot be null, empty, or contain empty rows.");
            }

            this.matrix = matrix.ToArray();
            this.rows = this.matrix.Length;
            this.cols = this.matrix[0].Length;

            // Ensure all rows are of the same length
            if (this.matrix.Any(row => row.Length != this.cols))
            {
                throw new ArgumentException("All rows in the matrix must have the same length.");
            }
        }

        // Method to find the top 10 most repeated words in the matrix from the word stream
        public IEnumerable<string> Find(IEnumerable<string> wordStream)
        {
             // Validate wordStream input
            if (wordStream == null || !wordStream.Any())
            {
                return Enumerable.Empty<string>();  // Return an empty result set
            }
            var wordFrequency = new Dictionary<string, int>();
            var uniqueWords = new HashSet<string>(wordStream);  

            foreach (var word in uniqueWords)
            {
                if (SearchMatrix(word))
                {
                    if (wordFrequency.ContainsKey(word))
                        wordFrequency[word]++;
                    else
                        wordFrequency[word] = 1;
                }
            }

            // Return top 10 most frequent words
            return wordFrequency.OrderByDescending(w => w.Value)
                                .Take(10)
                                .Select(w => w.Key);
        }

        // Helper method to search the matrix for a word horizontally and vertically
        private bool SearchMatrix(string word)
        {
            return SearchHorizontally(word) || SearchVertically(word);
        }

        // Search for the word horizontally (left to right)
        private bool SearchHorizontally(string word)
        {
            foreach (var row in matrix)
            {
                if (row.Contains(word))
                    return true;
            }
            return false;
        }

        // Search for the word vertically (top to bottom)
       private bool SearchVertically(string word)
        {
            // Check for each column
            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row <= rows - word.Length; row++)  // Ensure we don't overrun the matrix
                {
                    bool found = true;
                    for (int k = 0; k < word.Length; k++)
                    {
                        if (matrix[row + k][col] != word[k])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                        return true;
                }
            }
            return false;
        }
    }
}
