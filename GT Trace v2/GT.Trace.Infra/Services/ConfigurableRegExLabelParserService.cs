using GT.Trace.Domain.Entities;
using GT.Trace.Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace GT.Trace.Infra.Services
{
    internal sealed record ConfigurableRegExLabelParserService(IConfigurationRoot Configuration) : ILabelParserService
    {
        private const string LabelFormatRegExPatternsSectionName = "LabelFormatRegExPatterns";

        private const string RiderLabelFormatRegExPatternName = "IndividualLabels:Rider";

        private const string WalkBehindLabelFormatRegExPatternName = "IndividualLabels:WalkBehind";

        private const string EtiLabelFormatRegExPatternName = "EtiNumber";

        public const string InformationSeparatorThree = "\u001d";

        public const string EndOfTransmission = "\u0004";

        private static string RiderLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{RiderLabelFormatRegExPatternName}";

        private static string WalkBehindLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{WalkBehindLabelFormatRegExPatternName}";

        private static string EtiLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{EtiLabelFormatRegExPatternName}";

        /// <summary>
        /// Removes separator and end of transmission characters from input.
        /// </summary>
        /// <param name="input">Scanner input.</param>
        /// <returns>The input string without the expected special characters.</returns>
        private static string ClearInputFromSpecialCharacters(string input) => input.Replace(InformationSeparatorThree, "").Replace(EndOfTransmission, "");

        private static string GetJulianDay() => $"{DateTime.Now.DayOfYear:000}";

        public static bool CheckIsRiderFormat(string value) => Regex.Match(value, RiderLabelFormatRegExPattern).Success;

        public bool TryParseRiderFormat(string value, out Label? labelData)
        {
            if (long.TryParse(ClearInputFromSpecialCharacters(value), out long id))
            {
                labelData = new Label(id, Part.Create("", Revision.New(""), null, null), "", GetJulianDay());
            }
            else
            {
                labelData = null;
            }
            return labelData != null;
        }

        public bool TryParseNewWBFormat(string value, out Label? labelData)
        {
            var match = Regex.Match(
                ClearInputFromSpecialCharacters(value),
                Configuration.GetSection(WalkBehindLabelFormatRegExPattern).Value,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                labelData = new Label(
                    long.Parse(match.Groups["transmissionID"].Value),
                    Part.Create(
                        match.Groups["partNo"].Value,
                        Revision.New(match.Groups["partRev"].Value),
                        null, null),
                    match.Groups["clientPartNo"].Value,
                    match.Groups["julianDay"].Value);
            }
            else
            {
                labelData = null;
            }
            return labelData != null;
        }

        public bool TryParseEti(string value, out long? id, out string? no)
        {
            var input = ClearInputFromSpecialCharacters(value);
            var pattern = Configuration.GetSection(EtiLabelFormatRegExPattern).Value;
            var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            var match = Regex.Match(input, pattern, options);
            if (match.Success)
            {
                id = long.Parse(match.Groups["id"].Value);
                no = match.Groups["no"].Value;
            }
            else
            {
                id = null;
                no = null;
            }
            return id.HasValue;
        }
    }
}