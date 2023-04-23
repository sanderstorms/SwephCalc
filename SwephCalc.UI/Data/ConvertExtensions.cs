namespace SwephCalc.UI.Data;

public static class ConvertExtensions
{
    public static double TohPa(this double pressure) => pressure * 1.33322;

    public static double TommHg(this double pressure) => pressure * 0.750064;
}
