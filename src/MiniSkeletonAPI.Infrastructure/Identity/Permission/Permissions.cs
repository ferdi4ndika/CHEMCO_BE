using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MiniSkeletonAPI.Infrastructure.Identity.Permission
{
    public static class Permissions
    {
        //public static List<string> GeneratePermissionsForModule(string module)
        //{
        //    return new List<string>()
        //    {
        //        $"Permissions.{module}.Create",
        //        $"Permissions.{module}.View",
        //        $"Permissions.{module}.Edit",
        //        $"Permissions.{module}.Delete",
        //    };
        //}

        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
            public const string Create = "Permissions.Dashboards.Create";
            public const string Edit = "Permissions.Dashboards.Edit";
            public const string Delete = "Permissions.Dashboards.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }

        public static class TodoItems
        {
            public const string View = "Permissions.TodoItems.View";
            public const string Create = "Permissions.TodoItems.Create";
            public const string Edit = "Permissions.TodoItems.Edit";
            public const string Delete = "Permissions.TodoItems.Delete";
        }

        public static class GetPermissions
        {
            public const string View = "Permissions.GetPermissions.View";
            public const string Create = "Permissions.GetPermissions.Create";
            public const string Edit = "Permissions.GetPermissions.Edit";
            public const string Delete = "Permissions.GetPermissions.Delete";
        }

        public static class TodoLists
        {
            public const string View = "Permissions.TodoLists.View";
            public const string Create = "Permissions.TodoLists.Create";
            public const string Edit = "Permissions.TodoLists.Edit";
            public const string Delete = "Permissions.TodoLists.Delete";
        }

    }
    public class CustomClaimTypes
    {
        public const string Permission = "Permission";
    }
    public class StaticSerialization
    {
        //public static JObject Serialize(Type staticClass)
        //{
        //    //var props = staticClass.GetProperties(BindingFlags.Static | BindingFlags.Public);
        //    var props = staticClass.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        //    //Console.WriteLine(JsonSerializer.Serialize(staticClass.));

        //    var json = new JObject();
        //    foreach (var p in props)
        //    {
        //        var value = p.GetValue(null);
        //        //if (value == null || !p.CanWrite || !p.CanRead) continue;
        //        json[p.Name] = JToken.FromObject(value);
        //    }

        //    foreach (var t in staticClass.GetNestedTypes())
        //        json[t.Name] = Serialize(t);
        //    return json;
        //}
        public static IEnumerable<string> GetFieldFromStaticClass(Type staticClass)
        {
           
            var nestedTypes = staticClass.GetNestedTypes(BindingFlags.Public);
            var sClass = new List<FieldInfo[]>();
            foreach (Type type in nestedTypes)
            {
                sClass.Add(type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));

            }

            var permissions = new List<string>();

            var values = sClass.SelectMany(x => x).ToList();
            foreach(var value in values)
            {
                permissions.Add(value.GetValue(null).ToString());
            }
            return permissions;
        }

        //public static void Deserialize(Type staticClass, JObject json)
        //{
        //    if (json == null) return;
        //    var props = staticClass.GetProperties(BindingFlags.Static | BindingFlags.Public);
        //    foreach (var p in props)
        //    {
        //        if (!json.ContainsKey(p.Name) || !p.CanWrite) continue;
        //        p.SetValue(null, Convert.ChangeType(json[p.Name], p.PropertyType));
        //    }
        //    foreach (var t in staticClass.GetNestedTypes())
        //    {
        //        if (!json.ContainsKey(t.Name)) continue;
        //        Deserialize(t, json[t.Name] as JObject);
        //    }
        //}
    }

}
