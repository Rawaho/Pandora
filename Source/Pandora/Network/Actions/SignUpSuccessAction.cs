namespace Pandora.Network.Actions
{
    [ServerAction("$signUpSuccess")]
    public class SignUpSuccessAction : ServerAction
    {
        [ServerActionField("token")]
        public string Token { get; set; }

        public SignUpSuccessAction(string token)
        {
            Token = token;
        }
    }
}
