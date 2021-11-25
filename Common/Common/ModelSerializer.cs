using System;
using System.Runtime.Serialization;
using System.Xml;

namespace Common
{

    public class ModelSerializer
    {
        public virtual TModel Deserialize<TModel>(string path)
        {
            XmlReader reader = XmlReader.Create(path);

            DataContractSerializer serializer = new DataContractSerializer(typeof(TModel));
            TModel result = (TModel)serializer.ReadObject(reader);

            return result;
        }

        public virtual void Serialize<TModel>(TModel model, string path)
        {
            XmlWriter writer = XmlWriter.Create(path);

            DataContractSerializer serializer = new DataContractSerializer(typeof(TModel));
            serializer.WriteObject(writer, model);

            writer.Flush();
        }
    }
}
