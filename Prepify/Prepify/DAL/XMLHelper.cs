using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;


namespace Prepify.DAL
{
    class XMLHelper
    {
        public XDocument XMLDoc { get; set; }

        /// <summary>
        /// Takes in three doubles to generate an XDocument containing the user's
        /// preferred font size choice. Returns the XDocument as a string.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="label"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string CreateXDocumentString(double button, double label, 
            double entry)
        {
            var xmldoc =
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                        new XElement("FontSizes", 
                            new XElement("ButtonSize", button),
                            new XElement("LabelSize", label),
                            new XElement("EntrySize", entry)));

            string str = xmldoc.Document.ToString(SaveOptions.DisableFormatting);

            return str;
        }

        /// <summary>
        /// Takes in a string and parses it into an XDocument.
        /// </summary>
        /// <param name="xDocString"></param>
        /// <returns></returns>
        public static XDocument ParseXDocString(string xDocString)
        {
            return XDocument.Parse(xDocString);
        }

        /// <summary>
        /// Takes in an XDocument and reads the Font Size elements within.
        /// Converts these strings into doubles, then returns an array of doubles.
        /// </summary>
        /// <param name="xDocument"></param>
        /// <returns></returns>
        public static double[] RetrieveFontSettings(XDocument xDocument)
        {

            var buttonNode = xDocument.XPathSelectElement("//ButtonSize").Value;
            var labelNode = xDocument.XPathSelectElement("//LabelSize").Value;
            var entryNode = xDocument.XPathSelectElement("//EntrySize").Value;

            double button = Double.Parse(buttonNode);
            double label = Double.Parse(labelNode);
            double entry = Double.Parse(entryNode);

            double[] fontArray = new double[] { button, label, entry };

            return fontArray;
        }

       
    }

    
}
