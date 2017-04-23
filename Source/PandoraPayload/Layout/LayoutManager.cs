using Abrakam;

namespace PandoraPayload.Layout
{
    public static class LayoutManager
    {
        public static Abrakam.WelcomeLayout WelcomeLayout
        {
            get
            {
                WelcomeSceneLayout layout = (WelcomeSceneLayout)ApplicationManager.faeriaSceneManager.GetSceneLayout(FaeriaSceneManager.SceneType.WELCOME);
                return layout?.GetWelcomeLayout();
            }
        }
    }
}
