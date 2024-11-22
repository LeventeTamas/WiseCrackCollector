﻿using WiseCrackCollector.Models;

namespace WiseCrackCollector.ViewModels
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
        public GroupUserMembership Permissions { get; set; }
        public WisecrackListSortBy SortBy { get; set; }
        public WisecrackListSortOrder SortOrder { get; set; }
    }
}
