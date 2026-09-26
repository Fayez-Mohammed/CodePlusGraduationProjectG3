using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Shared.DTOs.Profile
{
    public class UpdateProfileDTO
    {
        //public string Id {  get; set; }
        public string? FullName { get; set; }
        public string? Bio {  get; set; }
        public string? PhoneNumber { get; set; }

    }
    public class GetProfileDTO
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
    }
}
