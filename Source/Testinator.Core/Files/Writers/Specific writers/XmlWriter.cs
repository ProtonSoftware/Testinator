using System.IO;
using System.Xml;

namespace Testinator.Core
{
    /// <summary>
    /// The file writer which writes in xml files
    /// </summary>
    public class XmlWriter : FileWriterBase
    {
        /// <summary>
        /// Saves <see cref="Grading"/> to xml file
        /// </summary>
        /// <param name="FileName">The name of the file</param>
        /// <param name="data">The data to be saved</param>
        public override void WriteToFile(string FileName, GradingPercentage data)
        {
            var doc = new XmlDocument();
            var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            var MarksNode = doc.CreateElement("Marks");
            doc.AppendChild(MarksNode);

            // A
            if (data.IsMarkAIncluded)
                AddMarkXml(data.MarkA, "A", MarksNode, doc);

            AddMarkXml(data.MarkB, "B", MarksNode, doc);
            AddMarkXml(data.MarkC, "C", MarksNode, doc);
            AddMarkXml(data.MarkD, "D", MarksNode, doc);
            AddMarkXml(data.MarkE, "E", MarksNode, doc);
            AddMarkXml(data.MarkF, "F", MarksNode, doc);
            
            try
            {
                doc.Save(Settings.Path + "Criteria\\" + FileName + ".xml");
            }
            catch
            {
                // TODO: error handling xml
            }
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

            try
            {
                // Save the file
                doc.Save(Settings.Path + "Config\\" + "config"+ ".xml");
            }
            catch
            {
                // TODO: error handling xml
            }
        }

        /// <summary>
        /// Deletes xml file by filename
        /// </summary>
        /// <param name="FileName">Name of the file to be delted</param>
        public void DeleteFile(string FileName)
        {
            try
            {
                File.Delete(Settings.Path + "Criteria\\" + FileName + ".xml");
            }
            catch { }
        }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlWriter()
        {
            Settings = new WriterSettings();
        }

        #endregion

        #region Private Helpers

        private void AddMarkXml(Mark mark, string MarkName, XmlElement rootElement, XmlDocument doc)
        {
            var MarkNode = doc.CreateElement("Mark");
            rootElement.AppendChild(MarkNode);

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
