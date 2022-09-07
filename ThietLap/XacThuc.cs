namespace ThietLap;

public static class StartUp
{
    public static void ThemDichVuXacThuc(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication("Cookie.XacThuc").AddCookie("Cookie.XacThuc", options =>
        {
            options.SlidingExpiration = true;
        });
    }
}