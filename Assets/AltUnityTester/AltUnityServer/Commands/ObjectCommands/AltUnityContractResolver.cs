using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityContractResolver : DefaultContractResolver
    {
        private readonly Func<bool> includeProperty;

        public AltUnityContractResolver(Func<bool> includeProperty)
        {
            this.includeProperty = includeProperty;
        }

        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Select(p => CreateProperty(p, memberSerialization))
                        .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                   .Select(f => CreateProperty(f, memberSerialization)))
                        .ToList();
            foreach (var prop in props)
            {
                if (prop.AttributeProvider.GetAttributes(true).OfType<ObsoleteAttribute>().Any())
                {
                    prop.ShouldSerialize = obj => false;
                }
                prop.Writable = true; prop.Readable = true;

            }
            return props;
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = instance =>
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    field.GetValue(instance);
                    if (includeProperty())
                        return true;
                }
                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo prop = (PropertyInfo)member;
                    prop.GetValue(instance, null);
                    if (includeProperty())
                        return true;
                }
                return false;


            };

            return property;
        }
    }
    public class CustomJsonTextWriter : JsonTextWriter
    {
        public CustomJsonTextWriter(System.IO.TextWriter textWriter) : base(textWriter) { }

        public int CurrentDepth { get; private set; }

        public override void WriteStartObject()
        {
            CurrentDepth++;
            base.WriteStartObject();
        }

        public override void WriteEndObject()
        {
            CurrentDepth--;
            base.WriteEndObject();
        }
    }
}
