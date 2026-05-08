using Drafts.Database;
using UnityEngine;

namespace TricksAndTreatsOrThreats
{
    [CreateAssetMenu(menuName = "TTT/Database", order = 0)]
    public class Database : DatabaseSO<DatabaseItem>
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("TTT/Database/Refresh")]
        public static void RefreshDatabase()
        {
            TTT.Database.FetchAssets();
        }

        [ContextMenu("Fetch Assets")]
        public void FetchAssets()
        {
            FetchItemsFromAssets();
            Species.GetRaces(null);
            foreach (var s in TTT.Database.GetAll<Species>())
                UnityEditor.EditorUtility.SetDirty(s);
            Debug.Log("Database Refreshed");
        }
#endif
    }

    public abstract class DatabaseItem : Drafts.Database.DatabaseItem { }
}