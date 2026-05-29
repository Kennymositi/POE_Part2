using System.Text;
using System.Text.RegularExpressions;

namespace POE_Part2
{//start of namespace
    public class Clean_input
    {//start of class

        public string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            StringBuilder sanitized = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c)
                    || char.IsWhiteSpace(c)
                    || c == '\''
                    || c == '-')
                {
                    sanitized.Append(c);
                }
                else
                {
                    sanitized.Append(' ');
                }
            }

            string result = sanitized.ToString();

            result = Regex.Replace(result, @"\s+", " ").Trim();

            return result;
        }




    }//end  of clas
}//end of namespace
