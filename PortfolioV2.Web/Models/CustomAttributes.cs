namespace PortfolioV2.Web.Models
{
    public static class CustomAttributes
    {
        public static string FormatWord(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                value = value.Trim().ToLower();
                value = char.ToUpper(value[0]) + value[1..];
            }

            return value;
        }

        public static string FormatString(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                value = value.Trim().ToLower();
            }

            return value;
        }

        public static string FormatFull(this string value)
        {
            if (!String.IsNullOrEmpty(value))
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

        public static string TrimString(this string value)
        {
            if (!String.IsNullOrEmpty (value))
            {
                value = value.Trim();
            }

            return value;
        }
    }
}