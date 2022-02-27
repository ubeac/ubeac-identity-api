namespace IntegrationTests;

public struct Endpoints
{
    public const string ACCOUNTS_LOGIN = "/API/Accounts/Login";

    public const string USERS_CREATE = "/API/Users/Create";
    public const string USERS_UPDATE = "/API/Users/Update";
    public const string USERS_ASSIGN_ROLES = "/API/Users/AssignRoles";
    public const string USERS_CHANGE_PASSWORD = "/API/Users/ChangePassword";
    public const string USERS_GET_BY_ID = "/API/Users/GetById";
    public const string USERS_GET_ALL = "/API/Users/GetAll";
}

public struct SuperAdmin
{
    public const string USERNAME = "admin";
    public const string PASSWORD = "1qaz!QAZ";
}