using GT.Trace.Packaging.App.Services;
using GT.Trace.Packaging.App.UseCases.LoadPackState.Dtos;
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

        private const string FramelessLabelFormatRegExPatternName = "FramlessMotorQR";

        private const string EZMotorsLabelFormatRegExPatternName = "EZ2000MotorsQR";

        private const string PalletLabelFormatRegExPatternName = "PalletQR";

        public const string InformationSeparatorThree = "\u001d";

        public const string EndOfTransmission = "\u0004";

        private static string RiderLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{RiderLabelFormatRegExPatternName}";
        private static string WalkBehindLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{WalkBehindLabelFormatRegExPatternName}";
        private static string FramelessLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{FramelessLabelFormatRegExPatternName}";
        private static string EZMotorsLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{EZMotorsLabelFormatRegExPatternName}";
        private static string PalletLabelFormatRegExPattern => $"{LabelFormatRegExPatternsSectionName}:{PalletLabelFormatRegExPatternName}";


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

        /// <summary>
        /// New From frameless
        /// </summary>
        /// <param name="value"> Scanner Input value</param>
        /// <param name="labelData">Out Data</param>
        /// <returns></returns>
        public bool TryParseFramelessFormat(string value, out FramelessMotorQR? labelData)
        {
            var match = Regex.Match(
                ClearInputFromSpecialCharacters(value),
                Configuration.GetSection(FramelessLabelFormatRegExPattern).Value,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                var dateTimeString = match.Groups["datetime"].Value;
                var dateTime = DateTime.ParseExact(dateTimeString, "yyMMdd-HHmmss", CultureInfo.InvariantCulture);
                labelData = new FramelessMotorQR(
                match.Groups["serial"].Value,
                match.Groups["current"].Value,
                match.Groups["rpm"].Value,
                dateTime
                    );
            }
            else
            {
                labelData = null;
            }
            return labelData != null;
        }

        public bool TryParseEZMotorsFormat(string value, out EZ2000MotorsQR? labelData)
        {
            var match = Regex.Match(
                ClearInputFromSpecialCharacters(value),
                Configuration.GetSection(EZMotorsLabelFormatRegExPattern).Value,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                labelData = new EZ2000MotorsQR(
                match.Groups["website"].Value,
                match.Groups["voltage"].Value,
                match.Groups["rpm"].Value,
                match.Groups["date"].Value,
                match.Groups["time"].Value,
                match.Groups["id"].Value,
                match.Groups["PN"].Value,
                match.Groups["AEM"].Value,
                ""//Reservado para la Rev
                );
            }
            else
            {
                labelData = null;
            }
            return labelData != null;
        }

        public bool TryParsePalletFormat(string value, out PalletQR? palletData)
        {
            var match = Regex.Match(
                ClearInputFromSpecialCharacters(value),
                //Configuration.GetSection(PalletLabelFormatRegExPattern).Value,
                "^[LM][A-Z0-9]{5}$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                //palletData = new PalletQR(
                //match.Groups["letraInicial"].Value);
                palletData = new PalletQR(match.Value);
            }
            else
            {
                palletData = null;
            }
            return palletData != null;
        }
    }
}