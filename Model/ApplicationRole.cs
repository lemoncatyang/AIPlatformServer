using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Model
{
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRoleType RoleType { get; set; }
    }
}
