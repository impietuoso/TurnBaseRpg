using System.Reflection;

namespace BlueGravity.Utility
{
    public static class ReflectionUtility
    {
        private static BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        public static object ReflectionGet(this object obj, string member)
        {
            var field = obj.GetType().GetField(member, Flags);
            var property = obj.GetType().GetProperty(member, Flags);
            return field?.GetValue(obj) ?? property?.GetValue(obj, null);
        }

        public static void ReflectionSet(this object obj, string member, object value)
        {
            var field = obj.GetType().GetField(member, Flags);
            var property = obj.GetType().GetProperty(member, Flags);
            field?.SetValue(obj, value);
            property?.SetValue(obj, value);
        }
    }
}