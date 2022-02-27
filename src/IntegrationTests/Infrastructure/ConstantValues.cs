namespace IntegrationTests;

public struct Endpoints
{
    public const string ACCOUNTS_REGISTER = "/API/Accounts/Register";
    public const string ACCOUNTS_LOGIN = "/API/Accounts/Login";
    public const string ACCOUNTS_REFRESH_TOKEN = "/API/Accounts/RefreshToken";
    public const string ACCOUNTS_FORGOT_PASSWORD = "/API/Accounts/ForgotPassword";
    public const string ACCOUNTS_RESET_PASSWORD = "/API/Accounts/ResetPassword";
    public const string ACCOUNTS_CHANGE_PASSWORD = "/API/Accounts/ChangePassword";

    public const string USERS_CREATE = "/API/Users/Create";
    public const string USERS_UPDATE = "/API/Users/Update";
    public const string USERS_ASSIGN_ROLES = "/API/Users/AssignRoles";
    public const string USERS_CHANGE_PASSWORD = "/API/Users/ChangePassword";
    public const string USERS_GET_BY_ID = "/API/Users/GetById";
    public const string USERS_GET_ALL = "/API/Users/GetAll";

    public const string ROLES_CREATE = "/API/Roles/Create";
    public const string ROLES_UPDATE = "/API/Roles/Update";
    public const string ROLES_DELETE = "/API/Roles/Delete";
    public const string ROLES_GET_ALL = "/API/Roles/GetAll";

    public const string UNIT_ROLES_CREATE = "/API/UnitRoles/Create";
    public const string UNIT_ROLES_UPDATE = "/API/UnitRoles/Update";
    public const string UNIT_ROLES_DELETE = "/API/UnitRoles/Delete";
    public const string UNIT_ROLES_GET_ALL = "/API/UnitRoles/GetAll";

    public const string UNITS_CREATE = "/API/Units/Create";
    public const string UNITS_UPDATE = "/API/Units/Update";
    public const string UNITS_DELETE = "/API/Units/Delete";
    public const string UNITS_GET_ALL = "/API/Units/GetAll";

    public const string UNIT_TYPES_CREATE = "/API/UnitTypes/Create";
    public const string UNIT_TYPES_UPDATE = "/API/UnitTypes/Update";
    public const string UNIT_TYPES_DELETE = "/API/UnitTypes/Delete";
    public const string UNIT_TYPES_GET_ALL = "/API/UnitTypes/GetAll";
}

public struct SuperAdmin
{
    public const string USERNAME = "admin";
    public const string PASSWORD = "1qaz!QAZ";
}