using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// The xml writer
    /// </summary>
    public class XmlWriter : WriterBase
    {
        /// <summary>
        /// Saves <see cref="Grading"/> to xml file
        /// </summary>
        /// <param name="FileName">The name of the file</param>
        /// <param name="data">The data to be saved</param>
        public void SaveGrading(string FileName, GradingPercentage data)
        {
            var grades = data as GradingPercentage;

            var doc = new XmlDocument();
            var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            var MarksNode = doc.CreateElement("Marks");
            doc.AppendChild(MarksNode);

            // A
            if (data.IsMarkAIncluded)
            {
                AddMarkXml(data.MarkA, MarksNode, doc);
            }

            AddMarkXml(data.MarkB, MarksNode, doc);
            AddMarkXml(data.MarkC, MarksNode, doc);
            AddMarkXml(data.MarkD, MarksNode, doc);
            AddMarkXml(data.MarkE, MarksNode, doc);
            AddMarkXml(data.MarkF, MarksNode, doc);
            
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

        private void AddMarkXml(Mark mark, XmlElement rootElement, XmlDocument doc)
        {
            var MarkNode = doc.CreateElement("Mark");
            rootElement.AppendChild(MarkNode);

            var ValueNodeA = doc.CreateElement("Value");
            ValueNodeA.AppendChild(doc.CreateTextNode("A"));
            MarkNode.AppendChild(ValueNodeA);

            var TopLimitNodeA = doc.CreateElement("TopLimit");
            TopLimitNodeA.AppendChild(doc.CreateTextNode(mark.TopLimit.ToString()));
            MarkNode.AppendChild(TopLimitNodeA);

            var BottomLimitNodeA = doc.CreateElement("BottomLimit");
            BottomLimitNodeA.AppendChild(doc.CreateTextNode(mark.BottomLimit.ToString()));
            MarkNode.AppendChild(BottomLimitNodeA);
        }

        #endregion
    }
}
