﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;
using System.Drawing;

namespace XMLReader_Add_In
{
    public class XMLHandler
    {
        
        /// <summary>
        /// Builds a tree view based on XML XDoc ant existing TreeView
        /// </summary>
        /// <param name="treeNode">The treeView root element that should be built</param>
        /// <param name="element">The Starting XElement in the XML</param>
        /// <example>BuildNodes(RootTreeNode, XMLElement)</example>
        public static void BuildNodes(TreeNode treeNode, XElement element)
        {
            TreeNode childTreeNode = new TreeNode();
            // If it is not a leaf element show the tag name
            if (element.HasElements)
            {
                childTreeNode.Text = element.Name.LocalName;
            } else
            {
                // It is a leaf because no it has no elements, show the value inside the tag
                if(element.Value.Equals(""))
                {
                    childTreeNode.Text = element.Name.LocalName;
                } else
                {
                    childTreeNode.NodeFont = new System.Drawing.Font(TreeView.DefaultFont, FontStyle.Bold);
                    childTreeNode.Text = element.Value;
                }
            }
            //Each treeNode indicates to the XDocument element, so later we can retrieve the XPath of that element
            childTreeNode.Tag = element;
            foreach (XElement child in element.Elements())
            {
                //The recursion
                BuildNodes(childTreeNode, child);
            }
            treeNode.Nodes.Add(childTreeNode);
        }


        public static void RemapCustomXMLPart(CustomXMLPart xmlPart)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {

                        XDocument tempXML = XDocument.Load(openFileDialog.FileName);
                        tempXML.Root.Name =
                                Globals.ThisAddIn.DEFAULT_NAMESPACE + tempXML.Root.Name.LocalName;
                        xmlPart.Delete();
                        Globals.ThisAddIn.currentDocument.CustomXMLParts.Add(tempXML.ToString());
                        
                    }
                }
            }
        }
        public static void RemapCustomXMLPart(ContentControl cc)
        {

        }
    }

}
