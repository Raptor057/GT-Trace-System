using GT.Trace.Etis.Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace GT.Trace.Etis.Infra.Services
{
    internal sealed record ConfigurableRegExEtiParserService(IConfigurationRoot Configuration) : IEtiParserService
    {
        private const string LabelFormatRegExPatternsSectionName = "LabelFormatRegExPatterns";

        private const string EtiLabelFormatRegExPatternName = "EtiNumber";

        public const string InformationSeparatorThree = "\u001d";

        public const string EndOfTransmission = "\u0004";

        private static string EtiLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{EtiLabelFormatRegExPatternName}";

        /// <summary>
        /// Removes separator and end of transmission characters from input.
        /// </summary>
        /// <param name="input">Scanner input.</param>
        /// <returns>The input string without the expected special characters.</returns>
        private static string ClearInputFromSpecialCharacters(string input) => input.Replace(InformationSeparatorThree, "").Replace(EndOfTransmission, "");

        public bool TryParseEti(string value, out long id, out string no)
        {
            var input = ClearInputFromSpecialCharacters(value);
            var pattern = Configuration.GetSection(EtiLabelFormatRegExPattern).Value;
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            var match = Regex.Match(input, pattern, options);
            if (match.Success)
            {
                id = long.Parse(match.Groups["id"].Value);
                no = match.Groups["no"].Value;
            }
            else
            {
                id = 0;
                no = "";
            }
            return id > 0;
        }
    }
}