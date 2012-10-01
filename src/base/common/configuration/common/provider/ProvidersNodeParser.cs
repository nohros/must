﻿using System;
using System.Collections.Generic;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class ProvidersNode
  {
    internal struct UnresolvedOptions
    {
      public IProviderOptions options;
      public IList<string> references;

      #region .ctor
      public UnresolvedOptions(IProviderOptions options,
        IList<string> references) {
        this.options = options;
        this.references = references;
      }
      #endregion
    }

    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="ProvidersNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the providers configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the providers's location.
    /// </param>
    /// <returns>
    /// A <see cref="ProvidersNode"/> containing the configured providers.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static ProvidersNode Parse(XmlElement element, string base_directory) {
      CheckPreconditions(element, base_directory);
      IList<UnresolvedOptions> unresolved_options_references =
        new List<UnresolvedOptions>();
      IDictionary<string, IProviderOptions> reference_table = null;
      ProvidersNode providers = new ProvidersNode();
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element) {
          IList<string> references = new List<string>();
          if (Strings.AreEquals(node.Name, Strings.kProviderNodeName)) {
            IProviderNode provider = ProviderNode.Parse(element, base_directory,
              out references);

            IProvidersNodeGroup providers_node_group;
            if (!providers.GetProvidersNodeGroup(provider.Group,
              out providers_node_group)) {
              providers_node_group = new ProvidersNodeGroup(provider.Group);
              providers.Add(providers_node_group);
            }
            providers_node_group.Add(provider);

            // Add the provider to the unresolved providers list if it has
            // references to be resolved.
            if (references.Count > 0) {
              unresolved_options_references
                .Add(new UnresolvedOptions(provider.Options, references));
            }
          } else if (Strings.AreEquals(node.Name, Strings.kOptionsNodeName)) {
            reference_table = ParseReferenceTable((XmlElement) node,
              unresolved_options_references);
          }
        }

        if (reference_table != null) {
          ResolveOptionsReferences(unresolved_options_references,
            reference_table);
        }
      }
      return providers;
    }

    static IDictionary<string, IProviderOptions> ParseReferenceTable(
      XmlElement element, IList<UnresolvedOptions> unresolved_options_references) {
      IDictionary<string, IProviderOptions> reference_table =
        new Dictionary<string, IProviderOptions>();
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          Strings.AreEquals(node.Name, Strings.kOptionsNodeName)) {
          IList<string> references;
          string name = GetAttributeValue(element, Strings.kNameAttribute);
          ProviderOptionsNode options =
            ProviderOptionsNode.Parse(name, (XmlElement) node, out references);

          // Add the provider options to the unresolved options list if it has
          // references to be resolved.
          if (references.Count > 0) {
            unresolved_options_references
              .Add(new UnresolvedOptions(options, references));
          }
        }
      }
      return reference_table;
    }

    static void ResolveOptionsReferences(
      IEnumerable<UnresolvedOptions> unresolved_options_references,
      IDictionary<string, IProviderOptions> reference_table) {
      foreach (
        UnresolvedOptions unresolved_options in unresolved_options_references) {
        IProviderOptions options = unresolved_options.options;
        IList<string> references = unresolved_options.references;
        foreach (string reference in references) {
          IProviderOptions referenced_options;
          if (reference_table.TryGetValue(reference, out referenced_options)) {
            foreach (
              KeyValuePair<string, string> referenced_option in
                referenced_options) {
              options[referenced_option.Key] = referenced_option.Value;
            }
          }
        }
      }
    }
  }
}
