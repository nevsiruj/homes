using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EIRL.Application.Services.Common
{
    public static class SlugGenerator
    {
        /// <summary>
        /// Generates a URL-friendly slug from a title and optional code.
        /// Example: "Apartamento en Venta Zona 14" + "ASV8508" -> "apartamento-en-venta-zona-14-asv8508"
        /// </summary>
        /// <param name="title">The property title</param>
        /// <param name="code">Optional property code to append at the end</param>
        /// <returns>A URL-friendly slug</returns>
        public static string GenerateSlug(string title, string code = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                // If no title, use code only
                if (!string.IsNullOrWhiteSpace(code))
                {
                    return NormalizeString(code);
                }
                return string.Empty;
            }

            // Normalize the title
            var slug = NormalizeString(title);

            // Append code if provided
            if (!string.IsNullOrWhiteSpace(code))
            {
                var normalizedCode = NormalizeString(code);
                if (!string.IsNullOrWhiteSpace(normalizedCode))
                {
                    slug = $"{slug}-{normalizedCode}";
                }
            }

            // Ensure slug is not too long (limit to 200 characters)
            if (slug.Length > 200)
            {
                slug = slug.Substring(0, 200);
                // Remove trailing dash if any
                slug = slug.TrimEnd('-');
            }

            return slug;
        }

        private static string NormalizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Convert to lowercase
            var normalized = input.Trim().ToLowerInvariant();

            // Remove accents and diacritics
            normalized = RemoveDiacritics(normalized);

            // Replace spaces and special characters with hyphens
            normalized = Regex.Replace(normalized, @"[^a-z0-9]+", "-");

            // Remove multiple consecutive hyphens
            normalized = Regex.Replace(normalized, @"-+", "-");

            // Remove leading and trailing hyphens
            normalized = normalized.Trim('-');

            return normalized;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
