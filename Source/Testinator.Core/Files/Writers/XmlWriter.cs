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
        public void SaveGrading(string FileName, Grading data)
        {
            var grades = data as Grading;

            var doc = new XmlDocument();
            var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            var MarksNode = doc.CreateElement("Marks");
            doc.AppendChild(MarksNode);
            
            foreach (var mark in grades.Marks)
            {
                var MarkNode = doc.CreateElement("Mark");
                MarksNode.AppendChild(MarkNode);

                var ValueNode = doc.CreateElement("Value");
                ValueNode.AppendChild(doc.CreateTextNode(mark.Value.ToString()));
                MarkNode.AppendChild(ValueNode);

                var TopLimitNode = doc.CreateElement("TopLimit");
                TopLimitNode.AppendChild(doc.CreateTextNode(mark.TopLimit.ToString()));
                MarkNode.AppendChild(TopLimitNode);

                var BottomLimitNode = doc.CreateElement("BottomLimit");
                BottomLimitNode.AppendChild(doc.CreateTextNode(mark.BottomLimit.ToString()));
                MarkNode.AppendChild(BottomLimitNode);
            }
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
    }
}
