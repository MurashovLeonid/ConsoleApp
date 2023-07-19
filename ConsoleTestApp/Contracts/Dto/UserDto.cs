using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp.Contracts.Dto
{
    internal class UserDto
    {
        public UserNameDto Name { get; set; }

        public UserDobDto Dob { get; set; }

        public UserPictureDto Picture { get; set; }
    }
}
