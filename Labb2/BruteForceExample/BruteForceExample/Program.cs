using System.Text.RegularExpressions;

namespace BruteForceExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbHandler.InitDB();

            string password = "";

            do
            {
                Console.Write("Lösenord (Minst 6 tecken. Lösenordet måste innehålla minst en stor bokstav, minst ett nummer och minst ett specialtecken): ");
                password = Console.ReadLine();
            } while (!IsPasswordValid(password));

            DbHandler.InsertDB(password);

            String current = "";

            int[] pos = { 0, 0, 0, 0, 0, 0 };

            String[] alphabet = {"", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "å", "ä", "ö" };

            int count = 0;

            while (!DbHandler.CheckPasswordInDB(current, "hackerID"))
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (pos[i] == alphabet.Length)
                    {
                        pos[i] = 0;
                        pos[i + 1]++;
                    }
                }

                current = (alphabet[pos[5]] + alphabet[pos[4]] + alphabet[pos[3]] + alphabet[pos[2]] + alphabet[pos[1]] + alphabet[pos[0]]).ToString();

                Console.WriteLine(current);
                pos[0]++;
                count++;
            }

            Console.WriteLine($"Hittat password: {current}");
        }

        static bool IsPasswordValid(string password)
        {
            if (password.Length < 6)
            {
                Console.WriteLine("Lösenordet måste vara minst 6 tecken långt.");
                return false;
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                Console.WriteLine("Lösenordet måste innehålla minst en stor bokstav.");
                return false;
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                Console.WriteLine("Lösenordet måste innehålla minst en siffra.");
                return false;
            }
            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                Console.WriteLine("Lösenordet måste innehålla minst ett specialtecken.");
                return false;
            }
            return true;
        }

    }
}