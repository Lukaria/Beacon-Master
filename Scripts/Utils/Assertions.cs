using UnityEngine;

namespace Utils
{
    public static class Assertions
    {
        public static void Assert(in string message = "")
        {
#if UNITY_ASSERTIONS
            var method = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod();
            var className = method.ReflectedType?.Name;
            var methodName = method.Name;
            UnityEngine.Assertions.Assert.IsTrue(false,
                "[" + className + "::" + methodName + "] " + message);
#endif
        }
        
        public static bool IsTrueAssert(bool condition, in string message = "")
        {
#if UNITY_ASSERTIONS
            var method = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod();
            var className = method.ReflectedType?.Name;
            var methodName = method.Name;
            UnityEngine.Assertions.Assert.IsTrue(condition,
                "[" + className + "::" + methodName + "] " + message);
#endif
            return condition;
        }
        
        public static void Warning(in string message = "")
        {
#if UNITY_ASSERTIONS
            var method = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod();
            var className = method.ReflectedType?.Name;
            var methodName = method.Name;
            Debug.LogWarning("[" + className + "::" + methodName + "] " + message);
#endif
        }
        
        public static void Log(in string message = "")
        {
#if UNITY_ASSERTIONS
            var method = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod();
            var className = method.ReflectedType?.Name;
            var methodName = method.Name;
            Debug.Log("[" + className + "::" + methodName + "] " + message);
#endif
        }

        public static void Log(in string message, Color color)
        {
#if UNITY_ASSERTIONS
            var method = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod();
            var className = method.ReflectedType?.Name;
            var methodName = method.Name;
            
            Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f),
                (byte)(color.g * 255f), (byte)(color.b * 255f), "[" + className + "::" + methodName + "] " + message));
#endif
        }
    }
}