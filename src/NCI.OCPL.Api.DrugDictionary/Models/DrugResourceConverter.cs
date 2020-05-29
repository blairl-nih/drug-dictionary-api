using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NCI.OCPL.Api.DrugDictionary
{

    /// <summary>
    /// Determines how to deserialize a generic IDrugResource into a specific implemenation.
    /// </summary>
    public class DrugResourceConverter : JsonConverter<IDrugResource>
    {
        /// <summary>
        /// Deserializes an instance of IDrugResource into either a DrugAlias or a DrugTerm.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override IDrugResource ReadJson(JsonReader reader, Type objectType, IDrugResource existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var drugItem = default(IDrugResource);
            string type = jsonObject["type"].Value<string>();

            switch (type != null ? type.ToLower() : type)
            {
                case "drugalias":
                    drugItem = new DrugAlias();
                    break;

                case "drugterm":
                    drugItem = new DrugTerm();
                    break;
            }
            serializer.Populate(jsonObject.CreateReader(), drugItem);
            return drugItem;
        }

        /// <summary>
        /// Report that this covnerter can't be used for serializing.
        /// Force the object's default (more specific) serialzation to be used instead.
        /// </summary>
        /// <value>Always returns false.</value>
        public override bool CanWrite { get { return false; } }

        /// <summary>
        /// This method exists for the sake of satisfying the abstract base class but is not used.
        /// </summary>
        public override void WriteJson(JsonWriter writer, IDrugResource value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}