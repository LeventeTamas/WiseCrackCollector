namespace WiseCrackCollector.Models
{
    public enum WisecrackListSortBy
    {
        Author,
        Content,
        Views,
        Date,
        None
    }

    public enum WisecrackListSortOrder
    {
        Ascending,
        Descending
    }

    public class GroupViewModel
    {
        public Group Group { get; set; }
        public List<Wisecrack> Wisecracks { get; set; }
        public UserGroupPermissionSet Permissions { get; set; }
        public WisecrackListSortBy SortBy { get; set; }
        public WisecrackListSortOrder SortOrder { get; set; }
    }
}
