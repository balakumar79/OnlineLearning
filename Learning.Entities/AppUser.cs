

using System;

namespace Learning.Entities
{
    public class AppUser : Microsoft.AspNetCore.Identity.IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string District { get; set; }

        public virtual Student Student { get; set; }
        public bool HasUserAccess { get; set; }
        public string UserProfileImage { get; set; }
        public DateTime LastAccessedOn { get; set; }
        public DateTime CreatedAt { get; set; }
        private int _gender;
        public GenderEnum GenderEnum { protected get => (GenderEnum)_gender; set => _gender = Gender; }

    }

}
