using Pandora.Database;
using Pandora.Game;

namespace Pandora.Managers
{
    public static class AssetManager
    {
        private static GuidGenerator accountGenerator;

        public static void Initialise()
        {
            accountGenerator = new GuidGenerator(DatabaseManager.DataBase.AccountMax());
        }

        public static uint GetNewAccountId()
        {
            return accountGenerator.Obtain();
        }
    }
}
