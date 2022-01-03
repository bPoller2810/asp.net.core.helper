using System;

namespace asp.net.core.helper.core.Auth
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    { }
}
