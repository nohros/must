﻿using System;

namespace Nohros.RestQL
{
  /// <summary>
  /// Defines all the language neutral constant strings used in the restql.
  /// </summary>
  public sealed class Strings
  {
    public const string kConfigFileName = "restql.config";

    public const string kConfigRootNode = "restql";

    const string kName = "name";

    const string kProviderName = "provider-name";

    const string kCommandType = "command-type";

    const string kText = "text";

    const string kStoredProcedure = "storedprocedure";

    /// <summary>
    /// The name of the xml node that contains the Query settings.
    /// </summary>
    public const string kQueryNode = "query";

    /// <summary>
    /// The name of the application cache provider.
    /// </summary>
    public const string kCacheProviderName = "CacheProvider";

    /// <summary>
    /// The name of the group which the query processor providers should be
    /// associated.
    /// </summary>
    public const string kQueryExecutorsGroup = "queryExecutors";

    public const string kTokenPrincipalMapperNode = "token-principal-mapper";

    /// <summary>
    /// The name of the common data provider.
    /// </summary>
    public const string kQueryDataProviderName = "QueryDataProvider";

    /// <summary>
    /// The name of the json collection provider
    /// </summary>
    public const string kJsonCollectionProvider = "JsonCollectionProvider";

    public const string kQueryStringQueryName = kName;

    public const string kSqlQueryType = "sqlquery";

    public const string kConnectionProviderOption = "connectionProvider";

    public const string kDataProviderGroup = "";

    public const string kCommandTypeOption = kCommandType;

    public const string kTextCommandType = kText;

    public const string kStoredProcedureCommandType = kStoredProcedure;

    public const string kCommandTimeoutOption = "command-timeout";

    public const string kJsonCollectionOption = "jsonCollection";

    public const string kDefaultJsonCollection = "arrayOfObjects";

    public const string kResponseAffectedRowsMemberName = "affectedRows";

    public const string kResponseDataMemberName = "data";
  }
}
