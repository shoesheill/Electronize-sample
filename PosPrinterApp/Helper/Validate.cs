public static class Validate
{
    private static string _EmailConstraints;
    private static string _RequiredString;
    private static string _NumericConstraints;
    private static string _PriceConstraints;

    public static string EmailConstraints
    {
        get
        {
            _EmailConstraints = @"^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,3})$";
            return Validate._EmailConstraints;
        }
    }

    

    public static string RequiredString
    {
        get
        {
            _RequiredString = "\nFields marked with an asterisk(*) are required.";
            return _RequiredString;
        }
    }

    public static string NumberConstraints
    {
        get
        {
            _NumericConstraints = @"^[0-9]+$";
            return _NumericConstraints;
        }
    }

    public static string DecimalConstraints
    {
        get
        {
            _PriceConstraints = @"^[0-9]*[.][0-9]{1,2}$";
            return _PriceConstraints;
        }
    }

    
}

