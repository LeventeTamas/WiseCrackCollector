using WiseCrackCollector.Models;

namespace WiseCrackCollector.Services
{
    public interface IWisecrackService
    {
        void DeleteWisecracksByGroupId(string groupId);
        void AddWisecrack(string groupId, Wisecrack newWisecrack);
        Wisecrack GetWisecrackById(string delete_wc_id);
        void DeleteWisecrack(Wisecrack wisecrack);
        void UpdateWisecrack(Wisecrack wisecrack);
        bool IsWisecrackExists(string wisecrackId);
    }
}
