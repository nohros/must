using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Nohros.Data;
using Nohros.Data.Collections;
using Nohros.Configuration;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// The default implmentation of the <see cref="ILoginConfiguration"/>. This
  /// class provides a basic configuration object that can be used to parse
  /// the login configuration file. It extends the
  /// <see cref="NohrosConfiguration"/> class and relies on the functionality
  /// of that class.
  /// </summary>
  /// <seealso cref="NohrosConfiguration"/>
  public class LoginConfiguration: NohrosConfiguration, ILoginConfiguration
  {
    ILoginModuleEntry[] modules_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginConfiguration"/>
    /// class that have no modules configured.
    /// </summary>
    public LoginConfiguration() {
      modules_ = new ILoginModuleEntry[0];
      LoadComplete += OnConfigLoadComplete;
    }
    #endregion

    /// <summary>
    /// The base <see cref="NohrosConfiguration"/> class has five Load method,
    /// so instead of to override all that methods we will sibscribe to the
    /// <see cref="IConfiguration.LoadComplete"/> in order our custom
    /// parsing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnConfigLoadComplete(object sender, EventArgs e) {
      // store the login modules into a array to speed up the
      // LoginModules property access.
      LoginModuleNode[] nodes = LoginModuleNodes.ToArray();
      modules_ = new ILoginModuleEntry[nodes.Length];
      for (int i = 0; i < modules_.Length; i++) {
        modules_[i] = nodes[i] as ILoginModuleEntry;
      }
    }

    /// <summary>
    /// Retrieve the <see cref="ILoginModuleEntry"/> object for the specified
    /// module name.
    /// </summary>
    /// <param name="name">The name used to index the module.</param>
    /// <returns>A <see cref="ILoginModuleEntry"/> for the spcifie
    /// <paramref name="name"/>, or null if there are no entry for the
    /// specified <paramref name="name"/></returns>
    public ILoginModuleEntry GetLoginModuleEntry(string name) {
      // a login module node is a login module entry too.
      return base.LoginModuleNodes[name] as ILoginModuleEntry;
    }

    /// <summary>
    /// Gets all the login modules configured for the application.
    /// </summary>
    /// <returns>An array of LoginModuleEntry containg all the login modules
    /// configured for the application.</returns>
    /// <remarks>
    /// Retrieving the value of this property is an O(1) operation.
    /// </remarks>
    public ILoginModuleEntry[] LoginModules {
      get {
        return modules_;
      }
    }
  }
}