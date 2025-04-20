using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GaavLearnAPIs.Dtos
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage ="Role Name Is Required.")]
        public string RoleName{get;set;}=null!;
    }
}