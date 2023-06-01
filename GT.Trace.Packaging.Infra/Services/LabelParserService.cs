using GT.Trace.Packaging.App.Services;
using GT.Trace.Packaging.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GT.Trace.Packaging.Infra.Services
{
    public sealed record LabelParserService(ILogger<LabelParserService> Logger, IConfigurationRoot Configuration) : ILabelParserService
    {
        private const string LabelFormatRegExPatternsSectionName = "LabelFormatRegExPatterns";

        private const string RiderLabelFormatRegExPatternName = "Rider";

        private const string WalkBehindLabelFormatRegExPatternName = "WalkBehind";

        private const string FramelessLabelFormatRegExPatternName = "PMDCMotor700"; //se agrego nuevo

        private const string EZMotorsLabelFormatRegExPatternName = "MotorsEZ2000"; //se agrego nuevo

        public const string InformationSeparatorThree = "\u001d";

        public const string EndOfTransmission = "\u0004";

        private static string RiderLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{RiderLabelFormatRegExPatternName}";
        private static string WalkBehindLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{WalkBehindLabelFormatRegExPatternName}";
        private static string FramelessLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{FramelessLabelFormatRegExPatternName}";
        private static string EZMotorsLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{EZMotorsLabelFormatRegExPatternName}";


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
                labelData = new Label(id, new Part("", Revision.New(""), ""), "", GetJulianDay());
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
                    new Part(
                        match.Groups["partNo"].Value,
                        Revision.New(match.Groups["partRev"].Value),
                        ""),
                    match.Groups["clientPartNo"].Value,
                    match.Groups["julianDay"].Value);
            }
            else
            {
                labelData = null;
            }
            return labelData != null;
        }
    }
}