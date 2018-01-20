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

        #region Public Methods

        /// <summary>
        /// Gets all saved grading templates from the disc
        /// </summary>
        /// <returns>All gradings that have beed read from the application folder</returns>
        public List<GradingPercentage> ReadXmlGrading()
        {
            // Create a grading list which we will return later
            var gradings = new List<GradingPercentage>();

            try
            {
                // For each every grading file in the folder
                foreach (var file in GetFileNames())
                {
                    // Catch only xml files
                    if (Path.GetExtension(file) != ".xml")
                        continue;

                    // Add every grading converted from file to the list
                    gradings.Add(GetGradingFromXml(file));
                }
            }
            catch
            {
                // TODO: Error handling
                return null;
            }

            // Finally return the grading list
            return gradings;
        }

        #endregion

        #region Private Helpers

        private GradingPercentage GetGradingFromXml(string file)
        {
            var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var reader = new XmlTextReader(file);

            var result = new GradingPercentage
            {
                Name = Path.GetFileNameWithoutExtension(file),
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
