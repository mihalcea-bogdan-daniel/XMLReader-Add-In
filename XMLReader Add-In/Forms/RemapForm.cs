﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Core;
using System.Diagnostics;
using System.Windows.Controls;

namespace XMLReader_Add_In.Forms
{

    public partial class RemapForm : Form
    {
        private CustomXMLPart customXMLPart;
        private Microsoft.Office.Interop.Word.ContentControl selectedCC;
        private string selectedXPath = string.Empty;
        public RemapForm(Microsoft.Office.Interop.Word.ContentControl _cc)
        {
            selectedCC = _cc;
            InitializeComponent();
            Populate_customXMLComboBox();

            customXMLPart = ((ComboBoxItem)customXMLComboBox.Items[0]).Tag as CustomXMLPart;
            
        }

        private void Populate_customXMLComboBox()
        {
            foreach (CustomXMLPart part in Globals.ThisAddIn.currentDocument.CustomXMLParts)
            {
                ComboboxItem cbItem = new ComboboxItem();
                cbItem.Text = part.NamespaceURI;
                cbItem.Tag = part;
                customXMLComboBox.Items.Add(cbItem);
                
            }
            foreach (ComboboxItem item in customXMLComboBox.Items)
            {
                if (item.Text == Globals.ThisAddIn.DEFAULT_NAMESPACE)
                {
                    
                    customXMLComboBox.SelectedItem = item;
                }
            }

        }


        private void Populate_customXMLPartTreeView(XDocument XDoc)
        {
            TreeNode RootNode = new TreeNode();
            RootNode.Text = XDoc.Root.Name.LocalName;
            RootNode.Tag = XDoc.Root;
            customXMLPartTreeView.Nodes.Add(RootNode);
            //Build the TreeView
            XMLHandler.BuildNodes(RootNode, XDoc.Root);
        }

        private void Populate_customXMLPartTreeView(CustomXMLPart _customXMLPart)
        {
            XDocument XDoc = XDocument.Parse(_customXMLPart.XML);
            //Initialize the root of treeview
            TreeNode RootNode = new TreeNode();
            RootNode.Text = XDoc.Root.Name.LocalName;
            RootNode.Tag = XDoc.Root;
            customXMLPartTreeView.Nodes.Add(RootNode);
            //Build the TreeView
            XMLHandler.BuildNodes(RootNode, XDoc.Root);
        }

        private void customXMLComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customXMLComboBox.SelectedItem != null)
            {
                //Clear the TreeView
                customXMLPartTreeView.Nodes.Clear();
                
                //Load the custom Xml Part and transform it into XDocument
                customXMLPart = ((ComboBoxItem)customXMLComboBox.SelectedItem).Tag as CustomXMLPart;
                Populate_customXMLPartTreeView(customXMLPart);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO MAke remapping to work
            MessageBox.Show(selectedCC.Title);
            string prefix = this.customXMLPart.NamespaceURI;
            if(selectedXPath != string.Empty)
            {
                selectedCC.XMLMapping.SetMapping(selectedXPath, prefix, customXMLPart);
            } else
            {
                MessageBox.Show("Please select a node from the xml treeview");
            }
        }

        private void customXMLPartTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Action)
            {
                case TreeViewAction.ByMouse:
                    XElement selectedNode = e.Node.Tag as XElement;
                    selectedXPath = Utils.ReturnXPath(selectedNode);
                    break;
            }
        }
    }
    public class ComboboxItem : ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}