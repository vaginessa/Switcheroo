using System.Text.RegularExpressions;
using System.Text;
using NPinyin;

namespace Switcheroo.Core.Matchers
{
    public class IndividualCharactersMatcher : IMatcher
    {
        public MatchResult Evaluate(string input, string pattern)
        {
            if (input == null || pattern == null)
            {
                return NonMatchResult(input);
            }

            var regexPattern = BuildRegexPattern(pattern);
            var input1 = PinYinFirstLetter(input);
            var match = Regex.Match(input1, regexPattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return NonMatchResult(input);
            }
            var matchResult = new MatchResult();
            int groupTextStartPos = 0;
            for (var groupIndex = 1; groupIndex < match.Groups.Count; groupIndex++)
            {
                var group = match.Groups[groupIndex];
                int ThisGroupTextLen = group.Value.Length;
                if (ThisGroupTextLen > 0)
                {
                    //matchResult.StringParts.Add(new StringPart(group.Value, groupIndex%2 == 0));

                    //valuex: get corresponding chinese charater by indexing it's position in converted PinYin strings.
                    string a = input.Substring(groupTextStartPos, group.Value.Length);
                    matchResult.StringParts.Add(new StringPart(a, groupIndex % 2 == 0)); 
                    groupTextStartPos = groupTextStartPos + ThisGroupTextLen ;

                }
            }

            matchResult.Matched = true;
            matchResult.Score = 1;

            return matchResult;
        }
        private string PinYinFirstLetter(string strInput)
        {
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            string s = Pinyin.ConvertEncoding(strInput, Encoding.UTF8, gb2312);
            return Pinyin.GetInitials(s, gb2312);
        }
        private static string BuildRegexPattern(string pattern)
        {
            var regexPattern = "";
            char? previousChar = null;
            foreach (var p in pattern)
            {
                if (previousChar != null)
                {
                    regexPattern += string.Format("([^{0}]*?)({1})", Regex.Escape(previousChar + ""),
                        Regex.Escape(p + ""));
                }
                else
                {
                    regexPattern += string.Format("(.*?)({0})", Regex.Escape(p + ""));
                }
                previousChar = p;
            }
            return regexPattern + "(.*)";
        }

        private static MatchResult NonMatchResult(string input)
        {
            var matchResult = new MatchResult();
            if (input != null)
            {
                matchResult.StringParts.Add(new StringPart(input));
            }
            return matchResult;
        }
    }
}