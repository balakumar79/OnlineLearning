

namespace Learning.Entities
{
    public class AppUser : Microsoft.AspNetCore.Identity.IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string District { get; set; }
        public virtual Tutor Tutor { get; set; }
        public virtual Student Student { get; set; }
        public bool HasUserAccess { get; set; }
        public string UserProfileImage { get; set; }
        private GenderEnum _gender;
        public GenderEnum GenderEnum  { get=>_gender=(GenderEnum)Gender ; set=>_gender=value; }

    }
    
}
