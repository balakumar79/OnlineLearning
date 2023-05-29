using Learning.Utils.Enums;

namespace Learning.ViewModel.Admin
{
    public class ScreenAccessViewModel
    {
        public string ScreenName { get; set; }
        public Roles Roles { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
