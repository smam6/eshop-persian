// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Identity.API.Resources;

namespace IdentityServerHost.Quickstart.UI;

public class LoginInputModel
{
    // پیام/برچسب از منابع AccountResources (fa-IR + neutral) خوانده می‌شود تا
    // با ?culture=en-US پیام انگلیسی درست برگردد.
    [Display(ResourceType = typeof(AccountResources), Name = nameof(AccountResources.Label_Username))]
    [Required(ErrorMessageResourceType = typeof(AccountResources), ErrorMessageResourceName = nameof(AccountResources.Validation_UsernameRequired))]
    public string Username { get; set; }
    [Display(ResourceType = typeof(AccountResources), Name = nameof(AccountResources.Label_Password))]
    [Required(ErrorMessageResourceType = typeof(AccountResources), ErrorMessageResourceName = nameof(AccountResources.Validation_PasswordRequired))]
    public string Password { get; set; }
    [Display(ResourceType = typeof(AccountResources), Name = nameof(AccountResources.Label_RememberLogin))]
    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }
}
