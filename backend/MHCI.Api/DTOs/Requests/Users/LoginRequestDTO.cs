﻿namespace MHCI.Api.DTOs.Requests.Users
{
    public class LoginRequestDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
