using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Common.Authorization;

/// <summary>
/// Scans controller assemblies for every <see cref="AuthorizeAttribute.Policy"/>
/// value so the seeder can auto-register the corresponding permission rows on
/// startup. Adding <c>[Authorize(Policy = SomePermissions.SomeAction)]</c> to a
/// new endpoint is therefore enough to make the permission appear in the
/// database — no manual seeding step required.
/// </summary>
public static class PermissionDiscovery
{
    public static IReadOnlyList<string> DiscoverPolicyCodes(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        var codes = new HashSet<string>(StringComparer.Ordinal);

        foreach (var assembly in assemblies)
        {
            IEnumerable<Type> types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t is not null)!;
            }

            foreach (var type in types.Where(IsController))
            {
                CollectFromAttributes(type.GetCustomAttributes<AuthorizeAttribute>(inherit: true), codes);

                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    CollectFromAttributes(method.GetCustomAttributes<AuthorizeAttribute>(inherit: true), codes);
                }
            }
        }

        return codes
            .OrderBy(code => code, StringComparer.Ordinal)
            .ToArray();
    }

    private static bool IsController(Type type)
    {
        if (type.IsAbstract || !type.IsClass)
        {
            return false;
        }

        return typeof(ControllerBase).IsAssignableFrom(type)
               || type.Name.EndsWith("Controller", StringComparison.Ordinal);
    }

    private static void CollectFromAttributes(IEnumerable<AuthorizeAttribute> attributes, HashSet<string> sink)
    {
        foreach (var attribute in attributes)
        {
            if (!string.IsNullOrWhiteSpace(attribute.Policy))
            {
                sink.Add(attribute.Policy.Trim());
            }
        }
    }
}


