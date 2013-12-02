using Mono.Math;
using MTProto.Core.TransportLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace MTProto.Core
{    
    public class AppSettings<T> where T : new()
    {         
        private const string DEFAULT_FILENAME = "settings.jsn";

        static IsolatedStorageScope isc = IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain;

        public void Save(string fileName = DEFAULT_FILENAME)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(isc, null, null);
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Create, isoStore))
             {
                  using (StreamWriter writer = new StreamWriter(isoStream))
                  {
                    writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
                  }
             }           
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(isc, null, null);
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Create, isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    writer.Write(JsonConvert.SerializeObject(pSettings, Formatting.Indented));
                }
            }  
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(isc, null, null);
            JsonSerializer serial = new JsonSerializer();
            T t = new T();

            if (isoStore.FileExists(fileName))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        t = (T)serial.Deserialize(reader, typeof( T));
                    }
                }
            }

            return t;
        }
    }
         
    /// <summary>
    /// Конвертор BigInteger в JSON через byte[]
    /// </summary>
    public class BigIntegerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (BigInteger);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new BigInteger(System.Convert.FromBase64String((string)reader.Value));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((value as BigInteger).GetBytes());
        }
    }

    public class CoreSettings : AppSettings<CoreSettings>
    {
        public byte[] AuthKey { get; set; }
        public long NonceNewNonceXor { get; set; }
        public int UserId { get; set; }

        public List<DcOptions> DataCenters { get; set; }
    }
}
