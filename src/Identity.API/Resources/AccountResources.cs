namespace Identity.API.Resources;

/// <summary>
/// کلاس نشانگر (marker) برای منابع فرم ورود/ثبت‌نام؛ نام کامل آن با نام منبعِ تولیدشده از
/// Resources/AccountResources.resx (RootNamespace = Identity.API) یکسان است.
/// </summary>
public sealed class AccountResources
{
    public const string Validation_UsernameRequired = nameof(Validation_UsernameRequired);
    public const string Validation_PasswordRequired = nameof(Validation_PasswordRequired);
    public const string Label_Username = nameof(Label_Username);
    public const string Label_Password = nameof(Label_Password);
    public const string Label_RememberLogin = nameof(Label_RememberLogin);
}
