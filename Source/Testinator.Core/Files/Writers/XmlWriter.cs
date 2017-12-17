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

        public override void Write(string FileName, object data)
        {
            if (!(data is Grading))
                return;

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
            Directory.CreateDirectory(Settings.Path);
            try
            {
                doc.Save(Settings.Path + FileName + ".xml");

            }
            catch
            {
                // TODO: error handling xml
            }


        }

        public XmlWriter()
        {
            Settings = new WriterSettings();
        }
    }
}
