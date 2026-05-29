using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace POE_Part2
{//start of namesace
    public class AI_Check
    {//start of class


        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();

        public AI_Check(ArrayList replies, ArrayList ignores)
        {//start of constructor
            reply = replies;
            ignore = ignores;
        }//end of constructor

        public string ai_check(string questions, string username)
        {
            if (string.IsNullOrWhiteSpace(questions))
            {
                return "Please enter a valid question.";
            }

            string[] words = questions.ToLower().Split(
                new char[] { ' ', ',', '.', '?', '!', ';', ':' },
                StringSplitOptions.RemoveEmptyEntries);

            bool found = false;

            string message = string.Empty;

            Random indexer = new Random();

            List<string> per_word = new List<string>();

            List<string> answers_found = new List<string>();

            string sentimentMessage = "";
            string topicMessage = "";

            foreach (string word in words)
            {
                if (word == "confused" ||
                    word == "frustrated" ||
                    word == "worried" ||
                    word == "angry" ||
                    word == "sad" ||
                    word == "happy")
                {
                    foreach (string answer in reply)
                    {
                        if (answer.ToLower().StartsWith(word))
                        {
                            sentimentMessage = answer.Substring(word.Length).Trim();
                            break;
                        }
                    }
                }
            }
            foreach (string word in words)
            {
                if (word.Length < 3 || ignore.Contains(word.ToLower()))
                    continue;

                per_word.Clear();

                bool wordFound = false;

                foreach (string answer in reply)
                {
                    string keyword =
    answer.ToLower().Split(' ')[0];

                    if (keyword == word)
                    {
                        wordFound = true;

                        per_word.Add(answer);
                    }
                }

                if (wordFound && per_word.Count > 0)
                {
                    found = true;

                    int indexing = indexer.Next(0, per_word.Count);

                    answers_found.Add(per_word[indexing]);
                }
            }

            if (found && answers_found.Count > 0)
            {
                answers_found = answers_found.Distinct().ToList();

                if (answers_found.Count > 0)
                {

                    topicMessage = answers_found[0];

                    int firstSpace = topicMessage.IndexOf(' ');

                    if (firstSpace >= 0) {

                        topicMessage = topicMessage.Substring(firstSpace + 1);

                    }

                }

                string finalMessage = "";

                if (!string.IsNullOrWhiteSpace(sentimentMessage))
                {
                    finalMessage += sentimentMessage + "\n";
                }

                if (!string.IsNullOrWhiteSpace(topicMessage)) {
                    finalMessage += topicMessage;
                }
                return finalMessage.Trim();
            }
            else
            {
                string[] fallbackMessages =
                {
                    "I'm sorry, I don't understand that. Could you rephrase your question?",
                    "I didn't quite get that. Try asking about cyber security topics.",
                    "Hmm, I'm not sure how to respond to that.",
                    "Please ask about cyber security, technology or programming.",
                    "I do not have information on that topic yet."
                };

                Random random = new Random();

                return fallbackMessages[random.Next(fallbackMessages.Length)];
            }
        }


    }//end of class
}//end of namspace
