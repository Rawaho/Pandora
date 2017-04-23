using Abrakam;

namespace PandoraPayload.Layout
{
    public static class WelcomeLayout
    {
        /// <summary>
        /// Prefix to Abrakam::WelcomeLayout::Awake
        /// </summary>
        public static void Awake()
        {
            ApplicationManager.worldNetworkManager.defaultServerIp = Payload.WorldAddress;
        }

        /// <summary>
        /// Prefix to Abrakam::WelcomeLayout::OnNewAccountButton
        /// </summary>
        public static bool OnNewAccountButton()
        {
            Abrakam.WelcomeLayout welcomeLayout = LayoutManager.WelcomeLayout;
            if (welcomeLayout == null)
                return true;

            MobileFriendlyTextInput usernameField = welcomeLayout.GetPrivateField<MobileFriendlyTextInput>("userNameInputField");
            if (usernameField == null)
                return true;

            MobileFriendlyTextInput passwordField = welcomeLayout.GetPrivateField<MobileFriendlyTextInput>("passwordInputField");
            if (passwordField == null)
                return true;

            string error = GuiManager.CheckUsername(usernameField.text);
            if (error != null)
            {
                welcomeLayout.errorPanelLayout.DisplayErrorMessage(error);
                return true;
            }

            if (passwordField.text.Length < 8)
            {
                welcomeLayout.errorPanelLayout.DisplayErrorMessage(LocalisationManager.GetLocalisedValue("PASSWORD_TOO_SHORT"));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Replaces Abrakam::WelcomeLayout::SendSignUpMessage
        /// </summary>
        public static void SendSignUpMessage()
        {
            Abrakam.WelcomeLayout welcomeLayout = LayoutManager.WelcomeLayout;
            if (welcomeLayout != null)
            {
                MobileFriendlyTextInput usernameField = welcomeLayout.GetPrivateField<MobileFriendlyTextInput>("userNameInputField");
                if (usernameField == null)
                    return;

                MobileFriendlyTextInput passwordField = welcomeLayout.GetPrivateField<MobileFriendlyTextInput>("passwordInputField");
                if (passwordField == null)
                    return;

                // send custom packet
                WorldNetworkManager worldNetworkManager = ApplicationManager.worldNetworkManager;
                worldNetworkManager.AppendParameter("payloadVersion", Payload.Version);
                worldNetworkManager.AppendParameter("clientVersion", ApplicationManager.applicationManager.GetBuildVersionNumber());
                worldNetworkManager.AppendParameter("username", usernameField.text);
                worldNetworkManager.AppendParameter("password", Utils.ComputeSha1Hash(passwordField.text));
                worldNetworkManager.SendEncryptedCommand("PandoraSignUp");
            }

            return;
        }

        /// <summary>
        /// Replaces Abrakam::WelcomeLayout::SendTokenSignInMessage
        /// </summary>
        public static void SendTokenSignInMessage()
        {
            // send custom packet
            WorldNetworkManager worldNetworkManager = ApplicationManager.worldNetworkManager;
            worldNetworkManager.AppendParameter("payloadVersion", Payload.Version);
            worldNetworkManager.AppendParameter("clientVersion", ApplicationManager.applicationManager.GetBuildVersionNumber());
            worldNetworkManager.AppendParameter("token", AuthenticationManager.GetAuthenticationManager().GetEncodedToken(AuthenticationManager.TokenType.STRONG));
            worldNetworkManager.SendEncryptedCommand("PandoraSignIn");
        }
    }
}
