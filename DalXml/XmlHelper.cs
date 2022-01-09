using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Dal
{
    public class XmlHelper
    {
        #region SaveLoadWithXElement
        /// <summary>
        /// saves data to xml
        /// </summary>
        /// <param name="rootElem"></param>
        /// <param name="filePath"></param>
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(/*dir + */filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        /// <summary>
        /// write data to file with xElemnt operation
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static XElement LoadListFromXMLElement(string FilePath)
        {
            try
            {
                if(File.Exists(FilePath))
                {
                    XElement Element;
                    Element = XElement.Load(FilePath);//load the data that in the file to element
                    return Element;
                }
                else
                {
                    XElement Element=new XElement(FilePath);
                    Element.Save(FilePath);
                    return Element;
                }

            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(FilePath, $"fail to load xml file: {FilePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithSerialazation

        /// <summary>
        ///save data- writhe to file with serialize operation
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="FilePath"></param>
        public static void SerializeData<T>(List<T> ts, string FilePath)
        {
            try
            {
                FileStream file = new FileStream(FilePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(ts.GetType());
                x.Serialize(file, ts);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(FilePath, $"fail to load xml file: {FilePath}", ex);
            }

        }

        /// <summary>
        /// looad data -read from file with deserialization oparation
        /// </summary>
        /// <param name="ObjType"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static List<T> DeserializeData<T>(string FilePath)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    List<T> ts;
                    var serializer = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    ts = (List<T>)serializer.Deserialize(file);
                    file.Close();
                    return ts;
                }
                else
                {
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(FilePath, $"fail to load xml file: {FilePath}", ex);
            }

        }
        #endregion

    }
}

