namespace IntegrationTests;

public static class StaticTests
{
    public static string[] Orders { get; set; } =
    {
        nameof(AccountsTests),
        nameof(RolesTests),
        nameof(UnitsTests),
        nameof(UnitTypesTests),
        nameof(UsersTests)
    };

    private static int _currentIndex = 0;
    public static string CurrentOrder { get; private set;  } = Orders[_currentIndex];

    public static void NextOrder()
    {
        _currentIndex += 1;
        if (_currentIndex == Orders.Length) return;
        CurrentOrder = Orders[_currentIndex];
    }
}