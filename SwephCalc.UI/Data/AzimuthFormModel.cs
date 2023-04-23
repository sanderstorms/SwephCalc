using System.ComponentModel.DataAnnotations;

namespace SwephCalc.UI.Data;

public class AzimuthFormModel : IValidatableObject
{
    public DateTime Date { get; set; }

    public int Longitude { get; set; }

    public double LongitudeMin { get; set; }

    public bool IsEastern { get; set; }

    public int Latitude { get; set; }

    public double LatitudeMin { get; set; }

    public bool IsNorthern { get; set; }

    public double Altitude { get; set; }

    public double Pressure { get; set; }

    public double Temperature { get; set; }

    public int Purpose { get; set; }

    public double KP { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Purpose < 1 || Purpose > 2)
        {
            yield return new ValidationResult("Поле восход/заход должна иметь значение 1 или 2", new[] { nameof(Purpose) });
        }

        if (Temperature < AstroCatalogue.DeltaT[0][0] || Temperature > AstroCatalogue.DeltaT[0][^1])
        {
            yield return new ValidationResult($"Температура должна быть задана в интервале [{AstroCatalogue.DeltaT[0][0]}, {AstroCatalogue.DeltaT[0][^1]}]", new[] { nameof(Temperature) });
        }

        var maxP = AstroCatalogue.DeltaP[0][^1].TommHg();
        var minP = AstroCatalogue.DeltaP[0][0].TommHg();
        if (Pressure < minP || Pressure > maxP)
        {
            yield return new ValidationResult($"Температура должна быть задана в интервале [{minP}, {maxP}]", new[] { nameof(Pressure) });
        }

        if (LongitudeMin < 0 || LongitudeMin > 59)
        {
            yield return new ValidationResult($"Минуты должны быть заданы в интервале [0, 59]", new[] { nameof(LongitudeMin) });
        }

        if (LatitudeMin < 0 || LatitudeMin > 59)
        {
            yield return new ValidationResult($"Минуты должны быть заданы в интервале [0, 59]", new[] { nameof(LatitudeMin) });
        }

        var longitude = Longitude + (LongitudeMin / 60d);
        if (longitude < 0 || longitude > 180)
        {
            yield return new ValidationResult("Долгота должна быть задана в интервале [0, 180]", new[] { nameof(Longitude), nameof(LongitudeMin) });
        }

        var latitude = Latitude + (LatitudeMin / 60d);
        if (latitude < 0 || latitude > 90)
        {
            yield return new ValidationResult($"Широта должна быть задана в интервале [0, 90]", new[] { nameof(Latitude), nameof(LatitudeMin) });
        }

        if (Altitude < 0 || Altitude > 50)
        {
            yield return new ValidationResult($"Высота должна быть задана в интервале [0, 50]", new[] { nameof(Altitude) });
        }

        if (KP < 0 || KP > 360)
        {
            yield return new ValidationResult($"КП должен быть задан в интервале [0, 360]", new[] { nameof(KP) });
        }
    }
}
