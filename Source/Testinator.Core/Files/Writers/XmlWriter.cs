using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Testinator.Core
{
    /// <summary>
    /// The file writer which writes in xml files
    /// </summary>
    public class XmlWriter : FileManagerBase
    {
        #region Constructor

        /// <summary>
        /// Default constructor which requires to specify the type of object to write
        /// </summary>
        public XmlWriter(SaveableObjects objectType)
        {
            ObjectType = objectType;
        }

        #endregion

        /// <summary>
        /// Saves <see cref="Grading"/> to xml file
        /// </summary>
        /// <param name="filename">The name of the file</param>
        /// <param name="data">The data to be saved</param>
        public override void WriteToFile(string filename, GradingPercentage data)
        {
            // Create brand-new xml document
            var doc = new XmlDocument();

            // Add crucial informations at the top
            var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            // Create marks subnode
            var MarksNode = doc.CreateElement("Marks");
            doc.AppendChild(MarksNode);

            // Add every mark to the marks subnode
            if (data.IsMarkAIncluded)
                AddMarkXml(data.MarkA, "A", MarksNode, doc);
            AddMarkXml(data.MarkB, "B", MarksNode, doc);
            AddMarkXml(data.MarkC, "C", MarksNode, doc);
            AddMarkXml(data.MarkD, "D", MarksNode, doc);
            AddMarkXml(data.MarkE, "E", MarksNode, doc);
            AddMarkXml(data.MarkF, "F", MarksNode, doc);

            // Finally save the file
            if (!WriteXmlDocumentFile(filename, doc))
                // If something went wrong, throw an error
                throw new Exception("Cannot save criteria file!");
        }

        /// <summary>
        /// Saves the specified property information to the xml file
        /// </summary>
        /// <param name="property">The property itself</param>
        public override void WriteToFile(SettingsPropertyInfo property, bool fileExists = true)
        {
            // Store Xml file
            var doc = new XmlDocument();

            // If file doesn't exists yet
            if (!fileExists)
            {             
                // Add crucial informations at the top
                var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                // Create config root node
                var rootNode = doc.CreateElement("Config");
                doc.AppendChild(rootNode);
            }
            else
            {
                // Load current file state to the doc variable
                var xmlReader = new XmlReader(SaveableObjects.Config);
                doc = xmlReader.LoadXmlFile("config");
            }

            // Get every property node in the file
            var propertyNodeList = doc.GetElementsByTagName("Property");
            foreach (XmlNode propertyXmlNode in propertyNodeList)
                // Find if property with that name already exists
                if (propertyXmlNode.FirstChild.InnerText == property.Name)
                {
                    // If yes, delete it to make a room for new value
                    doc.DocumentElement.RemoveChild(propertyXmlNode);
                    break;
                }

            // Create new property node
            var propertyNode = doc.CreateElement("Property");

            // Append it to config node tree
            doc.DocumentElement.AppendChild(propertyNode);

            // Put a property infos inside of property node
            AppendNodeXml("Name", property.Name, propertyNode, doc);
            AppendNodeXml("Type", property.Type.ToString(), propertyNode, doc);
            AppendNodeXml("Value", property.Value.ToString(), propertyNode, doc);

            // Finally save the file
            if (!WriteXmlDocumentFile("config", doc))
                // If something went wrong, throw an error
                throw new Exception("Cannot save config file!");
        }

        /// <summary>
        /// Writes every view model's property to the file
        /// </summary>
        /// <param name="vm">The view model with properties to save</param>
        public override void WriteToFile(BaseViewModel vm)
        {
            // Get list of every property in the view model
            var propertyList = vm.GetType().GetProperties();

            // Transfer them to SettingsPropertyInfo
            var propertyInfoList = new List<SettingsPropertyInfo>();
            foreach (var property in propertyList)
                propertyInfoList.Add(new SettingsPropertyInfo
                {
                    Name = property.Name,
                    Type = property.PropertyType,
                    Value = property.GetValue(vm, null)
                });

            // First property creates the config file
            WriteToFile(propertyInfoList[0], false);
            propertyInfoList.Remove(propertyInfoList[0]);

            // Append the rest properties
            foreach (var property in propertyInfoList)
                WriteToFile(property);
        }

        /// <summary>
        /// Deletes xml file by filename
        /// </summary>
        /// <param name="filename">Name of the file to be deleted</param>
        public bool DeleteXmlFileByName(string filename)
        {
            try
            {
                // Try to delete file by it's name in folder based on current object type
                File.Delete(DefaultPath + FolderNameBasedOnObjectType + filename + ".xml");
            }
            catch
            {
                // If failed to delete
                return false;
            }

            // Operation succeeded
            return true;
        }

        #region Private Helpers

        /// <summary>
        /// Writes the <see cref="XmlDocument"/> to file
        /// </summary>
        /// <param name="filename">The name of the file to write to</param>
        /// <param name="doc">The Xml document to write</param>
        private bool WriteXmlDocumentFile(string filename, XmlDocument doc)
        {
            try
            {
                // Save the document to the file
                doc.Save(DefaultPath + FolderNameBasedOnObjectType + filename + ".xml");
            }
            catch
            {
                // If failed to save
                return false;
            }

            // Operation succeeded
            return true;
        }

        /// <summary>
        /// Creates Xml Node from <see cref="Mark"/> object
        /// </summary>
        /// <param name="mark">The mark object which we base on</param>
        /// <param name="MarkName">Name of the mark</param>
        /// <param name="rootElement">The parent node</param>
        /// <param name="doc">The main Xml document</param>
        private void AddMarkXml(Mark mark, string MarkName, XmlElement rootElement, XmlDocument doc)
        {
            // Create new mark node
            var MarkNode = doc.CreateElement("Mark");
            rootElement.AppendChild(MarkNode);

            // Add mark info inside
            AppendNodeXml("Value", MarkName, MarkNode, doc);
            AppendNodeXml("TopLimit", mark.TopLimit.ToString(), MarkNode, doc);
            AppendNodeXml("BottomLimit", mark.BottomLimit.ToString(), MarkNode, doc);
        }

        /// <summary>
        /// Appends the Xml node to the file
        /// </summary>
        /// <param name="elementName">Name of the node</param>
        /// <param name="value">Value of the node</param>
        /// <param name="rootElement">Parent node</param>
        /// <param name="doc">File we are appending to</param>
        private void AppendNodeXml(string elementName, string value, XmlElement rootElement, XmlDocument doc)
        {
            var xmlNode = doc.CreateElement(elementName);
            xmlNode.AppendChild(doc.CreateTextNode(value));
            rootElement.AppendChild(xmlNode);
        }

        #endregion
    }
}
