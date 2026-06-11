using System.Text.Json;
using System.Text.Json.Nodes;

namespace Energy.Api.Common.Logger;

public static class SensitiveDataMasker
{
    private static readonly HashSet<string> SensitiveKeys =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "password",
            "passwordHash",
            "currentPassword",
            "newPassword",
            "confirmPassword",
            "token",
            "accessToken",
            "refreshToken",
            "authorization",
            "secret",
            "apiKey"
        };

    public static string? MaskJson(string? payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            return payload;
        }

        try
        {
            var node = JsonNode.Parse(payload);

            if (node is null)
            {
                return payload;
            }

            MaskNode(node);

            return node.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = false
            });
        }
        catch
        {
            return payload;
        }
    }

    private static void MaskNode(JsonNode node)
    {
        if (node is JsonObject jsonObject)
        {
            foreach (var property in jsonObject.ToList())
            {
                if (SensitiveKeys.Contains(property.Key))
                {
                    jsonObject[property.Key] = "***MASKED***";
                    continue;
                }

                if (property.Value is not null)
                {
                    MaskNode(property.Value);
                }
            }

            return;
        }

        if (node is JsonArray jsonArray)
        {
            foreach (var item in jsonArray)
            {
                if (item is not null)
                {
                    MaskNode(item);
                }
            }
        }
    }
}