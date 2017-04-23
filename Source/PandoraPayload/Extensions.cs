using System.Reflection;

namespace PandoraPayload
{
    public static class Extensions
    {
        public static T GetPrivateField<T>(this object obj, string member)
        {
            return (T)obj.GetType().GetField(member, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        }

        public static void SetPrivateField<T>(this object obj, string member, object value)
        {
            typeof(T).GetField(member, BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(obj, value);
        }
    }
}
