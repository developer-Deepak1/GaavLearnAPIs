using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaavLearnAPIs.Dtos
{
    public class AuthResponseDto
    {
        public string? Token{get;set;}=string.Empty;
        public bool IsSuccess{get;set;}
        public string? Message{get;set;}
        public string? RefreshToken{get;set;}=string.Empty;
    }
}