﻿using System;
using System.Collections.Generic;

namespace Nohros.Data
{
  /// <summary>
  /// A factory for <see cref="IAppState"/> class.
  /// </summary>
  public interface IStateRepositoryFactory
  {
    /// <summary>
    /// Creates a instance of the <see cref="IAppState"/> class using
    /// the given <see cref="options"/>.
    /// </summary>
    /// <param name="options">
    /// A collection of key/value pairs containing the configured options for
    /// the repository.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IAppState"/> object.
    /// </returns>
    IAppState CreateStateRepository(IDictionary<string, string> options);
  }
}
