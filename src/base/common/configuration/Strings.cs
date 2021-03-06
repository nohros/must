﻿using System;

using Nohros.Configuration;

namespace Nohros
{
  /// <summary>
  /// Defines the constant string that could appear on the nohros
  /// configuration files.
  /// </summary>
  internal sealed class Strings
  {
    public const string kNohrosNodeName = "nohros";
    public const string kBaseDirectoryAttribute = "base-directory";
    public const string kNameAttribute = "name";
    public const string kLocationAttribute = "location";
    public const string kTypeAttribute = "type";
    public const string kGroupAttribute = "group";
    public const string kControlFlagAttribute = "control-flag";
    public const string kOptionsRefAttribute = "ref";
    public const string kOptionNodeName = "option";
    public const string kValueAttribute = "value";

    // node names
    public const string kCommonNodeName = "common";
    public const string kWebNodeName = "common";
    public const string kRepositoriesNodeName = "repositories";
    public const string kRepositoryNodeName = "repository";
    public const string kProvidersNodeName = "providers";
    public const string kReplicasNodeName = "replicas";
    public const string kReplicaNodeName = "replica";
    public const string kProvidersNodeGroupName = "";
    public const string kDataProvidersNodeName = "data";
    public const string kCacheProvidersNodeName = "cache";
    public const string kSimpleProvidersNodeName = "simple";
    public const string kProviderNodeName = "provider";
    public const string kOptionsNodeName = "options";
    public const string kLoginModulesNodeName = "login-modules";
    public const string kLoginModuleNodeName = "module";
    public const string kXmlElementsNodeName = "xml-elements";

    public const string kAliasesNodeName = "aliases";
    public const string kAliasNodeName = "alias";

    /// <summary>
    /// The key that will be used to store the Xml element that was used to
    /// configure a <see cref="Nohros.Configuration.IConfiguration"/> object.
    /// </summary>
    public const string kRootXmlElementName = kNohrosNodeName;

    /// <summary>
    /// Compares two strings for equality using a ordinal string comparison.
    /// </summary>
    /// <returns>
    /// <c>true</c> if <paramref name="str_a"/> is ordinally equals to
    /// <paramref name="str_b"/>; otherwise, false.
    /// </returns>
    /// <remarks>
    /// This method performs a ordinal case-insensitive comparison.
    /// </remarks>
    public static bool AreEquals(string str_a, string str_b) {
      return string.Compare(str_a, str_b, StringComparison.Ordinal) == 0;
    }
  }
}