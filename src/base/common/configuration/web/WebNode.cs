using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;

using Nohros.Data;
using Nohros.Data.Collections;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class WebNode : ConfigurationNode
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the WebNode class by using the specified XML node and name.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="node">A XML node that contains the configuration data.</param>
        /// <param name="common_node">The configuration common node.</param>
        public WebNode() : base(NohrosConfiguration.kWebNodeName) { }
        #endregion

        /// <summary>
        /// Instantiate a new instane of the <see cref="WebNode"/> class and parses the specified node.
        /// </summary>
        /// <param name="node">A XML node that contains the configuration data.</param>
        /// <returns></returns>
        public static WebNode FromXmlNode(XmlNode node, NohrosConfiguration config) {
            WebNode web_node = new WebNode();
            web_node.Parse(node, config);
            return web_node;
        }

        string ContentGroupKey(string name, string build, string mime_type) {
            return string.Concat(name, ".", build, ".", mime_type);
        }

        /// <summary>
        /// Parses a XML node that contains information about a web node.
        /// </summary>
        /// <param name="node">A XML node containing the data to parse.</param>
        /// <param name="config">The configuration object which this node belongs to.</param>
        public override void Parse(XmlNode node, NohrosConfiguration config) {
            XmlNode data_node = IConfiguration.SelectNode(node, NohrosConfiguration.kContentGroupsNodeName);
            if (data_node == null)
                return; // content-group nodes are not mandatory

            foreach (XmlNode n in data_node.ChildNodes) {
                DictionaryValue<ContentGroupNode> content_groups = new DictionaryValue<ContentGroupNode>();
                config.Nodes[NohrosConfiguration.kContentGroupNodeTree] = content_groups;

                if (string.Compare(n.Name, "group", StringComparison.OrdinalIgnoreCase) == 0) {
                    string name;
                    if (!GetAttributeValue(n, "name", out name))
                        Thrower.ThrowConfigurationException("[Parse   Nohros.Configuration.WebNode]", string.Format(StringResources.Config_MissingAt, "name", NohrosConfiguration.kContentGroupNodeTree));

                    ContentGroupNode content_group = new ContentGroupNode(name);
                    content_group.Parse(n, config);
                    content_groups[ContentGroupKey(content_group.Name, (content_group.BuildType == BuildType.Release) ? "release" : "build", content_group.MimeType)] = content_group;
                }
            }
        }

        /// <summary>
        /// Gets a list of files related with the specified content group, build version and mime type.
        /// </summary>
        /// <param name="name">The name of the group to get.</param>
        /// <param name="build">The build version</param>
        /// <param name="mime_type">The mime type of the group.</param>
        /// <returns></returns>
        public ContentGroupNode GetContentGroup(string name, string build, string mime_type) {
            if (name == null || build == null || mime_type == null)
                throw new ArgumentException((name == null) ? (build == null) ? "mime_type" : "build" : "name");
            return this[ContentGroupKey(name, build, mime_type)] as ContentGroupNode;
        }
    }
}
