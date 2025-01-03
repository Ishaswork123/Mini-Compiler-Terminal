using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WinFormsApp3
{
    public partial class Form1 : Form
    {
        private List<string> invalidUsernames = new List<string>();
        private int totalUsernames = 0;
        private int validUsernames = 0;
        private int invalidUsernamesCount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private string ProcessUsername(string username, int number)
        {

            string result = $"{number}. {username} - ";

            // Rule 1: Username must start with a letter
            if (!Regex.IsMatch(username, "^[a-zA-Z]"))
            {
                result += "Invalid (Username must start with a letter)";
                return result;
            }

            // Rule 2: Username can only contain letters, digits, and underscores
            if (!Regex.IsMatch(username, "^[a-zA-Z0-9_]+$"))
            {
                result += "Invalid (Username can only contain letters, digits, and underscores)";
                return result;
            }

            // Rule 3: Length must be between 5 and 15 characters
            if (username.Length < 5 || username.Length > 15)
            {
                result += "Invalid (Username length must be between 5 and 15 characters)";
                return result;
            }

            // If all rules are valid, show additional info
            int uppercaseCount = Regex.Matches(username, "[A-Z]").Count;
            int lowercaseCount = Regex.Matches(username, "[a-z]").Count;
            int digitCount = Regex.Matches(username, "[0-9]").Count;
            int underscoreCount = Regex.Matches(username, "_").Count;

            result += $"Valid\n   Letters: {uppercaseCount + lowercaseCount} (Uppercase: {uppercaseCount}, Lowercase: {lowercaseCount}), Digits: {digitCount}, Underscores: {underscoreCount}\n";

            return result;
        }

    

    private string GenerateSecurePassword()
    {

        const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string digitChars = "0123456789";
        const string specialChars = "!@#$%^&*";

        List<char> passwordChars = new List<char>();

        // Ensure at least 2 of each character type
        passwordChars.AddRange(GetRandomChars(uppercaseChars, 2));
        passwordChars.AddRange(GetRandomChars(lowercaseChars, 2));
        passwordChars.AddRange(GetRandomChars(digitChars, 2));
        passwordChars.AddRange(GetRandomChars(specialChars, 2));

        // Fill remaining space with random characters
        var allChars = uppercaseChars + lowercaseChars + digitChars + specialChars;
        while (passwordChars.Count < 12)
        {
            passwordChars.Add(GetRandomChar(allChars));
        }

        // Shuffle to randomize
        return new string(passwordChars.OrderBy(x => Guid.NewGuid()).ToArray());
    }

    private List<char> GetRandomChars(string charSet, int count)
    {
        List<char> chars = new List<char>();

        for (int i = 0; i < count; i++)
        {
            chars.Add(GetRandomChar(charSet));
        }

        return chars;
    }

    private char GetRandomChar(string charSet)
    {
        Random random = new Random();
        int index = random.Next(charSet.Length);
        return charSet[index];

    }

    private string CheckPasswordStrength(string password)
    {
        int score = 0;
        if (password.Length >= 12) score++;
        if (Regex.IsMatch(password, "[A-Z]")) score++;
        if (Regex.IsMatch(password, "[a-z]")) score++;
        if (Regex.IsMatch(password, "[0-9]")) score++;
        if (Regex.IsMatch(password, "[!@#$%^&*]")) score++;

        if (score <= 2) return "Weak";
        if (score <= 4) return "Medium";
        return "Strong";
    }
                                                                                                                  
    private void SaveResultsToFile(List<string> results)
    {
        try
        {
            File.WriteAllLines("UserDetails.txt", results);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving to file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }



private void button1_Click(object sender, EventArgs e)
        {
            {
                ProcessUsernames();
            }

        }
        private void ProcessUsernames()
        {
            string input = richTextBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                richTextBox3.AppendText("Please enter usernames before submitting.\n");
                return;
            }

            string[] usernames = input.Split(',').Select(u => u.Trim()).ToArray();
            int validUsernamesCount = 0;
            int invalidUsernamesCount = 0;
            List<string> results = new List<string>();

            foreach (string username in usernames)
            {
                string result = ProcessUsername(username, results.Count + 1);
                if (result.Contains("Valid"))
                {
                    validUsernamesCount++;
                    string password = GenerateSecurePassword();
                    result += $"   Generated Password: {password} (Strength: {CheckPasswordStrength(password)})\n";
                }
                else
                {
                    invalidUsernamesCount++;
                }
                results.Add(result);
            }

            // Display summary
            results.Add("\nSummary:");
            results.Add($"- Total Usernames: {usernames.Length}");
            results.Add($"- Valid Usernames: {validUsernamesCount}");
            results.Add($"- Invalid Usernames: {invalidUsernamesCount}");

            richTextBox3.AppendText(string.Join("\n", results) + "\n");

            // Optionally, save results to a file
            SaveResultsToFile(results);
        }
    }
}
    
   
