using UnityEngine;

namespace RealtimeCSG
{
    /// <summary>
    /// Version-specific object id extensions — see partials ObjectIdExtensions.Unity6000_*.cs
    /// </summary>
    public static partial class ObjectIdExtensions
    {
        static bool IsNull(Object obj) => !obj;
    }
}
