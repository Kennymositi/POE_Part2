using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace POE_Part2
{//start of namespace
    public class Interest_handler
    {//start of class


        public string save_interest(
            string[] words,
            ArrayList ignore,
            string username)
        {
            string store_interests = string.Empty;

            bool found_interest = false;

            HashSet<string> currentInterests = new HashSet<string>();

            foreach (string interest in words)
            {
                string clean = interest.ToLower().Trim();

                clean = Regex.Replace(clean, @"[^a-zA-Z0-9\s]", "");

                if (!ignore.Contains(clean)
                    && clean != "interested"
                    && clean != "and"
                    && clean != "in"
                    && clean.Length >= 3)
                {
                    found_interest = true;

                    currentInterests.Add(clean);
                }
            }

            store_interests = string.Join(", ", currentInterests);

            if (found_interest && !string.IsNullOrWhiteSpace(store_interests))
            {
                string filename = "interested_topic.txt";

                bool userFound = false;

                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith(username))
                        {
                            userFound = true;

                            string existing = lines[i]
                                .Replace(username + " interested in:", "")
                                .ToLower();

                            HashSet<string> existingSet =
                                new HashSet<string>(
                                existing.Split(',')
                                .Select(x => x.Trim())
                                .Where(x => x != "")
                                );

                            foreach (string item in currentInterests)
                            {
                                existingSet.Add(item);
                            }

                            string finalList =
                                string.Join(", ", existingSet);

                            lines[i] =
                                username +
                                " interested in: " +
                                finalList;

                            File.WriteAllLines(filename, lines);

                            return "Great, I added "
                                + store_interests +
                                " to your interests.";
                        }
                    }
                }

                if (!userFound)
                {
                    File.AppendAllText(
                        filename,
                        username +
                        " interested in: " +
                        store_interests + "\n"
                    );

                    return "Great, I will remember that you are interested in "
                        + store_interests;
                }
            }

            return "Please specify your interests.";
        }

        public string reminder(string username)
        {
            string filename = "interested_topic.txt";

            if (File.Exists(filename))
            {
                string[] lines = File.ReadAllLines(filename);

                foreach (string line in lines)
                {
                    if (line.StartsWith(username))
                    {
                        int colonIndex =
                            line.IndexOf("interested in:");

                        if (colonIndex >= 0)
                        {
                            string interests =
                                line.Substring(colonIndex + 14).Trim();

                            return "Just a reminder, you are interested in "
                                + interests;
                        }
                    }
                }
            }

            return "";
        }


    }//end of class
}//end of namespace
