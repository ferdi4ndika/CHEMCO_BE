public static class TimeHelper
{
    public static DateTime GetJakartaTimeNow()
    {
        var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, jakartaTimeZone);
    }
}
