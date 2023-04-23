namespace SwephCalc.Test;

internal class AstroCatalogueTest
{
    [Test]
    public void GetBoundaryLatitudeValuesReturnsCorrectValues()
    {
        for (int i = 0; i < AstroCatalogue.TableLatitudeValues.Count - 1; ++i)
        {
            var delta = (AstroCatalogue.TableLatitudeValues[i + 1] - AstroCatalogue.TableLatitudeValues[i]) / 2;
            var latitude = AstroCatalogue.TableLatitudeValues[i] + delta;
            var (left, right) = AstroCatalogue.GetBoundaryLatitudeValues(latitude);

            left.Should().Be(AstroCatalogue.TableLatitudeValues[i]);
            right.Should().Be(AstroCatalogue.TableLatitudeValues[i + 1]);
        }
    }

    [Test]
    [TestCaseSource(nameof(SimpleInterpolationTestCases))]
    public void SimpleInterpolation(SimpleInterpolationTestCase @case)
    {
        var result = @case.GetInterpolatedCorrection(@case.Value);
        result.Should().Be(@case.ExpectedResult);
    }

    public class SimpleInterpolationTestCase
    {
        public string Name { get; }

        public double Value { get; }

        public double ExpectedResult { get; }

        public Func<double, double> GetInterpolatedCorrection { get; }

        public SimpleInterpolationTestCase(string name, double value, double expectedResult, Func<double, double> getInterpolatedCorrection)
        {
            Name = name;
            Value = value;
            ExpectedResult = expectedResult;
            GetInterpolatedCorrection = getInterpolatedCorrection;
        }

        public override string? ToString() => $"{Name}; Value: {Value}; ExpectedResult: {ExpectedResult}";
    }

    private static SimpleInterpolationTestCase[] SimpleInterpolationTestCases = new SimpleInterpolationTestCase[]
    {
		// Temperature
		new("Temp", AstroCatalogue.DeltaT[0][0], AstroCatalogue.DeltaT[1][0], AstroCatalogue.GetInterpolatedTemperatureCorrection),
        new("Temp", AstroCatalogue.DeltaT[0][^1], AstroCatalogue.DeltaT[1][^1], AstroCatalogue.GetInterpolatedTemperatureCorrection),

		// Pressure
		new("Press", AstroCatalogue.DeltaP[0][0], AstroCatalogue.DeltaP[1][0], AstroCatalogue.GetInterpolatedPressureCorrection),
        new("Press", AstroCatalogue.DeltaP[0][^1], AstroCatalogue.DeltaP[1][^1], AstroCatalogue.GetInterpolatedPressureCorrection),

		// K
		new("K", AstroCatalogue.Kt[0][0], AstroCatalogue.Kt[2][0], (_) => AstroCatalogue.GetInterpolatedK(_, 2)),
        new("K", AstroCatalogue.Kt[0][^1], AstroCatalogue.Kt[2][^1], (_) => AstroCatalogue.GetInterpolatedK(_, 2)),
    };
}
