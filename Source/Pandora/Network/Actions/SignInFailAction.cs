namespace Pandora.Network.Actions
{
    [ServerAction("$signInFail")]
    public class SignInFailAction : ServerAction
    {
        [ServerActionField("userId")]
        public uint UserId { get; set; }
        [ServerActionField("errorMessage")]
        public string ErrorMessage { get; set; }

        public SignInFailAction(uint id, string error)
        {
            UserId       = id;
            ErrorMessage = error;
        }
    }
}
