using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// The xml reader
    /// </summary>
    public class XmlReader : ReaderBase
    {
        #region Public Methods

        /// <summary>
        /// Gets all saved grading templates from the disc
        /// </summary>
        /// <returns>All gradings that have beed read from the application folder</returns>
        public List<GradingPercentage> ReadXmlGrading()
        {
            List<string> Files;
            try
            {
                Files = new List<string>(Directory.GetFiles(Settings.Path + "Criteria\\"));
            }
            catch
            {
                return null;
            }
            
            var gradings = new List<GradingPercentage>();

            foreach (var file in Files)
            {
                try
                {
                    if (Path.GetExtension(file) != ".xml")
                        continue;

                    gradings.Add(GetGradingFromXml(file));
                }
                catch
                { 
                    // No error handlig for now
                }
            }
            return gradings;
        }
        
        #endregion

        #region Private Methods

        private GradingPercentage GetGradingFromXml(string file)
        {
            var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var reader = new XmlTextReader(file);

            var result = new GradingPercentage
            {
                Name = Path.GetFileNameWithoutExtension(file),
            };


            bool InsideMarkNode = false;
            bool InsideValueNode = false;
            bool InsideTopLimitNode = false;
            bool InsideBottomLimitNode = false;

            int bottomLimit = 0;
            int topLimit = 0;
            Marks Mark = Marks.A;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var name = reader.Name;
                        if (name == "Mark")
                            InsideMarkNode = true;
                        else if (name == "Value" && InsideMarkNode)
                            InsideValueNode = true;
                        else if (name == "TopLimit" && InsideMarkNode)
                            InsideTopLimitNode = true;
                        else if (name == "BottomLimit" && InsideMarkNode)
                            InsideBottomLimitNode = true;
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
                            InsideValueNode = false;
                        }

                        else if (InsideBottomLimitNode)
                        {
                            bottomLimit = Int32.Parse(value);
                            InsideBottomLimitNode = false;
                        }
                        else if (InsideTopLimitNode)
                        {
                            topLimit = Int32.Parse(value);
                            InsideTopLimitNode = false;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        var _name = reader.Name;
                        if (_name == "Mark")
                            result.UpdateMark(Mark, topLimit, bottomLimit);
                        break;
                }
            }

            return result;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlReader()
        {
            Settings = new ReaderSettings();
        }

        #endregion
    }
}
