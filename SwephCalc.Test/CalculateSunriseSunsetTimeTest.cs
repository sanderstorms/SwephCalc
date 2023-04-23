namespace SwephCalc.Test;

internal class CalculateSunriseSunsetTimeTest
{
    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void CalculateSunriseSunsetTime(TestCase @case)
    {
        var calc = new AzimuthCalculator();
        var result = calc.CalculateSunriseSunsetTime(@case.ExpectedTime.Date, new GeoPosition
        {
            Longitude = 0,
            Latitude = @case.Latitude,
            Altitude = @case.Altitude,
        }, AstroCatalogue.StandardPressure, AstroCatalogue.StandardTemperature, @case.Purpose);

        result.Should().NotBeNull();
        result.dT.Should().BeApproximately(@case.ExpecteddT, 1);
        result.DateTime.Should().BeCloseTo(@case.ExpectedTime, TimeSpan.FromMinutes(1.5));
    }

    public class TestCase
    {
        public double Latitude { get; }

        public double Altitude { get; }

        public int Purpose { get; }

        public double ExpecteddT { get; }

        public DateTime ExpectedTime { get; }

        public TestCase(double latitude, double altitude, int purpose, double expecteddT, int y, int m, int d, int h, int min)
        {
            Latitude = latitude;
            Altitude = altitude;
            Purpose = purpose;
            ExpecteddT = expecteddT;
            ExpectedTime = new DateTime(y, m, d, h, min, 0, DateTimeKind.Utc);
        }

        public override string? ToString()
        {
            var stp = Purpose == SwephExp.SE_CALC_RISE ? "Rise" : "Set";
            return $"Latitude: {Latitude}; Altitude: {Altitude}; Purpose: {stp}; ExpectedTime: {ExpectedTime}";
        }
    }

    public static readonly TestCase[] TestCases = new TestCase[]
    {
        new(66, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 10, 26),
        new(64, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 09, 48),
        new(62, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 09, 22),
        new(60, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 09, 02),
        new(58, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 08, 45),
        new(56, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 08, 31),
        new(54, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 08, 19),
        new(52, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 08, 08),
        new(50, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 07, 58),
        new(45, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 07, 38),
        new(40, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 07, 22),
        new(30, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 06, 56),
        new(20, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 06, 35),
        new(10, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 06, 17),
        new(00, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 06, 00),
        new(-10, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 05, 43),
        new(-20, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 05, 25),
        new(-30, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 05, 03),
        new(-40, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 04, 36),
        new(-45, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 04, 18),
        new(-50, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 03, 56),
        new(-52, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 03, 46),
        new(-54, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 03, 34),
        new(-56, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 03, 20),
        new(-58, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 03, 04),
        new(-60, 0, SwephExp.SE_CALC_RISE, 0, 2018, 01, 01, 02, 44),

        //

         new(00, 40, SwephExp.SE_CALC_RISE, -0.8, 2018, 01, 01, 05, 59),
    };
}
