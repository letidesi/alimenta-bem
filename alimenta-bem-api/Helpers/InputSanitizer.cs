using System.Text.RegularExpressions;

namespace AlimentaBem.Helpers;

/// <summary>
/// Detecta e remove conteúdo HTML/script em campos de texto livre.
/// Usado como camada de defesa no backend contra XSS armazenado (stored XSS).
/// </summary>
public static class InputSanitizer
{
    // Detecta tags HTML abertas/fechadas e entidades de script comuns
    private static readonly Regex HtmlTagPattern = new(
        @"<\s*[a-zA-Z/][^>]*>|&lt;\s*script|javascript\s*:",
        RegexOptions.IgnoreCase | RegexOptions.Compiled
    );

    /// <summary>
    /// Retorna true se o valor contém marcação HTML ou tentativa de script injection.
    /// </summary>
    public static bool ContainsHtml(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        return HtmlTagPattern.IsMatch(value);
    }

    /// <summary>
    /// Remove tags HTML e retorna texto limpo.
    /// Usado em campos que devem aceitar apenas texto puro.
    /// </summary>
    public static string StripHtml(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        return HtmlTagPattern.Replace(value, string.Empty).Trim();
    }
}
