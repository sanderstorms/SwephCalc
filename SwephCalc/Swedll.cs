using System.Runtime.InteropServices;

namespace SwephCalc;

internal static class Swedll
{
    public const string SwedllPath = "sweph/bin/swedll64.dll";

    [DllImport(SwedllPath)]
    public static unsafe extern void swe_set_ephe_path(char* path);

    [DllImport(SwedllPath)]
    public static extern void swe_close();

    [DllImport(SwedllPath)]
    public static unsafe extern byte* swe_version(byte* svers);

    /// <summary>
    /// This function must be called before topocentric planet positions for a certain birth place can be computed.
    /// It tells Swiss Ephemeris, what geographic position is to be used.
    /// Geographic longitude geolon and latitude geolat must be in degrees, the altitude above sea must be in meters.
    /// Neglecting the altitude can result in an error of about 2 arc seconds with the Moon and at an altitude 3000 m.
    /// After calling swe_set_topo(), add SEFLG_TOPOCTR to iflag and call swe_calc() as with an ordinary computation.
    /// </summary>
    /// <remarks>
    /// The parameters set by swe_set_topo() survive swe_close().
    /// </remarks>
    [DllImport(SwedllPath)]
    public static extern void swe_set_topo(double geolon, double geolat, double altitude);

    [DllImport(SwedllPath)]
    public static extern double swe_julday(int year, int month, int day, double hour, int gregflag);

    [DllImport(SwedllPath)]
    public static unsafe extern void swe_revjul(double tjd,     /* Julian day number */
                                                int gregflag,   /* Gregorian calendar: 1, Julian calendar: 0 */
                                                int* year,      /* target addresses for year, etc. */
                                                int* month,
                                                int* day,
                                                double* hour);

    [DllImport(SwedllPath)]
    public static unsafe extern int swe_calc_ut(double tjd_ut,   // Julian day, Universal Time 
                                                int ipl,         // body number
                                                int iflag,       // a 32 bit integer containing bit flags that indicate what kind of computation is wanted
                                                double* xx,      // array of 6 doubles for longitude, latitude, distance, speed in long., speed in lat., and speed in dist.
                                                byte* serr);     // return address for error message

    [DllImport(SwedllPath)]
    public static unsafe extern int swe_rise_trans(double tjd_ut,    /* search after this time (UT) */
                                                   int ipl,          /* planet number, if planet or moon */
                                                   char* starname,   /* star name, if star */
                                                   int epheflag,     /* ephemeris flag */
                                                   int rsmi,         /* integer specifying that rise, set, or one of the two meridian transits is wanted. see definition below */
                                                   double* geopos,   /* array of three doubles containing geograph. long., lat., height of observer */
                                                   double atpress,   /* atmospheric pressure in mbar/hPa */
                                                   double attemp,    /* atmospheric temperature in deg. C */
                                                   double* tret,     /* return address (double) for rise time etc. */
                                                   byte* serr);      /* return address for error message */

    [DllImport(SwedllPath)]
    public static unsafe extern void swe_azalt(double tjd_ut,    // UT
                                               int calc_flag,    // SE_ECL2HOR or SE_EQU2HOR
                                               double* geopos,   // array of 3 doubles: geograph. long., lat., height
                                               double atpress,   // atmospheric pressure in mbar (hPa)
                                               double attemp,    // atmospheric temperature in degrees Celsius
                                               double* xin,      // array of 3 doubles: position of body in either ecliptical or equatorial coordinates, depending on calc_flag
                                               double* xaz);     // return array of 3 doubles, containing azimuth, true altitude, apparent altitude;
}
