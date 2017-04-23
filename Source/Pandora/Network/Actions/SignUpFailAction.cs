namespace Pandora.Network.Actions
{
    [ServerAction("$signUpFail")]
    public class SignUpFailAction :  ServerAction
    {
        [ServerActionField("errorMessage")]
        public string ErrorMessage { get; set; }

        public SignUpFailAction(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
