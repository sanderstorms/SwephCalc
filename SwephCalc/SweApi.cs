using System.Text;

namespace SwephCalc;

/// <summary>
/// Обертка для вызова функиций swedll64.dll.
/// Создание и использование экземпляров в разных потоках может привести к ошибкам.
/// </summary>
internal class SweApi : IDisposable
{
    private const int DefaultStringLength = 256;

    public unsafe SweApi()
    {
        Swedll.swe_set_ephe_path(null);
    }

    public unsafe string GetVersion()
    {
        var buff = stackalloc byte[DefaultStringLength];

        Swedll.swe_version(buff);

        return PointerToString(buff);
    }

    /// <summary>
    /// Вычисляет Юлианский день для восхода или захода Солнца.
    /// </summary>
    /// <param name="position">Географическая позиция наблюдателя.</param>
    /// <param name="pressure">Атмосферное давление в mbar/hPa.</param>
    /// <param name="temperature">Температура воздуха в градусах C.</param>
    /// <param name="purpose">
    /// Расчет времени захода или заката.
    /// Одно из значений: <see cref="SwephExp.SE_CALC_RISE"/> или <see cref="SwephExp.SE_CALC_SET"/>.
    /// </param>
    /// <param name="date">Дата. UTC.</param>
    /// <returns>Время восхода/заката в формате Юлианского дня.</returns>
    public unsafe double SunriseSunsetJulDay(GeoPosition position,
        double pressure,
        double temperature,
        int purpose,
        DateTime date)
    {
        Swedll.swe_set_topo(position.Longitude, position.Latitude, position.Altitude);
        var tjd = Swedll.swe_julday(date.Year, date.Month, date.Day, date.Hour, SwephExp.SE_GREG_CAL);

        var geopos = stackalloc double[] { position.Longitude, position.Latitude, position.Altitude };
        var sterr = stackalloc byte[DefaultStringLength];
        var riseTime = 0d;

        var result = Swedll.swe_rise_trans(tjd, SwephExp.SE_SUN, null, SwephExp.SEFLG_SWIEPH, purpose, geopos, pressure, temperature, &riseTime, sterr);

        if (result == -2)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Не удалось найти время восхода/захода");
        }

        if (result == Sweodef.ERR)
        {
            throw new SwedllException(PointerToString(sterr));
        }

        return riseTime;
    }

    /// <summary>
    /// Конвертирует Юлианский день в UTC дату.
    /// </summary>
    /// <param name="jday">Юлианский день. UTC.</param>
    /// <returns>UTC Дату.</returns>
    public unsafe DateTime JulDayToDateTime(double jday)
    {
        int day, month, year;
        double hour;

        Swedll.swe_revjul(jday, SwephExp.SE_GREG_CAL, &year, &month, &day, &hour);
        var date = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);

        return date.AddHours(hour);
    }

    /// <summary>
    /// Конвертирует UTC дату в Юлианский день.
    /// </summary>
    /// <param name="date">Дата. UTC.</param>
    /// <returns>UTC дату в формате юлианского дня.</returns>
    public unsafe double DateTimeToJulDay(DateTime date)
    {
        var hour = date.Hour + date.Minute / 60d;
        return Swedll.swe_julday(date.Year, date.Month, date.Day, hour, SwephExp.SE_GREG_CAL);
    }

    /// <summary>
    /// Вычисляет позицию Солнца для указанного юлианского дня.
    /// </summary>
    /// <param name="jday">Юлианский день. UTC.</param>       
    /// <param name="calcFlag">Тип вычислений. По умолчанию <see cref="SwephExp.SEFLG_EQUATORIAL"/>.</param>
    /// <returns><see cref="BodyPosition"/>.</returns>
    public unsafe BodyPosition GetSunPosition(double jday, int calcFlag = SwephExp.SEFLG_EQUATORIAL)
    {
        var sterr = stackalloc byte[DefaultStringLength];
        var position = stackalloc double[6];

        var result = Swedll.swe_calc_ut(jday, SwephExp.SE_SUN, calcFlag, position, sterr);
		if (result == Sweodef.ERR)
		{
			throw new SwedllException(PointerToString(sterr));
		}

        return new BodyPosition
        {
            Longitude = position[0],
            Latitude = position[1],
            Distance = position[2],
            LongitudeSpeed = position[3],
            LatitudeSpeed = position[4],
            DistanceSpeed = position[5],
        };
    }

    /// <summary>
    /// Расчитывает горизонтальные координаты тела.
    /// </summary>
    /// <param name="jday">Юлианский день. UTC.</param>
    /// <param name="position">Позиция наблюдателя.</param>
    /// <param name="pressure">Атмосферное давление mbar/hPa.</param>
    /// <param name="temperature">Температура в градусах C</param>
    /// <param name="bodyPosition">Позиция небесного тела. Обязательны только поля <see cref="BodyPosition.Longitude"/> и <see cref="BodyPosition.Latitude"/>.</param>
    /// <returns>Горизонтальные координаты тела.</returns>
    public unsafe HorizontalCoordinates GetHorizontalCoordinates(double jday,
        GeoPosition position,
        double pressure,
        double temperature,
        BodyPosition bodyPosition)
    {
        var geopos = stackalloc double[] { position.Longitude, position.Latitude, position.Altitude };
        var xin = stackalloc double[] { bodyPosition.Longitude, bodyPosition.Latitude, bodyPosition.Distance };
        var xout = stackalloc double[3];

        Swedll.swe_azalt(jday, SwephExp.SE_EQU2HOR, geopos, pressure, temperature, xin, xout);

        return new HorizontalCoordinates
        {
            Azimuth = xout[0],
            Altitude = xout[1],
            ApparentAltitude = xout[2],
        };
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Swedll.swe_close();
        }
    }

    public static unsafe string PointerToString(byte* pointer, int lenght = DefaultStringLength)
    {
        int i = 0;
        while (i < lenght)
        {
            if (pointer[i] == 0)
            {
                break;
            }
            ++i;
        }

        return Encoding.ASCII.GetString(pointer, i);
    }
}
