# Globalization Implementation Guide for N-Tier MVC Project

## Overview
This guide explains how to implement globalization (localization) in your ASP.NET Core N-Tier MVC application to support multiple languages (English and Arabic).

---

## Implementation Steps

### Step A: Create Resource Files (Presentation Layer)

**Location**: `NTier.Presentation/Resources/`

#### Files Created:
1. **SharedResource.cs**
   - An empty class used as a marker for the resource files
   - Namespace: `NTier.Presentation.Resources`
   - Purpose: Links the resource files with the localization system

2. **SharedResource.resx** (Default - English)
   - Contains key-value pairs for English text
   - Example keys: `Home`, `Privacy`, `Welcome`, `EmployeeList`, `Name`, `Email`, etc.
   - This is the fallback language if no specific culture is found

3. **SharedResource.ar-EG.resx** (Arabic - Egypt)
   - Contains the same keys with Arabic translations
   - Suffix format: `{FileName}.{Culture}.resx`
   - Example: "Home" → "الرئيسية", "Privacy" → "الخصوصية"

**Why Presentation Layer?**
- Resource files are placed in the Presentation layer because they contain UI-related text and messages
- The presentation layer directly uses these resources in views and controllers

---

### Step B: Configure Localization in Program.cs

**Location**: `NTier.Presentation/Program.cs`

#### B.1: Add Required Namespaces
```csharp
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using NTier.Presentation.Resources;
```

#### B.2: Configure Services (After AddControllersWithViews)
```csharp
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });
```

**Explanation:**
- `AddViewLocalization`: Enables localization in Razor views
- `LanguageViewLocationExpanderFormat.Suffix`: Allows culture-specific views (e.g., `Index.ar-EG.cshtml`)
- `AddDataAnnotationsLocalization`: Localizes data annotation validation messages
- `DataAnnotationLocalizerProvider`: Uses SharedResource for all validation messages

#### B.3: Configure Middleware (Before UseRouting)
```csharp
var supportedCultures = new[]
{
    new CultureInfo("ar-EG"),
    new CultureInfo("en-US"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    }
});
```

**Explanation:**
- `SupportedCultures`: Defines which cultures are supported for date/number formatting
- `SupportedUICultures`: Defines which cultures are supported for UI text
- `DefaultRequestCulture`: The default language (English - US)
- `QueryStringRequestCultureProvider`: Allows culture via query string (?culture=ar-EG)
- `CookieRequestCultureProvider`: Stores user's language preference in a cookie

---

### Step C: Update _ViewImports.cshtml

**Location**: `NTier.Presentation/Views/_ViewImports.cshtml`

#### Add the following lines:
```csharp
@using Microsoft.Extensions.Localization
@using NTier.Presentation.Resources
@inject IStringLocalizer<SharedResource> SharedLocalizer
```

**Explanation:**
- `IStringLocalizer<SharedResource>`: Interface for accessing localized strings
- `@inject`: Makes the localizer available in all views
- `SharedLocalizer`: The variable name used in views to access translations

---

### Step D: Use Localization in Views

**Usage Pattern:**
```csharp
@SharedLocalizer["Key"]
```

**Examples:**
- `@SharedLocalizer["Home"]` → "Home" (EN) or "الرئيسية" (AR)
- `@SharedLocalizer["Welcome"]` → "Welcome" (EN) or "مرحبا" (AR)
- `@SharedLocalizer["EmployeeList"]` → "Employee List" (EN) or "قائمة الموظفين" (AR)

**In your views, replace hard-coded text:**
```html
<!-- Before -->
<h1>Employee List</h1>

<!-- After -->
<h1>@SharedLocalizer["EmployeeList"]</h1>
```

---

### Step E: Add SetLanguage Action in Controller

**Location**: `NTier.Presentation/Controllers/HomeController.cs`

#### Added Method:
```csharp
[HttpGet]
public IActionResult SetLanguage(string culture, string returnUrl)
{
    Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
    );

    return LocalRedirect(returnUrl);
}
```

**Explanation:**
- Accepts `culture` parameter (e.g., "en-US" or "ar-EG")
- Stores the culture in a cookie that expires in 1 year
- Redirects back to the page the user came from (`returnUrl`)
- Cookie persists across browser sessions

---

### Step F: Add Language Switcher in Layout

**Location**: `NTier.Presentation/Views/Shared/_Layout.cshtml`

#### F.1: Add Culture Detection and RTL Support
At the top of the file:
```csharp
@{
    var culture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
    var direction = culture == "ar-EG" ? "rtl" : "ltr";
}
<!DOCTYPE html>
<html lang="@culture" dir="@direction">
```

**Explanation:**
- Gets the current thread's culture
- Sets text direction: RTL (Right-to-Left) for Arabic, LTR (Left-to-Right) for English
- Updates HTML lang and dir attributes dynamically

#### F.2: Add Language Dropdown in Navbar
```html
<ul class="navbar-nav">
    <li class="nav-item dropdown">
        <a class="nav-link dropdown-toggle text-dark" href="#" id="languageDropdown" 
           role="button" data-bs-toggle="dropdown" aria-expanded="false">
            @SharedLocalizer["Language"]
        </a>
        <ul class="dropdown-menu" aria-labelledby="languageDropdown">
            <li>
                <a class="dropdown-item d-flex justify-content-between align-items-center"
                   href="@Url.Action("SetLanguage", "Home", new {
                       culture = "en-US",
                       returnUrl = Context.Request.Path + Context.Request.QueryString
                   })">
                    <span>🇺🇸</span>
                    <span>@SharedLocalizer["English"]</span>
                </a>
            </li>
            <li>
                <a class="dropdown-item d-flex justify-content-between align-items-center"
                   href="@Url.Action("SetLanguage", "Home", new {
                       culture = "ar-EG",
                       returnUrl = Context.Request.Path + Context.Request.QueryString
                   })">
                    <span>🇪🇬</span>
                    <span>@SharedLocalizer["Arabic"]</span>
                </a>
            </li>
        </ul>
    </li>
</ul>
```

**Explanation:**
- Dropdown menu in the navbar to switch languages
- Each link calls the `SetLanguage` action with the desired culture
- `returnUrl` ensures the user stays on the current page after switching
- Flag emojis (🇺🇸, 🇪🇬) provide visual language indicators

---

## File Structure Summary

```
NTier.Presentation/
├── Resources/                          [NEW - Step A]
│   ├── SharedResource.cs              [NEW - Empty class marker]
│   ├── SharedResource.resx            [NEW - English resources]
│   └── SharedResource.ar-EG.resx      [NEW - Arabic resources]
├── Controllers/
│   └── HomeController.cs              [MODIFIED - Step E: Added SetLanguage]
├── Views/
│   ├── _ViewImports.cshtml            [MODIFIED - Step C: Added localization imports]
│   └── Shared/
│       └── _Layout.cshtml             [MODIFIED - Step F: Added language switcher]
└── Program.cs                         [MODIFIED - Step B: Added localization config]
```

---

## How It Works

### Flow Diagram:
1. **User visits the site** → Default culture (en-US) is loaded
2. **User clicks language dropdown** → Selects Arabic (ar-EG)
3. **SetLanguage action is called** → Culture is stored in cookie
4. **User is redirected back** → Same page, new language
5. **Next requests** → Cookie is read, Arabic culture is applied
6. **Views render** → `@SharedLocalizer["Key"]` returns Arabic text
7. **HTML updates** → `dir="rtl"` applies for Arabic

### Culture Providers Priority:
1. **Query String**: `?culture=ar-EG` (temporary, for testing)
2. **Cookie**: Persistent user preference
3. **Default**: Falls back to "en-US"

---

## Adding More Languages

To add more languages (e.g., French):

1. **Create resource file**: `SharedResource.fr-FR.resx`
2. **Add to supported cultures** in Program.cs:
   ```csharp
   var supportedCultures = new[]
   {
       new CultureInfo("ar-EG"),
       new CultureInfo("en-US"),
       new CultureInfo("fr-FR"),  // Add this
   };
   ```
3. **Add menu item** in _Layout.cshtml:
   ```html
   <a class="dropdown-item" href="@Url.Action("SetLanguage", "Home", new { culture = "fr-FR", returnUrl = ... })">
       <span>🇫🇷</span>
       <span>Français</span>
   </a>
   ```

---

## Adding More Resource Keys

To add new translatable text:

1. **Open SharedResource.resx** (default language)
2. **Add new key-value pair**: 
   - Name: `EmployeeNotFound`
   - Value: `Employee not found`
3. **Open SharedResource.ar-EG.resx** (Arabic)
4. **Add same key with Arabic translation**:
   - Name: `EmployeeNotFound`
   - Value: `الموظف غير موجود`
5. **Use in views**: `@SharedLocalizer["EmployeeNotFound"]`

---

## Best Practices

### ✅ Do:
- Use meaningful key names (e.g., `EmployeeList` not `Text1`)
- Keep all keys in sync across all resource files
- Use SharedResource for common UI elements
- Create controller-specific resources for complex features
- Test both LTR and RTL layouts

### ❌ Don't:
- Hard-code text in views
- Use spaces in resource keys
- Mix languages in the same resource file
- Forget to translate new keys in all supported languages

---

## Testing Localization

### Method 1: Using Query String
Navigate to: `https://localhost:5001/?culture=ar-EG`

### Method 2: Using Language Dropdown
Click on the language dropdown in the navbar and select your preferred language

### Method 3: Using Browser Tools
1. Open browser DevTools → Application → Cookies
2. Look for `.AspNetCore.Culture` cookie
3. Value should be: `c=ar-EG|uic=ar-EG` for Arabic

---

## Troubleshooting

### Issue: Text not translating
**Solution**: 
- Check if the key exists in both .resx files
- Verify the culture cookie is set correctly
- Clear browser cache and cookies

### Issue: RTL not working
**Solution**: 
- Check if `dir="@direction"` is in the `<html>` tag
- Add CSS for RTL support: `.rtl { text-align: right; }`

### Issue: Cookie not persisting
**Solution**: 
- Ensure HTTPS is configured (cookies may not work on HTTP)
- Check cookie expiration settings
- Verify `CookieRequestCultureProvider` is in the providers list

---

## Additional Resources

- [Microsoft Localization Docs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization)
- [Resource File Format](https://learn.microsoft.com/en-us/dotnet/framework/resources/creating-resource-files-for-desktop-apps)
- [CultureInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)

---

## Summary

This implementation provides:
✅ Multi-language support (English & Arabic)
✅ RTL support for Arabic
✅ Persistent language preference (cookie-based)
✅ Easy-to-use syntax in views (`@SharedLocalizer["Key"]`)
✅ Centralized resource management
✅ Support for data annotation localization
✅ User-friendly language switcher in navbar

**Created by**: Globalization Setup Guide
**Date**: October 24, 2025
**Project**: NTier MVC Application

