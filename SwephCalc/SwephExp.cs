namespace SwephCalc;

public static class SwephExp
{
    public const int SEFLG_SWIEPH = 2;

    /* values for gregflag in swe_julday() and swe_revjul() */
    public const int SE_JUL_CAL	= 0;
    public const int SE_GREG_CAL = 1;

    public const int SE_SUN = 0;

    public const int SEFLG_EQUATORIAL = 2 * 1024;

    /* for swe_azalt() and swe_azalt_rev() */
    public const int SE_ECL2HOR	=	0;
    public const int SE_EQU2HOR	=	1;
    public const int SE_HOR2ECL	=	0;
    public const int SE_HOR2EQU	=	1;

    public const int SE_CALC_RISE = 1;
    public const int SE_CALC_SET = 2;
}
