﻿using System;
using System.Collections.Generic;

namespace Nohros.RestQL
{
  /// <summary>
  /// A factory class that is used to create instances of classes that
  /// implements the <see cref="IQueryExecutor"/> interface.
  /// </summary>
  /// <remarks>
  /// This interface implies a constructor with no parameters.
  /// </remarks>
  public interface IQueryExecutorFactory
  {
    /// <summary>
    /// Creates a instance of the <see cref="IQueryExecutor"/> class by using
    /// the specified application settings.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the specific
    /// options configured for the query processor.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="IQueryExecutor"/> class.
    /// </returns>
    IQueryExecutor CreateQueryExecutor(IDictionary<string, string> options);
  }
}
