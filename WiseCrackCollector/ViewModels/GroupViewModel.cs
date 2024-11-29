using WiseCrackCollector.Models;

namespace WiseCrackCollector.ViewModels
{
    public class GroupViewModel
    {
        public Group Group { get; set; }
        public List<Wisecrack> Wisecracks { get; set; }
        public GroupUserMembership Permissions { get; set; }
        public string RedirectController {  get; set; }
        public string RedirectAction { get; set; }
        public string RedirectTitle { get; set; }
    }
}
