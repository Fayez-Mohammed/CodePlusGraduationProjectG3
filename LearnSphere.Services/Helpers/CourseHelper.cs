using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LearnSphere.Services.Helpers
{
    public static class CourseHelper
    {
        public static string GenerateSlug(string title)
        {
            var slug = title.ToLowerInvariant().Trim();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            return Regex.Replace(slug, @"\s+", "-").Trim('-');

        }
    }
}
