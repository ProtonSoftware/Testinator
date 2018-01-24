using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Testinator.Core
{
    /// <summary>
    /// The Xml files reader
    /// </summary>
    public class XmlReader : FileManagerBase
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlReader(SaveableObjects objectType)
        {
            ObjectType = objectType;
        }

        #endregion

        #region File Reading

        /// <summary>
        /// Gets all saved objects of <see cref="T"/> type in current directory
        /// </summary>
        /// <typeparam name="T">The type of object to read</typeparam>
        /// <returns>List of T objects</returns>
        public override List<T> ReadFile<T>()
        {
            // Create a T list which we will return later
            var resultList = new List<T>();

            // For each file in the folder
            foreach (var file in GetFileNames())
            {
                // Catch only xml files
                if (Path.GetExtension(file) != ".xml")
                    continue;

                // Add every T object converted from file to the list
                resultList.Add(ConvertObjectFromXml<T>(Path.GetFileNameWithoutExtension(file)));
            }

            // Finally return the grading list
            return resultList;
        }

        /// <summary>
        /// Reads the config file and returns the list of every property found
        /// </summary>
        /// <returns>List of every property in the config file</returns>
        public List<SettingsPropertyInfo> LoadConfig()
        {
            // Create the list of properties in the file
            var list = new List<SettingsPropertyInfo>();

            // Get the list of property nodes
            var propertyNodeList = GetNodeListByNameFromFile("config", "Property");

            // Loop each node for property info
            foreach (XmlNode node in propertyNodeList)
            {
                // Get property attributes
                var xmlName = node.ChildNodes.Item(0).InnerText;
                var xmlType = node.ChildNodes.Item(1).InnerText;
                var xmlValue = node.ChildNodes.Item(2).InnerText;

                // Cast the value to desired type
                var propertyType = Type.GetType(xmlType);
                var propertyValue = Convert.ChangeType(xmlValue, propertyType);

                // Add new property to the list
                list.Add(new SettingsPropertyInfo
                {
                    Name = xmlName,
                    Type = propertyType,
                    Value = propertyValue
                });
            }

            // Finally return the list
            return list;
        }

        /// <summary>
        /// Reads the Xml file and returns its content as <see cref="XmlDocument"/>
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <returns>XmlDocument filled with what's inside the file</returns>
        public XmlDocument LoadXmlFile(string filename)
        {
            // Create new Xml document
            var doc = new XmlDocument();

            // Load data from specified file
            doc.Load(DefaultPath + FolderNameBasedOnObjectType + filename + ".xml");

            // Return the document
            return doc;
        }

        /// <summary>
        /// Checks if file exists by it's name
        /// </summary>
        /// <param name="filename">The file name</param>
        public bool FileExists(string filename) => File.Exists(DefaultPath + FolderNameBasedOnObjectType + filename);

        #endregion

        #region Private Helpers

        /// <summary>
        /// Converts the file to <see cref="T"/> object
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="file">The filename</param>
        /// <returns>The converted T object</returns>
        private T ConvertObjectFromXml<T>(string filename) 
            where T : class, new()
        {
            // Get the type of T object
            var objectType = typeof(T);
            var objectToReturn = new T();

            // Based on that...
            if (objectType == typeof(GradingPercentage))
                // Get the object by this type
                objectToReturn = GetGradingFromXml(filename) as T;

            /* // Example of future objects reading 
            else if (objectType == typeof(Something))
                // Get the object by this type
                objectToReturn = GetSomethingFromXml(filename) as T; */

            // Finally return the object
            return objectToReturn;
        }

        /// <summary>
        /// Gets the list of every node with specified name from Xml file
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <param name="name">The name of every node which will be return</param>
        /// <returns></returns>
        private XmlNodeList GetNodeListByNameFromFile(string filename, string name)
        {
            // Load the XmlDocument from file name
            var doc = new XmlDocument();
            doc.Load(DefaultPath + FolderNameBasedOnObjectType + filename + ".xml");

            // Return the list of every node with specified name
            return doc.GetElementsByTagName(name);
        }

        /// <summary>
        /// Converts to <see cref="GradingPercentage"/> object from specified file
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <returns>The <see cref="GradingPercentage"/> object converted from file</returns>
        private GradingPercentage GetGradingFromXml(string filename)
        {
            // Create result object to return at the end
            var result = new GradingPercentage
            {
                // Set default values
                Name = Path.GetFileNameWithoutExtension(filename),
                IsMarkAIncluded = false,
            };

            // Get the list of mark nodes
            var markNodeList = GetNodeListByNameFromFile(filename, "Mark");

            // Loop each node for mark info
            foreach (XmlNode node in markNodeList)
            {
                // Get mark attributes
                var mark = (Marks)Enum.Parse(typeof(Marks), node.ChildNodes.Item(0).InnerText);
                var topLimit = int.Parse(node.ChildNodes.Item(1).InnerText);
                var bottomLimit = int.Parse(node.ChildNodes.Item(2).InnerText);

                // Add them to result object
                result.UpdateMark(mark, topLimit, bottomLimit);

                // Check if mark A should be included
                if (mark == Marks.A)
                    result.IsMarkAIncluded = true;
            }

            // Finally return the result object
            return result;
        }

        #endregion
    }
}
