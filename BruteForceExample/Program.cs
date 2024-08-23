using System.Text.RegularExpressions;

namespace BruteForceExample
{
    internal class Program
    {
        static int maxAttempts = 10;
        static int attemptCount = 0;
        static int lockoutTime = 30000; // Lockout-tid i millisekunder (30 sekunder)
        //static LockoutManager lockoutManager = new LockoutManager(5, TimeSpan.FromMinutes(1)); // 5 försök, 1 minut lockout

        static void Main(string[] args)
        {
            string password = "";

            do
            {
                Console.Write("Lösenord (Minst 6 tecken. Lösenordet måste innehålla minst en stor bokstav, minst ett nummer och minst ett specialtecken): ");
                password = Console.ReadLine();
            } while (!IsPasswordValid(password));

            Console.WriteLine("Konfiguration av säkerhetsfrågor:");
            Console.WriteLine("Vad är namnet på ditt första husdjur?");
            string firstAnimal = Console.ReadLine();

            String current = "";

            int[] pos = { 0, 0, 0, 0, 0, 0 };

            String[] alphabet = { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "å", "ä", "ö" };

            int count = 0;

            while (!current.Equals(password))
            {
                if (attemptCount >= maxAttempts)
                {
                    Lockout();
                }
                //if (lockoutManager.IsLockedOut())
                //{
                //    Console.WriteLine("För många misslyckade försök. Du är nu låst ute.");
                //    return;
                //}
                for (int i = 0; i < pos.Length; i++)
                {
                    if (pos[i] == alphabet.Length)
                    {
                        pos[i] = 0;
                        pos[i + 1]++;
                    }
                }

                current = (alphabet[pos[5]] + alphabet[pos[4]] + alphabet[pos[3]] + alphabet[pos[2]] + alphabet[pos[1]] + alphabet[pos[0]]).ToString();

                if (count % 100 == 0) Console.WriteLine(current);
                pos[0]++;
                count++;

                //lockoutManager.RegisterFailedAttempt();
                attemptCount++;
            }

            Console.Clear();
            Console.WriteLine($"Hittat password: {current}");
            Console.WriteLine("Vänligen mata in svaret på följande säkerhetsfråga\nVad är namnet på ditt första husdjur?");
            string securityQuestionAnswer = Console.ReadLine();
            if (!securityQuestionAnswer.Equals(firstAnimal))
            {
                Console.Clear();
                Console.WriteLine("Felaktigt svar!\nProgrammet stängs nu av");
                System.Environment.Exit(3000);
            }
            Console.WriteLine("Inloggning lyckades!");

        }

        static void Lockout()
        {
            Console.Clear();
            Console.WriteLine($"För många misslyckade försök. Du är nu låst ute i {lockoutTime / 1000} sekunder.");
            Thread.Sleep(lockoutTime); // Pausa programmet under lockout-tiden
            attemptCount = 0; // Återställ räknaren efter lockout-perioden
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