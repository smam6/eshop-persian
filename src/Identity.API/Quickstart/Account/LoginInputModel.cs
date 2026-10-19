// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServerHost.Quickstart.UI;

public class LoginInputModel
{
    [Required(ErrorMessage = "نام کاربری الزامی است.")]
    [Display(Name = "نام کاربری")]
    public string Username { get; set; }
    [Required(ErrorMessage = "گذرواژه الزامی است.")]
    [Display(Name = "گذرواژه")]
    public string Password { get; set; }
    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }
}
