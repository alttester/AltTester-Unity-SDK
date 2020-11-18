using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        private readonly Func<bool> _includeProperty;

        public AltUnityContractResolver(Func<bool> includeProperty)
        {
            _includeProperty = includeProperty;
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
                // try
                // {
                if (prop.AttributeProvider.GetAttributes(true).OfType<ObsoleteAttribute>().Any())
                {
                    prop.ShouldSerialize = obj => false;
                }
                prop.Writable = true; prop.Readable = true;
                // }
                // catch (Exception e)
                // {
                //     LogMessage(e.Message);
                // }

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
                    // try
                    // {
                    FieldInfo field = (FieldInfo)member;
                    field.GetValue(instance);
                    if (_includeProperty())
                        return true;
                    // }
                    // catch (Exception e)
                    // {
                    //     LogMessage(e.Message);
                    // }
                }
                if (member.MemberType == MemberTypes.Property)
                {
                    // try
                    // {
                    PropertyInfo prop = (PropertyInfo)member;
                    prop.GetValue(instance, null);
                    if (_includeProperty())
                        return true;
                    // }
                    // catch (Exception e)
                    // {
                    //     LogMessage(e.Message);
                    // }
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
