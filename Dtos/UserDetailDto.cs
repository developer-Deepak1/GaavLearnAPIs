using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaavLearnAPIs.Dtos
{
    public class UserDetailDto
    {
        public string? Id{get;set;}
        public string? FullName{get;set;}
        public string? Email{get;set;}
        public string[]? Roles{get;set;}
        public string? PhoneNumber{get;set;}
        public bool TwoFactorEnabled{get;set;}
        public bool PhoneNumberConfimed{get;set;}
        public int AccessFailedCount{get;set;}
    }
}