namespace SwephCalc.Test;

internal class SunBotAgeCorrectionTest
{
    [Test]
    [TestCaseSource(nameof(AzimuthCorrectionCases))]
    public void GetSunBotAgeCorrection(AgeCorrectionCase @case)
    {
        var calc = new AzimuthCalculator();
        var rezult = calc.GetSunBotAgeCorrection(@case.Latitude, @case.Azimuth, @case.Purpose);
        rezult.Should().BeApproximately(@case.ExpectedResult, 0.05);
    }

    public class AgeCorrectionCase
    {
        public double Latitude { get; set; }

        public int Purpose { get; set; }

        public double Azimuth { get; set; }

        public double ExpectedResult { get; set; }

        public AgeCorrectionCase(double latitude, int purpose, double azimuth, double expectedResult)
        {
            Latitude = latitude;
            Azimuth = azimuth;
            Purpose = purpose;
            ExpectedResult = expectedResult;
        }

        public override string? ToString()
        {
            var stp = Purpose == SwephExp.SE_CALC_RISE ? "Rise" : "Set";
            return $"Azimuth: {Azimuth}; Latitude: {Latitude}; Purpose: {stp}; ExpectedResult: {ExpectedResult}";
        }
    }

    private static readonly AgeCorrectionCase[] AzimuthCorrectionCases = new AgeCorrectionCase[]
    {
        new (80, SwephExp.SE_CALC_RISE, 90, 3),
        new (75, SwephExp.SE_CALC_RISE, 90, 2),
        new (70, SwephExp.SE_CALC_RISE, 90, 1.5),
        new (65, SwephExp.SE_CALC_RISE, 90, 1.1),
        new (60, SwephExp.SE_CALC_RISE, 90, 0.9),
        new (50, SwephExp.SE_CALC_RISE, 90, 0.6),
        new (40, SwephExp.SE_CALC_RISE, 90, 0.4),
        new (30, SwephExp.SE_CALC_RISE, 90, 0.3),
        new (20, SwephExp.SE_CALC_RISE, 90, 0.2),
        new (10, SwephExp.SE_CALC_RISE, 90, 0.1),

        new (80, SwephExp.SE_CALC_SET, 270, -3),
        new (75, SwephExp.SE_CALC_SET, 270, -2),
        new (70, SwephExp.SE_CALC_SET, 270, -1.5),
        new (65, SwephExp.SE_CALC_SET, 270, -1.1),
        new (60, SwephExp.SE_CALC_SET, 270, -0.9),
        new (50, SwephExp.SE_CALC_SET, 270, -0.6),
        new (40, SwephExp.SE_CALC_SET, 270, -0.4),
        new (30, SwephExp.SE_CALC_SET, 270, -0.3),
        new (20, SwephExp.SE_CALC_SET, 270, -0.2),
        new (10, SwephExp.SE_CALC_SET, 270, -0.1),

        new (80, SwephExp.SE_CALC_RISE, 60, 3.5),
        new (75, SwephExp.SE_CALC_RISE, 60, 2.3),
        new (70, SwephExp.SE_CALC_RISE, 60, 1.7),
        new (65, SwephExp.SE_CALC_RISE, 60, 1.3),
        new (60, SwephExp.SE_CALC_RISE, 60, 1.1),
        new (50, SwephExp.SE_CALC_RISE, 60, 0.7),
        new (40, SwephExp.SE_CALC_RISE, 60, 0.5),
        new (30, SwephExp.SE_CALC_RISE, 60, 0.4),
        new (20, SwephExp.SE_CALC_RISE, 60, 0.2),
        new (10, SwephExp.SE_CALC_RISE, 60, 0.1),

        new (80, SwephExp.SE_CALC_SET, 300, -3.5),
        new (75, SwephExp.SE_CALC_SET, 300, -2.3),
        new (70, SwephExp.SE_CALC_SET, 300, -1.7),
        new (65, SwephExp.SE_CALC_SET, 300, -1.3),
        new (60, SwephExp.SE_CALC_SET, 300, -1.1),
        new (50, SwephExp.SE_CALC_SET, 300, -0.7),
        new (40, SwephExp.SE_CALC_SET, 300, -0.5),
        new (30, SwephExp.SE_CALC_SET, 300, -0.4),
        new (20, SwephExp.SE_CALC_SET, 300, -0.2),
        new (10, SwephExp.SE_CALC_SET, 300, -0.1),
    };
}
