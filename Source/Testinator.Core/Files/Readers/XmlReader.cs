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
                resultList.Add(ConvertObjectFromXml<T>(file));
            }

            // Finally return the grading list
            return resultList;
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
            doc.Load(new FileStream(filename, FileMode.Open, FileAccess.Read));

            // Return the document
            return doc;
        }

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

            // Example of future objects reading
            /*else if (objectType == typeof(Something))
                // Get the object by this type
                objectToReturn = GetSomethingFromXml(filename) as T;*/

            // Finally return the object
            return objectToReturn;
        }

        /// <summary>
        /// Converts to <see cref="GradingPercentage"/> object from specified file
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <returns>The <see cref="GradingPercentage"/> object converted from file</returns>
        private GradingPercentage GetGradingFromXml(string filename)
        {
            // TODO: Optimize, comment and think of a better way of doing it
            //       Extract some things from this to a generic method to allow different object types
            var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            var reader = new XmlTextReader(filename);

            var result = new GradingPercentage
            {
                Name = Path.GetFileNameWithoutExtension(filename),
                IsMarkAIncluded = false,
            };

            var InsideMarkNode = false;
            var InsideValueNode = false;
            var InsideTopLimitNode = false;
            var InsideBottomLimitNode = false;

            var bottomLimit = 0;
            var topLimit = 0;
            var Mark = Marks.A;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var name = reader.Name;
                        if (name == "Mark")
                            InsideMarkNode = true;
                        else if (InsideMarkNode)
                        {
                            if (name == "Value")
                                InsideValueNode = true;
                            else if (name == "TopLimit")
                                InsideTopLimitNode = true;
                            else if (name == "BottomLimit")
                                InsideBottomLimitNode = true;
                        }
                        else
                        {
                            InsideMarkNode = false;
                            InsideValueNode = false;
                            InsideTopLimitNode = false;
                            InsideBottomLimitNode = false;
                        }
                        break;

                    case XmlNodeType.Text:
                        var value = reader.Value;
                        if (InsideValueNode)
                        {
                            Mark = (Marks)Enum.Parse(typeof(Marks), value);
                            if (Mark == Marks.A)
                                result.IsMarkAIncluded = true;
                            InsideValueNode = false;
                        }
                        else if (InsideBottomLimitNode)
                        {
                            bottomLimit = int.Parse(value);
                            InsideBottomLimitNode = false;
                        }
                        else if (InsideTopLimitNode)
                        {
                            topLimit = int.Parse(value);
                            InsideTopLimitNode = false;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Name == "Mark")
                            result.UpdateMark(Mark, topLimit, bottomLimit);
                        break;
                }
            }
            return result;
        }

        #endregion
    }
}
