using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Collections;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class ContentGroupNode : AbstractConfigurationNode
    {
        const string kFileNameAttributeName = "file-name";
        const string kNameAttributeName = "name";
        const string kBuildAttributeName = "build";
        const string kMimeTypeAttributeName = "mime-type";
        const string kPathRefAttributeName = "path-ref";
        const string kDataBaseOwnerAttributeName = "dbowner";
        const string kConnectionStringAttributeName = "dbstring";

        string base_path_;
        BuildType build_type_;
        string mime_type_;
        List<string> files_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the ContentGroupNode by using the specified name and parent
        /// node.
        /// </summary>
        /// <param name="common_node">A WebNode object which this provider belongs.</param>
        public ContentGroupNode(string name): base(name) {
            files_ = new List<string>();
            base_path_ = AppDomain.CurrentDomain.BaseDirectory;
            mime_type_ = null;
            build_type_ = BuildType.Release;
        }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a content group.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a content group.</exception>
        public void Parse(XmlNode node, DictionaryValue<RepositoryNode> repositories) {
            string name = null, build = null, mime_type = null, path_ref = null;
            if (!(GetAttributeValue(node, kNameAttributeName, out name) &&
                    GetAttributeValue(node, kBuildAttributeName, out build) &&
                    GetAttributeValue(node, kMimeTypeAttributeName, out mime_type) &&
                    GetAttributeValue(node, kPathRefAttributeName, out path_ref)
                )) {
                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt
                            ,"a required attribute"
                            ,NohrosConfiguration.kContentGroupNodeTree + ".any"
                        )
                        ,"[Parse   Nohros.Configuration.ContentGroupNode]"
                    );
            }

            // sanity check the build type
            if (build != "release" && build != "debug")
                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_ArgOutOfRange, build, NohrosConfiguration.kContentGroupNodeTree + "." + name + "." + kBuildAttributeName), "[Parse   Nohros.Configuration.ContentGroupNode]");

            // resolve the base path
            RepositoryNode str;
            str = repositories[path_ref] as RepositoryNode;

            if (str == null)
                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_ArgOutOfRange, path_ref, NohrosConfiguration.kContentGroupNodeTree + "." + name + "." + kPathRefAttributeName), "[Parse   Nohros.Configuration.ContentGroupNode]");

            build_type_ = (build == "release") ? BuildType.Release : BuildType.Debug;
            mime_type_ = mime_type;
            base_path_ = str.RelativePath;

            string file_name = null;
            foreach (XmlNode file_node in node.ChildNodes) {
                if (string.Compare(file_node.Name, "add", StringComparison.OrdinalIgnoreCase) == 0) {
                    if (!GetAttributeValue(file_node, kFileNameAttributeName, out file_name)) {
                        Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, kFileNameAttributeName, Name), "[Parse   Nohros.Configuration.ContentGroupNode]");
                    }
                    files_.Add(file_name);
                }
            }
        }

        /// <summary>
        /// Gets or sets a fully qualified path to the folder where the files that compose the content group are stored.
        /// </summary>
        public string BasePath {
            get { return base_path_; }
            set { base_path_ = value; }
        }

        /// <summary>
        /// Gets the content group build version.
        /// </summary>
        /// <remarks>
        /// The build version could be used to discriminate diferrent types of files for diferrent types
        /// of deployment cenarios. Usually a release version of a JScript file is mimimized to speed the
        /// load time, elsewhere, a debug version of a JSScript file may be huge and may contains a lot of
        /// comments.
        /// </remarks>
        public BuildType BuildType {
            get { return build_type_; }
        }

        /// <summary>
        /// Gets the related MIME type.
        /// </summary>
        public string MimeType {
            get { return mime_type_; }
        }

        /// <summary>
        /// Gets the files that composes the content group
        /// </summary>
        public List<string> Files {
            get { return files_; }
            set { files_ = value; }
        }
    }
}
