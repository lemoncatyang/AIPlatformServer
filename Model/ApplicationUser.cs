using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Model
{
    public class ApplicationUser : IdentityUser
    {
        public int IdentityNumber { get; set; }
    }
}
