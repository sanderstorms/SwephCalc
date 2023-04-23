namespace SwephCalc.Test;

internal class AzimuthCalculatorTest
{
    [Test]
    [TestCaseSource(nameof(AzimuthTestCases))]
    public void GetSunriseAzimuthForTopSunAge(AzimuthTestCase @case)
    {
        var calc = new AzimuthCalculator();
        var result = calc.GetSunriseSunsetAzimuthAndTime(@case.Date,
            @case.Position, AstroCatalogue.StandardPressure, AstroCatalogue.StandardTemperature, @case.Purpose);

        result.Should().NotBeNull();
        result.Coordinates.Should().NotBeNull();
        result.Coordinates.Azimuth.Should().BeApproximately(@case.ExpectedResult, 0.055);
    }

    [Test]
    [TestCaseSource(nameof(CalculateAzimuthTestCases))]
    public void CalculateSunriseSunsetAzimuthAndTime(CalculateAzimuthTestCase @case)
    {
        var calc = new AzimuthCalculator();
        var result = calc.CalculateSunriseSunsetAzimuth(@case.Date, @case.Position,
            @case.Pressure, @case.Temperature, @case.Purpose, @case.KP);

        result.Should().NotBeNull();
        result.At.Should().BeApproximately(@case.ExpectedResult.At, 0.05);
        result.dLatitude.Should().BeApproximately(@case.ExpectedResult.dLatitude, 0.05);
        result.dLongitude.Should().BeApproximately(@case.ExpectedResult.dLongitude, 0.05);
        result.dAltitude.Should().BeApproximately(@case.ExpectedResult.dAltitude, 0.05);
        result.dTemperature.Should().BeApproximately(@case.ExpectedResult.dTemperature, 0.05);
        result.dPressure.Should().BeApproximately(@case.ExpectedResult.dPressure, 0.05);
        result.dh.Should().BeApproximately(@case.ExpectedResult.dh, 0.05);
        result.K.Should().BeApproximately(@case.ExpectedResult.K, 0.05);
        result.dAzimuth.Should().BeApproximately(@case.ExpectedResult.dAzimuth, 0.05);
        result.AzimuthTop.Should().BeApproximately(@case.ExpectedResult.AzimuthTop, 0.05);
        result.AzimuthBot.Should().BeApproximately(@case.ExpectedResult.AzimuthBot, 0.05);
        result.dAzimuthAge.Should().BeApproximately(@case.ExpectedResult.dAzimuthAge, 0.05);
        result.KP.Should().BeApproximately(@case.ExpectedResult.KP, 0.05);
        result.dKPTop.Should().BeApproximately(@case.ExpectedResult.dKPTop, 0.05);
        result.dKPBot.Should().BeApproximately(@case.ExpectedResult.dKPBot, 0.05);
        result.Purpose.Should().Be(@case.ExpectedResult.Purpose);
    }

    public class AzimuthTestCase
    {
        public GeoPosition Position { get; set; }

        public DateTime Date { get; set; }

        public int Purpose { get; set; }

        public double ExpectedResult { get; set; }

        public AzimuthTestCase(double latitude, int year, int month, int day, int purpose, double expectedAzimuth)
        {
            Date = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            Position = new GeoPosition
            {
                Latitude = latitude,
                Longitude = 0,
                Altitude = AstroCatalogue.StandardAltitude,
            };
            Purpose = purpose;
            ExpectedResult = expectedAzimuth;
        }

        public override string? ToString()
        {
            var stp = Purpose == SwephExp.SE_CALC_RISE ? "Rise" : "Set";
            return $"f: {Position.Latitude}; Date: {Date}; Purpose: {stp}; Az: {ExpectedResult}";
        }
    }

    public class CalculateAzimuthTestCase
    {
        public DateTime Date { get; }
        public GeoPosition Position { get; }
        public double Pressure { get; }
        public double Temperature { get; }
        public int Purpose { get; }
        public double KP { get; }

        public AzimuthCalcResult ExpectedResult { get; set; }

        public CalculateAzimuthTestCase(int year, int month, int day,
            GeoPosition position, double pressure, double temperature, int purpose, double kp, AzimuthCalcResult expectedResult)
        {
            Date = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            Position = position;
            Pressure = pressure;
            Temperature = temperature;
            Purpose = purpose;
            KP = kp;
            ExpectedResult = expectedResult;
        }
        public override string? ToString()
        {
            var stp = Purpose == SwephExp.SE_CALC_RISE ? "Rise" : "Set";
            return $"Position: {Position.Latitude}/{Position.Longitude}/{Position.Altitude}; Date: {Date}; Purpose: {stp};";
        }
    }

    private static readonly AzimuthTestCase[] AzimuthTestCases = new AzimuthTestCase[]
    {
		// N, SE_CALC_RISE
		new (74, 2016, 4, 27, SwephExp.SE_CALC_RISE, 22.5),
        new (72, 2016, 4, 27, SwephExp.SE_CALC_RISE, 34.5),
        new (70, 2016, 4, 27, SwephExp.SE_CALC_RISE, 41.9),
        new (68, 2016, 4, 27, SwephExp.SE_CALC_RISE, 47.2),
        new (66, 2016, 4, 27, SwephExp.SE_CALC_RISE, 51.3),
        new (64, 2016, 4, 27, SwephExp.SE_CALC_RISE, 54.5),
        new (62, 2016, 4, 27, SwephExp.SE_CALC_RISE, 57.2),
        new (60, 2016, 4, 27, SwephExp.SE_CALC_RISE, 59.5),
        new (58, 2016, 4, 27, SwephExp.SE_CALC_RISE, 61.4),
        new (56, 2016, 4, 27, SwephExp.SE_CALC_RISE, 63.1),
        new (54, 2016, 4, 27, SwephExp.SE_CALC_RISE, 64.5),
        new (52, 2016, 4, 27, SwephExp.SE_CALC_RISE, 65.8),
        new (50, 2016, 4, 27, SwephExp.SE_CALC_RISE, 66.9),
        new (45, 2016, 4, 27, SwephExp.SE_CALC_RISE, 69.1),
        new (40, 2016, 4, 27, SwephExp.SE_CALC_RISE, 70.9),
        new (30, 2016, 4, 27, SwephExp.SE_CALC_RISE, 73.3),
        new (20, 2016, 4, 27, SwephExp.SE_CALC_RISE, 74.8),
        new (10, 2016, 4, 27, SwephExp.SE_CALC_RISE, 75.6),
        new (00, 2016, 4, 27, SwephExp.SE_CALC_RISE, 76.0),

		// S, SE_CALC_RISE
		new (-10, 2016, 4, 27, SwephExp.SE_CALC_RISE, 75.9),
        new (-20, 2016, 4, 27, SwephExp.SE_CALC_RISE, 75.4),
        new (-30, 2016, 4, 27, SwephExp.SE_CALC_RISE, 74.3),
        new (-40, 2016, 4, 27, SwephExp.SE_CALC_RISE, 72.3),
        new (-45, 2016, 4, 27, SwephExp.SE_CALC_RISE, 70.9),
        new (-50, 2016, 4, 27, SwephExp.SE_CALC_RISE, 69.0),
        new (-52, 2016, 4, 27, SwephExp.SE_CALC_RISE, 68.0),
        new (-54, 2016, 4, 27, SwephExp.SE_CALC_RISE, 67.0),
        new (-56, 2016, 4, 27, SwephExp.SE_CALC_RISE, 65.7),
        new (-58, 2016, 4, 27, SwephExp.SE_CALC_RISE, 64.3),
        new (-60, 2016, 4, 27, SwephExp.SE_CALC_RISE, 62.7),

		// N, SE_CALC_SET
		new (74, 2016, 4, 27, SwephExp.SE_CALC_SET, 340.2),
        new (72, 2016, 4, 27, SwephExp.SE_CALC_SET, 327.0),
        new (70, 2016, 4, 27, SwephExp.SE_CALC_SET, 319.2),
        new (68, 2016, 4, 27, SwephExp.SE_CALC_SET, 313.6),
        new (66, 2016, 4, 27, SwephExp.SE_CALC_SET, 309.4),
        new (64, 2016, 4, 27, SwephExp.SE_CALC_SET, 306.0),
        new (62, 2016, 4, 27, SwephExp.SE_CALC_SET, 303.3),
        new (60, 2016, 4, 27, SwephExp.SE_CALC_SET, 301.0),
        new (58, 2016, 4, 27, SwephExp.SE_CALC_SET, 299.0),
        new (56, 2016, 4, 27, SwephExp.SE_CALC_SET, 297.3),
        new (54, 2016, 4, 27, SwephExp.SE_CALC_SET, 295.9),
        new (52, 2016, 4, 27, SwephExp.SE_CALC_SET, 294.6),
        new (50, 2016, 4, 27, SwephExp.SE_CALC_SET, 293.4),
        new (45, 2016, 4, 27, SwephExp.SE_CALC_SET, 291.1),
        new (40, 2016, 4, 27, SwephExp.SE_CALC_SET, 289.4),
        new (30, 2016, 4, 27, SwephExp.SE_CALC_SET, 286.9),
        new (20, 2016, 4, 27, SwephExp.SE_CALC_SET, 285.4),
        new (10, 2016, 4, 27, SwephExp.SE_CALC_SET, 284.5),
        new (00, 2016, 4, 27, SwephExp.SE_CALC_SET, 284.1),

		// S, SE_CALC_SET
		new (-10, 2016, 4, 27, SwephExp.SE_CALC_SET, 284.2),
        new (-20, 2016, 4, 27, SwephExp.SE_CALC_SET, 284.7),
        new (-30, 2016, 4, 27, SwephExp.SE_CALC_SET, 285.9),
        new (-40, 2016, 4, 27, SwephExp.SE_CALC_SET, 287.8),
        new (-45, 2016, 4, 27, SwephExp.SE_CALC_SET, 289.3),
        new (-50, 2016, 4, 27, SwephExp.SE_CALC_SET, 291.2),
        new (-52, 2016, 4, 27, SwephExp.SE_CALC_SET, 292.2),
        new (-54, 2016, 4, 27, SwephExp.SE_CALC_SET, 293.3),
        new (-56, 2016, 4, 27, SwephExp.SE_CALC_SET, 294.5),
        new (-58, 2016, 4, 27, SwephExp.SE_CALC_SET, 295.9),
        new (-60, 2016, 4, 27, SwephExp.SE_CALC_SET, 297.6),

		// N, SE_CALC_RISE
		new (74, 2016, 4, 28, SwephExp.SE_CALC_RISE, 19.4),
        new (72, 2016, 4, 28, SwephExp.SE_CALC_RISE, 32.7),
        new (70, 2016, 4, 28, SwephExp.SE_CALC_RISE, 40.5),
        new (68, 2016, 4, 28, SwephExp.SE_CALC_RISE, 46.1),
        new (66, 2016, 4, 28, SwephExp.SE_CALC_RISE, 50.3),
        new (64, 2016, 4, 28, SwephExp.SE_CALC_RISE, 53.7),
        new (62, 2016, 4, 28, SwephExp.SE_CALC_RISE, 56.5),
        new (60, 2016, 4, 28, SwephExp.SE_CALC_RISE, 58.8),
        new (58, 2016, 4, 28, SwephExp.SE_CALC_RISE, 60.7),
        new (56, 2016, 4, 28, SwephExp.SE_CALC_RISE, 62.4),
        new (54, 2016, 4, 28, SwephExp.SE_CALC_RISE, 63.9),
        new (52, 2016, 4, 28, SwephExp.SE_CALC_RISE, 65.2),
        new (50, 2016, 4, 28, SwephExp.SE_CALC_RISE, 66.3),
        new (45, 2016, 4, 28, SwephExp.SE_CALC_RISE, 68.7),
        new (40, 2016, 4, 28, SwephExp.SE_CALC_RISE, 70.5),
        new (30, 2016, 4, 28, SwephExp.SE_CALC_RISE, 72.9),
        new (20, 2016, 4, 28, SwephExp.SE_CALC_RISE, 74.5),
        new (10, 2016, 4, 28, SwephExp.SE_CALC_RISE, 75.3),
        new (00, 2016, 4, 28, SwephExp.SE_CALC_RISE, 75.7),

		// S, SE_CALC_RISE
		new (-10, 2016, 4, 28, SwephExp.SE_CALC_RISE, 75.6),
        new (-20, 2016, 4, 28, SwephExp.SE_CALC_RISE, 75.1),
        new (-30, 2016, 4, 28, SwephExp.SE_CALC_RISE, 73.9),
        new (-40, 2016, 4, 28, SwephExp.SE_CALC_RISE, 71.9),
        new (-45, 2016, 4, 28, SwephExp.SE_CALC_RISE, 70.4),
        new (-50, 2016, 4, 28, SwephExp.SE_CALC_RISE, 68.5),
        new (-52, 2016, 4, 28, SwephExp.SE_CALC_RISE, 67.5),
        new (-54, 2016, 4, 28, SwephExp.SE_CALC_RISE, 66.4),
        new (-56, 2016, 4, 28, SwephExp.SE_CALC_RISE, 65.1),
        new (-58, 2016, 4, 28, SwephExp.SE_CALC_RISE, 63.7),
        new (-60, 2016, 4, 28, SwephExp.SE_CALC_RISE, 62.0),

		// N, SE_CALC_SET
		new (74, 2016, 4, 28, SwephExp.SE_CALC_SET, 343.8),
        new (72, 2016, 4, 28, SwephExp.SE_CALC_SET, 328.8),
        new (70, 2016, 4, 28, SwephExp.SE_CALC_SET, 320.5),
        new (68, 2016, 4, 28, SwephExp.SE_CALC_SET, 314.8),
        new (66, 2016, 4, 28, SwephExp.SE_CALC_SET, 310.4),
        new (64, 2016, 4, 28, SwephExp.SE_CALC_SET, 306.9),
        new (62, 2016, 4, 28, SwephExp.SE_CALC_SET, 304.1),
        new (60, 2016, 4, 28, SwephExp.SE_CALC_SET, 301.7),
        new (58, 2016, 4, 28, SwephExp.SE_CALC_SET, 299.7),
        new (56, 2016, 4, 28, SwephExp.SE_CALC_SET, 297.9),
        new (54, 2016, 4, 28, SwephExp.SE_CALC_SET, 296.4),
        new (52, 2016, 4, 28, SwephExp.SE_CALC_SET, 295.1),
        new (50, 2016, 4, 28, SwephExp.SE_CALC_SET, 294.0),
        new (45, 2016, 4, 28, SwephExp.SE_CALC_SET, 291.6),
        new (40, 2016, 4, 28, SwephExp.SE_CALC_SET, 289.8),
        new (30, 2016, 4, 28, SwephExp.SE_CALC_SET, 287.3),
        new (20, 2016, 4, 28, SwephExp.SE_CALC_SET, 285.7),
        new (10, 2016, 4, 28, SwephExp.SE_CALC_SET, 284.8),
        new (00, 2016, 4, 28, SwephExp.SE_CALC_SET, 284.5),

		// S, SE_CALC_SET
		new (-10, 2016, 4, 28, SwephExp.SE_CALC_SET, 284.5),
        new (-20, 2016, 4, 28, SwephExp.SE_CALC_SET, 285.1),
        new (-30, 2016, 4, 28, SwephExp.SE_CALC_SET, 286.2),
        new (-40, 2016, 4, 28, SwephExp.SE_CALC_SET, 288.3),
        new (-45, 2016, 4, 28, SwephExp.SE_CALC_SET, 289.8),
        new (-50, 2016, 4, 28, SwephExp.SE_CALC_SET, 291.7),
        new (-52, 2016, 4, 28, SwephExp.SE_CALC_SET, 292.7),
        new (-54, 2016, 4, 28, SwephExp.SE_CALC_SET, 293.8),
        new (-56, 2016, 4, 28, SwephExp.SE_CALC_SET, 295.1),
        new (-58, 2016, 4, 28, SwephExp.SE_CALC_SET, 296.5),
        new (-60, 2016, 4, 28, SwephExp.SE_CALC_SET, 298.2),

		// S, SE_CALC_SET
		new (-56, 2016, 4, 29, SwephExp.SE_CALC_SET, 295.7),
        new (-58, 2016, 4, 29, SwephExp.SE_CALC_SET, 297.2),
    };

    private static readonly CalculateAzimuthTestCase[] CalculateAzimuthTestCases = new CalculateAzimuthTestCase[]
    {
        new(2016, 4, 27,
            new GeoPosition
            {
                Latitude = 58,
                Longitude = 0,
                Altitude = AstroCatalogue.StandardAltitude,
            },
            AstroCatalogue.StandardPressure,
            AstroCatalogue.StandardTemperature,
            SwephExp.SE_CALC_RISE,
            60,
            new AzimuthCalcResult
            {
                At = 61.4,
                dLatitude = 0,
                dLongitude = 0,
                dAltitude = 0,
                dTemperature = 0,
                dPressure = 0,
                dh = 0,
                K = 0,
                dAzimuth = 0,
                AzimuthTop = 61.4,
                AzimuthBot = 62.4,
                dAzimuthAge = 1,
                KP = 60,
                dKPTop = 1.4,
                dKPBot = 2.4,
                Purpose = SwephExp.SE_CALC_RISE,
            }
        ),
        new(2016, 4, 29,
            new GeoPosition
            {
                Latitude = -58,
                Longitude = 0,
                Altitude = AstroCatalogue.StandardAltitude,
            },
            AstroCatalogue.StandardPressure,
            AstroCatalogue.StandardTemperature,
            SwephExp.SE_CALC_SET,
            297,
            new AzimuthCalcResult
            {
                At = 297.2,
                dLatitude = 0,
                dLongitude = 0,
                dAltitude = 0,
                dTemperature = 0,
                dPressure = 0,
                dh = 0,
                K = 0,
                dAzimuth = 0,
                AzimuthTop = 297.17,
                AzimuthBot = 298.13,
                dAzimuthAge = 0.96,
                KP = 297,
                dKPTop = 0.2,
                dKPBot = 1.1,
                Purpose = SwephExp.SE_CALC_SET,
            }
        ),
    };
}
