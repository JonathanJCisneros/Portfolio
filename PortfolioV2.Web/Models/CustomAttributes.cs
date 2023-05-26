namespace PortfolioV2.Web.Models
{
    public class CustomAttributes
    {
        public static string FormatWord(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim().ToLower();
                value = char.ToUpper((char)(value[0])) + value[1..];
            }

            return value;
        }

        public static string FormatString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim().ToLower();
            }

            return value;
        }

        public static string FormatFull(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] words = value.Trim().Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = FormatWord(words[i]);
                }

                value = string.Join(" ", words);
            }

            return value;
        }
    }
}
