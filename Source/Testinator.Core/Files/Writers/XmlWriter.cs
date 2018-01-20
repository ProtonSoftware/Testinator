using System.IO;
using System.Xml;

namespace Testinator.Core
{
    /// <summary>
    /// The file writer which writes in xml files
    /// </summary>
    public class XmlWriter : FileWriterBase
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
            WriteXmlDocumentFile(filename, doc);
        }

        /// <summary>
        /// Saves the specified property information to the xml file
        /// </summary>
        /// <param name="property">The property itself</param>
        public override void WriteToFile(object property, bool fileExists = true)
        {
            // Get informations about the property
            var propertyName = nameof(property);
            var propertyType = property.GetType();
            var propertyValue = property; // TODO: Test if this works

            // Store Xml file
            var doc = new XmlDocument();

            // If file doesn't exists yet
            if (/*!*/fileExists)
            {             
                // Add crucial informations at the top
                var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);
            }
            else
            {
                // TODO: Load current file state to the doc variable (then we can fix the if upside)
                // XmlReader.LoadConfig() or something
            }

            // Create property root
            var rootNode = doc.CreateElement(propertyName);
            doc.AppendChild(rootNode);

            // Put a property type and value inside
            AppendNodeXml("PropertyType", propertyType.ToString(), rootNode, doc);
            AppendNodeXml("PropertyValue", propertyValue.ToString(), rootNode, doc);

            // Finally save the file
            WriteXmlDocumentFile("config", doc);
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
                // TODO: Error handling
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
                // TODO: error handling xml
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
